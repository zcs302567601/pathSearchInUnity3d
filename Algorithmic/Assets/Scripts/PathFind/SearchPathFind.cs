using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFindSearch : PathFind
{
    protected int totalCost;
    protected double executionTimes;

    protected Dictionary<Node, Node> nodesComeFrom;        //record this node where come from
    protected Queue<Node> searchedPath;
    protected Queue<Node> searchSteps;

    public PathFindSearch()
    {
        nodesComeFrom = new Dictionary<Node, Node>();
        searchedPath = new Queue<Node>();
        searchSteps = new Queue<Node>();
    }

    public Queue<Node> GetPath()
    {
        return searchedPath;
    }

    public int MinCostFromStart()
    {
        return totalCost;
    }

    public double ExecutionTimes()
    {
        return executionTimes;
    }

    public Queue<Node> GetSearchSteps()
    {
        return searchSteps;
    }

    protected void CalculateCostAndGeneratePath(Node start, Node end, DirectionGraph graph)
    {
        Stack<Node> stack = new Stack<Node>();
        Node to = end;
        while(to != start)
        {
            stack.Push(to);
            Node from = nodesComeFrom[to];
            totalCost += graph.GetEdge(from, to).weight;
            to = from;
        }
        stack.Push(start);
        while(stack.Count > 0)
        {
            searchedPath.Enqueue(stack.Pop());
        }
    }

    public virtual bool Find(DirectionGraph graph, Node start, Node end)
    {
        return false;
    }

    public virtual string Name()
    {
        return "SearchPathFind";
    }
    public virtual string SearchResultDesc()
    {
        return "result desc";
    }
}
