using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode23.Processors
{
    public class BoatRaceProcessor
    {
        public long GetNumberOfWaysToWin(string input)
        {
            var times = input.Split("Time:      ")[1].Split("\n")[0].Split(' ').Where(s => !string.IsNullOrEmpty(s)).ToList();
            var distances = input.Split("Distance:  ")[1].Split("\n")[0].Split(' ').Where(s => !string.IsNullOrEmpty(s)).ToList();
            long result = 1;

            for (int i = 0; i < times.Count; i++)
            {
                result *= new BoatRace(long.Parse(times[i]), long.Parse(distances[i])).NumberOfWaysToWin;
            }

            return result;
        }

        public long GetNumberOfWaysToWinBadKerning(string input)
        {
            var time = input.Split("Time:      ")[1].Split("\n")[0].Replace(" ", "");
            var distance = input.Split("Distance:  ")[1].Split("\n")[0].Replace(" ", "");
            long result = 1;

            result *= new BoatRace(long.Parse(time), long.Parse(distance)).NumberOfWaysToWin;

            return result;
        }
    }

    public class BoatRace
    {
        public long Time { get; set; }
        public long RecordDistance { get; set; }
        public long NumberOfWaysToWin { get; set; }

        public BoatRace(long time, long recordDistance)
        {
            Time = time;
            RecordDistance = recordDistance;

            long firstWin = 0;

            if (time < 1000)
            {
                for (int i = 0; i < Time; i++)
                {
                    if (CalculateIfWon(i, Time - i))
                    {
                        firstWin = i;
                        break;
                    }
                }
            }
            else
            {
                var inputsToOutputs = new Dictionary<long, long>();

                for (long i = 0; i < Time; i += 1000000)
                    inputsToOutputs.Add(i, CalculateDistanceTraveled(i, Time - i));


                var closestWinToRecord = inputsToOutputs.Where(w => w.Value > recordDistance).MinBy(r => r.Key).Key;
                closestWinToRecord = GetClosestNumberToRecord(closestWinToRecord, 1000000);
                closestWinToRecord = GetClosestNumberToRecord(closestWinToRecord, 100000);
                closestWinToRecord = GetClosestNumberToRecord(closestWinToRecord, 10000);
                closestWinToRecord = GetClosestNumberToRecord(closestWinToRecord, 1000);

                for (long i = closestWinToRecord; i > closestWinToRecord - 100; i--)
                {
                    if (CalculateIfWon(i, Time - i)) 
                        continue;

                    firstWin = i + 1;
                    break;
                }
            }

            NumberOfWaysToWin = Time - firstWin - firstWin + 1;

        }

        private long GetClosestNumberToRecord(long currentClosest, int numberOfSkips)
        {
            var inputsToOutputs = new Dictionary<long, long>();

            for (long i = currentClosest; i > currentClosest - numberOfSkips*10; i -= numberOfSkips)
                inputsToOutputs.Add(i, CalculateDistanceTraveled(i, Time - i));

            return inputsToOutputs.Where(w => w.Value > RecordDistance).MinBy(r => r.Key).Key;
        }

        private bool CalculateIfWon(long ticksToAccelerate, long ticksToCoast)
            => CalculateDistanceTraveled(ticksToAccelerate, ticksToCoast) > RecordDistance;

        private long CalculateDistanceTraveled(long ticksToAccelerate, long ticksToCoast)
        {
            long distanceTraveled = 0;
            long speed = 0;
            for (long i = 0; i < ticksToAccelerate; i++)
            {
                speed++;
            }

            for (long i = 0; i < ticksToCoast; i++)
            {
                distanceTraveled += speed;
            }

            return distanceTraveled;
        }
    }
}
