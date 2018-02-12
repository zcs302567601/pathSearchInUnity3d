using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DirectionGraph {
	string name;
	int E;
	Dictionary<Node, List<DirectionEdge>> bags;
	public readonly Dictionary<string, Node> mapNodes;

	public DirectionGraph()
	{
		bags = new Dictionary<Node, List<DirectionEdge>>();
		name = "directionGraph";
	}

	public DirectionGraph(int[, ] tiles, NativeNode[, ] nativeNodes)
	{
		bags = new Dictionary<Node, List<DirectionEdge>>();
		name = "directionGraph";
		mapNodes = new Dictionary<string, Node>();
		UpdateGraph(tiles, nativeNodes);
	}

	public DirectionGraph(string name)
	{
		bags = new Dictionary<Node, List<DirectionEdge>>();
		this.name = name;
	}

	public void UpdateGraph(int[, ] tiles, NativeNode[, ] nativeNodes)
	{
		mapNodes.Clear();
		bags.Clear();
		int rows = tiles.GetLength(0);
		int cols = tiles.GetLength(1);
		for(int i = 0; i < rows; i++)
		{
			for(int j = 0; j < cols; j++)
			{
				if(tiles[i, j] == 1)continue;
				Node node = new Node(i + "_" + j);
				node.coord_x = i;
				node.coord_y = j;
				node.value = tiles[i, j];
				node.nativeNode = nativeNodes[i, j];
				mapNodes.Add(node.name, node);
				bags[node] = new List<DirectionEdge>();
			}
		}
		List<Node> nodeList = new List<Node>(mapNodes.Values);
		for(int i = 0, count = nodeList.Count; i < count; i++)
		{
			var node = nodeList[i];
			if(node.coord_y < cols - 1 && tiles[node.coord_x, node.coord_y + 1] != 1)
			{
				var upNode = mapNodes[node.coord_x + "_" + (node.coord_y + 1)];
				bags[node].Add(new DirectionEdge(node, upNode, 1));
			}
			if(node.coord_x > 0 && tiles[node.coord_x - 1, node.coord_y] != 1)
			{
				var leftNode = mapNodes[(node.coord_x - 1) + "_" + node.coord_y];
				bags[node].Add(new DirectionEdge(node, leftNode, 1));
			}
			if(node.coord_x < rows - 1 && tiles[node.coord_x + 1, node.coord_y] != 1)
			{
				var rightNode = mapNodes[(node.coord_x + 1) + "_" + node.coord_y];
				bags[node].Add(new DirectionEdge(node, rightNode, 1));
			}
			if(node.coord_y > 0 && tiles[node.coord_x, node.coord_y - 1] != 1)
			{
				var bottomNode = mapNodes[node.coord_x + "_" + (node.coord_y - 1)];
				bags[node].Add(new DirectionEdge(node, bottomNode, 1));
			}
		}
	}

	public DirectionEdge GetEdge(Node from, Node to)
	{
		return bags[from].Find(delegate(DirectionEdge edge){return edge.to == to;});
	}

	public Node GetNode(int tile_x, int tile_y)
	{
		return mapNodes[tile_x + "_" + tile_y];
	}

	public int VerticeCount()
	{
		return bags.Count;
	}

	public int EdgeCount()
	{
		return E;
	}

	public void AddEdge(DirectionEdge edge)
	{
		if(!bags.ContainsKey(edge.from))
		{
			bags.Add(edge.from, new List<DirectionEdge>());
		}
		bags[edge.from].Add(edge);
		E++;
	}

	public List<DirectionEdge> GetNeighborEdge(Node node)
	{
		List<DirectionEdge> neighbors;
		bags.TryGetValue(node, out neighbors);
		return neighbors;
	}

	public override string ToString()
	{
		return "Graph name = " + name + ", Vertice = " + VerticeCount() + ", Edge = " + E;
	}
}