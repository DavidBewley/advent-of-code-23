namespace AdventOfCode23.Processors
{
    public class ScratchCardProcessor
    {
        public int ProcessScratchCardsPart1(string input)
        {
            var cards = input.Split('\n');
            return cards.Where(c => !string.IsNullOrEmpty(c)).Sum(card => new ScratchCard(card).GetWinningPoints());
        }

        public int ProcessScratchCardsPart2(string input)
        {
            var cards = input.Split('\n');
            var processedCards = cards.Where(c => !string.IsNullOrEmpty(c)).Select(card => new ScratchCard(card)).Reverse().ToList();

            var numberOfCardsProcessed = 0;
            var stack = new Stack<ScratchCard>(processedCards);
            while (stack.Any())
            {
                numberOfCardsProcessed++;
                var currentCard = stack.Pop();
                for (int i = 1; i <= currentCard.GetNumberOfMatches(); i++)
                {
                    stack.Push(processedCards.Single(c=>c.GameId == i + currentCard.GameId));
                }
            }

            return numberOfCardsProcessed;
        }
    }

    public class ScratchCard
    {
        private readonly Dictionary<int,int> _matchesPointValues = new()
        {
            {0,0},
            {1,1},
            {2,2},
            {3,4},
            {4,8},
            {5,16},
            {6,32},
            {7,64},
            {8,128},
            {9,256},
            {10,512},
        };

        public int GameId { get; private set; }
        public List<int> WinningNumbers { get; private set; }
        public List<int> FoundNumbers { get; private  set; }

        public ScratchCard(string card)
        {
            WinningNumbers = new List<int>();
            FoundNumbers = new List<int>();
            GameId = int.Parse(card.Split(":")[0].Split("Card ")[1]);
            foreach (var winningNumber in card.Split(':')[1].Split('|')[0].Split(' '))
            {
                if(string.IsNullOrEmpty(winningNumber))
                    continue;
                WinningNumbers.Add(int.Parse(winningNumber));
            }
            foreach (var foundNumber in card.Split('|')[1].Split(' '))
            {
                if (string.IsNullOrEmpty(foundNumber))
                    continue;
                FoundNumbers.Add(int.Parse(foundNumber));
            }
        }

        public int GetNumberOfMatches()
            => FoundNumbers.Distinct().Count(foundNumber => WinningNumbers.Distinct().Contains(foundNumber));

        public int GetWinningPoints() 
            => _matchesPointValues[GetNumberOfMatches()];
    }
}
