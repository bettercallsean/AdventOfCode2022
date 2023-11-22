namespace AdventOfCode.Days;

public class Day01 : BaseDay
{
    private readonly List<int> _input;

    public Day01()
    {
        _input = File.ReadAllText(InputFilePath)
                    .TrimEnd()
                    .Split(Environment.NewLine + Environment.NewLine)
                    .Select(x => x.Split().Select(int.Parse).Sum())
                    .ToList();

        _input.Sort();
    }

    public override ValueTask<string> Solve_1() => new(_input[^1].ToString());

    public override ValueTask<string> Solve_2() => new((_input[^1] + _input[^2] + _input[^3]).ToString());
}
