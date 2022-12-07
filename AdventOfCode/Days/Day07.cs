namespace AdventOfCode.Days;

public class Day07 : BaseDay
{
    private readonly string[] _input;
    private readonly List<long> _directorySizes = new List<long>();
    private readonly Directory _baseDirectory = new()
    {
        DirectoryName = "/",
    };


    public Day07()
    {
        _input = File.ReadAllLines(InputFilePath).Select(x => x.Replace("$ ", "")).ToArray();
    }

    public override ValueTask<string> Solve_1()
    {
        var lsExecuted = false;
        var currentDirectory = _baseDirectory;

        foreach (var command in _input.Skip(1))
        {
            if (command[..2] == "cd")
            {
                lsExecuted = false;

                if (command[3..] == "..")
                {
                    _directorySizes.Add(currentDirectory.TotalDirectorySize);
                    currentDirectory = currentDirectory.ParentDirectory;
                }
                else
                {
                    currentDirectory = currentDirectory.Directories[command[3..]];
                }

                continue;
            }

            if (command[..2] == "ls")
            {
                lsExecuted = true;
                continue;
            }

            if (lsExecuted)
            {
                if (command[..3] == "dir")
                {
                    var directoryName = command[4..];
                    var newDirectory = new Directory
                    {
                        DirectoryName = directoryName,
                        ParentDirectory = currentDirectory,
                    };

                    currentDirectory.Directories.Add(newDirectory.DirectoryName, newDirectory);
                }
                else
                {
                    var file = command.Split(" ");
                    var directoryFile = new DirectoryFile
                    {
                        FileSize = long.Parse(file[0]),
                        FileName = file[1]
                    };

                    currentDirectory.Files.Add(directoryFile);
                }
            }
        }

        _directorySizes.Add(currentDirectory.TotalDirectorySize);

        _directorySizes.Sort();
        var total = _directorySizes.Where(x => x <= 100000).Sum();

        return new(total.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var spaceUsed = 70000000 - _baseDirectory.TotalDirectorySize;
        var spaceNeededToFree = 30000000 - spaceUsed;
        var directorySize = _directorySizes.Where(x => x >= spaceNeededToFree).First();

        return new(directorySize.ToString());
    }
}

public class Directory
{
    public string DirectoryName { get; set; }
    public List<DirectoryFile> Files { get; set; } = new List<DirectoryFile>();
    public long TotalFileSize => Files.Sum(f => f.FileSize);
    public long TotalDirectorySize => TotalFileSize + Directories.Sum(x => x.Value.TotalDirectorySize);
    public Directory ParentDirectory { get; set; }
    public Dictionary<string, Directory> Directories { get; set; } = new Dictionary<string, Directory>();
}

public class DirectoryFile
{
    public string FileName { get; set; }
    public long FileSize { get; set; }
}
