using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Priority_Queue;

public class GreedyBestFirstSearch : PathFindSearch 
{
	FastPriorityQueue<Node> priorityQueue;
    public GreedyBestFirstSearch():base()
    {
    }

    public override bool Find(DirectionGraph graph, Node start, Node end)
    {
        priorityQueue = new FastPriorityQueue<Node>(graph.VerticeCount());
        searchSteps = new Queue<Node>();

        System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
        watch.Start();

        priorityQueue.Enqueue(start, 0);
        while(priorityQueue.Count > 0)
        {
            Node min = priorityQueue.Dequeue();
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
                if(neighbor.visited)
                {
                    continue;
                }
                neighbor.visited = true;
                int newCostSoFar = Heuristic(neighbor, end);//Mathf.Abs(neighbor.coord_x - end.coord_x) + Mathf.Abs(neighbor.coord_y - end.coord_y);// costForStartToMin + minNeighbors[i].weight;
                nodesComeFrom[neighbor] = min;
                priorityQueue.Enqueue(neighbor, newCostSoFar);
            }
            if(min != start)searchSteps.Enqueue(min);
        }
        watch.Stop();
        executionTimes = watch.Elapsed.TotalMilliseconds;
        searchSteps.Clear();
        return false;
    }

	int Heuristic(Node start, Node end)
    {
        int xdistance = Mathf.Abs(start.coord_x - end.coord_x);
        int ydistance = Mathf.Abs(start.coord_y - end.coord_y);
        return  xdistance * xdistance + ydistance * ydistance;
    }

    public override string Name()
    {
        return "Greedy";
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