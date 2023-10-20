using System.Linq;
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
                string output = KeysToChars(input);
                if (!string.IsNullOrEmpty(output)) WriteLine("{0} -> {1}", input, output);

            } while (true);
        }

        /// <summary>
        /// Преобразует последовательность нажатых клавиш в вывод (с учетом клавишы отмена *)
        /// </summary>
        /// <param name="input">Последовательность нажатых</param>
        /// <returns>вывод</returns>
        public static string KeysToChars_WithBackspace(string? input)
        {
            if (!TryGetPayload(input, out string payload)) return string.Empty;

            var tokens = ParseKeys_WithBackspace(payload);
            return TokensToString(tokens);
        }

        /// <summary>
        /// Преобразует последовательность нажатых клавиш в вывод
        /// </summary>
        /// <param name="input">Последовательность нажатых</param>
        /// <returns>вывод</returns>
        public static string KeysToChars(string? input)
        {
            if (!TryGetPayload(input, out string payload)) return string.Empty;

            var tokens = ParseKeys(payload);
            return TokensToString(tokens);
        }

        /// <summary>
        /// Вовзращает последовательности клавиш из строки ввода.
        /// </summary>
        /// <param name="input">Последовательность нажатых клавиш</param>
        /// <param name="payload">Последовательность </param>
        /// <returns>Признак успешного извлечения</returns>
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

        /// <summary>
        /// Разбивает последовательность клавиш в цепочки нажатых клавиш,
        /// соотвествующих буквам (с учетом клавишы отмена *)
        /// </summary>
        /// <param name="input">последовательность клавиш</param>
        /// <returns>цепочки нажатых клавиш</returns>
        public static string[] ParseKeys_WithBackspace(string input)
        {
            List<string> tokens = new();
            int startIndex = 0;
            int endIndex;

            while (startIndex < input.Length)
            {
                endIndex = startIndex;

                // вычисляем индекс окончания текущей непрерывной последовательности
                while (endIndex < input.Length &&
                    input[endIndex] != space &&
                    input[startIndex] == input[endIndex]) endIndex++;

                // проверяем, если встретился символ отмена "*", то сокращаем
                // текущую найденную последовательность на 1 символ с конца
                var seqLenght = endIndex - startIndex;
                if (endIndex < input.Length &&
                    input[endIndex] == backspace) seqLenght--;

                // извлекаем текущую найденную последовательность
                var seq = input.Substring(startIndex, seqLenght);
                if (seq.Length > 0) tokens.Add(seq);

                // смещаем индекс начала следующей последовательности,
                // если встретились символ пробел или отмена "*"
                if (endIndex < input.Length &&
                    (input[endIndex] == space || input[endIndex] == backspace)) endIndex++;

                startIndex = endIndex;
            }

            return tokens.ToArray();
        }

        /// <summary>
        /// Разбивает последовательность клавиш в цепочки нажатых клавиш, соотвествующих буквам
        /// </summary>
        /// <param name="input">последовательность клавиш</param>
        /// <returns>цепочки нажатых клавиш</returns>
        public static string[] ParseKeys(string input)
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

                if (endIndex < input.Length && input[endIndex] == space) endIndex++;

                startIndex = endIndex;
            }

            return tokens.ToArray();
        }

        /// <summary>
        /// Преобразует цепочки нажатых клавиш в последовательность букв.
        /// </summary>
        /// <param name="tokens">Цепочки нажатых клавиш</param>
        /// <returns>последовательность букв</returns>
        public static string TokensToString(string[]? tokens)
        {
            if (tokens is null) return string.Empty;

            var sb = new StringBuilder();
            foreach (var t in tokens)
            {
                if (charsMap.TryGetValue(t, out char c)) sb.Append(c);
            }
            return sb.ToString();
        }
    }
}