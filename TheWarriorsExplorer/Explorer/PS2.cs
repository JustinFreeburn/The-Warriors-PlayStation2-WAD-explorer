namespace TheWarriorsExplorer
{
    public class PS2 : Container
    {
        public PS2()
        {
            wadMetadataContainer = new Dictionary<uint, WadMetadata>();
        }

        public override bool ParseLookupFile(string filePath)
        {
            workingFilePath = filePath;

            try
            {
                lookupFilePath = Path.Combine(workingFilePath.ToUpper(), "WARRIORS.DIR");
                containerFilePath = Path.Combine(workingFilePath.ToUpper(), "WARRIORS.WAD");

                lookupFile = new FileStream(lookupFilePath, FileMode.Open, FileAccess.ReadWrite);
                warriorsWAD_FileContainer = new FileStream(containerFilePath, FileMode.Open, FileAccess.ReadWrite);
            }
            catch (IOException error)
            {
                lookupFilePath = "";
                containerFilePath = "";

                MessageBox.Show(error.Message, Form1.programTitle);
                return false;
            }

            if (lookupFile == null || warriorsWAD_FileContainer == null)
            {
                lookupFilePath = "";
                containerFilePath = "";

                return false;
            }

            lookupFileBinaryReader = new BinaryReader(lookupFile);

            fileCount = lookupFileBinaryReader.ReadInt32();

            if (((lookupFile.Length - 16) / fileCount) != 12)
            {
                lookupFilePath = "";
                containerFilePath = "";

                MessageBox.Show("Not a valid PS2 Warriors DIR!", Form1.programTitle);

                return false;
            }

            platform = Enum.Platform.PS2;

            lookupFile.Seek(12, SeekOrigin.Current);

            warriorsWAD_FileContainerBinaryReader = new BinaryReader(warriorsWAD_FileContainer);

            int wadIndexCounter = 0;

            for (int iterator = 0; iterator < fileCount; iterator++)
            {
                WadMetadata entry = new WadMetadata();

                entry.fileOffset = lookupFileBinaryReader.ReadUInt32();
                entry.fileSize = lookupFileBinaryReader.ReadUInt32();
                entry.fileHash = lookupFileBinaryReader.ReadUInt32();
                entry.wadIndex = wadIndexCounter;
                entry.lookupFileHash = entry.fileHash;

                if (!wadMetadataContainer.TryAdd(entry.fileHash, entry))
                {
                    MessageBox.Show(string.Format("0x{0:X8} is a duplicate...", entry.fileHash), Form1.programTitle);
                }
                else
                {
                    wadIndexCounter++;
                }
            }

            PopulateHashes();

            return true;
        }
    }
}
