using static TheWarriorsExplorer.Form1;

namespace TheWarriorsExplorer
{
    public class Container
    {
        public class WadMetadata
        {
            public long fileOffset { get; set; }

            public long fileSize { get; set; }

            public uint lookupFileHash { get; set; }

            public uint fileHash { get; set; }

            public int wadIndex { get; set; }

            public string? fileName { get; set; } = "";

            public string? streamFileType { get; set; } = "UNK";

            public string? streamFileContentType { get; set; } = "Unknown.";
        }

        public static Dictionary<uint, WadMetadata> wadMetadataContainer = new Dictionary<uint, WadMetadata>();

        public static Enum.Platform platform { get; set; } = Enum.Platform.NONE;

        public FileStream? lookupFile { get; set; } = null;

        public FileStream? warriorsWAD_FileContainer { get; set; } = null;

        public BinaryReader? lookupFileBinaryReader { get; set; } = null;

        public BinaryReader? warriorsWAD_FileContainerBinaryReader { get; set; } = null;

        public string workingFilePath { get; set; } = "";

        public string lookupFilePath { get; set; } = "";

        public string containerFilePath { get; set; } = "";

        public Enum.Platform GetPlatform() => platform;

        public int fileCount { get; set; } = 0;

        public virtual List<KeyValuePair<uint, WadMetadata>> GetDictionaryData()
        {
            return new List<KeyValuePair<uint, WadMetadata>>(wadMetadataContainer);
        }

        public virtual bool ParseLookupFile(string filePath)
        {
            workingFilePath = filePath;

            return false;
        }

        public static WadMetadata? GetWadMetadata(uint hash)
        {
            return wadMetadataContainer.TryGetValue(hash, out var entry) ? entry : null;
        }

        public virtual void CloseFiles()
        {
            lookupFile?.Close();
            warriorsWAD_FileContainer?.Close();

            lookupFileBinaryReader?.Close();
            lookupFileBinaryReader?.Dispose();

            warriorsWAD_FileContainerBinaryReader?.Close();
            warriorsWAD_FileContainerBinaryReader?.Dispose();

            wadMetadataContainer.Clear();
        }

        public void PopulateHashes()
        {
            if (Form1.hashes == null)
            {
                return;
            }

            foreach (var entry in wadMetadataContainer.Values)
            {
                if (Form1.hashes.TryGetValue(entry.lookupFileHash, out var record1) && record1 != null)
                {
                    entry.fileName = record1.fileName;
                    entry.streamFileType = record1.streamFileType;

                    var thing = entry.streamFileType;

                    if (thing != null)
                    {
                        entry.streamFileContentType = GetThing(thing);
                    }
                }

                if (Form1.hashes.TryGetValue(entry.fileHash, out var record2) && record2 != null)
                {
                    entry.fileName = record2.fileName;
                    entry.streamFileType = record2.streamFileType;

                    var thing = entry.streamFileType;

                    if (thing != null)
                    {
                        entry.streamFileContentType = GetThing(thing);
                    }
                }
            }
        }

        public void ModifyContainer(FileStream stream, long offset, long bytes, bool expand)
        {
            byte[] buffer = new byte[4096];
            long length = stream.Length;

            if (expand)
            {
                stream.SetLength(length + bytes);

                long position = length;
                int toRead;

                while (position > offset)
                {
                    toRead = position - 4096 >= offset ? 4096 : (int)(position - offset);
                    position -= toRead;

                    stream.Position = position;
                    stream.Read(buffer, 0, toRead);
                    stream.Position = position + bytes;
                    stream.Write(buffer, 0, toRead);
                }
            }
            else
            {
                stream.SetLength(length - bytes);

                long position = offset;
                int toRead;

                while (position < length)
                {
                    toRead = (int)Math.Min(buffer.Length, length - position);

                    stream.Position = position;
                    stream.Read(buffer, 0, toRead);
                    stream.Position = position - bytes;
                    stream.Write(buffer, 0, toRead);

                    position += toRead;
                }
            }
        }

        public long CalculateMultipleOf(long value, long multiple)
        {
            long remainder = value % multiple;
            return remainder == 0 ? value : value + (multiple - remainder);
        }

        public void Replace(Container.WadMetadata wadMetadata, string replacementFilePath)
        {
            if (wadMetadata == null)
            {
                MessageBox.Show(String.Format("Failed to replace. Empty WAD metadata.").PadRight(40, ' '), programTitle);
                return;
            }

            if (string.IsNullOrEmpty(replacementFilePath) || string.IsNullOrEmpty(containerFilePath) || string.IsNullOrEmpty(lookupFilePath))
            {
                MessageBox.Show(String.Format("No file names specified.").PadRight(40, ' '), programTitle);
                return;
            }

            if (warriorsWAD_FileContainer == null || lookupFile == null)
            {
                return;
            }

            long replacementFileSize = 0;
            long bytesToDelete = 0;
            long insertionPoint = wadMetadata.fileOffset;

            if (File.Exists(replacementFilePath))
            {
                replacementFileSize = new FileInfo(replacementFilePath).Length;
            }

            bytesToDelete = CalculateMultipleOf(wadMetadata.fileSize, 2048);

            byte[] fileToInsertBytes = File.ReadAllBytes(replacementFilePath).PadToMultipleOf(2048);

            long difference = bytesToDelete - fileToInsertBytes.Length;

            if ((bytesToDelete - fileToInsertBytes.Length) > 0)
            {
                ModifyContainer(warriorsWAD_FileContainer, insertionPoint + difference, difference, true);
            }
            else
            {
                ModifyContainer(warriorsWAD_FileContainer, insertionPoint, difference, false);
            }

            warriorsWAD_FileContainer.Seek(insertionPoint, SeekOrigin.Begin);
            warriorsWAD_FileContainer.Write(fileToInsertBytes, 0, fileToInsertBytes.Length);

            if (difference != 0)
            {
                List<WadMetadata> metadataList = wadMetadataContainer.Values.ToList();

                metadataList.Sort((a, b) => a.wadIndex.CompareTo(b.wadIndex));

                for (int i = wadMetadata.wadIndex + 1; i < metadataList.Count; i++)
                {
                    metadataList[i].fileOffset += difference;
                }

                metadataList[wadMetadata.wadIndex].fileSize = replacementFileSize;

                using (BinaryWriter writer = new BinaryWriter(lookupFile))
                {
                    writer.Seek(16 + (wadMetadata.wadIndex * 12), SeekOrigin.Begin);

                    for (int i = wadMetadata.wadIndex; i < metadataList.Count; i++)
                    {
                        writer.Write((UInt32)metadataList[i].fileOffset);
                        writer.Write((UInt32)metadataList[i].fileSize);
                        writer.Write(metadataList[i].fileHash);
                    }
                }
            }

            MessageBox.Show("Replace complete.");
        }

        public string GetThing(string thing)
        {
            switch (thing.Trim())
            {
                case "ANM":
                    return "Single animation file?";
                case "ANM PAK":
                    return "Multiple animation container file?";
                case "ATO":
                    return "Atomic file???";
                case "BMP":
                    return "Bitmap file";
                case "DFF":
                    return "Single model file";
                case "DXT3":
                    return "Single texture file";
                case "LEV":
                    return "Level (collision/sky/cloud/shadow) file";
                case "LUA":
                    return "Compiled LUA script file";
                case "MDL":
                    return "Single model file";
                case "MEM":
                    return "Memory file";
                case "PAK":
                    return "Mixture of ANM/TXD/DFF files";
                case "SCN":
                    return "Scene file";
                case "SEC":
                    return "Level sector file";
                case "TXD":
                    return "Single texture file";
                case "TXT":
                    return "Item placement file";
                case "UNK":
                    return "Unknown file";
                case "WLD":
                    return "Level world file";
                case "XB":
                    return "xB file. XBox???";

            }

            return "";
        }
    }
}