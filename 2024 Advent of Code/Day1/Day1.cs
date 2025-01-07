using System.Collections;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace _2024_Advent_of_Code.Day1;

/* Sample Input
 */
public class Day1
{
    private readonly ITestOutputHelper _testOutputHelper;

    public Day1(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public Task SampleDistance()
    {
        var input1 = new[]{4,3,2,1,3,3};
        var input2 = new []{4, 3, 5, 3, 9, 3};
        
        var result = distance(input1, input2);
        Assert.Equal(11, result);
        return Task.CompletedTask;
    }

    [Fact]
    public Task SampleSimilarity()
    {
        var input1 = new[]{4,3,2,1,3,3};
        var input2 = new []{4, 3, 5, 3, 9, 3};
        
        var result = SimilarityScore(input1, input2);
        Assert.Equal(31, result);
        return Task.CompletedTask;
    }

    [Fact]
    public Task PartOne()
    {
        var file = ReadFile();
        var input1 = new List<int>();
        var input2 = new List<int>();
        
        foreach (var line in file)
        {
            if(string.IsNullOrEmpty(line))
                continue;
            
            var split = line.Split("   ");
            input1.Add(int.Parse(split[0]));
            input2.Add(int.Parse(split[1]));
        }
        
        var result = distance(input1.ToArray(), input2.ToArray());
        
        _testOutputHelper.WriteLine($"Result: {result}");
        return Task.CompletedTask;
    }

    [Fact]
    public void PartTwo()
    {
        var file = ReadFile();
        var input1 = new List<int>();
        var input2 = new List<int>();
        
        foreach (var line in file)
        {
            if(string.IsNullOrEmpty(line))
                continue;
            
            var split = line.Split("   ");
            input1.Add(int.Parse(split[0]));
            input2.Add(int.Parse(split[1]));
        }

        var result = SimilarityScore(input1.ToArray(), input2.ToArray());
        _testOutputHelper.WriteLine($"Result: {result}");
    }

    private string[] ReadFile()
    {
        var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Day1","inputs.txt");
        return File.ReadAllLines(path);
    }
    
    
    
    
    public int distance(int[] input1, int[] input2)
    {
        Array.Sort(input1);
        Array.Sort(input2);

        return input1.Select((t, i) => Math.Abs(t - input2[i])).Sum();
    }

    public int SimilarityScore(int[] input1, int[] input2)
    {
        int similarity = 0;

        foreach (var item in input1)
        {
            var count = input2.Count(x => x == item);
            similarity += (item * count);
        }

        return similarity;
    }
}