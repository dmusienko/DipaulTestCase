using System.Text;
using static System.Console;

namespace DupalTestCase
{
    public class Program
    {
        private static readonly char EOL = '#';
        private static readonly char space = ' ';
        private static readonly char backspace = '*';
        private static readonly Dictionary<string, char> charsMap = new()
        {
            {"2", 'A' },
            {"22", 'B' },
            {"222", 'C' },
            {"3", 'D' },
            {"33", 'E' },
            {"333", 'F' },
            {"4", 'G' },
            {"44", 'H' },
            {"444", 'I' },
            {"5", 'J' },
            {"55", 'K' },
            {"555", 'L' },
            {"6", 'M' },
            {"66", 'N' },
            {"666", 'O' },
            {"7", 'P' },
            {"77", 'Q' },
            {"777", 'R' },
            {"7777", 'S' },
            {"8", 'T' },
            {"88", 'U' },
            {"888", 'V' },
            {"9", 'W' },
            {"99", 'X' },
            {"999", 'Y' },
            {"9999", 'Z' },
            {"0", ' '}
        };

        public static void Main(string[] args)
        {
            WriteLine("Нажмите Ctrl+C для выхода.");

            do
            {
                Write("Введите последовательность кнопок от 0 до 9, затем в конце символ #: ");
                string? input = ReadLine();
                string output = ConvertKeysToChars(input);
                if (!string.IsNullOrEmpty(output)) WriteLine("{0} -> {1}", input, output);

            } while (true);
        }

        public static string ConvertKeysToChars(string? input)
        {
            if (!TryGetPayload(input, out string payload)) return string.Empty;

            var tokens = Parse(payload);
            return ConvertToString(tokens);
        }

        public static bool TryGetPayload(string? input, out string payload)
        {
            payload = string.Empty;
            
            if (string.IsNullOrWhiteSpace(input)) return false;

            var index = input.IndexOf(EOL);
            if (index > 0)
            {
                payload = input.Substring(0, index);
                return true;
            }

            return false;
        }

        public static string[] Parse(string input)
        {
            List<string> tokens = new();
            int startIndex = 0;
            int endIndex;

            while (startIndex < input.Length)
            {
                endIndex = startIndex;

                while (endIndex < input.Length &&
                    input[endIndex] != space &&
                    input[startIndex] == input[endIndex]) endIndex++;

                var seq = input.Substring(startIndex, endIndex - startIndex);
                if (seq.Length > 0) tokens.Add(seq);

                if (input[endIndex] == space && endIndex < input.Length) endIndex++;

                startIndex = endIndex;
            }

            return tokens.ToArray();
        }

        public static string ConvertToString(string[] tokens)
        {
            var sb = new StringBuilder();
            foreach (string t in tokens)
            {
                if (charsMap.TryGetValue(t, out char c)) sb.Append(c);
            }
            return sb.ToString();
        }
    }
}