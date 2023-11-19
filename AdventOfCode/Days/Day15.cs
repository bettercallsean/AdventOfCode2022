using System.Text.RegularExpressions;

namespace AdventOfCode.Days;

public partial class Day15 : BaseDay
{
    private readonly List<List<(int, int)>> _input;

    public Day15()
    {
        _input = File.ReadAllText(InputFilePath)
                    .Trim()
                    .Split(Environment.NewLine)
                    .Select(x => SensorAndBeaconRegex().Match(x))
                    .Select(x => new List<(int, int)>
                    {
                        (int.Parse(x.Groups[1].Value), int.Parse(x.Groups[2].Value)),
                        (int.Parse(x.Groups[3].Value), int.Parse(x.Groups[4].Value)),
                    })
                    .ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        const int YValue = 2000000;
        var lines = new List<int>();

        foreach (var sensorAndBeacon in _input)
        {
            var manhatanDistance = Math.Abs(sensorAndBeacon[0].Item1 - sensorAndBeacon[1].Item1) + Math.Abs(sensorAndBeacon[0].Item2 - sensorAndBeacon[1].Item2);
            var maxHeight = sensorAndBeacon[0].Item2 + manhatanDistance;
            var minHeight = sensorAndBeacon[0].Item2 - manhatanDistance;

            if (minHeight <= YValue && YValue <= maxHeight)
            {
                var distanceAwayFromSensor = Math.Abs(sensorAndBeacon[0].Item2 - YValue);
                var rightX = sensorAndBeacon[0].Item1 + manhatanDistance - distanceAwayFromSensor;
                var leftX = sensorAndBeacon[0].Item1 - manhatanDistance + distanceAwayFromSensor;

                lines.Add(leftX);
                lines.Add(rightX);
            }
        }

        lines.Sort();

        return new ValueTask<string>((lines[^1] - lines[0]).ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        const int MaxSearchSpace = 4000000;

        var beaconLocation = (0UL, 0UL);

        for (var i = 0; i < MaxSearchSpace; i++)
        {
            var lines = new List<(int, int)>();
            foreach (var sensorAndBeacon in _input)
            {
                var manhatanDistance = Math.Abs(sensorAndBeacon[0].Item1 - sensorAndBeacon[1].Item1) + Math.Abs(sensorAndBeacon[0].Item2 - sensorAndBeacon[1].Item2);
                var maxHeight = sensorAndBeacon[0].Item2 + manhatanDistance;
                var minHeight = sensorAndBeacon[0].Item2 - manhatanDistance;

                maxHeight = maxHeight > MaxSearchSpace ? MaxSearchSpace : maxHeight;
                minHeight = minHeight < 0 ? 0 : minHeight;

                if (minHeight <= i && i <= maxHeight)
                {
                    var distanceAwayFromSensor = Math.Abs(sensorAndBeacon[0].Item2 - i);
                    var rightX = sensorAndBeacon[0].Item1 + manhatanDistance - distanceAwayFromSensor;
                    var leftX = sensorAndBeacon[0].Item1 - manhatanDistance + distanceAwayFromSensor;

                    rightX = rightX > MaxSearchSpace ? MaxSearchSpace : rightX;
                    leftX = leftX < 0 ? 0 : leftX;

                    lines.Add((leftX, rightX));
                }

            }

            lines.Sort();

            var maxXValue = 0;
            var beaconFound = false;
            for (var j = 0; j < lines.Count; j++)
            {
                var currentLine = lines[j];
                if (currentLine.Item1 <= maxXValue)
                {
                    if (currentLine.Item2 > maxXValue)
                        maxXValue = currentLine.Item2;
                }
                else if (Math.Abs(currentLine.Item1 - maxXValue) > 1)
                {
                    beaconLocation = ((ulong)(maxXValue + (currentLine.Item1 > maxXValue ? 1 : -1)), (ulong)i);
                    beaconFound = true;

                    break;
                }
            }

            if (beaconFound)
                break;
        }

        return new ValueTask<string>(((beaconLocation.Item1 * 4000000) + beaconLocation.Item2).ToString());
    }

    [GeneratedRegex("x=(-?\\d*), y=(-?\\d*): closest beacon is at x=(-?\\d*), y=(-?\\d*)")]
    private static partial Regex SensorAndBeaconRegex();
}
