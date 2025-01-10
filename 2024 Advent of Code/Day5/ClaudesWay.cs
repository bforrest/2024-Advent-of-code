using System.Net;
using System.Reflection;
using System.Security;
using Xunit;
using Xunit.Abstractions;
using Xunit.Abstractions;


namespace _2024_Advent_of_Code.Day5;

public class ClaudesWay
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ClaudesWay(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public Task Sample1()
    {
        int result = CalculateMiddlePageSum(rules, updates);
        _testOutputHelper.WriteLine(result.ToString()); // Output the sum of middle page numbers
        return Task.CompletedTask;
    }
    
    static int CalculateMiddlePageSum(List<string> rules, List<string> updates)
    {
        var graph = new Dictionary<int, List<int>>();
        var inDegree = new Dictionary<int, int>();

        // Build the graph and in-degree count
        foreach (var rule in rules)
        {
            var parts = rule.Split('|');
            int x = int.Parse(parts[0]);
            int y = int.Parse(parts[1]);

            if (!graph.ContainsKey(x))
                graph[x] = new List<int>();
            graph[x].Add(y);

            if (!inDegree.ContainsKey(x))
                inDegree[x] = 0;
            if (!inDegree.ContainsKey(y))
                inDegree[y] = 0;

            inDegree[y]++;
        }

        var topologicalOrder = KahnTopologicalSort(graph, inDegree);

        int middleSum = 0;
        foreach (var update in updates)
        {
            var pages = Array.ConvertAll(update.Split(','), int.Parse);
            if (IsOrdered(pages, topologicalOrder))
            {
                int middlePage = pages[pages.Length / 2]; // Find the middle page
                middleSum += middlePage;
            }
        }

        return middleSum;
    }

    static List<int> KahnTopologicalSort(Dictionary<int, List<int>> graph, Dictionary<int, int> inDegree)
    {
        var queue = new Queue<int>();
        var topologicalOrder = new List<int>();

        // Initialize the queue with nodes having in-degree of 0
        foreach (var node in inDegree.Keys)
        {
            if (inDegree[node] == 0)
                queue.Enqueue(node);
        }

        while (queue.Count > 0)
        {
            int node = queue.Dequeue();
            topologicalOrder.Add(node);

            if (graph.ContainsKey(node))
            {
                foreach (var neighbor in graph[node])
                {
                    inDegree[neighbor]--;
                    if (inDegree[neighbor] == 0)
                        queue.Enqueue(neighbor);
                }
            }
        }

        // Check for cycles (if the topological order doesn't include all nodes)
        return topologicalOrder.Count == inDegree.Count ? topologicalOrder : new List<int>();
    }

    static bool IsOrdered(int[] update, List<int> topologicalOrder)
    {
        var indexMap = new Dictionary<int, int>();
        for (int i = 0; i < topologicalOrder.Count; i++)
        {
            indexMap[topologicalOrder[i]] = i;
        }

        for (int i = 0; i < update.Length - 1; i++)
        {
            if (indexMap[update[i]] >= indexMap[update[i + 1]])
                return false;
        }

        return true;
    }

    public List<string> rules = new List<string>
    {
        "47|53", "97|13", "97|61", "97|47", "75|29",
        "61|13", "75|53", "29|13", "97|29", "53|29",
        "61|53", "97|53", "61|29", "47|13", "75|47",
        "97|75", "47|61", "75|61", "47|29", "75|13",
        "53|13"
    };

    public List<string> updates = new List<string>
    {
        "75,47,61,53,29",
        "97,61,53,29,13",
        "75,29,13",
        "75,97,47,61,53",
        "61,13,29",
        "97,13,75,29,47"
    };
    
    
}