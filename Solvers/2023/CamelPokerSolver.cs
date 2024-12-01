using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solvers._2023;

public partial class CamelPokerSolver : ISolver
{
    public object Solve(string[] lines, int part)
    {
        var hands = lines
            .Select(line => {
                var lineMatch = InputLineRegex().Match(line);
                return new PokerHand{
                    Cards = lineMatch.Groups[1].Value,
                    Bid = int.Parse(lineMatch.Groups[2].Value),
                    UseJokerRule = part == 2
                };
            })
            .Order()
            .ToArray();
        var total = 0;
        for(var i = 0; i < hands.Length; i++) {
            total += hands[i].Bid * (i + 1);
        }
        return total;
    }

    public partial class PokerHand : IComparable<PokerHand>
    {
        private enum HandType {
            FiveOfAKind = 6,
            FourOfAKind = 5,
            FullHouse = 4,
            ThreeOfAKind = 3,
            TwoPair = 2,
            OnePair = 1,
            HighCard = 0
        }

        public required string Cards { get; init; }
        public required int Bid { get; init; }
        public required bool UseJokerRule { get; init; }

        private static HandType TypeForHand(string hand) {
            var sortedHand = new string(hand.Order().ToArray());
            if (FiveRegex().Match(sortedHand).Success) {
                return HandType.FiveOfAKind;
            }
            if (FourRegex().Match(sortedHand).Success) {
                return HandType.FourOfAKind;
            }
            if (FullHouseRegex().Match(sortedHand).Success
            || FullHouseRegex().Match(sortedHand.Reverse()).Success) {
                return HandType.FullHouse;
            }
            if (ThreeRegex().Match(sortedHand).Success) {
                return HandType.ThreeOfAKind;
            }
            var twoMatches = TwoRegex().Matches(sortedHand).Count;
            if (twoMatches == 2) {
                return HandType.TwoPair;
            }
            if (twoMatches == 1) {
                return HandType.OnePair;
            }
            return HandType.HighCard;
        }

        private static HandType BestTypeForHand(string hand) {
            var distinctNonJokers = hand
                .Where(card => card != 'J')
                .Distinct()
                .ToArray();
            if (distinctNonJokers.Length == 0) {
                return HandType.FiveOfAKind;
            }

            var jokerPositions = hand
                .Select((card, index) => (card, index))
                .Where(tuple => tuple.card == 'J')
                .Select(tuple => tuple.index)
                .ToArray();
            var jokerCount = jokerPositions.Length;
            var characterIndices = new int[jokerCount];
            var replacementCharacters = new char[jokerCount];
            Array.Fill(characterIndices, 0);
            Array.Fill(replacementCharacters, distinctNonJokers[0]);

            var bestType = HandType.HighCard;
            while(true) {
                var handToCheckBuilder = new StringBuilder(hand);
                for(var i = 0; i < jokerCount; i++) {
                    var jokerPosition = jokerPositions[i];
                    handToCheckBuilder[jokerPosition] = replacementCharacters[i];
                }
                var handToCheck = handToCheckBuilder.ToString();
                var typeForHand = TypeForHand(handToCheck);
                if (typeForHand > bestType) {
                    bestType = typeForHand;
                }
                for (var i = jokerCount - 1; i >= 0; i--) {
                    if (characterIndices[i] == distinctNonJokers.Length - 1) {
                        characterIndices[i] = 0;
                        continue;
                    }
                    characterIndices[i]++;
                    break;
                }
                if (characterIndices.All(i => i == 0)) {
                    break;
                }
                for(var i = 0; i < jokerCount; i++) {
                    replacementCharacters[i] = distinctNonJokers[characterIndices[i]];
                }
            }

            return bestType;
        }

        private int ValueForCard(char card) {
            if (int.TryParse(card.ToString(), out var cardValue)) {
                return cardValue;
            }
            return card.ToString().ToUpper() switch {
                "T" => 10,
                "J" => UseJokerRule ? 1 : 11,
                "Q" => 12,
                "K" => 13,
                "A" => 14,
                _ => throw new InvalidOperationException($"Invalid card detected: {card}")
            };
        }

        public int CompareTo(PokerHand? other)
        {
            if (other == null) {
                throw new InvalidOperationException("Null comparison is not supported");
            }
            HandType currentHandType;
            HandType otherHandType;
            if (UseJokerRule) {
                currentHandType = BestTypeForHand(Cards);
                otherHandType = BestTypeForHand(other.Cards);
            } else {
                currentHandType = TypeForHand(Cards);
                otherHandType = TypeForHand(other.Cards);
            }

            var comparison = currentHandType - otherHandType;

            if (comparison != 0) {
                return comparison;
            }

            for(var i = 0; i < 5; i++) {
                var currentCardValue = ValueForCard(Cards[i]);
                var otherCardValue = ValueForCard(other.Cards[i]);
                var cardComparison = currentCardValue - otherCardValue;
                if (cardComparison != 0) {
                    return cardComparison;
                }
            }

            return 0;
        }

        [GeneratedRegex("(.)\\1{4}")]
        private static partial Regex FiveRegex();
        [GeneratedRegex("(.)\\1{3}")]
        private static partial Regex FourRegex();
        [GeneratedRegex("(.)\\1{2}(.)\\2")]
        private static partial Regex FullHouseRegex();
        [GeneratedRegex("(.)\\1{2}")]
        private static partial Regex ThreeRegex();
        [GeneratedRegex("(.)\\1")]
        private static partial Regex TwoRegex();
    }

    [GeneratedRegex("(\\w{5}) (\\d+)")]
    private static partial Regex InputLineRegex();
}