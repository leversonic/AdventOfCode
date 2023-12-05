using System.Text.RegularExpressions;

namespace ADL.AdventOfCode2023;

public partial class ScratchCardSolver : ISolver
{
    public int Solve(string[] lines, int part)
    {
        var cardDict = new Dictionary<int, Card>();
        lines.ToList()
            .ForEach(line => {
                var cardId = int.Parse(CardIdRegex().Match(line).Groups[1].Value);
                var numberStrings = line[(line.IndexOf(':') + 1)..].Split("|");
                var winningNumbers = numberStrings[0]
                    .Split(" ")
                    .Where(s => s.Length > 0)
                    .Select(int.Parse);
                var cardNumbers = numberStrings[1]
                    .Split(" ")
                    .Where(s => s.Length > 0)
                    .Select(int.Parse);
                cardDict.Add(cardId, new Card {
                    WinningNumbers = winningNumbers,
                    CardNumbers = cardNumbers
                });
            });

        if (part == 1) {
            return cardDict.Values
                .Sum(card =>
                    (int) Math.Pow(2, WinningNumberCount(card) - 1)
                );
        } else {
            lines
                .Select((line, index) => (line, index))
                .ToList()
                .ForEach(tuple => {
                    var cardId = tuple.index + 1;
                    var card = cardDict[cardId];
                    var winningNumberCount = WinningNumberCount(card);
                    foreach(var copiedCardIndex in Enumerable.Range(cardId + 1, winningNumberCount)) {
                        cardDict[copiedCardIndex].Copies += card.Copies;
                    }
                });

            return cardDict.Values.Sum(card => card.Copies);
        }
    }

    private int WinningNumberCount(Card card) => card.CardNumbers.Count(number => card.WinningNumbers.Contains(number));

    public class Card {
        public required IEnumerable<int> WinningNumbers { get; init; }
        public required IEnumerable<int> CardNumbers { get; init; }
        public int Copies { get; set; }

        public Card() {
            Copies = 1;
        }
    }

    [GeneratedRegex("Card\\s+(\\d+):")]
    private static partial Regex CardIdRegex();
}