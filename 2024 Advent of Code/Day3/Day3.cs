using Xunit;
using Xunit.Abstractions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace _2024_Advent_of_Code.Day3;

public class Day3
{
    private readonly ITestOutputHelper _testOutputHelper;
    
    public Day3(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    [Fact]
    public Task Sample1()
    {
        var input = "xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))";
        var cmd = "xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))";
        var regex = new Regex(actionPattern);
        
        var accumulator = 0;
        
        var matches = regex.Matches(input);
        foreach (Match match in matches)
        {
            _testOutputHelper.WriteLine($"Match: {match.Groups["left"].Value} :: {match.Groups["right"].Value}");
            var left = int.Parse(match.Groups["left"].Value);
            var right = int.Parse(match.Groups["right"].Value);
            accumulator += left * right;
        }
        _testOutputHelper.WriteLine($"Total: {accumulator}");
        return Task.CompletedTask;
    }

    [Fact]
    public Task Sample2()
    {
        var cmd = "xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))";
        var regex = new Regex(cmdPattern);
        
        var accumulator = 0;
        
        var matches = regex.Matches(cmd);
        
        var donot = false;
        
        foreach (Match match in matches)
        {
            _testOutputHelper.WriteLine($"Match: {match}");
            
            if (match.Groups["do"].Success)
            {
                donot = false;
            }
            else if (match.Groups["not"].Success)
            {
                donot = true;
            }

            if (donot) continue;
            
            if(match.Groups["left"].Success)
            {
                var left = int.Parse(match.Groups["left"].Value);
                var right = int.Parse(match.Groups["right"].Value);
                accumulator += (left * right);
            }
        }
        _testOutputHelper.WriteLine($"Total: {accumulator}");
        Assert.Equal(48, accumulator);
        return Task.CompletedTask;
    }

    [Fact]
    public Task Part1()
    {
        var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Day3", "input.txt");
        var input = File.ReadAllText(path);
        var regex = new Regex(actionPattern);
        
        var accumulator = 0;
        
        var matches = regex.Matches(input);
        _testOutputHelper.WriteLine($"Matches: {matches.Count}");
        
        foreach (Match match in matches)
        {
            var left = int.Parse(match.Groups["left"].Value);
            var right = int.Parse(match.Groups["right"].Value);
            accumulator += (left * right);
        }
        _testOutputHelper.WriteLine($"Total: {accumulator}");
        return Task.CompletedTask;
    }

    [Fact]
    public Task Part2()
    {
        var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Day3", "input.txt");
        var input = File.ReadAllText(path);
        var regex = new Regex(cmdPattern);
        
        var accumulator = 0;
        
        var matches = regex.Matches(input);
        
        var donot = false;
        
        foreach (Match match in matches)
        {
            _testOutputHelper.WriteLine($"Match: {match}");
            
            if (match.Groups["do"].Success)
            {
                donot = false;
            }
            else if (match.Groups["not"].Success)
            {
                donot = true;
            }

            if (donot) continue;
            
            if(match.Groups["left"].Success)
            {
                var left = int.Parse(match.Groups["left"].Value);
                var right = int.Parse(match.Groups["right"].Value);
                accumulator += (left * right);
            }
        }
        _testOutputHelper.WriteLine($"Total: {accumulator}");
        return Task.CompletedTask;
    }


    private const string actionPattern = @"mul\((?<left>\d{1,3}),(?<right>\d{1,3})\)";
    private const string cmdPattern = @"(?<do>do\(\))|mul\((?<left>\d{1,3}),(?<right>\d{1,3})\)|(?<not>don\'t\(\))";
}