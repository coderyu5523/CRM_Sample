using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncsCRMData
{
    public class LogManager
    {
        private readonly string logDirectory;



        public LogManager()
        {
            // 실행 경로 하위에 Logs 폴더 경로 설정
            string baseDirectory = @"C:\Sync_CRMData";
            logDirectory = Path.Combine(baseDirectory, "Logs");


        }

        private static readonly object _fileLock = new object(); // 파일 접근을 동기화할 객체

        public void SaveLogToFile(string logMessage)
        {
            DateTime currentDate = DateTime.Now;
            string year = currentDate.ToString("yyyy");
            string month = currentDate.ToString("MM");
            string day = currentDate.ToString("dd");

            string logPath = Path.Combine(logDirectory, year, month);
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }

            string logFileName = $"{currentDate:yyyy-MM-dd}.log";
            string logFilePath = Path.Combine(logPath, logFileName);

            // 파일 접근 동기화
            lock (_fileLock)
            {
                File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
            }
        }

        // 로그 파일을 읽어서 그리드에 표시하는 메서드
        public string[] ReadLogsFromFile(DateTime date)
        {
            // 년/월/일에 해당하는 파일 경로 설정
            string logPath = Path.Combine(logDirectory, date.ToString("yyyy"), date.ToString("MM"));
            string logFileName = $"{date:yyyy-MM-dd}.log";
            string logFilePath = Path.Combine(logPath, logFileName);

            if (File.Exists(logFilePath))
            {
                // 파일 내용을 읽어서 문자열 배열로 반환
                return File.ReadAllLines(logFilePath);
            }
            else
            {
                Console.WriteLine($"로그 파일을 찾을 수 없습니다: {logFilePath}");
                return new string[0]; // 파일이 없을 경우 빈 배열 반환
            }
        }
    }

}
