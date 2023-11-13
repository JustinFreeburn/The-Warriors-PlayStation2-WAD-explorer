using System.Globalization;
using System.Reflection;

namespace TheWarriorsExplorer
{
    public partial class Form1 : Form
    {
        public class HashEntry
        {
            public string? streamFileType { get; set; } = "UNK";

            public string? fileName { get; set; } = "";
        }

        public static String programTitle { get; set; } = "The Warriors PS2 WAD Explorer";

        public Container? container { get; set; } = null;

        public static Dictionary<UInt32, HashEntry>? hashes { get; set; } = null;

        public Form1()
        {
            InitializeComponent();
            this.Text = container == null ? programTitle : String.Format("{0} - {1}", programTitle, container.containerFilePath);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            hashes = new Dictionary<UInt32, HashEntry>();

            LoadHashes(new List<string>
            {
                { "TheWarriorsExplorer.Hashes.PS2.ANM.txt" },
                { "TheWarriorsExplorer.Hashes.PS2.ANM PAK.txt" },
                { "TheWarriorsExplorer.Hashes.PS2.ATO.txt" },
                { "TheWarriorsExplorer.Hashes.PS2.BMP.txt" },
                { "TheWarriorsExplorer.Hashes.PS2.CNK.txt" },
                { "TheWarriorsExplorer.Hashes.PS2.DFF.txt" },
                { "TheWarriorsExplorer.Hashes.PS2.LEV.txt" },
                { "TheWarriorsExplorer.Hashes.PS2.LUA.txt" },
                { "TheWarriorsExplorer.Hashes.PS2.MEM.txt" },
                { "TheWarriorsExplorer.Hashes.PS2.PAK.txt" },
                { "TheWarriorsExplorer.Hashes.PS2.SCN.txt" },
                { "TheWarriorsExplorer.Hashes.PS2.SEC.txt" },
                { "TheWarriorsExplorer.Hashes.PS2.TXD.txt" },
                { "TheWarriorsExplorer.Hashes.PS2.TXT.txt" },
                { "TheWarriorsExplorer.Hashes.PS2.UNK.txt" },
                { "TheWarriorsExplorer.Hashes.PS2.WLD.txt" },
                { "TheWarriorsExplorer.Hashes.PS2.XB.txt" }
            }, hashes);
        }

        private void openWADToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFiles(Enum.Platform.PS2);
        }

        private void closeContainerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseFiles();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseFiles();
            this.Close();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void extractAllFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (container == null || container.warriorsWAD_FileContainer == null)
            {
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Are you sure you want to extract all of the files in the WAD?\n\nThis may take some time to complete.", "Extract all files...", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                FolderBrowserDialog FolderDialog = new FolderBrowserDialog();

                if (FolderDialog.ShowDialog() == DialogResult.OK)
                {
                    extractAllContainerFiles(FolderDialog.SelectedPath);
                }
            }
        }

        private void extractAllContainerFiles(string path)
        {
            if (container == null)
            {
                MessageBox.Show(String.Format("No open container.").PadRight(40, ' '), programTitle);
                return;
            }

            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show(String.Format("No path specified.").PadRight(40, ' '), programTitle);
                return;
            }

            foreach (var containerData in container.GetDictionaryData())
            {
                if (containerData.Value.fileName != null && containerData.Value.streamFileType != null)
                {
                    string filename = containerData.Value.fileName.Trim().ToLower();

                    if (string.IsNullOrEmpty(filename))
                    {

                        filename = String.Format("{0:X8}", containerData.Value.fileHash);
                    }

                    if (string.IsNullOrEmpty(filename))
                    {
                        MessageBox.Show(String.Format("Failed to generate a file name. Aborting extract.").PadRight(40, ' '), programTitle);
                        return;
                    }

                    filename += "." + containerData.Value.streamFileType.ToLower();

                    using (FileStream destinationFileStream = File.OpenWrite(path + "\\" + filename))
                    {
                        if (container.warriorsWAD_FileContainer != null)
                        {
                            byte[] buffer = new byte[containerData.Value.fileSize];
                            container.warriorsWAD_FileContainer.Seek(containerData.Value.fileOffset, SeekOrigin.Begin);
                            destinationFileStream.Write(buffer, 0, container.warriorsWAD_FileContainer.Read(buffer, 0, buffer.Length));
                        }
                    }
                }
            }
        }

        private void extractCurrentListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (container == null || container.warriorsWAD_FileContainer == null)
            {
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Are you sure you want to extract all of the filtered files in the WAD?\n\nThis may take some time to complete.", "Extract all files...", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                FolderBrowserDialog FolderDialog = new FolderBrowserDialog();

                if (FolderDialog.ShowDialog() == DialogResult.OK)
                {
                    extractCurrentListContainerFiles(FolderDialog.SelectedPath);
                }
            }
        }

        private void extractCurrentListContainerFiles(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show(String.Format("No path specified.").PadRight(40, ' '), programTitle);
                return;
            }

            uint result;

            foreach (ListViewItem item in listView1.Items)
            {
                string filename = item.SubItems[2].Text.Trim().ToLower();

                if (string.IsNullOrEmpty(filename))
                {
                    filename = item.SubItems[1].Text.Trim();
                }

                if (string.IsNullOrEmpty(filename))
                {
                    MessageBox.Show(String.Format("Failed to generate a file name. Aborting extract.").PadRight(40, ' '), programTitle);
                    return;
                }

                filename += "." + item.SubItems[5].Text.ToLower();

                if (UInt32.TryParse(item.SubItems[1].Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result))
                {
                    Container.WadMetadata? wadMetadata = TheWarriorsExplorer.Container.GetWadMetadata(result);

                    if (wadMetadata != null)
                    {
                        using (FileStream destinationFileStream = File.OpenWrite(path + "\\" + filename))
                        {
                            if (container?.warriorsWAD_FileContainer != null)
                            {
                                byte[] buffer = new byte[wadMetadata.fileSize];
                                container.warriorsWAD_FileContainer.Seek(wadMetadata.fileOffset, SeekOrigin.Begin);
                                destinationFileStream.Write(buffer, 0, container.warriorsWAD_FileContainer.Read(buffer, 0, buffer.Length));
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(String.Format("Failed to get WAD metadata for " + filename).PadRight(40, ' '), programTitle);
                    }
                }
                else
                {
                    MessageBox.Show(String.Format("Failed to extract " + filename).PadRight(40, ' '), programTitle);
                }
            }
        }

        private void extractSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (container == null || container.warriorsWAD_FileContainer == null)
            {
                return;
            }

            if (listView1.SelectedItems.Count < 1)
            {
                MessageBox.Show(String.Format("Nothing selected!").PadRight(40, ' '), programTitle);
                return;
            }

            FolderBrowserDialog FolderDialog = new FolderBrowserDialog();

            if (FolderDialog.ShowDialog() == DialogResult.OK)
            {
                extractSelectedContainerFiles(FolderDialog.SelectedPath);
            }
        }

        private void extractSelectedContainerFiles(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show(String.Format("No path specified.").PadRight(40, ' '), programTitle);
                return;
            }

            uint result;

            foreach (ListViewItem item in listView1.SelectedItems)
            {
                string filename = item.SubItems[2].Text.Trim().ToLower();

                if (string.IsNullOrEmpty(filename))
                {
                    filename = item.SubItems[1].Text.Trim();
                }

                if (string.IsNullOrEmpty(filename))
                {
                    MessageBox.Show(String.Format("Failed to generate a file name. Aborting extract.").PadRight(40, ' '), programTitle);
                    return;
                }

                filename += "." + item.SubItems[5].Text.ToLower();

                if (UInt32.TryParse(item.SubItems[1].Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result))
                {
                    Container.WadMetadata? wadMetadata = TheWarriorsExplorer.Container.GetWadMetadata(result);

                    if (wadMetadata != null)
                    {
                        using (FileStream destinationFileStream = File.OpenWrite(path + "\\" + filename))
                        {
                            if (container?.warriorsWAD_FileContainer != null)
                            {
                                byte[] buffer = new byte[wadMetadata.fileSize];
                                container.warriorsWAD_FileContainer.Seek(wadMetadata.fileOffset, SeekOrigin.Begin);
                                destinationFileStream.Write(buffer, 0, container.warriorsWAD_FileContainer.Read(buffer, 0, buffer.Length));
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(String.Format("Failed to get WAD metadata for " + filename).PadRight(40, ' '), programTitle);
                    }
                }
                else
                {
                    MessageBox.Show(String.Format("Failed to extract " + filename).PadRight(40, ' '), programTitle);
                }
            }
        }

        private void sortByToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (container == null || container.warriorsWAD_FileContainer == null)
            {
                return;
            }

            if (listView1.SelectedItems.Count < 1)
            {
                MessageBox.Show(String.Format("Nothing selected.").PadRight(40, ' '), programTitle);
            }
            else if (listView1.SelectedItems.Count == 1)
            {
                ListViewItem item = listView1.SelectedItems[0];
                uint result;
                string selectedFilePath;
                string filename = item.SubItems[2].Text.Trim().ToLower();

                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Title = "Open File",
                    Filter = "All files (*.*)|*.*",
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedFilePath = openFileDialog.FileName;

                    if (string.IsNullOrEmpty(selectedFilePath))
                    {
                        MessageBox.Show(String.Format("No replacement file selected.").PadRight(40, ' '), programTitle);
                        return;
                    }

                    if (string.IsNullOrEmpty(filename))
                    {
                        filename = item.SubItems[1].Text.Trim();
                    }

                    if (string.IsNullOrEmpty(filename))
                    {
                        MessageBox.Show(String.Format("Failed to generate a file name. Aborting replace.").PadRight(40, ' '), programTitle);
                        return;
                    }

                    if (UInt32.TryParse(item.SubItems[1].Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result))
                    {
                        Container.WadMetadata? wadMetadata = TheWarriorsExplorer.Container.GetWadMetadata(result);

                        if (wadMetadata != null)
                        {
                            container.Replace(wadMetadata, selectedFilePath);
                        }
                        else
                        {
                            MessageBox.Show(String.Format("Failed to get WAD metadata for " + filename).PadRight(40, ' '), programTitle);
                            return;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(String.Format("You can only replace one item at a time.").PadRight(40, ' '), programTitle);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(programTitle +
                "\nVersion 0.03 BETA" +
                "\n12.09.23" +
                "\n\nSpecial thanks to herbert3000 for all of your support." +
                "\nSpecial thanks to atlas for the PCSX2 memory address." +
                "\n\nby thewarriorsmp." +
                "\nEmail: thewarriorsmp@gmail.com" +
                "\n\nRenderWare is a registered trademark of Canon Inc.", programTitle);
        }

        private void listView1_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
            e.NewWidth = listView1.Columns[e.ColumnIndex].Width;
        }

        public static void LoadHashes(List<string> hashFileNameList, Dictionary<UInt32, HashEntry> hashes)
        {
            if (hashFileNameList == null || hashes == null)
            {
                return;
            }

            foreach (var hashFileName in hashFileNameList.Where(hash => !string.IsNullOrEmpty(hash)))
            {
                using Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(hashFileName);

                if (stream == null)
                {
                    continue;
                }

                using StreamReader reader = new StreamReader(stream);

                while (!reader.EndOfStream)
                {
                    string? line = reader.ReadLine();

                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }

                    string[] values = line.Split(',');

                    if (values.Length == 3 && UInt32.TryParse(values[0], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint hashKey))
                    {
                        if (!hashes.ContainsKey(hashKey))
                        {
                            HashEntry entry = new HashEntry
                            {
                                streamFileType = values[1],
                                fileName = values[2]
                            };

                            hashes.Add(hashKey, entry);
                        }
                    }
                }
            }
        }

        public void OpenFiles(Enum.Platform platform)
        {
            string filter;
            string expectedFileName;
            string expectedExtension;

            switch (platform)
            {
                case Enum.Platform.PS2:
                    {
                        filter = "PS2 directory file (*.dir)|*.dir";
                        expectedFileName = "WARRIORS";
                        expectedExtension = ".DIR";
                    }
                    break;
                default:
                    {
                        return;
                    }
            }

            if (container != null && MessageBox.Show("A container is already open. Do you want to close it?", Form1.programTitle, MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }

            CloseFiles();

            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Title = "Select a container file to open...",
                Filter = filter
            };

            if (fileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string? path = Path.GetDirectoryName(fileDialog.FileName);
            string fileName = Path.GetFileNameWithoutExtension(fileDialog.FileName);
            string fileExtension = Path.GetExtension(fileDialog.FileName);

            if (path == null)
            {
                return;
            }

            if (!fileName.ToUpper().Equals(expectedFileName) || !fileExtension.ToUpper().Equals(expectedExtension))
            {
                MessageBox.Show("Expected to find " + expectedFileName + expectedExtension, Form1.programTitle);
                return;
            }

            container = platform switch
            {
                Enum.Platform.PS2 => new PS2(),
                _ => null
            };

            if (container == null || !container.ParseLookupFile(path))
            {
                container = null;
                return;
            }

            EnableFormControls();
            SetupFormControls();
            PopulateFormControls();

            this.Text = container == null ? programTitle : String.Format("{0} - {1}", programTitle, container.containerFilePath);
        }

        public void CloseFiles()
        {
            CloseContainer();
            DisableAndCleanupFormControls();
            this.Text = programTitle;
        }

        private void CloseContainer()
        {
            if (container != null)
            {
                container.CloseFiles();
                container = null;
            }
        }

        private void DisableAndCleanupFormControls()
        {
            DisableFormControls();
            CleanupFormControls();
        }

        public void EnableFormControls()
        {
            closeWADToolStripMenuItem.Enabled =
          /*optionsToolStripMenuItem.Enabled =*/
            extractAllFilesToolStripMenuItem.Enabled =
            extractCurrentListToolStripMenuItem.Enabled =
            extractSelectedToolStripMenuItem.Enabled = true;

            if (container != null)
            {
                replaceToolStripMenuItem.Enabled = true;
            }
        }

        public void DisableFormControls()
        {
            closeWADToolStripMenuItem.Enabled =
          /*optionsToolStripMenuItem.Enabled =*/
            extractAllFilesToolStripMenuItem.Enabled =
            extractCurrentListToolStripMenuItem.Enabled =
            extractSelectedToolStripMenuItem.Enabled =
            replaceToolStripMenuItem.Enabled = false;
        }

        public void SetupFormControls()
        {
            ColumnHeader columnHeader1 = new ColumnHeader { Text = "Entry", Width = 55 };
            ColumnHeader columnHeader2 = new ColumnHeader { Text = "File Hash", Width = 90 };
            ColumnHeader columnHeader3 = new ColumnHeader { Text = "File Name", Width = 225 };
            ColumnHeader columnHeader4 = new ColumnHeader { Text = "File Size", Width = 120 };
            ColumnHeader columnHeader5 = new ColumnHeader { Text = "File Offset", Width = 100 };
            ColumnHeader columnHeader6 = new ColumnHeader { Text = "File Type", Width = 125 };
            ColumnHeader columnHeader7 = new ColumnHeader { Text = "Content Type", Width = 300 };

            listView1.Columns.Clear();

            if (container != null)
            {
                var columnsToAdd = container.GetPlatform() switch
                {
                    Enum.Platform.PS2 => new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4, columnHeader5, columnHeader6, columnHeader7 },
                    _ => null
                };

                if (columnsToAdd != null)
                {
                    listView1.Columns.AddRange(columnsToAdd);
                    listView1.Columns[listView1.Columns.Count - 1].Width = -2;
                }
            }
        }

        public void CleanupFormControls()
        {
            listView1.Items.Clear();
            listView1.Columns.Clear();
        }

        public void PopulateFormControls()
        {
            if (container != null)
            {
                listView1.BeginUpdate();

                foreach (var entry in container.GetDictionaryData())
                {
                    ListViewItem? ListViewItem = null;

                    ListViewItem = new ListViewItem(entry.Value.wadIndex.ToString());

                    ListViewItem.Tag = entry.Value.wadIndex;
                    ListViewItem.SubItems.Add(String.Format("{0:X8}", entry.Value.fileHash));
                    ListViewItem.SubItems.Add(entry.Value.fileName);
                    ListViewItem.SubItems.Add(entry.Value.fileSize.ToString() + " bytes");
                    ListViewItem.SubItems.Add(entry.Value.fileOffset.ToString());
                    ListViewItem.SubItems.Add(entry.Value.streamFileType);
                    ListViewItem.SubItems.Add(entry.Value.streamFileContentType);

                    if (ListViewItem != null)
                    {
                        listView1.Items.Add(ListViewItem);
                    }
                }

                listView1.EndUpdate();
            }
        }
    }
}