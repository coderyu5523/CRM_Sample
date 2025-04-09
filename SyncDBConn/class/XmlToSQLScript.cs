using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SyncDBConn
{
    public class XmlToSQLScript
    {
        public string GenerateInsertSql(string xmlData, string tableName, Dictionary<string, string> fieldTypes)
        {
            // XML 데이터를 파싱
            XDocument xmlDocument = XDocument.Parse(xmlData);

            // dma100_batch_test 엘리먼트 내의 모든 row 요소들을 찾기
            var rows = xmlDocument.Descendants("row");

            // SQL INSERT 문들을 저장할 StringBuilder
            StringBuilder sqlInserts = new StringBuilder();

            // 각 row에 대한 SQL INSERT 문 생성
            foreach (var rowElement in rows)
            {
                StringBuilder columns = new StringBuilder();
                StringBuilder values = new StringBuilder();
                string comma = "";

                // row의 모든 하위 요소를 순회하여 컬럼과 값 추출
                foreach (var element in rowElement.Elements())
                {
                    string columnName = element.Name.LocalName;
                    string value = element.Value;

                    columns.Append(comma + columnName);

                    // 필드 타입에 따라 값 처리
                    if (fieldTypes.ContainsKey(columnName))
                    {
                        string fieldType = fieldTypes[columnName].ToLower();

                        if (fieldType == "int" || fieldType == "decimal" || fieldType == "float")
                        {
                            values.Append(comma + value); // 숫자는 그대로 삽입
                        }
                        else if (fieldType == "date" || fieldType == "datetime")
                        {
                            values.Append(comma + $"'{DateTime.Parse(value).ToString("yyyy-MM-dd HH:mm:ss")}'"); // 날짜는 포맷팅 후 삽입
                        }
                        else
                        {
                            values.Append(comma + $"'{value.Replace("'", "''")}'"); // 문자열은 이스케이프 처리 후 삽입
                        }
                    }
                    else
                    {
                        // 필드 타입이 정의되지 않은 경우 기본적으로 문자열로 처리
                        values.Append(comma + $"'{value.Replace("'", "''")}'");
                    }

                    comma = ", ";
                }

                // 동적 SQL INSERT 문 생성
                string sqlInsert = $"INSERT INTO {tableName} ({columns}) VALUES ({values});";
                sqlInserts.AppendLine(sqlInsert);
            }

            // 모든 SQL INSERT 문 반환
            return sqlInserts.ToString();
        }

        public string GenerateInsertSql(string xmlData, string tableName, Dictionary<string, (string DataType, int? MaxLength, int? Precision, int? Scale)> fieldTypes)
        {
            // XML 데이터를 파싱
            XDocument xmlDocument = XDocument.Parse(xmlData);

            // dma100_batch_test 엘리먼트 내의 모든 row 요소들을 찾기
            var rows = xmlDocument.Descendants("row");

            // SQL INSERT 문들을 저장할 StringBuilder
            StringBuilder sqlInserts = new StringBuilder();

            // 각 row에 대한 SQL INSERT 문 생성
            foreach (var rowElement in rows)
            {
                StringBuilder columns = new StringBuilder();
                StringBuilder values = new StringBuilder();
                string comma = "";

                // row의 모든 하위 요소를 순회하여 컬럼과 값 추출
                foreach (var element in rowElement.Elements())
                {
                    string columnName = element.Name.LocalName;
                    string value = element.Value;

                    columns.Append(comma + columnName);

                    // 필드 타입에 따라 값 처리
                    if (fieldTypes.ContainsKey(columnName))
                    {
                        string fieldType = fieldTypes[columnName].DataType.ToLower();

                        if (fieldType == "int" || fieldType == "decimal" || fieldType == "float")
                        {
                            values.Append(comma + value); // 숫자는 그대로 삽입
                        }
                        else if (fieldType == "date" || fieldType == "datetime")
                        {
                            values.Append(comma + $"'{DateTime.Parse(value).ToString("yyyy-MM-dd HH:mm:ss")}'"); // 날짜는 포맷팅 후 삽입
                        }
                        else
                        {
                            values.Append(comma + $"'{value.Replace("'", "''")}'"); // 문자열은 이스케이프 처리 후 삽입
                        }
                    }
                    else
                    {
                        // 필드 타입이 정의되지 않은 경우 기본적으로 문자열로 처리
                        values.Append(comma + $"'{value.Replace("'", "''")}'");
                    }

                    comma = ", ";
                }

                // 동적 SQL INSERT 문 생성
                string sqlInsert = $"INSERT INTO {tableName} ({columns}) VALUES ({values});";
                sqlInserts.AppendLine(sqlInsert);
            }

            // 모든 SQL INSERT 문 반환
            return sqlInserts.ToString();
        }
        //(string DataType, int? MaxLength, int? Precision, int? Scale)
        public string GenerateUpdateSql(string xmlData, string tableName, List<string> keyColumns, Dictionary<string, (string DataType, int? MaxLength, int? Precision, int? Scale)> fieldTypes)
        {
            // XML 데이터를 파싱
            XDocument xmlDocument = XDocument.Parse(xmlData);

            // 'row' 엘리먼트 찾기
            var rowElement = xmlDocument.Root.Element("row");

            if (rowElement != null)
            {
                StringBuilder setClauses = new StringBuilder();
                StringBuilder whereClauses = new StringBuilder();
                string comma = "";
                string whereComma = "";

                // 모든 하위 요소를 순회하여 SET 절 구성
                foreach (var element in rowElement.Elements())
                {
                    string columnName = element.Name.LocalName;
                    string value = element.Value;

                    if (keyColumns.Contains(columnName))
                    {
                        // 필드 타입에 따라 키 컬럼의 WHERE 절 구성
                        if (fieldTypes.ContainsKey(columnName))
                        {
                            string fieldType = fieldTypes[columnName].DataType.ToLower();

                            if (fieldType == "int" || fieldType == "decimal" || fieldType == "float")
                            {
                                whereClauses.Append($"{whereComma}{columnName} = {value}"); // 숫자는 그대로 삽입
                            }
                            else if (fieldType == "date" || fieldType == "datetime")
                            {
                                whereClauses.Append($"{whereComma}{columnName} = '{DateTime.Parse(value).ToString("yyyy-MM-dd HH:mm:ss")}'"); // 날짜는 포맷팅 후 삽입
                            }
                            else
                            {
                                whereClauses.Append($"{whereComma}{columnName} = '{value.Replace("'", "''")}'"); // 문자열은 이스케이프 처리 후 삽입
                            }
                        }
                        else
                        {
                            // 필드 타입이 정의되지 않은 경우 기본적으로 문자열로 처리
                            whereClauses.Append($"{whereComma}{columnName} = '{value.Replace("'", "''")}'");
                        }

                        whereComma = " AND ";
                    }
                    else
                    {
                        // 필드 타입에 따라 SET 절 구성
                        if (fieldTypes.ContainsKey(columnName))
                        {
                            string fieldType = fieldTypes[columnName].DataType.ToLower();

                            if (fieldType == "int" || fieldType == "decimal" || fieldType == "float")
                            {
                                setClauses.Append($"{comma}{columnName} = {value}"); // 숫자는 그대로 삽입
                            }
                            else if (fieldType == "date" || fieldType == "datetime")
                            {
                                setClauses.Append($"{comma}{columnName} = '{DateTime.Parse(value).ToString("yyyy-MM-dd HH:mm:ss")}'"); // 날짜는 포맷팅 후 삽입
                            }
                            else
                            {
                                setClauses.Append($"{comma}{columnName} = '{value.Replace("'", "''")}'"); // 문자열은 이스케이프 처리 후 삽입
                            }
                        }
                        else
                        {
                            // 필드 타입이 정의되지 않은 경우 기본적으로 문자열로 처리
                            setClauses.Append($"{comma}{columnName} = '{value.Replace("'", "''")}'");
                        }

                        comma = ", ";
                    }
                }

                // 동적 SQL UPDATE 문 생성
                string sqlUpdate = $"UPDATE {tableName} SET {setClauses} WHERE {whereClauses};";
                return sqlUpdate;
            }
            else
            {
                throw new Exception("row element not found in the provided XML.");
            }
        }
        public string GenerateUpdateSql(string xmlData, string tableName, List<string> keyColumns, Dictionary<string, string> fieldTypes)
        {
            // XML 데이터를 파싱
            XDocument xmlDocument = XDocument.Parse(xmlData);

            // 'row' 엘리먼트 찾기
            var rowElement = xmlDocument.Root.Element("row");

            if (rowElement != null)
            {
                StringBuilder setClauses = new StringBuilder();
                StringBuilder whereClauses = new StringBuilder();
                string comma = "";
                string whereComma = "";

                // 모든 하위 요소를 순회하여 SET 절 구성
                foreach (var element in rowElement.Elements())
                {
                    string columnName = element.Name.LocalName;
                    string value = element.Value;

                    if (keyColumns.Contains(columnName))
                    {
                        // 필드 타입에 따라 키 컬럼의 WHERE 절 구성
                        if (fieldTypes.ContainsKey(columnName))
                        {
                            string fieldType = fieldTypes[columnName].ToLower();

                            if (fieldType == "int" || fieldType == "decimal" || fieldType == "float")
                            {
                                whereClauses.Append($"{whereComma}{columnName} = {value}"); // 숫자는 그대로 삽입
                            }
                            else if (fieldType == "date" || fieldType == "datetime")
                            {
                                whereClauses.Append($"{whereComma}{columnName} = '{DateTime.Parse(value).ToString("yyyy-MM-dd HH:mm:ss")}'"); // 날짜는 포맷팅 후 삽입
                            }
                            else
                            {
                                whereClauses.Append($"{whereComma}{columnName} = '{value.Replace("'", "''")}'"); // 문자열은 이스케이프 처리 후 삽입
                            }
                        }
                        else
                        {
                            // 필드 타입이 정의되지 않은 경우 기본적으로 문자열로 처리
                            whereClauses.Append($"{whereComma}{columnName} = '{value.Replace("'", "''")}'");
                        }

                        whereComma = " AND ";
                    }
                    else
                    {
                        // 필드 타입에 따라 SET 절 구성
                        if (fieldTypes.ContainsKey(columnName))
                        {
                            string fieldType = fieldTypes[columnName].ToLower();

                            if (fieldType == "int" || fieldType == "decimal" || fieldType == "float")
                            {
                                setClauses.Append($"{comma}{columnName} = {value}"); // 숫자는 그대로 삽입
                            }
                            else if (fieldType == "date" || fieldType == "datetime")
                            {
                                setClauses.Append($"{comma}{columnName} = '{DateTime.Parse(value).ToString("yyyy-MM-dd HH:mm:ss")}'"); // 날짜는 포맷팅 후 삽입
                            }
                            else
                            {
                                setClauses.Append($"{comma}{columnName} = '{value.Replace("'", "''")}'"); // 문자열은 이스케이프 처리 후 삽입
                            }
                        }
                        else
                        {
                            // 필드 타입이 정의되지 않은 경우 기본적으로 문자열로 처리
                            setClauses.Append($"{comma}{columnName} = '{value.Replace("'", "''")}'");
                        }

                        comma = ", ";
                    }
                }

                // 동적 SQL UPDATE 문 생성
                string sqlUpdate = $"UPDATE {tableName} SET {setClauses} WHERE {whereClauses};";
                return sqlUpdate;
            }
            else
            {
                throw new Exception("row element not found in the provided XML.");
            }
        }
    }
    }
