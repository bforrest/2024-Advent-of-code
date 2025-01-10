using System.Net;
using System.Reflection;
using System.Security;
using Xunit;
using Xunit.Abstractions;
using Xunit.Abstractions;

namespace _2024_Advent_of_Code.Day5;

public class Puzzle
{
    private readonly ITestOutputHelper _testOutputHelper;

    public Puzzle
        (ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    [Fact]
    public Task Sample1()
    {
        var input = Sample;
        var (rules, updates) = ParseInput(input);

        var ruleBook = BuildRuleBook(rules);
        _testOutputHelper.WriteLine($"Rules in the Rule Book: {ruleBook.Count}");
        
        foreach (var update in updates)
        {
            var pages = update.Split(",");
            
            var queue = new Queue<string>();

            foreach (var page in pages)
            {
                queue.Enqueue(page);
            }
            
            
            _testOutputHelper.WriteLine("Update: " + update);
        }

        return Task.CompletedTask;
    }

    private (List<string> rules, List<string> updates) ParseInput(string[] input)
    {
        var rules = new List<string>();
        var updates = new List<string>();
        
        foreach (var line in input)
        {
            if(string.IsNullOrWhiteSpace(line)) continue;
            
            if(line.Contains("|")) rules.Add(line);

            if(line.Contains(",")) updates.Add(line);
        }

        _testOutputHelper.WriteLine($"rule count: {rules.Count}");
        _testOutputHelper.WriteLine($"update count: {updates.Count}");
        return (rules, updates);
    }

    private string[] ReadInput()
    {
        var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Day4", "input.txt");
        return File.ReadAllLines(path);
    }

    private string[] Sample = new string[]
    {
        "47|53",
        "97|13",
        "97|61",
        "97|47",
        "75|29",
        "61|13",
        "75|53",
        "29|13",
        "97|29",
        "53|29",
        "61|53",
        "97|53",
        "61|29",
        "47|13",
        "75|47",
        "97|75",
        "47|61",
        "75|61",
        "47|29",
        "75|13",
        "53|13",
        "",
        "75,47,61,53,29",
        "97,61,53,29,13",
        "75,29,13",
        "75,97,47,61,53",
        "61,13,29",
        "97,13,75,29,47"
    };
    
    public List<Rule> BuildRuleBook(List<string> rules)
    {
        var ruleBook = new List<Rule>();
        foreach (var rule in rules)
        {
            var parts = rule.Split("|");
            var page = parts[0];
            var printBefore = parts[1];
            var existingRule = ruleBook.FirstOrDefault(r => r.Page == page);
            if(existingRule == null)
            {
                ruleBook.Add(new Rule(page, printBefore));
            }
            else
            {
                existingRule.AddPage(printBefore);
            }
        }

        return ruleBook;
    }

    public class Rule
    {
        public Rule(string page, string printBefore)
        {
            Page = page;
            PrintBefore.Add(printBefore);
            Visited = false;
        }

        public string Page { get; init; }
        private List<string> PrintBefore { get; set; } = new();
        public bool Visited { get; set; }
        public void AddPage(string page)
        {
            PrintBefore.Add(page);
        }
    }
}