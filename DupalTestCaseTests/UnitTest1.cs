using DupalTestCase;

namespace DupalTestCaseTests
{
    public class UnitTest1
    {
        #region TryGetPayload
        [Fact]
        public void TryGetPayload_FailedIfNull()
        {
            var done = Program.TryGetPayload(null, out string payload);
            Assert.False(done);
            Assert.Empty(payload);
        }

        [Fact]
        public void TryGetPayload_FailedIfEmpty()
        {
            var done = Program.TryGetPayload(string.Empty, out string payload);
            Assert.False(done);
            Assert.Empty(payload);
        }

        [Fact]
        public void TryGetPayload_FailedWithoutEOL()
        {
            var done = Program.TryGetPayload("4345576", out string payload);
            Assert.False(done);
            Assert.Empty(payload);
        }

        [Fact]
        public void TryGetPayload_FailedIfEmptyWithEOL()
        {
            var done = Program.TryGetPayload("#5657", out string payload);
            Assert.False(done);
            Assert.Empty(payload);
        }

        [Theory]
        [InlineData("5657#03928", "5657")]
        public void TryGetPayload_Success(string input, string output)
        {
            var done = Program.TryGetPayload(input, out string payload);
            Assert.True(done);
            Assert.Equal(payload, output);
        }
        #endregion

        #region ConvertToChars
        [Theory]
        [InlineData("11111***#")]
        [InlineData("#")]
        [InlineData(null)]
        [InlineData(".+-/*[]{}")]
        public void ConvertToChars_Failed(string input)
        {
            var output = Program.ConvertKeysToChars(input);
            Assert.Empty(output);
        }

        [Theory]
        [InlineData("44 45*#", "HGJ")]
        [InlineData("101044 101045*#", "  H  GJ")]
        [InlineData("4433555 555666096667775553#", "HELLO WORLD")]
        public void ConvertToChars_Success(string input, string output)
        {
            var payload = Program.ConvertKeysToChars(input);
            Assert.Equal(payload, output);
        }
        #endregion

        #region Parse
        [Fact]
        public void Parse_Success()
        {
            var tokens = Program.Parse("44335559   10*");
            Assert.True(tokens.SequenceEqual(new string[] { "44", "33", "555", "9", "1", "0", "*" }));
        }

        [Fact]
        public void Parse_Success2()
        {
            var tokens = Program.Parse("44335559");
            Assert.True(tokens.SequenceEqual(new string[] { "44", "33", "555", "9" }));
        }

        [Fact]
        public void Parse_Success3()
        {
            var tokens = Program.Parse("  44 3*9  ");
            Assert.True(tokens.SequenceEqual(new string[] { "44", "3", "*", "9"}));
        }
        #endregion
    }
}