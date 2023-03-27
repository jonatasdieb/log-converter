using CandidateTesting.JonatasDiebAraujoLima.Adapters;
using CandidateTesting.JonatasDiebAraujoLima.Interfaces;
using CandidateTesting.JonatasDiebAraujoLima.Models;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Xunit;

namespace Tests.Adapters
{
    public class FakeTargetFormatter : ILogsToTargetFormatter
    {
        public string Format(List<Log> logs) => "formatted log";
        public string GetFormat() => "fakeFormatter";
    }
    public class MinhaCdnLogAdapterTest
    {

        private readonly Mock<IWebClientWrapper> _webClientWrapperMock;
        private readonly ILogAdapter _adapter;
        private readonly Mock<ILogsToTargetFormatter> _logsToTargetFormattersMock;
        private readonly MethodInfo MapTextToLogs;        

        public MinhaCdnLogAdapterTest()
        {
            _logsToTargetFormattersMock = new Mock<ILogsToTargetFormatter>();
            _webClientWrapperMock = new Mock<IWebClientWrapper>();

            _adapter = new MinhaCdnLogAdapter(_webClientWrapperMock.Object, new List<ILogsToTargetFormatter>() { new FakeTargetFormatter() });          

            //Necessário devido método Validate() ser private
            MapTextToLogs = typeof(MinhaCdnLogAdapter).GetMethod("MapTextToLogs", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        [Fact]
        public void GetProvider_ReturnsCorrectProvider()
        {
            var provider = _adapter.GetProvider();
            Assert.Equal("minha_cdn", provider);
        }

        [Fact]
        public void Adapt_ShouldRetornFormattedString()
        {
            var logLine1 = "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2";
            var logLine2 = "101|200|MISS|\"POST /myImages HTTP/1.1\"|319.4";
            var logsText = logLine1 + "\n" + logLine2;

            _webClientWrapperMock
               .Setup(x => x.OpenRead(It.IsAny<string>()))
               .Returns(new MemoryStream(Encoding.UTF8.GetBytes(logsText)));

            _logsToTargetFormattersMock
              .Setup(x => x.Format(It.IsAny<List<Log>>()))
              .Returns("formatted log");

            var convertedLogs = _adapter.Adapt("fakeSoruce", "fakeFormatter");

            Assert.Equal(convertedLogs, "formatted log");

        }

        [Fact]
        public void MapTextToLogs_ShouldMapTxtLogToLogList()
        {

            var logLine1 = "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2";
            var logLine2 = "101|200|MISS|\"POST /myImages HTTP/1.1\"|319.4";
            var logsText = logLine1 + "\n" + logLine2;

            _webClientWrapperMock
               .Setup(x => x.OpenRead(It.IsAny<string>()))
               .Returns(new MemoryStream(Encoding.UTF8.GetBytes(logsText)));

            var expectedLogs = new List<Log> {
                        new Log(_adapter.GetProvider(), "GET", "200", "/robots.txt", "100", "312", "HIT"),
                        new Log(_adapter.GetProvider(), "POST", "200", "/myImages", "319", "101", "MISS")
                        };           

            var logs = (List<Log>)MapTextToLogs.Invoke(_adapter, new object[] { "source", _adapter.GetProvider() });

            logs.Should().BeEquivalentTo(expectedLogs);
        }
    }


}
