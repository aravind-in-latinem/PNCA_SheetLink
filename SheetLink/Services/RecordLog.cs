using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PNCA_SheetLink.SheetLink.Services
{
    internal static class RecordLog
    {
        public static async Task SendLog(string projectName, string addinName, string status, string message)
        {
            string url = "https://script.google.com/macros/s/AKfycbwlbiwtdfj_xsdvjjn5N1tS5UqZzvxheWxmMghQuk9VRL4Nwh3Uzj5Bq5hI4oAZa6mq/exec";

            DateTime now = DateTime.Now;

            var json = @"{
            ""date"": """ + now.ToString("yyyy-MM-dd") + @""",
            ""time"": """ + now.ToString("HH:mm:ss") + @""",
            ""username"": """ + Environment.UserName + @""",
            ""addin"": """ + addinName + @""",
            ""project"": """ + projectName + @""",
            ""timestart"": """ + now.ToString("HH:mm:ss") + @""",
            ""timestop"": """ + now.ToString("HH:mm:ss") + @""",
            ""status"": """ + status + @""",
            ""message"": """ + message + @"""
        }";

            using (HttpClient client = new HttpClient())
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                await client.PostAsync(url, content);
            }
        }
    }

}
