using AdventOfCode23.Processors;
using FluentAssertions;

namespace AdventOfCode23.Specs
{
    public class _07_CamelCards
    {
        private const string TestInput = "32T3K 765\nT55J5 684\nKK677 28 \nKTJJT 220\nQQQJA 483\n";

        [Fact]
        public void TestInputHandsGivesCorrectResult()
            => new CamelCardsProcessor().ProcessHands(TestInput).Should().Be(6440);

        [Theory]
        [InlineData("AAAAA", HandType.FiveOfAKind)]
        [InlineData("AAAAK", HandType.FourOfAKind)]
        [InlineData("KAAAA", HandType.FourOfAKind)]
        [InlineData("AAKAA", HandType.FourOfAKind)]
        [InlineData("AAAKK", HandType.FullHouse)]
        [InlineData("KKAAA", HandType.FullHouse)]
        [InlineData("AAKAK", HandType.FullHouse)]
        [InlineData("AAAKQ", HandType.ThreeOfAKind)]
        [InlineData("KQAAA", HandType.ThreeOfAKind)]
        [InlineData("AQAKA", HandType.ThreeOfAKind)]
        [InlineData("AAKKQ", HandType.TwoPair)]
        [InlineData("AQAKK", HandType.TwoPair)]
        [InlineData("AAKQJ", HandType.OnePair)]
        [InlineData("AKQJA", HandType.OnePair)]
        [InlineData("AKQJT", HandType.HighCard)]
        public void HandsAreAssignedCorrectTypes(string hand, HandType expectedHandType)
            => new CamelCardHand(hand, 0).HandType.Should().Be(expectedHandType);

        [Fact]
        public void InputFileProducesResultPart1()
        {
            var input = File.ReadAllText("Inputs/07-CamelCards.txt");
            new CamelCardsProcessor().ProcessHands(input).Should().Be(248453531);
        }

        [Theory]
        [InlineData("JJJJJ", HandType.FiveOfAKind)]
        [InlineData("AAAAA", HandType.FiveOfAKind)]
        [InlineData("JAAAA", HandType.FiveOfAKind)]
        [InlineData("AAJAA", HandType.FiveOfAKind)]
        [InlineData("AJJJJ", HandType.FiveOfAKind)]
        [InlineData("AAJJJ", HandType.FiveOfAKind)]
        [InlineData("AAAJJ", HandType.FiveOfAKind)]
        [InlineData("AAAAJ", HandType.FiveOfAKind)]
        [InlineData("AAAAK", HandType.FourOfAKind)]
        [InlineData("AAAJK", HandType.FourOfAKind)]
        [InlineData("JAAAK", HandType.FourOfAKind)]
        [InlineData("KKAAA", HandType.FullHouse)]
        [InlineData("AAKAK", HandType.FullHouse)]
        [InlineData("AAAKQ", HandType.ThreeOfAKind)]
        [InlineData("KQAAA", HandType.ThreeOfAKind)]
        [InlineData("AQAKA", HandType.ThreeOfAKind)]
        [InlineData("AAJKQ", HandType.ThreeOfAKind)]
        [InlineData("AJJKQ", HandType.ThreeOfAKind)]
        [InlineData("AAKKQ", HandType.TwoPair)]
        [InlineData("AQAKK", HandType.TwoPair)]
        [InlineData("AJKQT", HandType.OnePair)]
        [InlineData("A7KQJ", HandType.OnePair)]
        [InlineData("AKQJT", HandType.OnePair)]
        public void HandsAreAssignedCorrectTypesJoker(string hand, HandType expectedHandType)
            => new CamelCardHand(hand, 0, true).HandType.Should().Be(expectedHandType);

        [Fact]
        public void TestInputHandsGivesCorrectResultWithJokers()
            => new CamelCardsProcessor().ProcessHandsWithJoker(TestInput).Should().Be(5905);

        [Fact]
        public void InputFileProducesResultPart2()
        {
            var input = File.ReadAllText("Inputs/07-CamelCards.txt");
            new CamelCardsProcessor().ProcessHandsWithJoker(input).Should().Be(248781813);
        }

        //249083748
        //249083748
    }
}
