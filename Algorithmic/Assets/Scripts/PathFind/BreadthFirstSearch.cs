using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Priority_Queue;

public class BreadthFirstSearch : PathFindSearch
{
	Queue<Node> queue;

    public BreadthFirstSearch():base()
    {

    }

    public override bool Find(DirectionGraph graph, Node start, Node end)
    {
		queue = new Queue<Node>(graph.VerticeCount());
        System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
        watch.Start();
        start.visited = true;
        queue.Enqueue(start);
        while(queue.Count > 0)
        {
            Node min = queue.Dequeue();
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
				queue.Enqueue(neighbor);
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
        return "BFS";
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