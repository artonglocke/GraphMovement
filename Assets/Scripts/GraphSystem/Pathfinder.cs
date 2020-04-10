using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using CustomGenerics;
using System;
using System.Linq;

namespace GraphSystem
{
	[RequireComponent(typeof(GridSystem))]
	public class Pathfinder : MonoBehaviour
	{
		private GridSystem grid;
		private PathfinderProcessor processor;

		private void Awake()
		{
			grid = GetComponent<GridSystem>();
			processor = GetComponent<PathfinderProcessor>();
		}

		IEnumerator OnFindPath(Vector3 start, Vector3 target)
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();

			Vector3[] waypoints = new Vector3[0];
			bool pathFound = false;

			Node startingNode = grid.GetNodeFromPosition(start);
			Node endNode = grid.GetNodeFromPosition(target);

			if (startingNode.isWalkable && endNode.isWalkable)
			{
				Heap<Node> openNodes = new Heap<Node>(grid.GetSize());
				HashSet<Node> closedNodes = new HashSet<Node>();

				openNodes.Add(startingNode);

				while (openNodes.Count > 0)
				{
					Node current = openNodes.RemoveFirst();
					closedNodes.Add(current);

					if (current == endNode)
					{
						sw.Stop();
						UnityEngine.Debug.Log($"Path found in: {sw.ElapsedMilliseconds} miliseconds");
						pathFound = true;
						break;
					}

					foreach (var neighbour in grid.GetNeighbouringNodes(current))
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

			processor.FinishProcessing(waypoints, pathFound);
			
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
			int destinationX = Mathf.Abs(first.gridX - second.gridY);
			int destinationY = Mathf.Abs(first.gridY - second.gridY);

			if (destinationX > destinationY)
			{
				return 14 * destinationY + 10 * (destinationX - destinationY);
			}
			return 14 * destinationX + 10 * (destinationY - destinationX); ;
		}
	}
}