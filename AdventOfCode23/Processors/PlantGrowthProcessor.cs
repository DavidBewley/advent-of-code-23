using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;

namespace AdventOfCode23.Processors
{
    public class PlantGrowthProcessor
    {
        private readonly string _input;
        private readonly List<MapRoute> _routes;
        private List<SeedRange> _nextSeedRanges;

        public PlantGrowthProcessor(string input)
        {
            _input = input;
            _routes = new List<MapRoute>();

            AddRoutes("seed-to-soil map:", MapType.SeedToSoil);
            AddRoutes("soil-to-fertilizer map:", MapType.SoilToFertilizer);
            AddRoutes("fertilizer-to-water map:", MapType.FertilizerToWater);
            AddRoutes("water-to-light map:", MapType.WaterToLight);
            AddRoutes("light-to-temperature map:", MapType.LightToTemp);
            AddRoutes("temperature-to-humidity map:", MapType.TempToHumidity);
            AddRoutes("humidity-to-location map:", MapType.HumidityToLocation);
        }

        private void AddRoutes(string splitText, MapType mapType)
        {
            foreach (var route in _input.Split(splitText)[1].Split("\n\n")[0].Split("\n").Where(s => !string.IsNullOrEmpty(s)))
                _routes.Add(new MapRoute(route, mapType));
        }

        public long FindClosetSeedLocation(long? seed = null)
        {
            var seeds = seed != null
                ? new() { seed.Value }
                : _input.Split("seeds: ")[1].Split('\n')[0].Split(' ').Select(long.Parse).ToList();

            return DetermineLowestSeedPlot(seeds);
        }

        public long FindClosetSeedLocationWithRange()
        {
            var seedRanges = new List<SeedRange>();
            var inputSeeds = _input.Split("seeds: ")[1].Split('\n')[0].Split(' ').Select(long.Parse).ToList();

            for (var i = 0; i < inputSeeds.Count; i += 2)
                seedRanges.Add(new SeedRange(inputSeeds[i], inputSeeds[i + 1]));

            _nextSeedRanges = new List<SeedRange>();

            GetNextSeedRanges(MapType.SeedToSoil, seedRanges);
            var newSeedRange = _nextSeedRanges = _nextSeedRanges.ConvertAll(range => new SeedRange(range.RangeStart, range.RangeLength));
            _nextSeedRanges = new List<SeedRange>();
            GetNextSeedRanges(MapType.SoilToFertilizer, newSeedRange);
            newSeedRange = _nextSeedRanges = _nextSeedRanges.ConvertAll(range => new SeedRange(range.RangeStart, range.RangeLength));
            _nextSeedRanges = new List<SeedRange>();
            GetNextSeedRanges(MapType.FertilizerToWater, newSeedRange);
            newSeedRange = _nextSeedRanges = _nextSeedRanges.ConvertAll(range => new SeedRange(range.RangeStart, range.RangeLength));
            _nextSeedRanges = new List<SeedRange>();
            GetNextSeedRanges(MapType.WaterToLight, newSeedRange);
            newSeedRange = _nextSeedRanges = _nextSeedRanges.ConvertAll(range => new SeedRange(range.RangeStart, range.RangeLength));
            _nextSeedRanges = new List<SeedRange>();
            GetNextSeedRanges(MapType.LightToTemp, newSeedRange);
            newSeedRange = _nextSeedRanges = _nextSeedRanges.ConvertAll(range => new SeedRange(range.RangeStart, range.RangeLength));
            _nextSeedRanges = new List<SeedRange>();
            GetNextSeedRanges(MapType.TempToHumidity, newSeedRange);
            newSeedRange = _nextSeedRanges = _nextSeedRanges.ConvertAll(range => new SeedRange(range.RangeStart, range.RangeLength));
            _nextSeedRanges = new List<SeedRange>();
            GetNextSeedRanges(MapType.HumidityToLocation, newSeedRange);

            return _nextSeedRanges.OrderBy(r=>r.RangeStart).First().RangeStart;
        }

        private void GetNextSeedRanges(MapType mapType, List<SeedRange> currentRanges)
        {
            foreach (var range in currentRanges)
            {
                var validRoutes = _routes.Where(r => r.MapType == mapType).ToList();
                var partialRoutes = validRoutes.Where(r => r.ContainsPartialSeedRange(range) && !r.ContainsFullSeedRange(range)).ToList();
                var fullRoute = validRoutes.Where(r => r.ContainsFullSeedRange(range)).ToList();

                if (!validRoutes.Any() && !partialRoutes.Any())
                {
                    _nextSeedRanges.Add(range);
                    continue;
                }

                if (fullRoute.Any())
                {
                    _nextSeedRanges.Add(fullRoute.First().ConvertToNewSeedRange(range));
                    continue;
                }

                var splits = new List<SeedRange>();
                foreach (var route in partialRoutes)
                {
                    splits = route.SplitSeedRangeInTwo(range).ToList();
                    Console.WriteLine();
                }
                GetNextSeedRanges(mapType, splits);
            }
        }

        private long DetermineLowestSeedPlot(List<long> seeds)
        {
            var lowestLocation = long.MaxValue;
            foreach (var s in seeds)
            {
                var mappedValue = GetDestinationFromRoute(s, MapType.SeedToSoil);
                mappedValue = GetDestinationFromRoute(mappedValue, MapType.SoilToFertilizer);
                mappedValue = GetDestinationFromRoute(mappedValue, MapType.FertilizerToWater);
                mappedValue = GetDestinationFromRoute(mappedValue, MapType.WaterToLight);
                mappedValue = GetDestinationFromRoute(mappedValue, MapType.LightToTemp);
                mappedValue = GetDestinationFromRoute(mappedValue, MapType.TempToHumidity);
                mappedValue = GetDestinationFromRoute(mappedValue, MapType.HumidityToLocation);

                if (mappedValue < lowestLocation)
                    lowestLocation = mappedValue;
            }
            return lowestLocation;
        }

        private long GetDestinationFromRoute(long input, MapType mapType)
        {
            var validRoutes = _routes.Where(r => r.MapType == mapType && r.SourceNumberIsInRoute(input));
            if (validRoutes.Count() > 1)
                throw new Exception("Multiple routes found!");

            return validRoutes.Any()
                ? validRoutes.First().GetDestinationNumber(input)
                : input;
        }
    }

    public class MapRoute
    {
        public long DestinationRange { get; set; }
        public long SourceRange { get; set; }
        public long RangeLength { get; set; }
        public MapType MapType { get; set; }

        public bool SourceNumberIsInRoute(long source)
            => source >= SourceRange && source < SourceRange + RangeLength;

        public long GetDestinationNumber(long source)
        {
            var diff = source - SourceRange;
            return DestinationRange + diff;
        }

        public bool ContainsFullSeedRange(SeedRange range) => range.RangeStart >= SourceRange && range.GetFinalSeedValue() < SourceRange + RangeLength;

        public bool ContainsPartialSeedRange(SeedRange range)
        {
            if (range.RangeStart >= SourceRange && range.GetFinalSeedValue() < SourceRange + RangeLength)
                return true;

            return false;
        }

        public List<SeedRange> SplitSeedRangeInTwo(SeedRange range)
        {
            var rangeList = new List<SeedRange>();
            if (range.RangeStart > SourceRange)
            {
                //Split must be at the other end
                var diff = SourceRange + RangeLength - range.GetFinalSeedValue();
                rangeList.Add(new SeedRange(range.RangeStart, diff));
                rangeList.Add(new SeedRange(SourceRange + RangeLength, diff));
            }

            return rangeList;
        }

        public SeedRange ConvertToNewSeedRange(SeedRange oldRange)
            => new(GetDestinationNumber(oldRange.RangeStart), oldRange.RangeLength);

        public MapRoute(string route, MapType type)
        {
            var routeValues = route.Split(' ');
            DestinationRange = long.Parse(routeValues[0]);
            SourceRange = long.Parse(routeValues[1]);
            RangeLength = long.Parse(routeValues[2]);
            MapType = type;
        }
    }

    public class SeedRange
    {
        public long RangeStart { get; set; }
        public long RangeLength { get; set; }
        public long GetFinalSeedValue() => RangeStart + RangeLength;
        public SeedRange(long rangeStart, long rangeLength)
        {
            RangeStart = rangeStart;
            RangeLength = rangeLength;
        }
    }

    public enum MapType
    {
        SeedToSoil,
        SoilToFertilizer,
        FertilizerToWater,
        WaterToLight,
        LightToTemp,
        TempToHumidity,
        HumidityToLocation,
    }
}
