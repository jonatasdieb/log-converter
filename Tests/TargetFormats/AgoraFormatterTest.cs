using CandidateTesting.JonatasDiebAraujoLima.Models;
using CandidateTesting.JonatasDiebAraujoLima.TargetFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.TargetFormats
{
    public class AgoraFormatterTest
    {
        AgoraFormatter _formatter;
        public AgoraFormatterTest()
        {
            _formatter = new AgoraFormatter();
        }

        [Fact]
        public void GetFormat_ShouldReturnStringWithFormatName()
        {     
            string expected = "agora";
            string result = _formatter.GetFormat();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Format_ShouldFormatStringFromLogList()
        {         
            string expected =
                "# Version: 1.0\r\n" +
                $"# Date: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}\r\n" +
                "# Fields: provider http-method status-code uri-path time-taken response-size cache-status\r\n" +
                "\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT\r\n" +
                "\"MINHA CDN\" POST 200 /myImages 319 101 MISS\r\n";

            var logs = new List<Log> {
                        new Log("MINHA CDN", "GET", "200", "/robots.txt", "100", "312", "HIT"),
                        new Log("MINHA CDN", "POST", "200", "/myImages", "319", "101", "MISS")
                        };

            string result = _formatter.Format(logs);

            Assert.Equal(expected, result);
        }
      
    }
}
