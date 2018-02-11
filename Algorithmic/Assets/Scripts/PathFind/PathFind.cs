using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface PathFind 
{
    Queue<Node> GetPath();
    Queue<Node> GetSearchSteps();
    bool Find(DirectionGraph graph, Node start, Node end);
    int MinCostFromStart();
    double ExecutionTimes();
    string Name();
    string SearchResultDesc();
}