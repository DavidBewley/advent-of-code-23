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
                .ToList();

            return 0;
        }
    }

    public class CamelCardHand
    {
        public string Hand { get; set; }
        public int Bid { get; set; }
        public HandType HandType { get; set; }

        public CamelCardHand(string hand, int bid)
        {
            Hand = hand;
            Bid = bid;
            HandType = DetermineHandType();
        }

        private HandType DetermineHandType()
        {
            if(IsFiveOfAKind())
                return HandType.FiveOfAKind;
            if (IsFourOfAKind())
                return HandType.FourOfAKind;
            if(IsFullHouse())
                return HandType.FullHouse;
            if (IsThreeOfAKind())
                return HandType.ThreeOfAKind;
            if (IsTwoPair())
                return HandType.TwoPair;
            if (IsOnePair())
                return HandType.OnePair;

            return HandType.HighCard;
        }

        private bool IsFiveOfAKind() 
            => Hand.GroupBy(h => h).OrderByDescending(h => h.Count()).First().Count() == 5;

        private bool IsFourOfAKind()
            => Hand.GroupBy(h => h).OrderByDescending(h => h.Count()).First().Count() == 4;

        private bool IsFullHouse()
            => Hand.GroupBy(h => h).OrderByDescending(h => h.Count()).ToList()[0].Count() == 3 
               && Hand.GroupBy(h => h).OrderByDescending(h => h.Count()).ToList()[1].Count() == 2;

        private bool IsThreeOfAKind()
            => Hand.GroupBy(h => h).OrderByDescending(h => h.Count()).First().Count() == 3;

        private bool IsTwoPair()
            => Hand.GroupBy(h => h).OrderByDescending(h => h.Count()).ToList()[0].Count() == 2
               && Hand.GroupBy(h => h).OrderByDescending(h => h.Count()).ToList()[1].Count() == 2;

        private bool IsOnePair()
            => Hand.GroupBy(h => h).OrderByDescending(h => h.Count()).First().Count() == 2;
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
