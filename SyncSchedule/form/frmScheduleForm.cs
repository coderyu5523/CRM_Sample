using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SyncScheduleManager
{
    public partial class frmScheduleForm : Form
    {
        //public frmScheduleForm()
        //{
        //    InitializeComponent();
        //}
        private ComboBox cboScheduleType;
        private DateTimePicker dtpSpecificTime;
        private NumericUpDown numInterval;
        private CheckedListBox clbWeekDays;
        private Button btnSave, btnLoad;

        public frmScheduleForm()
        {
            InitializeComponent();

            this.dgvTasks.AutoGenerateColumns = false; // 자동으로 컬럼 생성 안 함
            this.dgvTasks.AllowUserToAddRows = true; // 사용자가 직접 행을 추가할 수 있음
            this.dgvTasks.AllowUserToDeleteRows = true; // 사용자가 직접 행을 삭제할 수 있음


            pnl_one.Visible = false;
            pnl_re.Visible = false;


            //SetControlVisibility(false, false, false); // 초기에는 모두 숨김
            LoadTasks("Init");
        }



        private void SetControlVisibility1(bool showSpecificTime, bool showInterval, bool showWeekDays)
        {
            pnl_one.Visible = showSpecificTime;
            pnl_re.Visible = showInterval;

        }
        // 설정 저장 버튼 클릭 시 처리
        private void BtnSave_Click(object sender, EventArgs e)
        {

        }

        // 스케줄 불러오기 버튼 클릭 시 처리
        private void LoadSchedule(int taskId)
        {
          //  SyncSchedule loadedSchedule = ScheduleFileManager.LoadSchedule(taskId);
          //  if (loadedSchedule != null)
          //  {
          //      // 불러온 스케줄 데이터를 폼에 적용
          //      cboScheduleType1.SelectedItem = GetScheduleTypeDisplayName(loadedSchedule.ScheduleType);
          //      if (loadedSchedule.ScheduleType == "OneTime" || loadedSchedule.ScheduleType == "Daily")
          //      {
          //          dtpSpecificTime1.Value = loadedSchedule.SpecificTime.Value;
          //      }
          //      else if (loadedSchedule.ScheduleType == "Recurring")
          //      {
          //          numInterval1.Value = (decimal)loadedSchedule.Interval.Value.TotalMinutes;
          //      }
          //      else if (loadedSchedule.ScheduleType == "Weekly")
          //      {
          //          clbWeekDays.SetItemChecked((int)loadedSchedule.WeekDay.Value, true);
          //          dtpSpecificTime.Value = loadedSchedule.SpecificTime.Value;
          //      }
          //
          //      //MessageBox.Show("Task-"+taskId.ToString()+"번 조회가 완료되었습니다.");
          //      toolStripStatusLabel1.Text = "Task-" + taskId.ToString() + "번 조회가 완료되었습니다.";
          //  }
          //  else
          //  {
          //      MessageBox.Show("저장된 TaskId가 없습니다.");
          //      cboScheduleType1.SelectedIndex = -1;
          //      dtpSpecificTime1.Value = DateTime.Now;
          //      numInterval1.Value = 60;
          //      pnl_one.Visible = false;
          //      pnl_re.Visible = false;
          //  }
        }
        // 스케줄 타입에 대한 표시 이름 반환
        private string GetScheduleTypeDisplayName(string scheduleType)
        {
            switch (scheduleType)
            {
                case "OneTime": return "한 번 수행";
                case "Recurring": return "되풀이 수행";
                case "Daily": return "일별 수행";
                case "Weekly": return "주별 수행";
                default: return string.Empty;
            }
        }
        private List<SyncTask> tasks = new List<SyncTask>();
        private int nextTaskId = 1; // TaskID 자동 증가용 변수
        private void LoadTasks(string ty)
        {
            // 선택된 원본 DB 값 가져오기

         //   if (cboSourceDB.SelectedItem == null)
         //   {
         //       tasks = TaskFileManager.LoadTasks();
         //
         //   }
         //   else
         //   {
         //       string selectedSourceDB = cboSourceDB.SelectedItem.ToString();
         //       // JSON 파일에서 작업 목록을 불러와 선택된 원본 DB에 해당하는 작업들만 필터링
         //       tasks = TaskFileManager.LoadTasks(selectedSourceDB);
         //   }
         //
         //
         //   dgvTasks.Rows.Clear(); // 기존 데이터 지우기
         //
         //   // 필터링된 작업 목록을 DataGridView에 표시
         //   foreach (var task in tasks)
         //   {
         //       dgvTasks.Rows.Add(task.TaskId, task.TaskName, task.SourceDB,              // 원본 DB
         //       task.TargetDB, string.Join(",", task.ReferenceTables), task.ProcedureName, task.SyncDirection, task.IsActive);
         //   }
         //
         //   // 불러온 Task 중 가장 큰 TaskID를 찾아서 nextTaskId 설정
         //   if (tasks.Count > 0)
         //   {
         //       nextTaskId = tasks.Max(t => t.TaskId) + 1; // 가장 큰 TaskID에 1을 더해 nextTaskId로 설정
         //   }
         //   else
         //   {
         //       nextTaskId = 1; // 작업이 없으면 1부터 시작
         //   }
         //   if (ty == "Q")
         //   {
         //       //MessageBox.Show($"{tasks.Count}개의 작업이 불러와졌습니다.");
         //       toolStripStatusLabel1.Text = $"{tasks.Count}개의 작업이 조회되었습니다.";
         //   }

        }
        private void btnLoadTasks_Click(object sender, EventArgs e)
        {
            LoadTasks("Q");
        }

        private void dgvTasks_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {

            // 파일에 저장된 TaskID 중 가장 큰 값을 확인
         //   int maxStoredTaskId = GetMaxStoredTaskId();
         //
         //   // 2. 현재 그리드에 있는 TaskID 중 가장 큰 값 확인
         //   int maxTaskIdInGrid = maxStoredTaskId; // 기본값은 파일에 저장된 최대 TaskID
         //   if (maxStoredTaskId > 0)
         //   {
         //       foreach (DataGridViewRow row in dgvTasks.Rows)
         //       {
         //           if (row.IsNewRow) continue; // 새 행은 무시
         //
         //           // TaskID가 있는 경우 확인
         //           if (row.Cells[0].Value != null && int.TryParse(row.Cells[0].Value.ToString(), out int taskId))
         //           {
         //               if (taskId > maxTaskIdInGrid)
         //               {
         //                   maxTaskIdInGrid = taskId; // 현재 가장 큰 TaskID 업데이트
         //               }
         //           }
         //       }
         //   }
         //   // 새로운 행에 저장된 TaskID보다 1 큰 값을 할당
         //   e.Row.Cells[0].Value = maxTaskIdInGrid + 1;
         //
         //   // nextTaskId 값을 업데이트
         //   nextTaskId = maxTaskIdInGrid + 2; // 다음 TaskID로 설정
        }
        // 파일에 저장된 TaskID 중 가장 큰 값을 반환하는 메서드
        //private int GetMaxStoredTaskId()
        //{
        //    int maxTaskId = 0;
        //    var storedTasks = TaskFileManager.LoadTasks();
        //    if (storedTasks.Count == 0)
        //    {
        //        return 0;
        //    }
        //    else
        //    {
        //        maxTaskId = storedTasks.Max(t => t.TaskId); // 가장 큰 TaskID 찾기 
        //   
        //        return maxTaskId; // 가장 큰 TaskID 반환 (없으면 0 반환)
        //    }
        //}

        private void btnSaveTasks_Click(object sender, EventArgs e)
        {
        //    tasks.Clear(); // 기존 리스트 비우기
        //   
        //    // DataGridView의 모든 행을 List<SyncTask>로 변환
        //    foreach (DataGridViewRow row in dgvTasks.Rows)
        //    {
        //        if (row.IsNewRow) continue; // 새 행은 무시
        //   
        //   
        //        // 필수값 체크 (각 셀에 값이 있는지 확인)
        //        if (row.Cells[0].Value == null || row.Cells[1].Value == null ||
        //            row.Cells[2].Value == null || row.Cells[3].Value == null ||
        //            row.Cells[4].Value == null || row.Cells[5].Value == null ||
        //            row.Cells[6].Value == null)
        //        {
        //            MessageBox.Show("모든 필수값을 입력하세요.", "필수값 누락", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return; // 필수값이 누락되면 저장 중단
        //        }
        //   
        //        SyncTask task = new SyncTask
        //        {
        //            TaskId = Convert.ToInt32(row.Cells[0].Value), // TaskId
        //            TaskName = row.Cells[1].Value?.ToString(), // TaskName
        //            ReferenceTables = new List<string>(row.Cells[4].Value?.ToString().Split(',')), // 참조 테이블 목록
        //            ProcedureName = row.Cells[5].Value?.ToString(), // ProcedureName                    
        //            SourceDB = row.Cells[2].Value?.ToString(), // 원본 DB
        //            TargetDB = row.Cells[3].Value?.ToString(), // 원본 DB
        //            SyncDirection = row.Cells[6].Value?.ToString(), // 동기화 방향
        //            IsActive = Convert.ToBoolean(row.Cells[7].Value?.ToString())
        //        };
        //   
        //        tasks.Add(task); // 리스트에 추가
        //    }
        //   
        //    // List<SyncTask>를 파일에 저장
        //    TaskFileManager.SaveTasks(tasks);
        //    //MessageBox.Show("작업이 저장되었습니다.");
        //    toolStripStatusLabel1.Text = $"작업이 저장되었습니다.";
        }



        private void SaveSchedule(SyncSchedule schedule)
        {
        
            ScheduleFileManager.SaveSchedule(schedule);
            // 스케줄 데이터를 저장하는 로직 (파일 또는 데이터베이스)
            //MessageBox.Show($"Task 스케줄이 저장되었습니다: {schedule.ScheduleType}");
            toolStripStatusLabel1.Text = $"Task 스케줄이 저장되었습니다: {schedule.TaskId} 번 ";
        }
        
        private void btnSave1_Click(object sender, EventArgs e)
        {
        
            string selectedType = cboScheduleType1.SelectedItem.ToString();
            SyncSchedule schedule = new SyncSchedule();
            schedule.TaskId = int.Parse(lblTaskId.Text);
            if (schedule.TaskId <= 0)
            {
                MessageBox.Show("TaskId가 선택되지 않았습니다. 다시 확인 후 저장하세요.");
                return;
            }
            schedule.SrtDate = dtsrtdt.Value.ToShortDateString();
            switch (selectedType)
            {
                //case "한 번 수행":
                //    schedule.ScheduleType = "OneTime";
                //    schedule.SpecificTime = dtpSpecificTime.Value;
                //    break;
                case "되풀이 수행":
                    schedule.ScheduleType = "Recurring";
                    schedule.Interval = TimeSpan.FromMinutes((double)numInterval1.Value);
                    break;
                case "일별 수행":
                    schedule.ScheduleType = "Daily";
                    schedule.SpecificTime = dtpSpecificTime1.Value;
                    break;
                    //case "주별 수행":
                    //    schedule.ScheduleType = "Weekly";
                    //    schedule.WeekDay = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), clbWeekDays.CheckedItems[0].ToString()); // 하나의 요일만 선택한다고 가정
                    //    schedule.SpecificTime = dtpSpecificTime.Value;
                    //    break;
            }
        
            // schedule 데이터를 파일이나 DB에 저장하는 로직 추가
            SaveSchedule(schedule);
        }

        private void dgvTasks_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTasks.SelectedRows.Count > 0) // 선택된 행이 있는지 확인
            {
                // 선택된 행의 첫 번째 행의 TaskId 값 가져오기
                DataGridViewRow selectedRow = dgvTasks.SelectedRows[0];

                // TaskId 값이 있는지 확인하고, lblTaskID.Text에 할당
                if (selectedRow.Cells[0].Value != null)
                {
                    lblTaskId.Text = selectedRow.Cells[0].Value.ToString(); // TaskId를 Label에 표시
                    LoadSchedule(int.Parse(lblTaskId.Text));
                }
            }
            else
            {
                lblTaskId.Text = "";
            }
        }

        private void dgvTasks_SelectionChanged_1(object sender, EventArgs e)
        {
            //if (dgvTasks.SelectedRows.Count > 0) // 선택된 행이 있는지 확인
            //{
            //    // 선택된 행의 첫 번째 행의 TaskId 값 가져오기
            //    DataGridViewRow selectedRow = dgvTasks.SelectedRows[0];

            //    // TaskId 값이 있는지 확인하고, lblTaskID.Text에 할당
            //    if (selectedRow.Cells[0].Value != null)
            //    {
            //        lblTaskId.Text = selectedRow.Cells[0].Value.ToString(); // TaskId를 Label에 표시
            //        LoadSchedule(int.Parse(lblTaskId.Text));
            //    }
            //}
            //else
            //{
            //    lblTaskId.Text = "";
            //}
        }

        private void dgvTasks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // 클릭된 셀이 유효한지 확인 (행 인덱스가 0 이상인 경우)
            if (e.RowIndex >= 0 && e.ColumnIndex <= 1)
            {
                dgvrow_change(e.RowIndex);
            }
            else
            {
                lblTaskId.Text = "";
            }
        }
        private void dgvrow_change(int rowindex)
        {
            // 클릭된 행의 TaskId 값 가져오기
            DataGridViewRow selectedRow = dgvTasks.Rows[rowindex];

            // TaskId 값이 있는지 확인하고, lblTaskID.Text에 할당
            if (selectedRow.Cells[0].Value != null)
            {
                lblTaskId.Text = selectedRow.Cells[0].Value.ToString(); // TaskId를 Label에 표시
                LoadSchedule(int.Parse(lblTaskId.Text));
            }
        }
        private void dgvTasks_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            // 클릭된 셀이 유효한지 확인 (행 인덱스가 0 이상인 경우)
            if (e.RowIndex >= 0 && e.ColumnIndex <= 1)
            {
                dgvrow_change(e.RowIndex);
            }
            else
            {
                lblTaskId.Text = "";
            }
        }

        private void cboScheduleType1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboScheduleType1.SelectedItem == null)
            {
                return;
            }
            string selectedType = cboScheduleType1.SelectedItem.ToString();

            switch (selectedType)
            {
                //case "한 번 수행":
                //    SetControlVisibility(true, false, false);
                //    break;
                case "되풀이 수행":
                    SetControlVisibility1(false, true, false);
                    break;
                case "일별 수행":
                    SetControlVisibility1(true, false, false);
                    break;
                //case "주별 수행":
                //    SetControlVisibility(false, false, true);
                //    break;
            }
        }
    }
}
