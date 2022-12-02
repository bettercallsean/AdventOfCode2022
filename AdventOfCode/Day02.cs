﻿namespace AdventOfCode.Days;

public class Day02 : BaseDay
{
    private const int Rock = 1;
    private const int Paper = 2;
    private const int Scissors = 3;

    private readonly int[][] _input;
    private readonly Dictionary<int, int> _moveToWin = new Dictionary<int, int>
    {
        { Scissors, Rock },
        { Rock, Paper },
        { Paper, Scissors }
    };
    private readonly Dictionary<int, int> _moveToLose = new Dictionary<int, int>
    {
        { Paper, Rock },
        { Scissors, Paper },
        { Rock, Scissors }
    };

    public Day02()
    {
        var charToGameMove = new Dictionary<char, int>
        {
            { 'X', Rock },
            { 'Y', Paper },
            { 'Z', Scissors },
            { 'A', Rock },
            { 'B', Paper },
            { 'C', Scissors },
        };

        _input = File.ReadAllLines(InputFilePath)
                .Select(x => x.Split(" ")
                            .Select(char.Parse)
                            .Select(y => charToGameMove[y])
                            .ToArray())
                .ToArray();
    }

    public override ValueTask<string> Solve_1()
    {
        var score = 0;
        foreach (var move in _input)
        {
            if (move[0] == _moveToWin[move[1]])
            {
                score += 6;
            }
            else if (move[0] == move[1])
            {
                score += 3;
            }

            score += move[1];
        }

        return new(score.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        const int Lose = 1;
        const int Win = 3;

        var score = 0;
        foreach (var move in _input)
        {
            var gameOutcome = move[1];
            if (gameOutcome == Win)
            {
                score += 6;
                score += _moveToWin[move[0]];
            }
            else if (gameOutcome == Lose)
            {
                score += _moveToLose[move[0]];
            }
            else
            {
                score += 3;
                score += move[0];
            }
        }

        return new(score.ToString());
    }
}