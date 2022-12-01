namespace AdventOfCode;

public class Day01 : BaseDay
{
    private readonly string[] _input;
    private readonly List<int> _caloryCount = new();

    public Day01()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var count = 0;
        foreach (var caloryCount in _input)
        {
            if (string.IsNullOrWhiteSpace(caloryCount))
            {
                _caloryCount.Add(count);
                count = 0;
                continue;
            }

            count += int.Parse(caloryCount);
        }

        _caloryCount.Sort();
        return new(_caloryCount[^1].ToString());
    }

    public override ValueTask<string> Solve_2() => new((_caloryCount[^1] + _caloryCount[^2] + _caloryCount[^3]).ToString());
}
