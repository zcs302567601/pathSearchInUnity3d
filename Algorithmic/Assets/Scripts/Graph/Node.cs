using UnityEngine;
using System.Collections;

public class Node : Priority_Queue.FastPriorityQueueNode
{
	public string name;
	public int coord_x;
	public int coord_y;
	public int value;
	public bool visited;
	public NativeNode nativeNode;

	public Node()
	{

	}

	public Node(string name)
	{
		this.name = name;
	}

	public Node(string name, int x, int y)
	{
		this.name = name;
		this.coord_y = y;
		this.coord_x = x;
	}
}
