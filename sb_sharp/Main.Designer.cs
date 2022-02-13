namespace sb_sharp
{
    partial class Main
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.pTabBar = new System.Windows.Forms.Panel();
            this.bNewTab = new System.Windows.Forms.Button();
            this.pWebBrowser = new System.Windows.Forms.Panel();
            this.bClose = new System.Windows.Forms.Button();
            this.bMinimise = new System.Windows.Forms.Button();
            this.bMaximise = new System.Windows.Forms.Button();
            this.bBack = new System.Windows.Forms.Button();
            this.bForward = new System.Windows.Forms.Button();
            this.bReload = new System.Windows.Forms.Button();
            this.eURL = new System.Windows.Forms.TextBox();
            this.pControls = new System.Windows.Forms.Panel();
            this.bGo = new System.Windows.Forms.Button();
            this.pHeader = new System.Windows.Forms.Panel();
            this.cmTab = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsCopyURL = new System.Windows.Forms.ToolStripMenuItem();
            this.pControls.SuspendLayout();
            this.pHeader.SuspendLayout();
            this.cmTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // pTabBar
            // 
            this.pTabBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pTabBar.BackColor = System.Drawing.Color.LightCoral;
            this.pTabBar.Location = new System.Drawing.Point(3, 3);
            this.pTabBar.Name = "pTabBar";
            this.pTabBar.Size = new System.Drawing.Size(1089, 30);
            this.pTabBar.TabIndex = 1;
            this.pTabBar.DoubleClick += new System.EventHandler(this.pTabBar_DoubleClick);
            this.pTabBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pTabBar_MouseMove);
            // 
            // bNewTab
            // 
            this.bNewTab.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bNewTab.BackColor = System.Drawing.Color.LightCoral;
            this.bNewTab.FlatAppearance.BorderSize = 0;
            this.bNewTab.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bNewTab.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.bNewTab.Image = ((System.Drawing.Image)(resources.GetObject("bNewTab.Image")));
            this.bNewTab.Location = new System.Drawing.Point(1092, 3);
            this.bNewTab.Name = "bNewTab";
            this.bNewTab.Size = new System.Drawing.Size(43, 30);
            this.bNewTab.TabIndex = 0;
            this.bNewTab.UseVisualStyleBackColor = false;
            this.bNewTab.Click += new System.EventHandler(this.bNewTab_Click);
            // 
            // pWebBrowser
            // 
            this.pWebBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pWebBrowser.Location = new System.Drawing.Point(1, 63);
            this.pWebBrowser.Name = "pWebBrowser";
            this.pWebBrowser.Size = new System.Drawing.Size(1261, 568);
            this.pWebBrowser.TabIndex = 2;
            // 
            // bClose
            // 
            this.bClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bClose.BackColor = System.Drawing.Color.LightCoral;
            this.bClose.FlatAppearance.BorderSize = 0;
            this.bClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bClose.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.bClose.Image = ((System.Drawing.Image)(resources.GetObject("bClose.Image")));
            this.bClose.Location = new System.Drawing.Point(1221, 3);
            this.bClose.Name = "bClose";
            this.bClose.Size = new System.Drawing.Size(43, 30);
            this.bClose.TabIndex = 3;
            this.bClose.UseVisualStyleBackColor = false;
            this.bClose.Click += new System.EventHandler(this.bClose_Click);
            // 
            // bMinimise
            // 
            this.bMinimise.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bMinimise.BackColor = System.Drawing.Color.LightCoral;
            this.bMinimise.FlatAppearance.BorderSize = 0;
            this.bMinimise.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bMinimise.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.bMinimise.Image = ((System.Drawing.Image)(resources.GetObject("bMinimise.Image")));
            this.bMinimise.Location = new System.Drawing.Point(1135, 3);
            this.bMinimise.Name = "bMinimise";
            this.bMinimise.Size = new System.Drawing.Size(43, 30);
            this.bMinimise.TabIndex = 4;
            this.bMinimise.UseVisualStyleBackColor = false;
            this.bMinimise.Click += new System.EventHandler(this.bMinimise_Click);
            // 
            // bMaximise
            // 
            this.bMaximise.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bMaximise.BackColor = System.Drawing.Color.LightCoral;
            this.bMaximise.FlatAppearance.BorderSize = 0;
            this.bMaximise.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bMaximise.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.bMaximise.Image = ((System.Drawing.Image)(resources.GetObject("bMaximise.Image")));
            this.bMaximise.Location = new System.Drawing.Point(1178, 3);
            this.bMaximise.Name = "bMaximise";
            this.bMaximise.Size = new System.Drawing.Size(48, 30);
            this.bMaximise.TabIndex = 5;
            this.bMaximise.UseVisualStyleBackColor = false;
            this.bMaximise.Click += new System.EventHandler(this.bMaximise_Click);
            // 
            // bBack
            // 
            this.bBack.BackColor = System.Drawing.Color.LightCoral;
            this.bBack.FlatAppearance.BorderSize = 0;
            this.bBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bBack.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.bBack.Image = ((System.Drawing.Image)(resources.GetObject("bBack.Image")));
            this.bBack.Location = new System.Drawing.Point(0, 0);
            this.bBack.Name = "bBack";
            this.bBack.Size = new System.Drawing.Size(43, 30);
            this.bBack.TabIndex = 6;
            this.bBack.UseVisualStyleBackColor = false;
            this.bBack.Click += new System.EventHandler(this.bBack_Click);
            // 
            // bForward
            // 
            this.bForward.BackColor = System.Drawing.Color.LightCoral;
            this.bForward.FlatAppearance.BorderSize = 0;
            this.bForward.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bForward.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.bForward.Image = ((System.Drawing.Image)(resources.GetObject("bForward.Image")));
            this.bForward.Location = new System.Drawing.Point(45, 0);
            this.bForward.Name = "bForward";
            this.bForward.Size = new System.Drawing.Size(43, 30);
            this.bForward.TabIndex = 7;
            this.bForward.UseVisualStyleBackColor = false;
            this.bForward.Click += new System.EventHandler(this.bForward_Click);
            // 
            // bReload
            // 
            this.bReload.BackColor = System.Drawing.Color.LightCoral;
            this.bReload.FlatAppearance.BorderSize = 0;
            this.bReload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bReload.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.bReload.Image = ((System.Drawing.Image)(resources.GetObject("bReload.Image")));
            this.bReload.Location = new System.Drawing.Point(92, 0);
            this.bReload.Name = "bReload";
            this.bReload.Size = new System.Drawing.Size(43, 30);
            this.bReload.TabIndex = 8;
            this.bReload.UseVisualStyleBackColor = false;
            this.bReload.Click += new System.EventHandler(this.bReload_Click);
            // 
            // eURL
            // 
            this.eURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.eURL.Location = new System.Drawing.Point(141, 4);
            this.eURL.Name = "eURL";
            this.eURL.Size = new System.Drawing.Size(1060, 23);
            this.eURL.TabIndex = 9;
            this.eURL.KeyDown += new System.Windows.Forms.KeyEventHandler(this.eURL_KeyDown);
            // 
            // pControls
            // 
            this.pControls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pControls.BackColor = System.Drawing.Color.LightCoral;
            this.pControls.Controls.Add(this.bGo);
            this.pControls.Controls.Add(this.eURL);
            this.pControls.Controls.Add(this.bReload);
            this.pControls.Controls.Add(this.bBack);
            this.pControls.Controls.Add(this.bForward);
            this.pControls.Location = new System.Drawing.Point(3, 33);
            this.pControls.Name = "pControls";
            this.pControls.Size = new System.Drawing.Size(1261, 30);
            this.pControls.TabIndex = 2;
            // 
            // bGo
            // 
            this.bGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bGo.BackColor = System.Drawing.Color.LightCoral;
            this.bGo.FlatAppearance.BorderSize = 0;
            this.bGo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bGo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.bGo.Image = ((System.Drawing.Image)(resources.GetObject("bGo.Image")));
            this.bGo.Location = new System.Drawing.Point(1207, 0);
            this.bGo.Name = "bGo";
            this.bGo.Size = new System.Drawing.Size(43, 30);
            this.bGo.TabIndex = 6;
            this.bGo.UseVisualStyleBackColor = false;
            this.bGo.Click += new System.EventHandler(this.bGo_Click);
            // 
            // pHeader
            // 
            this.pHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pHeader.Controls.Add(this.pControls);
            this.pHeader.Controls.Add(this.pTabBar);
            this.pHeader.Controls.Add(this.bMaximise);
            this.pHeader.Controls.Add(this.bNewTab);
            this.pHeader.Controls.Add(this.bMinimise);
            this.pHeader.Controls.Add(this.bClose);
            this.pHeader.Location = new System.Drawing.Point(1, -3);
            this.pHeader.Name = "pHeader";
            this.pHeader.Size = new System.Drawing.Size(1261, 65);
            this.pHeader.TabIndex = 6;
            // 
            // cmTab
            // 
            this.cmTab.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsCopyURL});
            this.cmTab.Name = "contextMenuStrip1";
            this.cmTab.Size = new System.Drawing.Size(127, 26);
            this.cmTab.Opened += new System.EventHandler(this.cmTab_Opened);
            // 
            // tsCopyURL
            // 
            this.tsCopyURL.Name = "tsCopyURL";
            this.tsCopyURL.Size = new System.Drawing.Size(126, 22);
            this.tsCopyURL.Text = "Copy URL";
            this.tsCopyURL.Click += new System.EventHandler(this.tsCopyURL_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 631);
            this.ControlBox = false;
            this.Controls.Add(this.pHeader);
            this.Controls.Add(this.pWebBrowser);
            this.KeyPreview = true;
            this.Name = "Main";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Main_KeyDown);
            this.Resize += new System.EventHandler(this.Main_Resize);
            this.pControls.ResumeLayout(false);
            this.pControls.PerformLayout();
            this.pHeader.ResumeLayout(false);
            this.cmTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Panel pTabBar;
        private Button bNewTab;
        private Panel pWebBrowser;
        private Button bClose;
        private Button bMinimise;
        private Button bMaximise;
        private Button bBack;
        private Button bForward;
        private Button bReload;
        private TextBox eURL;
        private Panel pControls;
        private Button bGo;
        private Panel pHeader;
        private ContextMenuStrip cmTab;
        private ToolStripMenuItem tsCopyURL;
    }
}