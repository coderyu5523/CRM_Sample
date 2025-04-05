using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRM_Test3
{
    public partial class frmSync_CRMData : Form
    {
        private Main_Sync_CRMData mdiParentForm; // MDI 부모 폼 참조
        public frmSync_CRMData(Main_Sync_CRMData parentForm)
        {
            InitializeComponent();
            // MDI 자식 폼을 최대화 상태로 설정
            this.WindowState = FormWindowState.Maximized;
            mdiParentForm = parentForm; // 부모 폼 저장
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Start_Sync_CRMData();
        }
        private async void Start_Sync_CRMData()
        {
            // 부모 폼을 통해 스케줄러 실행
      //     IScheduler scheduler = mdiParentForm.GetScheduler();
      //
      //     if (scheduler == null)
      //     {
      //         MessageBox.Show("스케줄러가 실행 중이지 않습니다.");
      //         return;
      //     }
      //     // 스케줄링 로직 호출
      //     await StartScheduling_Add(scheduler);

            // UI 피드백 (상태 표시)
            toolStripStatusLabel1.Text = "스케줄러 시작됨";
        }



        private async void btn_Task_Exec_Click(object sender, EventArgs e)
        {
            int taskId;
            if (int.TryParse(txt_TaskID.Text, out taskId))
            {
              //  IScheduler scheduler = mdiParentForm.GetScheduler();
              //  await TriggerSpecificTaskAsync(scheduler, taskId);
            }
            else
            {
                MessageBox.Show("유효한 Task ID를 입력하세요.");
            }
        }
    }

}
