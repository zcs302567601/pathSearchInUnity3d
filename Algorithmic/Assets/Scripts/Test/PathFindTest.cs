using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SearchAlgrithm
{
	BFS = 0,
	DFS,
	Dijkstra,
	Greedy,
	AStar
}

public class PathFindTest : MonoBehaviour {

	public MapView mapView;
	public MainUI mainUI;
	DirectionGraph graph;
	Queue<Node> path;

	void Start () 
	{
		graph = new DirectionGraph(mapView.tiles, mapView.nativeNodes);
		mainUI.onSelectedTile = OnSelectedTile;
		path = new Queue<Node>();
		mapView.onUpdateStartEndPos = delegate(Point start, Point end){
			mainUI.UpdateCoordLable(mapView.startPos, mapView.endPos);		
		};
		mainUI.UpdateCoordLable(mapView.startPos, mapView.endPos);
	}

	void OnSelectedTile(TileType type)
	{
		mapView.selectedTileType = type;
	}

	public void Search(int algrithm)
	{
		ClearPath();
		SearchAlgrithm ag = (SearchAlgrithm)algrithm;
		PathFind pathFind;
		switch(ag)
		{
			case SearchAlgrithm.BFS:
				pathFind = new BreadthFirstSearch();
				break;
			case SearchAlgrithm.DFS:
				pathFind = new DeepFirstSearch();
				break;
			case SearchAlgrithm.Dijkstra:
				pathFind = new DijkstraSearch();
				break;
			case SearchAlgrithm.Greedy:
				pathFind = new GreedyBestFirstSearch();
				break;
			case SearchAlgrithm.AStar:
				pathFind = new AStarSearch();
				break;
			default:
				pathFind = new AStarSearch();
				break;
		}
		Node start = graph.GetNode((int)mapView.startPos.x, (int)mapView.startPos.y);
		Node end = graph.GetNode((int)mapView.endPos.x, (int)mapView.endPos.y);
		if(pathFind.Find(graph, start, end))
		{
			path = pathFind.GetPath();
			Queue<Node> searchSteps = pathFind.GetSearchSteps();
			mainUI.labelSearchDesc.text = pathFind.Name() + " " + pathFind.SearchResultDesc();
			mainUI.SearchConsoleTxt = "";
			StartCoroutine(DrawSearchSteps(searchSteps, path));
		}
		else
		{
			mainUI.labelConsole.text = "no path to the target point from the start point";
		}
	}

	bool isSearching;
	IEnumerator DrawSearchSteps(Queue<Node> steps, Queue<Node> path)
	{
		isSearching = true;
		Node previous = null;
		while(steps.Count > 0)
		{
			if(previous != null)
			{
				previous.nativeNode.GetComponent<NativeNode>().BlinkOff();
			}
			previous = steps.Dequeue();
			{
				previous.nativeNode.GetComponent<NativeNode>().BlinkOn();
			}
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
		}
		StartCoroutine(DrawPath(path));
		yield return null;
	}

	IEnumerator DrawPath(Queue<Node> path)
	{
		Node[] array = path.ToArray();
		for(int i = 0; i < array.Length; i++)
		{
			Node node = array[i];
			if(node.coord_x == mapView.startPos.x && node.coord_y == mapView.startPos.y)
			{
				continue;
			}
			if(node.coord_x == mapView.endPos.x && node.coord_y == mapView.endPos.y)
			{
				continue;
			}
			mapView.UpdateTile(node.coord_x, node.coord_y, MapView.PATHNODE);
			yield return new WaitForEndOfFrame();
		}
		isSearching = false;
	}



	public void ClearPath()
	{
		if(isSearching)
		{
			mainUI.SearchConsoleTxt = "it is drawing, please wait!!!";
			return;
		}
		while(path.Count > 0)
		{
			Node node = path.Dequeue();
			if(node.coord_x == mapView.startPos.x && node.coord_y == mapView.startPos.y)
			{
				continue;
			}
			if(node.coord_x == mapView.endPos.x && node.coord_y == mapView.endPos.y)
			{
				continue;
			}
			mapView.UpdateTile(node.coord_x, node.coord_y, 0);
		}
		for(int i = 0; i < mapView.map_width; i++)
		{
			for(int j = 0; j < mapView.map_height; j++)
			{
				mapView.nativeNodes[i, j].GetComponent<NativeNode>().Reset();
			}
		}
		graph.UpdateGraph(mapView.tiles, mapView.nativeNodes);
	}
}
