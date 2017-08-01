namespace SqlBulkCopySample
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnStart = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.gBSource = new System.Windows.Forms.GroupBox();
            this.SourceBox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSelectFilter = new System.Windows.Forms.TextBox();
            this.btoSourceTest = new System.Windows.Forms.Button();
            this.txtSourceScheme = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSourceTable = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblSource = new System.Windows.Forms.Label();
            this.gBDestination = new System.Windows.Forms.GroupBox();
            this.DestBox = new System.Windows.Forms.ComboBox();
            this.btoDestinationTest = new System.Windows.Forms.Button();
            this.txtDestinationTable = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDestination = new System.Windows.Forms.TextBox();
            this.gBGeneral = new System.Windows.Forms.GroupBox();
            this.chkHeap = new System.Windows.Forms.CheckBox();
            this.chkNoLock = new System.Windows.Forms.CheckBox();
            this.txtNotifyAfter = new System.Windows.Forms.TextBox();
            this.txtThreads = new System.Windows.Forms.TextBox();
            this.chkAppend = new System.Windows.Forms.CheckBox();
            this.chkKeepIdentity = new System.Windows.Forms.CheckBox();
            this.chkKeepNulls = new System.Windows.Forms.CheckBox();
            this.chkFireTriggers = new System.Windows.Forms.CheckBox();
            this.chkTablock = new System.Windows.Forms.CheckBox();
            this.chkCheckConstraints = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gBSource.SuspendLayout();
            this.gBDestination.SuspendLayout();
            this.gBGeneral.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(434, 463);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "Copy";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(6, 16);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(251, 160);
            this.listBox1.TabIndex = 0;
            // 
            // txtSource
            // 
            this.txtSource.Location = new System.Drawing.Point(156, 19);
            this.txtSource.Name = "txtSource";
            this.txtSource.Size = new System.Drawing.Size(341, 20);
            this.txtSource.TabIndex = 0;
            // 
            // gBSource
            // 
            this.gBSource.Controls.Add(this.SourceBox);
            this.gBSource.Controls.Add(this.label7);
            this.gBSource.Controls.Add(this.txtSelectFilter);
            this.gBSource.Controls.Add(this.btoSourceTest);
            this.gBSource.Controls.Add(this.txtSourceScheme);
            this.gBSource.Controls.Add(this.label2);
            this.gBSource.Controls.Add(this.txtSourceTable);
            this.gBSource.Controls.Add(this.label1);
            this.gBSource.Controls.Add(this.lblSource);
            this.gBSource.Controls.Add(this.txtSource);
            this.gBSource.Location = new System.Drawing.Point(12, 12);
            this.gBSource.Name = "gBSource";
            this.gBSource.Size = new System.Drawing.Size(503, 127);
            this.gBSource.TabIndex = 0;
            this.gBSource.TabStop = false;
            this.gBSource.Text = "Source Configuration";
            // 
            // SourceBox
            // 
            this.SourceBox.DisplayMember = "(none)";
            this.SourceBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SourceBox.FormattingEnabled = true;
            this.SourceBox.Items.AddRange(new object[] {
            "SQL Server",
            "Oracle"});
            this.SourceBox.Location = new System.Drawing.Point(310, 45);
            this.SourceBox.Name = "SourceBox";
            this.SourceBox.Size = new System.Drawing.Size(95, 21);
            this.SourceBox.TabIndex = 9;
            this.SourceBox.ValueMember = "SqlServer";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(29, 100);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(122, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "TSQL Select Filter Data:";
            // 
            // txtSelectFilter
            // 
            this.txtSelectFilter.Location = new System.Drawing.Point(156, 97);
            this.txtSelectFilter.Multiline = true;
            this.txtSelectFilter.Name = "txtSelectFilter";
            this.txtSelectFilter.Size = new System.Drawing.Size(341, 20);
            this.txtSelectFilter.TabIndex = 3;
            // 
            // btoSourceTest
            // 
            this.btoSourceTest.Location = new System.Drawing.Point(422, 45);
            this.btoSourceTest.Name = "btoSourceTest";
            this.btoSourceTest.Size = new System.Drawing.Size(75, 22);
            this.btoSourceTest.TabIndex = 4;
            this.btoSourceTest.Text = "Test";
            this.btoSourceTest.UseVisualStyleBackColor = true;
            this.btoSourceTest.Click += new System.EventHandler(this.btoSourceTest_Click);
            // 
            // txtSourceScheme
            // 
            this.txtSourceScheme.Location = new System.Drawing.Point(156, 71);
            this.txtSourceScheme.Name = "txtSourceScheme";
            this.txtSourceScheme.Size = new System.Drawing.Size(72, 20);
            this.txtSourceScheme.TabIndex = 2;
            this.txtSourceScheme.Text = "dbo";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Source Table Scheme:";
            // 
            // txtSourceTable
            // 
            this.txtSourceTable.Location = new System.Drawing.Point(156, 45);
            this.txtSourceTable.Name = "txtSourceTable";
            this.txtSourceTable.Size = new System.Drawing.Size(148, 20);
            this.txtSourceTable.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(46, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Source Table Name:";
            // 
            // lblSource
            // 
            this.lblSource.AutoSize = true;
            this.lblSource.Location = new System.Drawing.Point(20, 22);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(131, 13);
            this.lblSource.TabIndex = 5;
            this.lblSource.Text = "Source Connection String:";
            // 
            // gBDestination
            // 
            this.gBDestination.Controls.Add(this.DestBox);
            this.gBDestination.Controls.Add(this.btoDestinationTest);
            this.gBDestination.Controls.Add(this.txtDestinationTable);
            this.gBDestination.Controls.Add(this.label4);
            this.gBDestination.Controls.Add(this.label5);
            this.gBDestination.Controls.Add(this.txtDestination);
            this.gBDestination.Location = new System.Drawing.Point(12, 145);
            this.gBDestination.Name = "gBDestination";
            this.gBDestination.Size = new System.Drawing.Size(503, 78);
            this.gBDestination.TabIndex = 1;
            this.gBDestination.TabStop = false;
            this.gBDestination.Text = "Destination Configuration";
            // 
            // DestBox
            // 
            this.DestBox.DisplayMember = "(none)";
            this.DestBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DestBox.FormattingEnabled = true;
            this.DestBox.Items.AddRange(new object[] {
            "SQL Server",
            "Oracle"});
            this.DestBox.Location = new System.Drawing.Point(310, 44);
            this.DestBox.Name = "DestBox";
            this.DestBox.Size = new System.Drawing.Size(95, 21);
            this.DestBox.TabIndex = 5;
            // 
            // btoDestinationTest
            // 
            this.btoDestinationTest.Location = new System.Drawing.Point(422, 45);
            this.btoDestinationTest.Name = "btoDestinationTest";
            this.btoDestinationTest.Size = new System.Drawing.Size(75, 22);
            this.btoDestinationTest.TabIndex = 2;
            this.btoDestinationTest.Text = "Test";
            this.btoDestinationTest.UseVisualStyleBackColor = true;
            this.btoDestinationTest.Click += new System.EventHandler(this.btoDestinationTest_Click);
            // 
            // txtDestinationTable
            // 
            this.txtDestinationTable.Location = new System.Drawing.Point(156, 45);
            this.txtDestinationTable.Name = "txtDestinationTable";
            this.txtDestinationTable.Size = new System.Drawing.Size(148, 20);
            this.txtDestinationTable.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(124, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Destination Table Name:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(150, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Destination Connection String:";
            // 
            // txtDestination
            // 
            this.txtDestination.Location = new System.Drawing.Point(156, 19);
            this.txtDestination.Name = "txtDestination";
            this.txtDestination.Size = new System.Drawing.Size(341, 20);
            this.txtDestination.TabIndex = 0;
            // 
            // gBGeneral
            // 
            this.gBGeneral.Controls.Add(this.chkHeap);
            this.gBGeneral.Controls.Add(this.chkNoLock);
            this.gBGeneral.Controls.Add(this.txtNotifyAfter);
            this.gBGeneral.Controls.Add(this.txtThreads);
            this.gBGeneral.Controls.Add(this.chkAppend);
            this.gBGeneral.Controls.Add(this.chkKeepIdentity);
            this.gBGeneral.Controls.Add(this.chkKeepNulls);
            this.gBGeneral.Controls.Add(this.chkFireTriggers);
            this.gBGeneral.Controls.Add(this.chkTablock);
            this.gBGeneral.Controls.Add(this.chkCheckConstraints);
            this.gBGeneral.Controls.Add(this.label3);
            this.gBGeneral.Controls.Add(this.label6);
            this.gBGeneral.Location = new System.Drawing.Point(12, 229);
            this.gBGeneral.Name = "gBGeneral";
            this.gBGeneral.Size = new System.Drawing.Size(234, 257);
            this.gBGeneral.TabIndex = 2;
            this.gBGeneral.TabStop = false;
            this.gBGeneral.Text = "General Configuration";
            // 
            // chkHeap
            // 
            this.chkHeap.AutoSize = true;
            this.chkHeap.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkHeap.Location = new System.Drawing.Point(66, 230);
            this.chkHeap.Name = "chkHeap";
            this.chkHeap.Size = new System.Drawing.Size(105, 17);
            this.chkHeap.TabIndex = 11;
            this.chkHeap.Text = "HEAP METHOD";
            this.chkHeap.UseVisualStyleBackColor = true;
            // 
            // chkNoLock
            // 
            this.chkNoLock.AutoSize = true;
            this.chkNoLock.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkNoLock.Location = new System.Drawing.Point(31, 207);
            this.chkNoLock.Name = "chkNoLock";
            this.chkNoLock.Size = new System.Drawing.Size(140, 17);
            this.chkNoLock.TabIndex = 10;
            this.chkNoLock.Text = "READ UNCOMMITTED";
            this.chkNoLock.UseVisualStyleBackColor = true;
            // 
            // txtNotifyAfter
            // 
            this.txtNotifyAfter.Location = new System.Drawing.Point(156, 45);
            this.txtNotifyAfter.MaxLength = 10;
            this.txtNotifyAfter.Name = "txtNotifyAfter";
            this.txtNotifyAfter.Size = new System.Drawing.Size(72, 20);
            this.txtNotifyAfter.TabIndex = 1;
            // 
            // txtThreads
            // 
            this.txtThreads.Location = new System.Drawing.Point(156, 22);
            this.txtThreads.MaxLength = 2;
            this.txtThreads.Name = "txtThreads";
            this.txtThreads.Size = new System.Drawing.Size(38, 20);
            this.txtThreads.TabIndex = 0;
            // 
            // chkAppend
            // 
            this.chkAppend.AutoSize = true;
            this.chkAppend.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkAppend.Location = new System.Drawing.Point(79, 71);
            this.chkAppend.Name = "chkAppend";
            this.chkAppend.Size = new System.Drawing.Size(92, 17);
            this.chkAppend.TabIndex = 2;
            this.chkAppend.Text = "Append Data:";
            this.chkAppend.UseVisualStyleBackColor = true;
            // 
            // chkKeepIdentity
            // 
            this.chkKeepIdentity.AutoSize = true;
            this.chkKeepIdentity.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkKeepIdentity.Location = new System.Drawing.Point(58, 184);
            this.chkKeepIdentity.Name = "chkKeepIdentity";
            this.chkKeepIdentity.Size = new System.Drawing.Size(113, 17);
            this.chkKeepIdentity.TabIndex = 7;
            this.chkKeepIdentity.Text = "KEEP_IDENTITY:";
            this.chkKeepIdentity.UseVisualStyleBackColor = true;
            // 
            // chkKeepNulls
            // 
            this.chkKeepNulls.AutoSize = true;
            this.chkKeepNulls.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkKeepNulls.Location = new System.Drawing.Point(73, 161);
            this.chkKeepNulls.Name = "chkKeepNulls";
            this.chkKeepNulls.Size = new System.Drawing.Size(98, 17);
            this.chkKeepNulls.TabIndex = 6;
            this.chkKeepNulls.Text = "KEEP_NULLS:";
            this.chkKeepNulls.UseVisualStyleBackColor = true;
            // 
            // chkFireTriggers
            // 
            this.chkFireTriggers.AutoSize = true;
            this.chkFireTriggers.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkFireTriggers.Location = new System.Drawing.Point(56, 115);
            this.chkFireTriggers.Name = "chkFireTriggers";
            this.chkFireTriggers.Size = new System.Drawing.Size(115, 17);
            this.chkFireTriggers.TabIndex = 4;
            this.chkFireTriggers.Text = "FIRE_TRIGGERS:";
            this.chkFireTriggers.UseVisualStyleBackColor = true;
            // 
            // chkTablock
            // 
            this.chkTablock.AutoSize = true;
            this.chkTablock.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkTablock.Checked = true;
            this.chkTablock.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTablock.Location = new System.Drawing.Point(93, 92);
            this.chkTablock.Name = "chkTablock";
            this.chkTablock.Size = new System.Drawing.Size(78, 17);
            this.chkTablock.TabIndex = 3;
            this.chkTablock.Text = "TABLOCK:";
            this.chkTablock.UseVisualStyleBackColor = true;
            // 
            // chkCheckConstraints
            // 
            this.chkCheckConstraints.AutoSize = true;
            this.chkCheckConstraints.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkCheckConstraints.Location = new System.Drawing.Point(23, 138);
            this.chkCheckConstraints.Name = "chkCheckConstraints";
            this.chkCheckConstraints.Size = new System.Drawing.Size(148, 17);
            this.chkCheckConstraints.TabIndex = 5;
            this.chkCheckConstraints.Text = "CHECK_CONSTRAINTS:";
            this.chkCheckConstraints.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Rows copied after notify:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(50, 25);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Number of Threads:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBox1);
            this.groupBox1.Location = new System.Drawing.Point(252, 229);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(263, 189);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Progress";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 496);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.gBGeneral);
            this.Controls.Add(this.gBDestination);
            this.Controls.Add(this.gBSource);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SqlBulkCopy";
            this.gBSource.ResumeLayout(false);
            this.gBSource.PerformLayout();
            this.gBDestination.ResumeLayout(false);
            this.gBDestination.PerformLayout();
            this.gBGeneral.ResumeLayout(false);
            this.gBGeneral.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.GroupBox gBSource;
        private System.Windows.Forms.Label lblSource;
        private System.Windows.Forms.TextBox txtSourceTable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btoSourceTest;
        private System.Windows.Forms.TextBox txtSourceScheme;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox gBDestination;
        private System.Windows.Forms.Button btoDestinationTest;
        private System.Windows.Forms.TextBox txtDestinationTable;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtDestination;
        private System.Windows.Forms.GroupBox gBGeneral;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkKeepIdentity;
        private System.Windows.Forms.CheckBox chkKeepNulls;
        private System.Windows.Forms.CheckBox chkFireTriggers;
        private System.Windows.Forms.CheckBox chkTablock;
        private System.Windows.Forms.CheckBox chkCheckConstraints;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSelectFilter;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkAppend;
        private System.Windows.Forms.TextBox txtNotifyAfter;
        private System.Windows.Forms.TextBox txtThreads;
        private System.Windows.Forms.CheckBox chkNoLock;
        private System.Windows.Forms.CheckBox chkHeap;
        private System.Windows.Forms.ComboBox SourceBox;
        private System.Windows.Forms.ComboBox DestBox;
    }
}

