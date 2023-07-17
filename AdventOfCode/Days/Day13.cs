using System.Text;
using AdventOfCode.Utilities.Helpers;

namespace AdventOfCode.Days;

public class Day13 : BaseDay
{
    private readonly string[] _input;
    public Day13()
    {
        _input = File.ReadAllLines(InputFilePath);

    }
    public override ValueTask<string> Solve_1()
    {
        var bracketPairs = 1;
        var total = 0;
        for (var i = 0; i < _input.Length; i+=3)
        {
            var correctPairs = AreBracketsInOrder(_input[i][1..^1].ToCharArray(), _input[i + 1][1..^1].ToCharArray());

            if (correctPairs == 1)
                total += bracketPairs;

            bracketPairs++;
        }
        
        // Incorrect values - too low
        // 5854
        // 5977
        return new(total.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        throw new NotImplementedException();
    }

    // private static Dictionary<int, int> ProcessPacketBrackets(char[] packet)
    // {
    //     var foo = packet.ToList();
    //     var openingBrackets = new Stack<int>();
    //     var bracketPairs = new Dictionary<int, int>();
    //
    //     for (var i = 0; i < packet.Length; i++)
    //     {
    //         switch (packet[i])
    //         {
    //             case '[':
    //                 openingBrackets.Push(i);
    //                 break;
    //             case ']':
    //             {
    //                 var openingBracket = openingBrackets.Pop();
    //                 bracketPairs.Add(openingBracket, i);
    //                 Console.WriteLine(packet[(openingBracket + 1)..i]);
    //                 break;
    //             }
    //         }
    //     }
    //
    //     return bracketPairs;
    // }
    
    private static List<string> ProcessPacketBrackets(char[] packet)
    {
        var bracketPairs = new List<string>();
    
        for (var i = 0; i < packet.Length; i++)
        {
            if (int.TryParse(packet[i].ToString(), out var num))
                bracketPairs.Add(packet[i].ToString());
            
            else if (packet[i] == '[')
            {
                var closingBracketIndex = ClosingBracketFinder(packet, i);
                
                bracketPairs.Add(new string(packet[i..(closingBracketIndex + 1)]));

                i = closingBracketIndex;
            }
        }
        
        return bracketPairs;
    }

    private int AreBracketsInOrder(char[] leftPacket, char[] rightPacket)
    {
        var firstPacket = ProcessPacketBrackets(leftPacket);
        var secondPacket = ProcessPacketBrackets(rightPacket);

        if (firstPacket.Count == 0 && secondPacket.Count >= 0)
        {
            if(secondPacket.Count > 0)
                return 1;

            return -1;
        }
        
        for (var j = 0; j < firstPacket.Count; j++)
        {
            if (j >= secondPacket.Count)
                return 0;
            
            var left = firstPacket[j];
            var right = secondPacket[j];

            var leftIsInt = int.TryParse(left, out var leftValue);
            var rightIsInt = int.TryParse(right, out var rightValue);
            var valuesAreEqual = false;

            if (leftIsInt && rightIsInt)
            {
                if (leftValue < rightValue)
                    return 1;

                if (leftValue > rightValue)
                    return 0;
                
                valuesAreEqual = true;
            }

            if (valuesAreEqual)
            {
                if (j == firstPacket.Count - 1)
                    return 1;
                
                continue;
            }

            int bracketOrder;
            if (leftIsInt)
            {
                bracketOrder = AreBracketsInOrder(new[] { char.Parse(left) }, right[1..^1].ToCharArray());
            }

            else if (rightIsInt)
            {
                bracketOrder = AreBracketsInOrder(left[1..^1].ToCharArray(), new[] { char.Parse(right) });
 
            }
            else
            {
                bracketOrder = AreBracketsInOrder(left[1..^1].ToCharArray(), right[1..^1].ToCharArray());
            }

            if (bracketOrder == -1)
            {
                if (j == firstPacket.Count - 1)
                    return 1;
                
                continue;
            };
            
            return bracketOrder;
        }

        return -1;
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

public record Packet
{
    public int? Value { get; set; }
    public List<Packet> InnerPackets { get; set; } = new();
}