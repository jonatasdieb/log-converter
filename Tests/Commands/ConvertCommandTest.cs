using CandidateTesting.JonatasDiebAraujoLima.Interfaces;
using JonatasDiebAraujoLima.Commands;
using Moq;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace Tests.Commands
{
    public class FakeAdapter : ILogAdapter
    {
        public string Adapt(string sourceUrl, string targetFormat)
        {
            return "adapted string";
        }

        public string GetProvider() => "minha_cdn";
    }

    public class ConvertCommandTest
    {
        private readonly Mock<ISaveConvertedLogs> _saveConvertedLogs;
        private readonly ConvertCommand _convertCommand;
        private readonly FakeAdapter _fakeAdapter;
        private readonly string[] fakeArgs;
        private readonly MethodInfo Validate;

        public ConvertCommandTest()
        {
            _fakeAdapter = new FakeAdapter();
            _saveConvertedLogs = new Mock<ISaveConvertedLogs>();
            fakeArgs = new string[] { @"https://url/sourceUrl.txt", @"c:\targetPath.txt" };            

            _convertCommand = new ConvertCommand(new List<ILogAdapter> { _fakeAdapter }, _saveConvertedLogs.Object);

            //Necessário devido método Validate() ser private
            Validate = typeof(ConvertCommand).GetMethod("Validate", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        [Fact]
        public void Validate_ShouldNotThrowException()
        {
            try
            {
                Validate.Invoke(_convertCommand, new object[] { fakeArgs, _fakeAdapter });
                Assert.True(true);
            }
            catch (Exception)
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void Execute_ShouldNotThrowException()
        {
            var convertCommand = new ConvertCommand(new List<ILogAdapter> { _fakeAdapter }, _saveConvertedLogs.Object);
            convertCommand.Execute(fakeArgs);

            Assert.True(true);
        }


        [Theory]
        [InlineData(new string[] { @"https://url/sourceUrl.txt" }, "Correct format: convert <sourceUrl> <targetPath>")]
        [InlineData(new string[] { @"https://url/sourceUrl.txt", @"c:\targetPath.txt", "incorret" }, "Correct format: convert <sourceUrl> <targetPath>")]
        public void Validate_ThrowsArgumentExceptionWhenArgsLengthNot2(string[] args, string expectedError)
        {
            try
            {
                Validate.Invoke(_convertCommand, new object[] { args, _fakeAdapter });
            }
            catch (TargetInvocationException e)
            {
                if (e.InnerException is ArgumentException argumentException)
                {
                    Assert.Equal(expectedError, argumentException.Message);
                    return;
                }
            }

            Assert.True(false);
        }

        [Fact]
        public void Validate_ThrowsArgumentExceptionWhenAdapterIsNull()
        {
            string expectedError = "Unknown Provider";

            try
            {
                Validate.Invoke(_convertCommand, new object[] { fakeArgs, null });
            }
            catch (TargetInvocationException e)
            {
                if (e.InnerException is ArgumentException argumentException)
                {
                    Assert.Equal(expectedError, argumentException.Message);
                }
                else
                {
                    Assert.True(false);
                }
            }
        }


        [Theory]
        [InlineData(new string[] { @"https://url/sourceUrl", @"c:\targetPath.txt" }, "sourceUrl and targetPath should be a .txt file")]
        [InlineData(new string[] { @"https://url/sourceUrl.txt", @"c:\targetPath" }, "sourceUrl and targetPath should be a .txt file")]
        public void Validate_ShouldThrowArgumentExceptionWhenSourceOrTargetNotTxt(string[] args, string expectedError)
        {
            try
            {
                Validate.Invoke(_convertCommand, new object[] { args, _fakeAdapter });
            }
            catch (TargetInvocationException e)
            {
                if (e.InnerException is ArgumentException argumentException)
                {
                    Assert.Equal(expectedError, argumentException.Message);
                    return;
                }
            }

            Assert.True(false);
        }
    }
}
