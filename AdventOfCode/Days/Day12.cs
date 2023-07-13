using AdventOfCode.Utilities.Helpers;

namespace AdventOfCode.Days;

public class Day12 : BaseDay
{
    private const char StartCharacter = '`';
    private const char EndCharacter = '{';
    private readonly char[][] _input;
    private readonly int[][] _values;
    private readonly bool[][] _explored;
    
    public Day12()
    {
        _input = File.ReadAllLines(InputFilePath).Select(x => x.ToCharArray()).ToArray();
        _values = new int[_input.Length][];
        _explored = new bool[_input.Length][];
        for (var i = 0; i < _values.Length; i++)
        {
            _values[i] = new int[_input[i].Length];
            _explored[i] = new bool[_input[i].Length];
        }
    }
    
    public override ValueTask<string> Solve_1()
    {
        var inputNodes = new Dictionary<(int i, int j), Node>();
        var endCoords = (0, 0);
        var queue = new PriorityQueue<Node, int>();

        for (var i = 0; i < _input.Length; i++)
        {
            for (var j = 0; j < _input[i].Length; j++)
            {
                _values[i][j] = _input[i][j] == StartCharacter ? 0 : int.MaxValue;

                var node = new Node
                {
                    Value = _input[i][j],
                    Coords = (i, j)
                };
                
                inputNodes.Add((i,j), node);

                if (_input[i][j] == EndCharacter)
                    endCoords = (i, j);

                if (_input[i][j] == StartCharacter)
                    queue.Enqueue(node, 0);
            }
        }

        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();

            if (_explored[currentNode.Coords.i][currentNode.Coords.j]) 
                continue;
            
            _explored[currentNode.Coords.i][currentNode.Coords.j] = true;

            var nodes = GetEndpointNodes(currentNode.Coords.i, currentNode.Coords.j,
                (char)(_input[currentNode.Coords.i][currentNode.Coords.j] + 1));
            
            foreach (var node in nodes)
            {
                var cost = _values[currentNode.Coords.i][currentNode.Coords.j] + 1;

                if (cost >= _values[node.i][node.j]) continue;
                
                _values[node.i][node.j] = cost;
                queue.Enqueue(inputNodes[node], cost);
            }
        }

        return new(_values[endCoords.Item1][endCoords.Item2].ToString());
    }
    
    public override ValueTask<string> Solve_2()
    {
        var inputNodes = new Dictionary<(int i, int j), Node>();
        var endCoords = (0, 0);
        var queue = new PriorityQueue<Node, int>();

        for (var i = 0; i < _input.Length; i++)
        {
            for (var j = 0; j < _input[i].Length; j++)
            {
                _values[i][j] = _input[i][j] == 'a' ? 0 : int.MaxValue;

                var node = new Node
                {
                    Value = _input[i][j],
                    Coords = (i, j)
                };
                
                inputNodes.Add((i,j), node);

                if (_input[i][j] == EndCharacter)
                    endCoords = (i, j);
            }
        }

        var shortestRoute = int.MaxValue;
        foreach (var aNode in inputNodes.Where(x => x.Value.Value == 'a'))
        {
            for (var i = 0; i < _input.Length; i++)
            {
                for (var j = 0; j < _input[i].Length; j++)
                {
                    _values[i][j] = _input[i][j] == 'a' ? 0 : int.MaxValue;
                    _explored[i][j] = false;
                }
            }
            
            queue.Enqueue(aNode.Value, 0);
            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();

                if (_explored[currentNode.Coords.i][currentNode.Coords.j]) 
                    continue;
            
                _explored[currentNode.Coords.i][currentNode.Coords.j] = true;

                var nodes = GetEndpointNodes(currentNode.Coords.i, currentNode.Coords.j,
                    (char)(_input[currentNode.Coords.i][currentNode.Coords.j] + 1));
            
                foreach (var node in nodes)
                {
                    var cost = _values[currentNode.Coords.i][currentNode.Coords.j] + 1;

                    if (cost >= _values[node.i][node.j]) continue;
                
                    _values[node.i][node.j] = cost;
                    queue.Enqueue(inputNodes[node], cost);
                }
            }

            if (_values[endCoords.Item1][endCoords.Item2] < shortestRoute)
                shortestRoute = _values[endCoords.Item1][endCoords.Item2];
        }

        return new(shortestRoute.ToString());
    }
    
    private IEnumerable<(int i, int j)> GetEndpointNodes(int i, int j, char charToFind)
    {
        // Up
        if (ArrayHelper.IsValidCoordinate(i - 1, j, _input) && _input[i - 1][j] - _input[i][j] <= 1)
        {
            yield return (i - 1, j);
        }

        // Down
        if (ArrayHelper.IsValidCoordinate(i + 1, j, _input) && _input[i + 1][j] - _input[i][j] <= 1)
        {
            yield return (i + 1, j);
        }

        // Left
        if (ArrayHelper.IsValidCoordinate(i, j - 1, _input) && _input[i][j - 1] - _input[i][j] <= 1)
        {
            yield return (i, j - 1);
        }

        // Right
        if (ArrayHelper.IsValidCoordinate(i, j + 1, _input) && _input[i][j + 1] - _input[i][j] <= 1)
        {
            yield return (i, j + 1);
        }
    }
}

public class Node
{
    public char Value { get; set; }
    public (int i, int j) Coords { get; set; } 
}