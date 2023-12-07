using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode23.Processors
{
    public class CamelCardsProcessor
    {
        public long ProcessHands(string input)
        {
            var hands = input
                .Split('\n')
                .Where(i => !string.IsNullOrEmpty(i)).ToList()
                .Select(hand => new CamelCardHand(hand.Split(' ')[0], int.Parse(hand.Split(' ')[1])))
                .OrderByDescending(hand => hand.HandType)
                .ThenBy(hand => hand.GetCardValue(0))
                .ThenBy(hand => hand.GetCardValue(1))
                .ThenBy(hand => hand.GetCardValue(2))
                .ThenBy(hand => hand.GetCardValue(3))
                .ThenBy(hand => hand.GetCardValue(4))
                .ToList();

            var result = 0L;
            for (var i = 0; i < hands.Count; i++)
                result += (i + 1) * hands[i].Bid;

            return result;
        }

        public long ProcessHandsWithJoker(string input)
        {
            var hands = input
                .Split('\n')
                .Where(i => !string.IsNullOrEmpty(i)).ToList()
                .Select(hand => new CamelCardHand(hand.Split(' ')[0], int.Parse(hand.Split(' ')[1]), true))
                .OrderByDescending(hand => hand.HandType)
                .ThenBy(hand => hand.GetCardValue(0))
                .ThenBy(hand => hand.GetCardValue(1))
                .ThenBy(hand => hand.GetCardValue(2))
                .ThenBy(hand => hand.GetCardValue(3))
                .ThenBy(hand => hand.GetCardValue(4))
                .ToList();

            var result = 0L;
            for (var i = 0; i < hands.Count; i++)
                result += (i + 1) * hands[i].Bid;

            return result;
        }
    }

    public class CamelCardHand
    {
        public string Hand { get; set; }
        public int Bid { get; set; }
        public HandType HandType { get; set; }
        public bool ContainsJoker { get; set; }
        private int JokerCount { get; set; }

        private readonly Dictionary<char, int> _cardValues = new()
        {
            { 'A', 14 },
            { 'K', 13 },
            { 'Q', 12 },
            { 'J', 11 },
            { 'T', 10 },
            { '9', 9 },
            { '8', 8 },
            { '7', 7 },
            { '6', 6 },
            { '5', 5 },
            { '4', 4 },
            { '3', 3 },
            { '2', 2 },
        };

        private readonly Dictionary<char, int> _cardValuesJoker = new()
        {
            { 'A', 14 },
            { 'K', 13 },
            { 'Q', 12 },
            { 'T', 10 },
            { '9', 9 },
            { '8', 8 },
            { '7', 7 },
            { '6', 6 },
            { '5', 5 },
            { '4', 4 },
            { '3', 3 },
            { '2', 2 },
            { 'J', 1 },
        };

        public CamelCardHand(string hand, int bid, bool useJoker = false)
        {
            Hand = hand;
            Bid = bid;
            JokerCount = hand.Count(h => h == 'J');
            ContainsJoker = useJoker && JokerCount > 0;
            HandType = DetermineHandType(useJoker);
        }

        public int GetCardValue(int positionInHand)
            => ContainsJoker
                ? _cardValuesJoker[Hand[positionInHand]]
                : _cardValues[Hand[positionInHand]];

        private HandType DetermineHandType(bool useJoker = false)
        {
            if (Hand == "JJJJJ")
                return HandType.FiveOfAKind;
            if (IsFiveOfAKind(useJoker))
                return HandType.FiveOfAKind;
            if (IsFourOfAKind(useJoker))
                return HandType.FourOfAKind;
            if (IsFullHouse(useJoker))
                return HandType.FullHouse;
            if (IsThreeOfAKind(useJoker))
                return HandType.ThreeOfAKind;
            if (IsTwoPair(useJoker))
                return HandType.TwoPair;
            if (IsOnePair(useJoker))
                return HandType.OnePair;

            return HandType.HighCard;
        }

        private bool IsFiveOfAKind(bool useJoker)
        {
            if (useJoker && Hand.GroupBy(h => h).Where(h => h.Key != 'J').OrderByDescending(h => h.Count()).First().Count() + JokerCount == 5)
                return true;
            return Hand.GroupBy(h => h).OrderByDescending(h => h.Count()).First().Count() == 5;
        }

        private bool IsFourOfAKind(bool useJoker)
        {
            if (useJoker && Hand.GroupBy(h => h).Where(h => h.Key != 'J').OrderByDescending(h => h.Count()).First().Count() + JokerCount == 4)
                return true;
            return Hand.GroupBy(h => h).OrderByDescending(h => h.Count()).First().Count() == 4;
        }

        private bool IsFullHouse(bool useJoker)
        {
            if (useJoker && Hand.GroupBy(h => h).Where(h => h.Key != 'J').OrderByDescending(h => h.Count()).ToList()[0].Count()
                         + Hand.GroupBy(h => h).Where(h => h.Key != 'J').OrderByDescending(h => h.Count()).ToList()[1].Count()
                         + JokerCount == 5)
                return true;
            return Hand.GroupBy(h => h).OrderByDescending(h => h.Count()).ToList()[0].Count() == 3
                   && Hand.GroupBy(h => h).OrderByDescending(h => h.Count()).ToList()[1].Count() == 2;
        }

        private bool IsThreeOfAKind(bool useJoker)
        {
            if (useJoker && Hand.GroupBy(h => h).Where(h => h.Key != 'J').OrderByDescending(h => h.Count()).First().Count() + JokerCount == 3)
                return true;
            return Hand.GroupBy(h => h).OrderByDescending(h => h.Count()).First().Count() == 3;
        }

        private bool IsTwoPair(bool useJoker)
        {
            if (useJoker && JokerCount > 0) 
                return false;

            return Hand.GroupBy(h => h).OrderByDescending(h => h.Count()).ToList()[0].Count() == 2
                   && Hand.GroupBy(h => h).OrderByDescending(h => h.Count()).ToList()[1].Count() == 2;
        }

        private bool IsOnePair(bool useJoker)
        {
            if (useJoker && JokerCount > 0)
                return true;
            return Hand.GroupBy(h => h).OrderByDescending(h => h.Count()).First().Count() == 2;
        }
    }

    public enum HandType
    {
        FiveOfAKind,
        FourOfAKind,
        FullHouse,
        ThreeOfAKind,
        TwoPair,
        OnePair,
        HighCard
    }
}
