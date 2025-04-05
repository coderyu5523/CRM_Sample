using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CRM_Test3
{
    public partial class Main_Sync_CRMData : Form

    {
        // 자식폼 번호 초기화
        private int childFormNumber = 0;

        public Main_Sync_CRMData()
        {
            InitializeComponent();
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
                if (frm is frmSync_CRMData)
                {
                    frm.Activate(); // 이미 열려있으면 활성화
                    return;
                }
            }
          
            frmSync_CRMData sync_CRMData = new frmSync_CRMData(this);
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
           // foreach (Form frm in this.MdiChildren)
           // {
           //     if (frm is frmScheduleForm)
           //     {
           //         frm.Activate(); // 이미 열려있으면 활성화
           //         return;
           //     }
           // }
           // frmScheduleForm scheduleForm = new frmScheduleForm();
           // scheduleForm.MdiParent = this; // MDI 부모 설정
           // scheduleForm.Show();

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

