namespace AdventOfCode.Days;

public class Day14 : BaseDay
{
    const int SandEntryPoint = 500;
    private readonly List<List<(int, int)>> _input;
    private readonly char[][] _cave = new char[200][];
    private int _highestYValue = 0;

    public Day14()
    {
        var tmpInput = File.ReadAllLines(InputFilePath).Select(x => x.Split(" -> "));
        _input = tmpInput.Select(x => x.Select(y => y.Split(',')).Select(x => (x: int.Parse(x[0]), y: int.Parse(x[1]))).ToList()).ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        CreateCaveWalls();

        var sandCount = 0;
        var sandIsInFreeFall = false;

        while (!sandIsInFreeFall)
        {
            var sandHasStoppedFalling = false;
            var sand = (x: SandEntryPoint, y: 0);

            while (!sandHasStoppedFalling)
            {
                if (sand.y == 199)
                {
                    sandIsInFreeFall = true;
                    sandCount--;
                    break;
                }

                if (_cave[sand.y + 1][sand.x] == '.')
                {
                    sand = (sand.x, sand.y + 1);
                }
                else
                {
                    if (_cave[sand.y + 1][sand.x - 1] == '.')
                    {
                        sand = (sand.x - 1, sand.y + 1);
                    }
                    else if (_cave[sand.y + 1][sand.x + 1] == '.')
                    {
                        sand = (sand.x + 1, sand.y + 1);
                    }
                    else
                    {
                        sandHasStoppedFalling = true;
                        _cave[sand.y][sand.x] = 'o';
                    }
                }
            }

            sandCount++;
        }

        return new ValueTask<string>(sandCount.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        CreateCaveWalls();

        var sandCount = 0;
        var sandIsInFreeFall = false;

        while (!sandIsInFreeFall)
        {
            var sandHasStoppedFalling = false;
            var sand = (x: SandEntryPoint, y: 0);

            while (!sandHasStoppedFalling)
            {
                if (sand.y == _highestYValue + 1)
                {
                    _cave[sand.y][sand.x] = 'o';
                    break;
                }

                if (_cave[sand.y + 1][sand.x] == '.')
                {
                    sand = (sand.x, sand.y + 1);
                }
                else
                {
                    if (_cave[sand.y + 1][sand.x - 1] == '.')
                    {
                        sand = (sand.x - 1, sand.y + 1);
                    }
                    else if (_cave[sand.y + 1][sand.x + 1] == '.')
                    {
                        sand = (sand.x + 1, sand.y + 1);
                    }
                    else
                    {
                        sandHasStoppedFalling = true;
                        _cave[sand.y][sand.x] = 'o';

                        if (sand == (x: SandEntryPoint, y: 0))
                        {
                            sandIsInFreeFall = true;
                        }
                    }
                }
            }

            sandCount++;
        }

        return new ValueTask<string>(sandCount.ToString());
    }

    private void CreateCaveWalls()
    {
        for (var i = 0; i < _cave.Length; i++)
        {
            _cave[i] = Enumerable.Repeat('.', 700).ToArray();
        }

        foreach (var wall in _input)
        {
            var lastWallPoint = wall[0];

            if (lastWallPoint.Item2 > _highestYValue)
                _highestYValue = lastWallPoint.Item2;

            for (var i = 1; i < wall.Count; i++)
            {
                var currentWallPoint = wall[i];
                var xDirection = lastWallPoint.Item1 - currentWallPoint.Item1;

                if (xDirection == 0)
                {
                    var start = lastWallPoint.Item2 > currentWallPoint.Item2 ? currentWallPoint.Item2 : lastWallPoint.Item2;
                    var end = start == lastWallPoint.Item2 ? currentWallPoint.Item2 : lastWallPoint.Item2;
                    for (var j = start; j <= end; j++)
                    {
                        _cave[j][lastWallPoint.Item1] = '#';
                    }
                }
                else
                {
                    var start = lastWallPoint.Item1 > currentWallPoint.Item1 ? currentWallPoint.Item1 : lastWallPoint.Item1;
                    var end = start == lastWallPoint.Item1 ? currentWallPoint.Item1 : lastWallPoint.Item1;
                    for (var j = start; j <= end; j++)
                    {
                        _cave[lastWallPoint.Item2][j] = '#';
                    }
                }

                if (lastWallPoint.Item2 > _highestYValue)
                    _highestYValue = currentWallPoint.Item2;

                lastWallPoint = currentWallPoint;
            }
        }
    }
}