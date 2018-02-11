using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Priority_Queue;

public class DeepFirstSearch : PathFindSearch
{
	Stack<Node> stack;

    public DeepFirstSearch():base()
    {
    }


    public override bool Find(DirectionGraph graph, Node start, Node end)
    {
		stack = new Stack<Node>(graph.VerticeCount());
        System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
        watch.Start();
        start.visited = true;
        stack.Push(start);
        while(stack.Count > 0)
        {
            Node min = stack.Pop();
            if(min == end)
            {
                watch.Stop();
                executionTimes = watch.Elapsed.TotalMilliseconds;
                CalculateCostAndGeneratePath(start, end, graph);
                return true;
            }
            List<DirectionEdge> minNeighbors = graph.GetNeighborEdge(min);
            for(int i = 0, count = minNeighbors.Count; i < count; i++)
            {
                Node neighbor = minNeighbors[i].to;
				if(neighbor.visited)continue;
				stack.Push(neighbor);
				neighbor.visited = true;
				nodesComeFrom[neighbor] = min;
            }
            if(min != start)searchSteps.Enqueue(min);
        }
        watch.Stop();
        executionTimes = watch.Elapsed.TotalMilliseconds;
        searchSteps.Clear();
        return false;
    }

    public override string Name()
    {
        return "DFS";
    }

    public override string SearchResultDesc()
    {
        if(searchSteps.Count == 0)
        {
            return "cannot find one path which to the target";
        }
        return "cost : " + MinCostFromStart() + "\nelapsed time : " + ExecutionTimes() + " " +  "\nsteps : " + searchSteps.Count;
    }
}