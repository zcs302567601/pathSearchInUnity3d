using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Priority_Queue;

public class AStarSearch : PathFindSearch 
{
    FastPriorityQueue<Node> priorityQueue;
    Dictionary<Node, int> costForStartToTarget;
   
    public AStarSearch():base()
    {
    }

    public override bool Find(DirectionGraph graph, Node start, Node end)
    {
		priorityQueue = new FastPriorityQueue<Node>(graph.VerticeCount());
        costForStartToTarget = new Dictionary<Node, int>();

        System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
        watch.Start();
        
        costForStartToTarget[start] = 0;
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
            int costForStartToMin = costForStartToTarget[min];
            for(int i = 0, count = minNeighbors.Count; i < count; i++)
            {
                Node neighbor = minNeighbors[i].to;
                int costForStartToNeighbor = int.MaxValue;
                if(costForStartToTarget.ContainsKey(neighbor))
                {
                    costForStartToNeighbor = costForStartToTarget[neighbor];
                }
                int heuristic = Heuristic(neighbor, end);
                int newCostSoFar = costForStartToMin + minNeighbors[i].weight;
                if(newCostSoFar < costForStartToNeighbor)
                {
                    costForStartToTarget[neighbor] = newCostSoFar;
                    nodesComeFrom[neighbor] = min;
                    if(priorityQueue.Contains(neighbor))
                    {
                        priorityQueue.UpdatePriority(neighbor, newCostSoFar + heuristic);
                    }
                    else
                    {
                        priorityQueue.Enqueue(neighbor, newCostSoFar + heuristic);
                    }
                }
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
        return "AStar";
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