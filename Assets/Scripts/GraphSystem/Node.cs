using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphSystem
{
	public class Node
	{
		public bool isWalkable;
		public Vector3 position;

		public Node(bool walkable, Vector3 worldPosition)
		{
			isWalkable = walkable;
			position = worldPosition;
		}
	}
}