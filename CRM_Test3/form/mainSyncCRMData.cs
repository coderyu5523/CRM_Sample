using Quartz;
using Quartz.Impl;
using SyncCRMData.SyncMonitoring;
using SyncScheduleManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRM_Test3
{
    public partial class mainSyncCRMData : Form

    {
        private IScheduler scheduler; // 스케줄러 객체
        private ScheduleStats scheduleStats; // 스케줄 상태 객체

        // 자식폼 번호 초기화
        private int childFormNumber = 0;

        public mainSyncCRMData()
        {
            InitializeComponent();
            scheduleStats = new ScheduleStats(); // 스케줄 상태 객체 생성'

            this.Load += Main_Sync_CRMData_Load;

            string baseDirectory = @"C:\Sync_CRMData\config";
            string baseDirectory1 = @"C:\Sync_CRMData\Logs";

            // 폴더 존재하지 않으면 생성
            if (!Directory.Exists(baseDirectory)) 
            {
                Directory.CreateDirectory(baseDirectory);
                Console.WriteLine($"디렉토리를 생성했습니다: {baseDirectory}");
            }
            if (!Directory.Exists(baseDirectory1))
            {
                Directory.CreateDirectory(baseDirectory1);
                Console.WriteLine($"디렉토리를 생성했습니다: {baseDirectory1}");
            }
        }

        // 폼이 로드될 때 스케줄러를 초기화 및 시작
        private async void Main_Sync_CRMData_Load(object sender, EventArgs e)
        {
            await StartScheduler(scheduleStats); // 스케줄러 초기화 및 시작
        }

        // 스케줄러를 시작하는 메서드
        public async Task StartScheduler(ScheduleStats stats)
        { 
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            scheduler = await schedulerFactory.GetScheduler();

            await scheduler.Start();
        }


        // 스케줄러 객체를 자식 폼에서 참조하도록 getter 제공
        public IScheduler GetScheduler()
        {
            return scheduler;
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "창 " + childFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "텍스트 파일 (*.txt)|*.txt|모든 파일 (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "텍스트 파일 (*.txt)|*.txt|모든 파일 (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        // CRM 스케줄 실행
        private void cRMDataSyncToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 자식 폼이 이미 열려있는지 확인
            foreach (Form frm in this.MdiChildren)
            {
                if (frm is frmSyncCRMData)
                {
                    frm.Activate(); // 이미 열려있으면 활성화
                    return;
                }
            }
          
            frmSyncCRMData sync_CRMData = new frmSyncCRMData(this);
            sync_CRMData.MdiParent = this; // MDI 부모 설정
            sync_CRMData.Show();
        }

        private void dataSync모니터링ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 자식 폼이 이미 열려있는지 확인
         //   foreach (Form frm in this.MdiChildren)
         //   {
         //       if (frm is frmSyncMonitoring)
         //       {
         //           frm.Activate(); // 이미 열려있으면 활성화
         //           return;
         //       }
         //   }
         //   if (scheduler == null || !scheduler.IsStarted)
         //   {
         //       MessageBox.Show("스케줄러가 실행 중이지 않습니다.");
         //       return;
         //   }
         //   // Schedule_Stats 객체를 생성하거나 가져오기
         //   //Schedule_Stats scheduleStats = GetScheduleStats();
         //   frmSyncMonitoring syncMonitorying = new frmSyncMonitoring(this);
         //   syncMonitorying.MdiParent = this; // MDI 부모 설정
         //   syncMonitorying.Show();
        }

        private void sync스케줄설정ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 자식 폼이 이미 열려있는지 확인
            foreach (Form frm in this.MdiChildren)
            {
                if (frm is frmScheduleForm)
                {
                    frm.Activate(); // 이미 열려있으면 활성화
                    return;
                }
            }
            frmScheduleForm scheduleForm = new frmScheduleForm();
            scheduleForm.MdiParent = this; // MDI 부모 설정
            scheduleForm.Show();

        }

        private void dataSync이력확인ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 자식 폼이 이미 열려있는지 확인
        //    foreach (Form frm in this.MdiChildren)
        //    {
        //        if (frm is frmSyncMonitoring_history)
        //        {
        //            frm.Activate(); // 이미 열려있으면 활성화
        //            return;
        //        }
        //    }
        //    frmSyncMonitoring_history syncMonitoring_History = new frmSyncMonitoring_history();
        //    syncMonitoring_History.MdiParent = this; // MDI 부모 설정
        //    syncMonitoring_History.Show();
        }
    }

}

