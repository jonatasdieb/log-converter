using CandidateTesting.JonatasDiebAraujoLima;
using CandidateTesting.JonatasDiebAraujoLima.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Tests
{
    public class ProgramTest
    {
        private readonly IEnumerable<ICommand> commands;
        private readonly Mock<ICommand> mockCommand;

        public ProgramTest()
        {
            mockCommand = new Mock<ICommand>();
            commands = new List<ICommand>() { mockCommand.Object };
        }

        [Theory]
        [InlineData(new string[] { }, "Command not found")]
        [InlineData(new string[] { "unknownFakeCommand" }, "Unknown command: unknownFakeCommand")]
        public void ValidateArgs_ShouldThrowArgumentExceptionWhenNoCommandOrUnkownCommand(string[] args, string expectedErrorMsg)
        {
            var ex = Assert.Throws<ArgumentException>(() => Program.ValidateArgs(args, commands));
            Assert.Equal(expectedErrorMsg, ex.Message);
        }

        [Fact]
        public void ValidateArgs_CommandFoundShouldNotThrowException()
        {
            mockCommand.Setup(x => x.GetContext()).Returns("convert");
            var commandFound = mockCommand.Object.GetContext();

            try
            {
                Program.ValidateArgs(new string[] { commandFound }, commands);
                Assert.True(true);
            }
            catch (Exception)
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void NormalizeArgs_ShouldConvertAllArgsToLowerCase()
        {
            var fakeArgs = new string[] { "Convert" };
            var expectedArgNormalized = fakeArgs[0].ToLower();

            var normalizedArgs = Program.NormalizeArgs(fakeArgs);

            Assert.Equal(expectedArgNormalized, normalizedArgs[0]);
        }
    }
}
