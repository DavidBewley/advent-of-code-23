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
        private readonly List<string> _ghostStartLocations;

        public HauntedWastelandProcessor(string input)
        {
            _pattern = input.Split("\n\n")[0].Trim();
            _mapDirections = new Dictionary<string, (string left, string right)>();
            var directions = input.Split("\n\n")[1].Split("\n").Where(l => !string.IsNullOrEmpty(l)).ToList();
            _ghostStartLocations = new List<string>();
            foreach (var direction in directions)
            {
                var node = direction.Split(" = ")[0];
                var left = direction.Split("(")[1].Split(",")[0].Trim();
                var right = direction.Split(",")[1].Split(")")[0].Trim();
                _mapDirections.Add(node, (left, right));
                if (node.EndsWith("A"))
                    _ghostStartLocations.Add(node);
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

        public int CalculateNumberOfTurnsForGhostsToReachEnd()
        {
            var indexInPattern = 0;
            var ghostCurrentLocations = _ghostStartLocations.Select(ghost => new GhostLocation(ghost)).ToList();


            for (int i = 0; i < 1000000; i++)
            {
                foreach (var ghost in ghostCurrentLocations)
                {
                    ghost.UpdateLocation(GetNextDirection(indexInPattern) == 'L'
                        ? _mapDirections[ghost.CurrentLocation].left
                        : _mapDirections[ghost.CurrentLocation].right);
                }

                indexInPattern = GetNextIndexForPattern(indexInPattern);
            }

            return new LcmCalculator().AddManyInputs(ghostCurrentLocations.Select(g => g.GetNumberOfMovesToHitZPattern()).ToList()).CalculateLcm();
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

    public class LcmCalculator
    {
        private readonly List<int> _inputs = new();

        public LcmCalculator AddManyInputs(List<int> inputs)
        {
            _inputs.AddRange(inputs);
            return this;
        }

        public int CalculateLcm()
        {
            var currentLcmValue = _inputs.Select(input => (input, input)).ToList();
            while (true)
            {
                for (int i = 0; i < currentLcmValue.Count; i++)
                {
                    currentLcmValue[i] = (currentLcmValue[i].Item1, currentLcmValue[i].Item2 + currentLcmValue[i].Item1);
                }

                if (currentLcmValue.Count(lcm => lcm.Item2 == currentLcmValue[0].Item2) == currentLcmValue.Count)
                    return currentLcmValue[0].Item2;
            }
        }
    }

    public class GhostLocation
    {
        public string CurrentLocation { get; set; }
        public List<int> MovesToHitZ { get; set; }

        private int _currentMove;

        public GhostLocation(string currentLocation)
        {
            CurrentLocation = currentLocation;
            MovesToHitZ = new List<int>();
            _currentMove = 0;
        }

        public void UpdateLocation(string newLocation)
        {
            CurrentLocation = newLocation;
            _currentMove++;
            if (CurrentLocation.EndsWith('Z'))
                MovesToHitZ.Add(_currentMove);
        }

        public int GetNumberOfMovesToHitZPattern()
            => MovesToHitZ[^1] - MovesToHitZ[^2];
    }
}
