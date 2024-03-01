using System;
namespace FromAncientGreeksToModernGeeks
{
	public class Part02
	{
        private IDictionary<string, int> CalculateWordFrequencies(IEnumerable<string> bookLines)
        {
            var words = string.Join(" ", bookLines).Split(" ")
                .GroupBy(x => x)
                .Select(x => (x.Key, x.Count()))
                .ToDictionary(x => x.Key, x => x.Item2);
            return words;
        }


        private IEnumerable<string> GetBookLines(IEnumerable<string> input)
        {
            var stopWords = File.ReadAllLines("stopwords.txt")
                .Select(x => x.ToLower());

            var result = input
                .Select(x => x.ToLower())
                .Select(x =>
                    x.Replace(".", "")
                        .Replace("\"", " ")
                        .Replace(".", " ")
                        .Replace(",", "")
                        .Replace("_", "")
                        .Replace("-", " ")
                        .Replace("—", " ")
                        .Replace("’", "")
                        .Replace(";", "")
                        .Replace(":", "")
                        .Replace("(", "")
                        .Replace(")", "")
                        .Replace("'", "")
                        .Replace("ä", "a")
                        .Replace("ö", "o")
                        .Replace("ü", "u")
                        .Replace("ß", "ss")
                        .Replace("?", "")
                        .ReplaceRegex("[0-9]", "")
                        .Split(" ", StringSplitOptions.RemoveEmptyEntries).Except(stopWords)
                        .Where(y => !string.IsNullOrWhiteSpace(y))
                )
                .Where(x => x.Count() > 1)
                .Select(x => string.Join(" ", x))
                .Where(x => !string.IsNullOrWhiteSpace(x));

            return result;
        }

        public IDictionary<string, decimal> CalculateWordProbabilities(IDictionary<string, int> wordCounts, int totalWordCount) =>
            wordCounts.Select(x => (
                            x.Key,
                            (decimal)x.Value / totalWordCount
                        )
                ).ToDictionary(x => x.Key, x => x.Item2);

        private static int CalculateWordCount(IEnumerable<string> input) =>
            string.Join(" ", input).Split(" ").Length;

        public (decimal BookA, decimal BookB) CalculateLineProbability(string input, IDictionary<string, decimal> probA, IDictionary<string, decimal> probB, int totalWordCount)
        {
            var bookAProb = input.Split(" ").Aggregate(0.5m, (acc, x) =>
            {
                var wordGivenBookA = probA.TryGetValue(x, out var valA) ? valA : 0.00000000000001M;
                var wordGivenBookB = probB.TryGetValue(x, out var valB) ? valB : 0.00000000000001M;
                var pWord = wordGivenBookA + wordGivenBookB;
                var answer = (wordGivenBookA * acc) / pWord;
                return answer;
            });

            var bookBProb = input.Split(" ").Aggregate(0.5m, (acc, x) =>
            {
                var wordGivenBookA = probA.TryGetValue(x, out var valA) ? valA : 0.00000000000001M;
                var wordGivenBookB = probB.TryGetValue(x, out var valB) ? valB : 0.00000000000001M;
                var pWord = wordGivenBookA + wordGivenBookB;
                var answer = (wordGivenBookB * acc) / pWord;
                return answer;
            });

            return (bookAProb, bookBProb);
        }

		[Fact]
		public void Part02_Test01()
        {


            var wotwText = GetBookLines(File.ReadAllLines("wotw.txt")).ToArray();
            var wotwWordFrequencies = CalculateWordFrequencies(wotwText);
            var wotwWordCount = CalculateWordCount(wotwText);
            var wotwWordProbabilities = CalculateWordProbabilities(wotwWordFrequencies, wotwWordCount);
            


            var jttcoteText = GetBookLines(File.ReadAllLines("jttcote.txt")).ToArray();
            var jttcoteWordFrequencies = CalculateWordFrequencies(jttcoteText);
            var jttcoteWordCount = CalculateWordCount(jttcoteText);
            var jttcotewWordProbabilities = CalculateWordProbabilities(jttcoteWordFrequencies, jttcoteWordCount);


            var wotwResult = wotwText.Select(x => (
                x,
                CalculateLineProbability(x, wotwWordProbabilities, jttcotewWordProbabilities,
                    wotwWordCount + jttcoteWordCount)
            )).Select(x => (
                x.x,
                x.Item2,
                Correct: x.Item2.BookA > x.Item2.BookB
            ));

            var jttcoteResult = jttcoteText.Select(x => (
                x,
                CalculateLineProbability(x, wotwWordProbabilities, jttcotewWordProbabilities,
                    wotwWordCount + jttcoteWordCount)
            )).Select(x => (
                x.x,
                x.Item2,
                Correct: x.Item2.BookA < x.Item2.BookB
            ));



            var wrong = wotwResult.Where(x => !x.Correct).ToArray();
            var wrong2 = jttcoteResult.Where(x => !x.Correct).ToArray();


            var accuracy = (decimal)(wrong.Length + wrong2.Length) / (wotwText.Length + jttcoteText.Length);
        }
	}
}

