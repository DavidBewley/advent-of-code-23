namespace AdventOfCode23.Processors
{
    public class PlantGrowthProcessor
    {
        private readonly string _input;
        private List<long> _seeds;
        private readonly List<MapRoute> _routes;
        private Dictionary<long, long> _seedToSoil;
        private Dictionary<long, long> _soilToFertilizer;
        private Dictionary<long, long> _fertilizerToWater;
        private Dictionary<long, long> _waterToLight;
        private Dictionary<long, long> _lightToTemperature;
        private Dictionary<long, long> _temperatureToHumidity;
        private Dictionary<long, long> _humidityToLocation;

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

        private async Task<Dictionary<long,long>> CreateMap(List<MapRoute> routes)
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

        public async Task<long> FindClosetSeedLocation(long? seed = null)
        {
            var map1 = CreateMap(_routes.Where(r => r.MapType == MapType.SeedToSoil).ToList());
            var map2 = CreateMap(_routes.Where(r => r.MapType == MapType.SoilToFertilizer).ToList());
            var map3 = CreateMap(_routes.Where(r => r.MapType == MapType.FertilizerToWater).ToList());
            var map4 = CreateMap(_routes.Where(r => r.MapType == MapType.WaterToLight).ToList());
            var map5 = CreateMap(_routes.Where(r => r.MapType == MapType.LightToTemp).ToList());
            var map6 = CreateMap(_routes.Where(r => r.MapType == MapType.TempToHumidity).ToList());
            var map7 = CreateMap(_routes.Where(r => r.MapType == MapType.HumidityToLocation).ToList());

            var results = await Task.WhenAll(map1, map2, map3, map4, map5, map6, map7);

            _seedToSoil = results[0];
            _soilToFertilizer = results[1];
            _fertilizerToWater = results[2];
            _waterToLight = results[3];
            _lightToTemperature = results[4];
            _temperatureToHumidity = results[5];
            _humidityToLocation = results[6];

            _seeds = seed != null
                ? new() { seed.Value }
                : _input.Split("seeds: ")[1].Split('\n')[0].Split(' ').Select(long.Parse).ToList();

            var lowestLocation = long.MaxValue;
            foreach (var s in _seeds)
            {
                var mappedValue = GetValueFromDictionaryMap(_seedToSoil, s);
                mappedValue = GetValueFromDictionaryMap(_soilToFertilizer, mappedValue);
                mappedValue = GetValueFromDictionaryMap(_fertilizerToWater, mappedValue);
                mappedValue = GetValueFromDictionaryMap(_waterToLight, mappedValue);
                mappedValue = GetValueFromDictionaryMap(_lightToTemperature, mappedValue);
                mappedValue = GetValueFromDictionaryMap(_temperatureToHumidity, mappedValue);
                mappedValue = GetValueFromDictionaryMap(_humidityToLocation, mappedValue);
                if(mappedValue < lowestLocation)
                    lowestLocation = mappedValue;
            }

            return lowestLocation;
        }

        private long GetValueFromDictionaryMap(Dictionary<long, long> map, long source) 
            => !map.ContainsKey(source) ? source : map[source];
    }

    public class MapRoute
    {
        public long DestinationRange { get; set; }
        public long SourceRange { get; set; }
        public long RangeLength { get; set; }
        public MapType MapType { get; set; }

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
