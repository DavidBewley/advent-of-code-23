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
        [InlineData("AAAAA",HandType.FiveOfAKind)]
        [InlineData("AAAAK",HandType.FourOfAKind)]
        [InlineData("KAAAA",HandType.FourOfAKind)]
        [InlineData("AAKAA",HandType.FourOfAKind)]
        [InlineData("AAAKK", HandType.FullHouse)]
        [InlineData("KKAAA", HandType.FullHouse)]
        [InlineData("AAKAK", HandType.FullHouse)]
        [InlineData("AAAKQ",HandType.ThreeOfAKind)]
        [InlineData("KQAAA",HandType.ThreeOfAKind)]
        [InlineData("AQAKA",HandType.ThreeOfAKind)]
        [InlineData("AAKKQ",HandType.TwoPair)]
        [InlineData("AQAKK",HandType.TwoPair)]
        [InlineData("AAKQJ",HandType.OnePair)]
        [InlineData("AKQJA",HandType.OnePair)]
        [InlineData("AKQJT",HandType.HighCard)]
        public void HandsAreAssignedCorrectTypes(string hand, HandType expectedHandType)
            => new CamelCardHand(hand, 0).HandType.Should().Be(expectedHandType);
    }
}
