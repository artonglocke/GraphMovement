using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomGenerics;
using System;
using System.Linq;

namespace GraphSystem
{
	public enum HeuristicsType
	{
		Manhattan,
		Diagonal,
		Euclidean
	}

	[RequireComponent(typeof(GridSystem))]
	public class Pathfinder : MonoBehaviour
	{
		public HeuristicsType heuristics = HeuristicsType.Euclidean;
		private GridSystem m_grid;
		private PathfinderProcessor m_processor; // Circular dependency *sigh*

		private void Awake()
		{
			m_grid = GetComponent<GridSystem>();
			m_processor = GetComponent<PathfinderProcessor>();
		}

		IEnumerator OnFindPath(Vector3 start, Vector3 target)
		{
			Vector3[] waypoints = new Vector3[0];
			bool pathFound = false;

			Node startingNode = m_grid.GetNodeFromPosition(start);
			Node endNode = m_grid.GetNodeFromPosition(target);

			if (startingNode.isWalkable && endNode.isWalkable)
			{
				Heap<Node> openNodes = new Heap<Node>(m_grid.GetSize());
				List<Node> closedNodes = new List<Node>();

				openNodes.Add(startingNode);

				while (openNodes.Count > 0)
				{
					Node current = openNodes.RemoveFirst();
					closedNodes.Add(current);

					if (current == endNode)
					{
						pathFound = true;
						break;
					}

					foreach (var neighbour in m_grid.GetNeighbouringNodes(current))
					{
						if (!neighbour.isWalkable || closedNodes.Contains(neighbour))
						{
							continue;
						}

						int neighbourCost = current.gCost + GetDistance(current, neighbour);
						if (neighbourCost < neighbour.gCost || !openNodes.Contains(neighbour))
						{
							neighbour.gCost = neighbourCost;
							neighbour.hCost = GetDistance(neighbour, endNode);
							neighbour.parent = current;
							if (!openNodes.Contains(neighbour))
							{
								openNodes.Add(neighbour);
							}
							else
							{
								openNodes.Update(neighbour);
							}
						}
					}
				}
			}

			yield return null;

			if (pathFound)
			{
				waypoints = Retrace(startingNode, endNode);
			}

			m_processor.FinishProcessing(waypoints, pathFound);
			
		}

		public void Begin(Vector3 start, Vector3 end)
		{
			StartCoroutine(OnFindPath(start, end));
		}

		private Vector3[] Retrace(Node start, Node end)
		{
			List<Node> path = new List<Node>();
			Node currentNode = end;

			// Retracing path through parent
			while (currentNode != start)
			{
				path.Add(currentNode);
				currentNode = currentNode.parent;
			}

			// Path starts from the end!
			Vector3[] waypoints = OptimizeAndConvertPath(path);
			Array.Reverse(waypoints);
			return waypoints;
		}

		private Vector3[] OptimizeAndConvertPath(List<Node> path)
		{
			List<Vector3> waypoints = new List<Vector3>();
			Vector2 oldDirection = Vector2.zero;

			for (int i = 1; i < path.Count; i++)
			{
				Vector2 newDirection = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
				if (oldDirection != newDirection)
				{
					waypoints.Add(path[i].position);
				}
				oldDirection = newDirection;
			}
			return waypoints.ToArray();
		}

		private int GetDistance(Node first, Node second)
		{
			int distanceX = Mathf.Abs(first.gridX - second.gridX);
			int distanceY = Mathf.Abs(first.gridY - second.gridY);

			switch (heuristics)
			{
				case HeuristicsType.Manhattan:
					return distanceX + distanceY;
				case HeuristicsType.Diagonal:
					return Math.Max(distanceX, distanceY);
				case HeuristicsType.Euclidean:
					if (distanceX > distanceY)
					{
						return 14 * distanceY + 10 * (distanceX - distanceY);
					}
					return 14 * distanceX + 10 * (distanceY - distanceX); ;
				default:
					// Manhattan for default, not needed, but since its a switch case, must be
					return distanceX + distanceY;
			}
			
		}
	}
}