using UnityEngine;
using System.Collections;

public class DirectionEdge
{
	public Node from;
	public Node to;
	public int weight;

	public DirectionEdge(Node from, Node to, int weight)
	{
		this.from = from;
		this.to = to;
		this.weight = weight;
	}
}
