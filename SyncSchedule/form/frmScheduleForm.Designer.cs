using System.Windows.Forms;

namespace SyncScheduleManager
{
    partial class frmScheduleForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvTasks = new System.Windows.Forms.DataGridView();
            this.TaskId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaskName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SourceDB = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.TargetDB = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ReferenceTables = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProcedureName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SyncDirection = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.IsActive = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSave1 = new System.Windows.Forms.Button();
            this.pnl_re = new System.Windows.Forms.Panel();
            this.lblInterval1 = new System.Windows.Forms.Label();
            this.numInterval1 = new System.Windows.Forms.NumericUpDown();
            this.lblTaskId = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cboScheduleType1 = new System.Windows.Forms.ComboBox();
            this.dtsrtdt = new System.Windows.Forms.DateTimePicker();
            this.pnl_one = new System.Windows.Forms.Panel();
            this.lblSpecificTime1 = new System.Windows.Forms.Label();
            this.dtpSpecificTime1 = new System.Windows.Forms.DateTimePicker();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSaveTasks = new System.Windows.Forms.Button();
            this.cboSourceDB = new System.Windows.Forms.ComboBox();
            this.btnLoadTasks = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTasks)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.pnl_re.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInterval1)).BeginInit();
            this.pnl_one.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(1387, 599);
            this.splitContainer1.SplitterDistance = 1101;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvTasks);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Size = new System.Drawing.Size(1101, 599);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sync Task List";
            // 
            // dgvTasks
            // 
            this.dgvTasks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTasks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TaskId,
            this.TaskName,
            this.SourceDB,
            this.TargetDB,
            this.ReferenceTables,
            this.ProcedureName,
            this.SyncDirection,
            this.IsActive});
            this.dgvTasks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTasks.Location = new System.Drawing.Point(3, 22);
            this.dgvTasks.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvTasks.Name = "dgvTasks";
            this.dgvTasks.RowHeadersWidth = 51;
            this.dgvTasks.RowTemplate.Height = 23;
            this.dgvTasks.Size = new System.Drawing.Size(1095, 573);
            this.dgvTasks.TabIndex = 0;
            this.dgvTasks.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTasks_CellClick);
            this.dgvTasks.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgvTasks_DefaultValuesNeeded);
            this.dgvTasks.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTasks_RowEnter);
            this.dgvTasks.SelectionChanged += new System.EventHandler(this.dgvTasks_SelectionChanged_1);
            // 
            // TaskId
            // 
            this.TaskId.HeaderText = "TaskId";
            this.TaskId.MinimumWidth = 6;
            this.TaskId.Name = "TaskId";
            this.TaskId.Width = 50;
            // 
            // TaskName
            // 
            this.TaskName.HeaderText = "작업명칭";
            this.TaskName.MinimumWidth = 6;
            this.TaskName.Name = "TaskName";
            this.TaskName.Width = 200;
            // 
            // SourceDB
            // 
            this.SourceDB.HeaderText = "원본DB";
            this.SourceDB.Items.AddRange(new object[] {
            "CRM",
            "KR"});
            this.SourceDB.MinimumWidth = 6;
            this.SourceDB.Name = "SourceDB";
            this.SourceDB.Width = 60;
            // 
            // TargetDB
            // 
            this.TargetDB.HeaderText = "대상 DB";
            this.TargetDB.Items.AddRange(new object[] {
            "CRM",
            "KR"});
            this.TargetDB.MinimumWidth = 6;
            this.TargetDB.Name = "TargetDB";
            this.TargetDB.Width = 60;
            // 
            // ReferenceTables
            // 
            this.ReferenceTables.HeaderText = "참조 테이블";
            this.ReferenceTables.MinimumWidth = 6;
            this.ReferenceTables.Name = "ReferenceTables";
            this.ReferenceTables.Width = 125;
            // 
            // ProcedureName
            // 
            this.ProcedureName.HeaderText = "프로시저명";
            this.ProcedureName.MinimumWidth = 6;
            this.ProcedureName.Name = "ProcedureName";
            this.ProcedureName.Width = 150;
            // 
            // SyncDirection
            // 
            this.SyncDirection.HeaderText = "동기화 방향";
            this.SyncDirection.Items.AddRange(new object[] {
            "CTE",
            "ETC"});
            this.SyncDirection.MinimumWidth = 6;
            this.SyncDirection.Name = "SyncDirection";
            this.SyncDirection.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.SyncDirection.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.SyncDirection.Width = 120;
            // 
            // IsActive
            // 
            this.IsActive.HeaderText = "활성화 여부";
            this.IsActive.MinimumWidth = 6;
            this.IsActive.Name = "IsActive";
            this.IsActive.Width = 80;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnSave1);
            this.groupBox1.Controls.Add(this.pnl_re);
            this.groupBox1.Controls.Add(this.lblTaskId);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cboScheduleType1);
            this.groupBox1.Controls.Add(this.dtsrtdt);
            this.groupBox1.Controls.Add(this.pnl_one);
            this.groupBox1.Location = new System.Drawing.Point(17, 18);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(225, 404);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "동기화 일정";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "선택 TaskId";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(141, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(22, 15);
            this.label3.TabIndex = 11;
            this.label3.Text = "번";
            // 
            // btnSave1
            // 
            this.btnSave1.Location = new System.Drawing.Point(22, 311);
            this.btnSave1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave1.Name = "btnSave1";
            this.btnSave1.Size = new System.Drawing.Size(166, 45);
            this.btnSave1.TabIndex = 0;
            this.btnSave1.Text = "일정 저장";
            this.btnSave1.UseVisualStyleBackColor = true;
            this.btnSave1.Click += new System.EventHandler(this.btnSave1_Click);
            // 
            // pnl_re
            // 
            this.pnl_re.Controls.Add(this.lblInterval1);
            this.pnl_re.Controls.Add(this.numInterval1);
            this.pnl_re.Location = new System.Drawing.Point(19, 186);
            this.pnl_re.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnl_re.Name = "pnl_re";
            this.pnl_re.Size = new System.Drawing.Size(166, 88);
            this.pnl_re.TabIndex = 10;
            // 
            // lblInterval1
            // 
            this.lblInterval1.AutoSize = true;
            this.lblInterval1.Location = new System.Drawing.Point(3, 19);
            this.lblInterval1.Name = "lblInterval1";
            this.lblInterval1.Size = new System.Drawing.Size(119, 15);
            this.lblInterval1.TabIndex = 6;
            this.lblInterval1.Text = "되풀이 (분 단위)";
            // 
            // numInterval1
            // 
            this.numInterval1.Location = new System.Drawing.Point(7, 51);
            this.numInterval1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.numInterval1.Maximum = new decimal(new int[] {
            1440,
            0,
            0,
            0});
            this.numInterval1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numInterval1.Name = "numInterval1";
            this.numInterval1.Size = new System.Drawing.Size(138, 25);
            this.numInterval1.TabIndex = 5;
            this.numInterval1.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // lblTaskId
            // 
            this.lblTaskId.AutoSize = true;
            this.lblTaskId.Location = new System.Drawing.Point(118, 40);
            this.lblTaskId.Name = "lblTaskId";
            this.lblTaskId.Size = new System.Drawing.Size(23, 15);
            this.lblTaskId.TabIndex = 2;
            this.lblTaskId.Text = "00";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 15);
            this.label2.TabIndex = 9;
            this.label2.Text = "시작 날짜";
            // 
            // cboScheduleType1
            // 
            this.cboScheduleType1.FormattingEnabled = true;
            this.cboScheduleType1.Items.AddRange(new object[] {
            "되풀이 수행",
            "일별 수행"});
            this.cboScheduleType1.Location = new System.Drawing.Point(26, 71);
            this.cboScheduleType1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboScheduleType1.Name = "cboScheduleType1";
            this.cboScheduleType1.Size = new System.Drawing.Size(138, 23);
            this.cboScheduleType1.TabIndex = 3;
            this.cboScheduleType1.SelectedIndexChanged += new System.EventHandler(this.cboScheduleType1_SelectedIndexChanged);
            // 
            // dtsrtdt
            // 
            this.dtsrtdt.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtsrtdt.Location = new System.Drawing.Point(25, 140);
            this.dtsrtdt.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtsrtdt.Name = "dtsrtdt";
            this.dtsrtdt.ShowUpDown = true;
            this.dtsrtdt.Size = new System.Drawing.Size(138, 25);
            this.dtsrtdt.TabIndex = 8;
            // 
            // pnl_one
            // 
            this.pnl_one.Controls.Add(this.lblSpecificTime1);
            this.pnl_one.Controls.Add(this.dtpSpecificTime1);
            this.pnl_one.Location = new System.Drawing.Point(19, 186);
            this.pnl_one.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnl_one.Name = "pnl_one";
            this.pnl_one.Size = new System.Drawing.Size(166, 88);
            this.pnl_one.TabIndex = 7;
            // 
            // lblSpecificTime1
            // 
            this.lblSpecificTime1.AutoSize = true;
            this.lblSpecificTime1.Location = new System.Drawing.Point(3, 18);
            this.lblSpecificTime1.Name = "lblSpecificTime1";
            this.lblSpecificTime1.Size = new System.Drawing.Size(77, 15);
            this.lblSpecificTime1.TabIndex = 6;
            this.lblSpecificTime1.Text = "한 번 수행";
            // 
            // dtpSpecificTime1
            // 
            this.dtpSpecificTime1.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpSpecificTime1.Location = new System.Drawing.Point(2, 52);
            this.dtpSpecificTime1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpSpecificTime1.Name = "dtpSpecificTime1";
            this.dtpSpecificTime1.ShowUpDown = true;
            this.dtpSpecificTime1.Size = new System.Drawing.Size(138, 25);
            this.dtpSpecificTime1.TabIndex = 4;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.label4);
            this.splitContainer2.Panel1.Controls.Add(this.btnSaveTasks);
            this.splitContainer2.Panel1.Controls.Add(this.cboSourceDB);
            this.splitContainer2.Panel1.Controls.Add(this.btnLoadTasks);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.statusStrip1);
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer1);
            this.splitContainer2.Size = new System.Drawing.Size(1387, 682);
            this.splitContainer2.SplitterDistance = 78;
            this.splitContainer2.SplitterWidth = 5;
            this.splitContainer2.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 15);
            this.label4.TabIndex = 4;
            this.label4.Text = "원본DB";
            // 
            // btnSaveTasks
            // 
            this.btnSaveTasks.Location = new System.Drawing.Point(359, 15);
            this.btnSaveTasks.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSaveTasks.Name = "btnSaveTasks";
            this.btnSaveTasks.Size = new System.Drawing.Size(103, 49);
            this.btnSaveTasks.TabIndex = 3;
            this.btnSaveTasks.Text = "저장";
            this.btnSaveTasks.UseVisualStyleBackColor = true;
            this.btnSaveTasks.Click += new System.EventHandler(this.btnSaveTasks_Click);
            // 
            // cboSourceDB
            // 
            this.cboSourceDB.FormattingEnabled = true;
            this.cboSourceDB.Items.AddRange(new object[] {
            "CRM",
            "KR"});
            this.cboSourceDB.Location = new System.Drawing.Point(82, 28);
            this.cboSourceDB.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboSourceDB.Name = "cboSourceDB";
            this.cboSourceDB.Size = new System.Drawing.Size(138, 23);
            this.cboSourceDB.TabIndex = 2;
            // 
            // btnLoadTasks
            // 
            this.btnLoadTasks.Location = new System.Drawing.Point(246, 14);
            this.btnLoadTasks.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnLoadTasks.Name = "btnLoadTasks";
            this.btnLoadTasks.Size = new System.Drawing.Size(106, 50);
            this.btnLoadTasks.TabIndex = 1;
            this.btnLoadTasks.Text = "조회";
            this.btnLoadTasks.UseVisualStyleBackColor = true;
            this.btnLoadTasks.Click += new System.EventHandler(this.btnLoadTasks_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 573);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1387, 26);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(152, 20);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // frmScheduleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1387, 682);
            this.Controls.Add(this.splitContainer2);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmScheduleForm";
            this.Text = "SyncSchedule Manager";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTasks)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnl_re.ResumeLayout(false);
            this.pnl_re.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInterval1)).EndInit();
            this.pnl_one.ResumeLayout(false);
            this.pnl_one.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnSave1;
        private System.Windows.Forms.DataGridView dgvTasks;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button btnLoadTasks;
        private System.Windows.Forms.ComboBox cboSourceDB;
        private System.Windows.Forms.Button btnSaveTasks;
        private System.Windows.Forms.Label lblTaskId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboScheduleType1;
        private System.Windows.Forms.DateTimePicker dtpSpecificTime1;
        private Label lblInterval1;
        private NumericUpDown numInterval1;
        private Panel pnl_re;
        private Label label2;
        private DateTimePicker dtsrtdt;
        private Panel pnl_one;
        private Label lblSpecificTime1;
        private Label label3;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private DataGridViewTextBoxColumn TaskId;
        private DataGridViewTextBoxColumn TaskName;
        private DataGridViewComboBoxColumn SourceDB;
        private DataGridViewComboBoxColumn TargetDB;
        private DataGridViewTextBoxColumn ReferenceTables;
        private DataGridViewTextBoxColumn ProcedureName;
        private DataGridViewComboBoxColumn SyncDirection;
        private DataGridViewCheckBoxColumn IsActive;
        private Label label4;
    }
}