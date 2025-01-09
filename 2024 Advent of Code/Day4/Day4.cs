using Xunit;
using Xunit.Abstractions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;


namespace _2024_Advent_of_Code.Day4;

public class Day4
{
    private readonly ITestOutputHelper _testOutputHelper;
    
    public Day4(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        
        // Searching for XMAS in any orientation.
        // Find all X's
        // Search in all directions for 'M'
        // If not found, remove from queue
        // Otherwise, search for 'A'
        // If not found, remove from queue
        // Otherwise, search for 'S'
        // If not found, remove from queue
        // Otherwise increment found count
    }
    
    [Fact]
    public Task Sample1()
    {
        var grid = BuildGrid(sample);
        
        SearchForXMAS(grid);
       
        Assert.Equal(18, foundCount);
        return Task.CompletedTask;
    }

    private Queue<(int, int)> FindX(char[,] grid)
    {
        var queue = new Queue<(int, int)>();
        for(var x = 0; x < grid.GetLength(0); x++)
        {
            for(var y = 0; y < grid.GetLength(1); y++)
            {
                // is this cell an 'X'?
                if (grid[x, y] != 'X') continue;
                queue.Enqueue((x,y));
                
            }
        }
        _testOutputHelper.WriteLine($"Found {queue.Count} X's");
        return queue;
    }
    

    // Helper method to get new coordinates based on direction
    private (int, int) GetNewCoordinates(int x, int y, Direction direction)
    {
        return direction switch
        {
            Direction.N => (x - 1, y),
            Direction.NE => (x - 1, y + 1),
            Direction.E => (x, y + 1),
            Direction.SE => (x + 1, y + 1),
            Direction.S => (x + 1, y),
            Direction.SW => (x + 1, y - 1),
            Direction.W => (x, y - 1),
            Direction.NW => (x - 1, y - 1),
            _ => (x, y)
        };
    }

    // Helper method to check if coordinates are within bounds
    private bool IsInBounds(char[,] grid, int x, int y)
    {
        return x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1);
    }

    [Fact]
    public Task Part1()
    {
        var grid = BuildGrid(ReadInput());

        var found = 0;
        _testOutputHelper.WriteLine($"Found: {found}");
        return Task.CompletedTask;
    }

    private char[,] BuildGrid(string[] input)
    {
        _testOutputHelper.WriteLine($"Input: {input.Length} x {input[0].Length}");
        
        var grid = new char[input.Length, input[0].Length];
        for(var i = 0; i < input.Length; i++)
        {
            for(var j = 0; j < input[i].Length; j++)
            {
                grid[i, j] = input[i][j];
            }
        }
        return grid;
    }

    private string[] ReadInput()
    {
        var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Day4", "input.txt");
        return File.ReadAllLines(path);
    }
    
    private string[] sample = new string[]
    {
        "MMMSXXMASM",
        "MSAMXMSMSA",
        "AMXSXMAAMM",
        "MSAMASMSMX",
        "XMASAMXAMM",
        "XXAMMXXAMA",
        "SMSMSASXSS",
        "SAXAMASAAA",
        "MAMMMXMMMM",
        "MXMXAXMASX"
    };

    enum Direction
    {
        N,
        NE,
        E,
        SE,
        S,
        SW,
        W,
        NW
    }

    private int foundCount = 0; // To keep track of found "XMAS"

    // Start searching for "XMAS" from each 'X'
    private void SearchForXMAS(char[,] grid)
    {
        for (var x = 0; x < grid.GetLength(0); x++)
        {
            for (var y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[x, y] == 'X')
                {
                    // Start recursive search for "XMAS"
                    SearchRecursive(grid, x, y, 1, new bool[grid.GetLength(0), grid.GetLength(1)]); // Start with the second character 'M'
                }
            }
        }
    }

    // Recursive method to search for the next character in "XMAS"
    private void SearchRecursive(char[,] grid, int x, int y, int index, bool[,] visited)
    {
        if (index >= "XMAS".Length) // Base case: found all characters
        {
            foundCount++;
            return;
        }

        char nextChar = "XMAS"[index]; // Get the next character to find
        _testOutputHelper.WriteLine($"Searching for '{nextChar}' from ({x}, {y}) at index {index}");

        // Check all directions
        foreach (Direction dir in Enum.GetValues(typeof(Direction)))
        {
            (int newX, int newY) = GetNewCoordinates(x, y, dir);
            if (IsInBounds(grid, newX, newY) && grid[newX, newY] == nextChar && !visited[newX, newY])
            {
                visited[newX, newY] = true; // Mark this cell as visited
                SearchRecursive(grid, newX, newY, index + 1, visited); // Recur for the next character
                visited[newX, newY] = false; // Unmark this cell after returning
            }
        }
    }
}