using System.Text.RegularExpressions;

namespace ADL.AdventOfCode2023;

public partial class ScratchCardSolver : ISolver
{
    public int Solve(string[] lines, int part)
    {
        var cards = lines
            .Select(line => {
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
                return new Card
                {
                    Id = cardId,
                    WinningNumbers = winningNumbers,
                    CardNumbers = cardNumbers
                };
            });

        if (part == 1) {
            return cards
                .Sum(card =>
                    (int) Math.Pow(2, card.CardNumbers.Count(number => card.WinningNumbers.Contains(number)) - 1)
                );
        } else {
            throw new NotImplementedException();
        }
    }

    public class Card {
        public required int Id { get; init; }
        public required IEnumerable<int> WinningNumbers { get; init; }
        public required IEnumerable<int> CardNumbers { get; init; }
    }

    [GeneratedRegex("Card\\s+(\\d+):")]
    private static partial Regex CardIdRegex();
}