using DupalTestCase;
using Newtonsoft.Json.Linq;

namespace DupalTestCaseTests
{
    public class UnitTest1
    {
        #region TokensToStringWithStringBuilder
        [Fact]
        public void TokensToStringWithStringBuilder_Success()
        {
            var output = Program.TokensToString(new string[] { "3", "444", "7", "2", "88", "555" });
            Assert.Equal("DIPAUL", output);
        }

        [Fact]
        public void TokensToStringWithStringBuilder_EmptyResultWithNull()
        {
            var output = Program.TokensToString(null);
            Assert.Empty(output);
        }

        [Fact]
        public void TokensToStringWithStringBuilder_EmptyResultIfEmpty()
        {
            var output = Program.TokensToString(new string[0]);
            Assert.Empty(output);
        }

        #endregion

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

        #region KeysToChars_WithBackspace
        [Theory]
        [InlineData("11111*#")]
        [InlineData("#")]
        [InlineData(null)]
        [InlineData(".+-/*[]{}")]
        public void KeysToChars_WithBackspace_Failed(string input)
        {
            var output = Program.KeysToChars_WithBackspace(input);
            Assert.Empty(output);
        }

        [Theory]
        [InlineData("44 45*#", "HG")]
        [InlineData("101044 101045*#", "  H  G")]
        [InlineData("34447288555#", "DIPAUL")]
        [InlineData("4433555 555666096667775553#", "HELLO WORLD")]
        public void KeysToChars_WithBackspace_Success(string input, string output)
        {
            var payload = Program.KeysToChars_WithBackspace(input);
            Assert.Equal(payload, output);
        }
        #endregion

        #region KeysToChars
        [Theory]
        [InlineData("11111***#")]
        //[InlineData("#")]
        //[InlineData(null)]
        //[InlineData(".+-/*[]{}")]
        public void KeysToChars_Failed(string input)
        {
            var output = Program.KeysToChars(input);
            Assert.Empty(output);
        }

        [Theory]
        [InlineData("44 45*#", "HGJ")]
        [InlineData("101044 101045*#", "  H  GJ")]
        [InlineData("34447288555#", "DIPAUL")]
        [InlineData("4433555 555666096667775553#", "HELLO WORLD")]
        public void KeysToChars_Success(string input, string output)
        {
            var payload = Program.KeysToChars(input);
            Assert.Equal(payload, output);
        }
        #endregion

        #region Parse
        [Fact]
        public void ParseKeys_WithBackspace_SuccessWithSpaces()
        {
            var tokens = Program.ParseKeys_WithBackspace("44335559   10*");
            Assert.True(tokens.SequenceEqual(new string[] { "44", "33", "555", "9", "1" }));
        }

        [Fact]
        public void ParseKeys_WithBackspace_SuccessSimple()
        {
            var tokens = Program.ParseKeys_WithBackspace("44335559");
            Assert.True(tokens.SequenceEqual(new string[] { "44", "33", "555", "9" }));
        }

        [Fact]
        public void ParseKeys_WithBackspace_SuccessWithSpacesAtBeggingAndEnd()
        {
            var tokens = Program.ParseKeys_WithBackspace("  44 3*9  ");
            Assert.True(tokens.SequenceEqual(new string[] { "44", "9"}));
        }
        #endregion
    }
}