using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Windows.Forms;
using System.Diagnostics;

namespace SyncDBConn
{
    public class ProxyServerInfoManager
    {
      
        private static string baseDirectory = @"C:\Sync_CRMData";
        private static readonly string filePath = Path.Combine(baseDirectory, "config", "proxy_serverinfo.json");


        public static ProxyServerInfo LoadServerInfo()
        {

            if (File.Exists(filePath))
            {

                var json = File.ReadAllText(filePath);

                try
                {
                    var serverInfoList = JsonSerializer.Deserialize<List<ProxyServerInfo>>(json);

                    if (serverInfoList != null && serverInfoList.Count > 0)
                    {
                        return serverInfoList[0];
                    }
                }
                catch (JsonException)
                {
                    // JSON 데이터가 단일 객체일 가능성을 재확인
                    try
                    {
                        return JsonSerializer.Deserialize<ProxyServerInfo>(json);
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"JSON 변환 실패: {ex.Message}");

                    }
                }
            }
            MessageBox.Show("Proxy서버 정보가 저장되지 않았습니다. Proxy DB서버 연결정보를 확인하세요.");
            return null;
        }

        public static void SaveProxyServerInfo(ProxyServerInfo serverInfo)
        {
            List<ProxyServerInfo> serverInfos = new List<ProxyServerInfo>();
            // 기존 파일 있는지 확인, 있으면 불러오기
            if (File.Exists(filePath))
            {
                var exsistingJson = File.ReadAllText(filePath);

                serverInfo = JsonSerializer.Deserialize<ProxyServerInfo>(exsistingJson);
            }

            var exsistingServerInfo = serverInfos.FirstOrDefault(s => s.serverIp == serverInfo.serverIp);

            if (exsistingServerInfo != null)
            {
                exsistingServerInfo.dbId = serverInfo.dbId;
                exsistingServerInfo.dbPw = serverInfo.dbPw;
                exsistingServerInfo.dbName = serverInfo.dbName;
                exsistingServerInfo.dbPort = serverInfo.dbPort;
            }
            else
            {
                // taskId가 중복되지 않으면 새로 추가
                serverInfos.Add(serverInfo);
            }
            var updatedJson = JsonSerializer.Serialize(serverInfos, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, updatedJson);
            MessageBox.Show("Proxy서버 정보가 저장되었습니다.");
        }
    }
}