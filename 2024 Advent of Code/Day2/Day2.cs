using Xunit;
using Xunit.Abstractions;
using System.Reflection;

namespace _2024_Advent_of_Code.Day2;

public class Day2
{
    private readonly ITestOutputHelper _testOutputHelper;

    public Day2(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public Task Part1Sample()
    {
        // at least 1 
        // no more than 3 is `SAFE`
        var number_safe = 0;

        var reports = PreparedSample();

        // report
        foreach (var report in reports)
        {
            var safe = IsReportSafe(report);
            if (safe)
                number_safe++;

            _testOutputHelper.WriteLine($"Input: {report}, safe: {safe}");
        }

        Assert.Equal(2, number_safe);
        return Task.CompletedTask;
    }

    [Fact]
    public Task Part2Sample()
    {
        var number_safe = 0;

        var reports = PreparedSample();
        foreach (var report in reports)
        {
            var safe = ProblemDampener(report);
            if (safe) number_safe++;
            _testOutputHelper.WriteLine($"Input: {report}, safe: {safe}");
        }

        Assert.Equal(4, number_safe);
        return Task.CompletedTask;
    }

    [Fact]
    public Task Part1()
    {
        var reports = PreparedSample();

        var number_safe = 0;

        foreach (var item in reports)
        {
            var safe = IsReportSafe(item);
            if (IsReportSafe(item))
                number_safe++;
        }

        _testOutputHelper.WriteLine($"Safe: {number_safe}");

        Assert.Equal(2, number_safe);
        return Task.CompletedTask;
    }

    [Fact]
    public Task Part2()
    {
        var reports = ReadFile();

        var number_safe = 0;
        foreach (var report in reports)
        {
            var safe = ProblemDampener(report);
            if (safe) number_safe++;
        }


        Assert.Equal(4, number_safe);
        return Task.CompletedTask;
    }

    public bool IsReportSafe(int[] reports)
    {
        if (reports.Length < 2)
            return false; // Not enough data to determine trend

        bool isIncreasing = true;
        bool isDecreasing = true;

        for (int i = 0; i < reports.Length - 1; i++)
        {
            int diff = reports[i + 1] - reports[i];

            // Check if the difference is within the allowed range
            if (Math.Abs(diff) < 1 || Math.Abs(diff) > 3)
                return false; // Unsafe report

            if (diff > 0)
                isDecreasing = false; // Not decreasing
            else if (diff < 0)
                isIncreasing = false; // Not increasing
        }

        // The report is safe if it's either all increasing or all decreasing
        return isIncreasing || isDecreasing;
    }

    public bool ProblemDampener(int[] reports)
    {
        if (reports.Length < 2)
            return false; // Not enough data to determine trend

        // Check if the report is already safe
        if (IsReportSafe(reports))
            return true;

        // Try removing each level one by one
        for (int i = 0; i < reports.Length; i++)
        {
            // Create a new array excluding the current level
            var modifiedReport = reports.Where((_, index) => index != i).ToArray();

            // Check if the modified report is safe
            if (IsReportSafe(modifiedReport))
                return true; // Found a safe report by removing one level
        }

        return false; // No safe report found
    }

    public static int[] ConvertStringArrayToIntArray(string[] stringArray)
    {
        return stringArray.Select(int.Parse).ToArray();
    }

    private List<int[]> PreparedSample()
    {
        return Sample.Select(item => item.Split(" ")).Select(stuff => ConvertStringArrayToIntArray(stuff)).ToList();
    }
    private List<string> Sample =
    [
        "7 6 4 2 1",
        "1 2 7 8 9",
        "9 7 6 2 1",
        "1 3 2 4 5",
        "8 6 4 4 1",
        "1 3 6 7 9"
    ];

    private List<int[]> ReadFile()
    {
        var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Day2", "input.txt");

        var lines = File.ReadAllLines(path);

        return lines.Select(item => item.Split(" ")).Select(stuff => ConvertStringArrayToIntArray(stuff)).ToList();
    }
}