namespace TheWarriorsExplorer
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openWADToolStripMenuItem = new ToolStripMenuItem();
            closeWADToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            toolsToolStripMenuItem = new ToolStripMenuItem();
            optionsToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            extractAllFilesToolStripMenuItem = new ToolStripMenuItem();
            extractCurrentListToolStripMenuItem = new ToolStripMenuItem();
            extractSelectedToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            replaceToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            listView1 = new ListView();
            statusStrip1 = new StatusStrip();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, toolsToolStripMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1084, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openWADToolStripMenuItem, closeWADToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // openWADToolStripMenuItem
            // 
            openWADToolStripMenuItem.Name = "openWADToolStripMenuItem";
            openWADToolStripMenuItem.Size = new Size(165, 22);
            openWADToolStripMenuItem.Text = "Open container...";
            openWADToolStripMenuItem.Click += openWADToolStripMenuItem_Click;
            // 
            // closeWADToolStripMenuItem
            // 
            closeWADToolStripMenuItem.Enabled = false;
            closeWADToolStripMenuItem.Name = "closeWADToolStripMenuItem";
            closeWADToolStripMenuItem.Size = new Size(165, 22);
            closeWADToolStripMenuItem.Text = "Close container";
            closeWADToolStripMenuItem.Click += closeContainerToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(162, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(165, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { optionsToolStripMenuItem, toolStripSeparator2, extractAllFilesToolStripMenuItem, extractCurrentListToolStripMenuItem, extractSelectedToolStripMenuItem, toolStripSeparator3, replaceToolStripMenuItem });
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new Size(46, 20);
            toolsToolStripMenuItem.Text = "Tools";
            // 
            // optionsToolStripMenuItem
            // 
            optionsToolStripMenuItem.Enabled = false;
            optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            optionsToolStripMenuItem.Size = new Size(180, 22);
            optionsToolStripMenuItem.Text = "Options...";
            optionsToolStripMenuItem.Click += optionsToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(177, 6);
            // 
            // extractAllFilesToolStripMenuItem
            // 
            extractAllFilesToolStripMenuItem.Enabled = false;
            extractAllFilesToolStripMenuItem.Name = "extractAllFilesToolStripMenuItem";
            extractAllFilesToolStripMenuItem.Size = new Size(180, 22);
            extractAllFilesToolStripMenuItem.Text = "Extract all files";
            extractAllFilesToolStripMenuItem.Click += extractAllFilesToolStripMenuItem_Click;
            // 
            // extractCurrentListToolStripMenuItem
            // 
            extractCurrentListToolStripMenuItem.Enabled = false;
            extractCurrentListToolStripMenuItem.Name = "extractCurrentListToolStripMenuItem";
            extractCurrentListToolStripMenuItem.Size = new Size(180, 22);
            extractCurrentListToolStripMenuItem.Text = "Extract current list";
            extractCurrentListToolStripMenuItem.Click += extractCurrentListToolStripMenuItem_Click;
            // 
            // extractSelectedToolStripMenuItem
            // 
            extractSelectedToolStripMenuItem.Enabled = false;
            extractSelectedToolStripMenuItem.Name = "extractSelectedToolStripMenuItem";
            extractSelectedToolStripMenuItem.Size = new Size(180, 22);
            extractSelectedToolStripMenuItem.Text = "Extract selected";
            extractSelectedToolStripMenuItem.Click += extractSelectedToolStripMenuItem_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(177, 6);
            // 
            // replaceToolStripMenuItem
            // 
            replaceToolStripMenuItem.Enabled = false;
            replaceToolStripMenuItem.Name = "replaceToolStripMenuItem";
            replaceToolStripMenuItem.Size = new Size(180, 22);
            replaceToolStripMenuItem.Text = "Replace";
            replaceToolStripMenuItem.Click += replaceToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(44, 20);
            helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(116, 22);
            aboutToolStripMenuItem.Text = "About...";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // listView1
            // 
            listView1.Dock = DockStyle.Fill;
            listView1.Font = new Font("Lucida Console", 9.25F, FontStyle.Regular, GraphicsUnit.Point);
            listView1.FullRowSelect = true;
            listView1.Location = new Point(0, 24);
            listView1.Name = "listView1";
            listView1.Size = new Size(1084, 587);
            listView1.TabIndex = 1;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
            listView1.ColumnWidthChanging += listView1_ColumnWidthChanging;
            // 
            // statusStrip1
            // 
            statusStrip1.Location = new Point(0, 589);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1084, 22);
            statusStrip1.TabIndex = 2;
            statusStrip1.Text = "statusStrip1";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1084, 611);
            Controls.Add(statusStrip1);
            Controls.Add(listView1);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("Rockstar.Icon");
            MainMenuStrip = menuStrip1;
            MinimumSize = new Size(1100, 650);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "The Warriors Explorer";
            Load += Form1_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem closeWADToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem extractAllFilesToolStripMenuItem;
        private ToolStripMenuItem extractCurrentListToolStripMenuItem;
        private ToolStripMenuItem extractSelectedToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem replaceToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ListView listView1;
        private StatusStrip statusStrip1;
        private ToolStripMenuItem openWADToolStripMenuItem;
    }
}