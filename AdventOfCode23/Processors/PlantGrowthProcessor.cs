using System.Runtime.InteropServices;

namespace AdventOfCode23.Processors
{
    public class PlantGrowthProcessor
    {
        private readonly string _input;
        private readonly List<MapRoute> _routes;

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

        private async Task<Dictionary<long, long>> CreateMap(List<MapRoute> routes)
        {
            var resultRoutes = new Dictionary<long, long>();
            foreach (var route in routes)
            {
                for (long i = 0; i < route.RangeLength; i++)
                {
                    resultRoutes.Add(route.SourceRange + i, route.DestinationRange + i);
                }
            }
            return resultRoutes;
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
            //Do each of them on a thread for every pairing?
            //Check the lowest of each thread?
            var seeds = new List<long>();
            var inputSeeds = _input.Split("seeds: ")[1].Split('\n')[0].Split(' ').Select(long.Parse).ToList();
            for (var i = 0; i < inputSeeds.Count; i += 2)
                for (long j = inputSeeds[0]; j < inputSeeds[1] + inputSeeds[0]; j++)
                    seeds.Add(j);

            return DetermineLowestSeedPlot(seeds);
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

        public MapRoute(string route, MapType type)
        {
            var routeValues = route.Split(' ');
            DestinationRange = long.Parse(routeValues[0]);
            SourceRange = long.Parse(routeValues[1]);
            RangeLength = long.Parse(routeValues[2]);
            MapType = type;
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
