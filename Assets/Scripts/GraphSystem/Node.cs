using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomGenerics;

namespace GraphSystem
{
	public class Node : IHeapElement<Node>
	{
		public bool isWalkable;
		public Vector3 position;
		public Node parent;

		public int gCost;
		public int hCost;
		public int fCost
		{
			get
			{
				return gCost + hCost;
			}
		}

		public int gridX { get; private set; }
		public int gridY { get; private set; }
		public int index
		{
			get
			{
				return m_heapIndex;
			}
			set
			{
				m_heapIndex = value;
			}
		}

		private int m_heapIndex;

		public Node(bool walkable, Vector3 worldPosition, int indexX, int indexY)
		{
			isWalkable = walkable;
			position = worldPosition;
			gridX = indexX;
			gridY = indexY;
		}

		public int CompareTo(Node other)
		{
			int comparison = fCost.CompareTo(other.fCost);
			if (comparison == 0)
			{
				comparison = hCost.CompareTo(other.hCost);
			}

			// Nodes priority is reversed, return 1 if the weight is lower
			return -comparison;
		}
	}
}