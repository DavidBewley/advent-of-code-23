namespace AdventOfCode23.Processors
{
    public class PlantGrowthProcessor
    {
        private readonly string _input;
        private List<int> _seeds;
        private readonly List<MapRoute> _routes;
        private Dictionary<int, int> _seedToSoil;
        private Dictionary<int, int> _soilToFertilizer;
        private Dictionary<int, int> _fertilizerToWater;
        private Dictionary<int, int> _waterToLight;
        private Dictionary<int, int> _lightToTemperature;
        private Dictionary<int, int> _temperatureToHumidity;
        private Dictionary<int, int> _humidityToLocation;

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

            _seedToSoil = CreateMap(_routes.Where(r => r.MapType == MapType.SeedToSoil).ToList());
            _soilToFertilizer = CreateMap(_routes.Where(r => r.MapType == MapType.SoilToFertilizer).ToList());
            _fertilizerToWater = CreateMap(_routes.Where(r => r.MapType == MapType.FertilizerToWater).ToList());
            _waterToLight = CreateMap(_routes.Where(r => r.MapType == MapType.WaterToLight).ToList());
            _lightToTemperature = CreateMap(_routes.Where(r => r.MapType == MapType.LightToTemp).ToList());
            _temperatureToHumidity = CreateMap(_routes.Where(r => r.MapType == MapType.TempToHumidity).ToList());
            _humidityToLocation = CreateMap(_routes.Where(r => r.MapType == MapType.HumidityToLocation).ToList());
        }

        private void AddRoutes(string splitText, MapType mapType)
        {
            foreach (var route in _input.Split(splitText)[1].Split("\n\n")[0].Split("\n").Where(s => !string.IsNullOrEmpty(s)))
                _routes.Add(new MapRoute(route, mapType));
        }

        private Dictionary<int,int> CreateMap(List<MapRoute> routes)
        {
            var resultRoutes = new Dictionary<int, int>();
            foreach (var route in routes)
            {
                for (int i = 0; i < route.RangeLength; i++)
                {
                    resultRoutes.Add(route.SourceRange + i, route.DestinationRange + i);
                }
            }
            return resultRoutes;
        }

        public int FindClosetSeedLocation(int? seed = null)
        {
            _seeds = seed != null
                ? new() { seed.Value }
                : _input.Split("seeds: ")[1].Split('\n')[0].Split(' ').Select(int.Parse).ToList();

            var lowestLocation = int.MaxValue;
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

        private int GetValueFromDictionaryMap(Dictionary<int, int> map, int source) 
            => !map.ContainsKey(source) ? source : map[source];
    }

    public class MapRoute
    {
        public int DestinationRange { get; set; }
        public int SourceRange { get; set; }
        public int RangeLength { get; set; }
        public MapType MapType { get; set; }

        public MapRoute(string route, MapType type)
        {
            var routeValues = route.Split(' ');
            DestinationRange = int.Parse(routeValues[0]);
            SourceRange = int.Parse(routeValues[1]);
            RangeLength = int.Parse(routeValues[2]);
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
