using Xunit;
using Xunit.Abstractions;
using System.Reflection;

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
        
        // Part 2: Find all XMAS in the grid
        // Start with 'M' search for 'A', 'S' where 'A' is center for an additional 'M' - 'S' pair
        
    }
    
    [Fact]
    public Task Sample1()
    {
        var grid = BuildGrid(sample);
        
        SearchForXMAS(grid);
       
        Assert.Equal(18, foundCount);
        return Task.CompletedTask;
    }

    [Fact]
    public Task Part1()
    {
        var grid = BuildGrid(ReadInput());

        SearchForXMAS(grid);

        _testOutputHelper.WriteLine($"Found: {foundCount}");
        return Task.CompletedTask;
    }
    
    [Fact]
    public Task Sample2()
    {
        var grid = BuildGrid(sample);
        SearchForCrossedMAS(grid);
        _testOutputHelper.WriteLine($"Found: {foundCount}");
        Assert.Equal(9, foundCount);
        return Task.CompletedTask;
    }

    [Fact]
    public Task Part2()
    {
        var grid = BuildGrid(ReadInput());
        SearchForCrossedMAS(grid);
        _testOutputHelper.WriteLine($"Found: {foundCount}");
        //Assert.Equal(9, foundCount);
        return Task.CompletedTask;
    }

    private void SearchForCrossedMAS(char[,] grid)
    {
        for (var x = 0; x < grid.GetLength(0); x++)
        {
            for (var y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[x, y] == 'M' || grid[x, y] == 'S')
                {
                    DirectionalCrossedMAS_Search(grid, x, y);
                }
            }
        }
    }

    
    // Starting from either 'M' or 'S'
    /// <summary>
    /// .M.S......
    /// ..A..MSMS.
    /// .M.S.MAA..
    /// ..A.ASMSM.
    /// .M.S.M....
    /// ..........
    /// S.S.S.S.S.
    /// .A.A.A.A..
    /// M.M.M.M.M.
    /// ..........
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="index"></param>
    /// <param name="direction"></param>
    /// <param name="visited"></param>
    private void DirectionalCrossedMAS_Search(char[,] grid, int x, int y)
    {
        // see if 'A' is down and to the right
        var (cX, cY) = LowerDiag(x, y);
        
        if( !IsInBounds(grid, cX, cY)) return;

        if ( grid[cX, cY] != 'A') return;
        
        var source = grid[x, y];
        var (lrX, lrY) = LowerDiag(cX, cY);
        
        var (urX, urY) = UpperRight(x, y);
        var (llX, llY) = LowerLeft(x, y);
        
        switch (source)
        {
            case 'M':
                if( IsInBounds(grid, lrX, lrY) && grid[lrX, lrY] == 'S')
                {
                    // one leg 
                    //_testOutputHelper.WriteLine($"Found MAS from {x},{y} to {lrX},{lrY}");;
                    
                    if (IsInBounds(grid, urX, urY) && IsInBounds(grid, llX, llY))
                    {
                        if( (grid[urX, urY] == 'M' && grid[llX, llY] == 'S') 
                           ||  (grid[urX, urY] == 'S' && grid[llX, llY] == 'M'))
                        {
                            _testOutputHelper.WriteLine($"Found the cross from {llX},{llY} to {urX},{urY}");
                            foundCount++;
                        }
                    }
                }
                break;
            case 'S':
                if( IsInBounds(grid, lrX, lrY) && grid[lrX, lrY] == 'M')
                {
                    // one leg found
                    //_testOutputHelper.WriteLine($"Found SAM from {x},{y} to {lrX},{lrY}");;
                    if (IsInBounds(grid, urX, urY) && IsInBounds(grid, llX, llY))
                    {
                        if( (grid[urX, urY] == 'M' && grid[llX, llY] == 'S') 
                            ||  (grid[urX, urY] == 'S' && grid[llX, llY] == 'M'))
                        {
                            _testOutputHelper.WriteLine($"Found the cross from {llX},{llY} to {urX},{urY}");
                            foundCount++;
                        }
                    }
                }
                break;
        }
    }

    private (int, int) LowerDiag(int x, int y)
    {
        return (x + 1, y + 1);
    }

    private (int, int) UpperRight(int x, int y)
    {
        return (x, y + 2);
    }

    private (int, int) LowerLeft(int x, int y)
    {
        return (x+2, y);
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
                    // Start searching in each direction from the 'X'
                    foreach (Direction dir in Enum.GetValues(typeof(Direction)))
                    {
                        DirectionalXMASSearch(grid, x, y, 1, dir, new bool[grid.GetLength(0), grid.GetLength(1)]); // Start with the second character 'M'
                    }
                }
            }
        }
    }

    // Method to search in a specific direction
    private void DirectionalXMASSearch(char[,] grid, int x, int y, int index, Direction direction, bool[,] visited)
    {
        if (index >= "XMAS".Length) // Base case: found all characters
        {
            foundCount++;
            return;
        }

        char nextChar = "XMAS"[index]; // Get the next character to find

        (int newX, int newY) = GetNewCoordinates(x, y, direction); // Get the new coordinates based on the direction

        if (IsInBounds(grid, newX, newY) && grid[newX, newY] == nextChar && !visited[newX, newY])
        {
            visited[newX, newY] = true; // Mark this cell as visited
            DirectionalXMASSearch(grid, newX, newY, index + 1, direction, visited); // Recur for the next character in the same direction
            visited[newX, newY] = false; // Unmark this cell after returning
        }
    }
}