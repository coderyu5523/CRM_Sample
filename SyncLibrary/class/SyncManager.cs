using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLibrary
{
    public class SyncManager
    {
        private readonly List<string> _selectedTargetData;

        private IDataSyncProcessor _dataSyncProcessor;
        private readonly SqlLogger _logger; // Logger 인스턴스 추가

        // 진행 상태 및 로그 업데이트를 위한 이벤트
        public event Action<string> StatusUpdated;
        public event Action<string> LogUpdated;

        public SyncManager(IDataSyncProcessor dataSyncLogProcessor, SqlLogger logger)
        {
            _dataSyncProcessor = dataSyncLogProcessor;
            _logger = logger; // Logger 인스턴스 초기화
            _selectedTargetData = new List<string>();
        }
        public void SetDataSyncProcessor(IDataSyncProcessor dataSyncProcessor)
        {
            _dataSyncProcessor = dataSyncProcessor; // 선택된 동기화 프로세서 설정
        }

        public async Task StartSync()
        {
            // 동기화 작업을 시작하는 메서드
            try
            {
                await _dataSyncProcessor.ProcessLogsAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError($"SyncManager - Error during sync: {ex.Message}", ex.ToString());
                throw;
            }
        }


        public void UpdateStatus(string message)
        {
            StatusUpdated?.Invoke(message);
        }

        private void UpdateLog(string message)
        {
            LogUpdated?.Invoke(message);
        }

        private void OnTimerElapsed(object sender, EventArgs e)
        {

        }
    }

}