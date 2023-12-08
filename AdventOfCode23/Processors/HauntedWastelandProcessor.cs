using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode23.Processors
{
    public class HauntedWastelandProcessor
    {
        private readonly string _pattern;
        private readonly Dictionary<string, (string left, string right)> _mapDirections;

        public HauntedWastelandProcessor(string input)
        {
            _pattern = input.Split("\n\n")[0].Trim();
            _mapDirections = new Dictionary<string, (string left, string right)>();
            var directions = input.Split("\n\n")[1].Split("\n").Where(l => !string.IsNullOrEmpty(l)).ToList();
            foreach (var direction in directions)
            {
                var node = direction.Split(" = ")[0];
                var left = direction.Split("(")[1].Split(",")[0].Trim();
                var right = direction.Split(",")[1].Split(")")[0].Trim();
                _mapDirections.Add(node, (left, right));
            }
        }

        public int CalculateNumberOfTurnsToReachDestination()
        {
            var numberOfTurnsToReachDestination = 0;
            var destinationFound = false;
            var currentLocation = "AAA";
            var indexInPattern = 0;

            while (!destinationFound)
            {
                currentLocation = GetNextDirection(indexInPattern) == 'L' 
                    ? _mapDirections[currentLocation].left 
                    : _mapDirections[currentLocation].right;
                numberOfTurnsToReachDestination++;
                indexInPattern = GetNextIndexForPattern(indexInPattern);
                if (currentLocation == "ZZZ")
                    destinationFound = true;
            }

            return numberOfTurnsToReachDestination;
        }

        private char GetNextDirection(int indexInPattern) 
            => _pattern[indexInPattern];

        private int GetNextIndexForPattern(int indexInPattern)
        {
            indexInPattern++;
            return indexInPattern == _pattern.Length 
                ? 0 
                : indexInPattern;
        }
    }
}
