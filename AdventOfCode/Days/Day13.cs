using System.Diagnostics;

namespace AdventOfCode.Days;

public class Day13 : BaseDay
{
    private readonly List<string[]> _input;

    public Day13()
    {
        _input = File.ReadAllText(InputFilePath).TrimEnd().Split("\n\n").Select(x => x.Split()).ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        var index = 1;
        var total = 0;
        foreach (var item in _input)
        {
            var left = ProcessPacketBrackets(item[0].ToCharArray());
            var right = ProcessPacketBrackets(item[1].ToCharArray());

            var ordered = ArePacketsInOrder(left, right);

            if (ordered == -1)
                total += index;

            index++;
        }

        return new ValueTask<string>(total.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        throw new NotImplementedException();
    }

    private int ArePacketsInOrder(List<string> left, List<string> right)
    {
        var i = 0;
        for (; i < right.Count; i++)
        {
            var ordered = 0;
            if (i == left.Count)
                return -1;

            if (int.TryParse(left[i], out var x))
            {
                if (int.TryParse(right[i], out var y))
                {
                    if (x < y)
                        return -1;
                    else if (x > y)
                        return 1;
                }
                else
                {
                    var rightTmp = ProcessPacketBrackets(right[i].ToCharArray());

                    ordered = ArePacketsInOrder(new List<string> { left[i] }, rightTmp);
                }
            }
            else if (int.TryParse(right[i], out _))
            {
                var leftTmp = ProcessPacketBrackets(left[i].ToCharArray());

                ordered = ArePacketsInOrder(leftTmp, new List<string> { right[i] });
            }
            else
            {
                var leftTmp = ProcessPacketBrackets(left[i].ToCharArray());
                var rightTmp = ProcessPacketBrackets(right[i].ToCharArray());

                ordered = ArePacketsInOrder(leftTmp, rightTmp);
            }

            if (ordered != 0)
                return ordered;
        }

        return right.Count < left.Count ? 1 : 0;
    }

    private static List<string> ProcessPacketBrackets(char[] packet)
    {
        packet = packet[1..^1];
        var bracketPairs = new List<string>();

        for (var i = 0; i < packet.Length; i++)
        {
            if (int.TryParse(packet[i].ToString(), out var num))
            {
                if (i + 1 < packet.Length && packet[i + 1] != ',')
                    bracketPairs.Add(packet[i].ToString() + packet[i].ToString());
                else
                    bracketPairs.Add(packet[i].ToString());

            }

            else if (packet[i] == '[')
            {
                var closingBracketIndex = ClosingBracketFinder(packet, i);

                bracketPairs.Add(new string(packet[i..(closingBracketIndex + 1)]));
                i = closingBracketIndex;
            }
        }

        return bracketPairs;
    }

    private static int ClosingBracketFinder(IReadOnlyList<char> packet, int openingBracketIndex)
    {
        var openingBrackets = 0;
        var closingBrackets = 0;

        for (var i = openingBracketIndex + 1; i < packet.Count; i++)
        {
            switch (packet[i])
            {
                case ']' when openingBrackets == closingBrackets:
                    return i;
                case ']':
                    closingBrackets++;
                    break;
                case '[':
                    openingBrackets++;
                    break;
            }
        }

        return -1;
    }
}
