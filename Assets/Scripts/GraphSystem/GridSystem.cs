using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphSystem
{
	public class GridSystem : MonoBehaviour
	{
		public LayerMask unwalkableLayer;
		public Vector2 gridArea;
		public float nodeRadius;
		public float offsetY = 1f;
		public bool drawGizmos;

		private Node[,] m_grid;
		private float m_nodeDiameter;
		private int m_gridX;
		private int m_gridY;
		private Obstacle[] m_obstacles;

		void Awake()
		{		
			m_obstacles = FindObjectsOfType<Obstacle>();
			InitializeGrid();
		}

		public int GetSize()
		{
			return m_gridX * m_gridY;
		}

		public void InitializeGrid()
		{
			// Calculating the grid area for node placement
			m_nodeDiameter = nodeRadius * 2f;
			m_gridX = Mathf.RoundToInt(gridArea.x / m_nodeDiameter);
			m_gridY = Mathf.RoundToInt(gridArea.y / m_nodeDiameter);

			m_grid = new Node[m_gridX, m_gridY];
			Vector3 bottomLeft = transform.position - Vector3.right * gridArea.x / 2f - Vector3.forward * gridArea.y / 2f; // Bottom left point of the grid area

			for (int i = 0; i < m_gridX; ++i)
			{
				for (int j = 0; j < m_gridY; ++j)
				{
					// Calculate node point, starting with bottom left corner
					Vector3 point = bottomLeft + Vector3.right * (i * m_nodeDiameter + nodeRadius) + Vector3.forward * (j * m_nodeDiameter + nodeRadius);

					//Apply offset
					point.y = offsetY;

					// Check whether or not an there is an obstacle at the point
					bool isWalkable = !CheckCollisions(point, nodeRadius);

					// Create node
					m_grid[i, j] = new Node(isWalkable, point, i, j);
				}
			}
		}

		public Node GetNodeFromPosition(Vector3 point)
		{
			float ratioX = Mathf.Clamp01((point.x + gridArea.x / 2f) / gridArea.x); // Clamping values so the ratio never falls offgrid
			float ratioZ = Mathf.Clamp01((point.z + gridArea.y / 2f) / gridArea.y);


			int coordX = Mathf.RoundToInt((m_gridX - 1) * ratioX);
			int coordY = Mathf.RoundToInt((m_gridY - 1) * ratioZ);

			return m_grid[coordX, coordY];
		}

		public List<Node> GetNeighbouringNodes(Node node)
		{
			List<Node> neighbours = new List<Node>();

			// Checking only neighbouring nodes, only 8 possible nodes
			for (int i = -1; i <= 1; i++)
			{
				for (int j = -1; j <= 1; j++)
				{
					// Check whether its the original node position
					if (i == 0 && j == 0)
					{
						continue;
					}

					// Check if out of bounds
					int coordX = node.gridX + i;
					int coordY = node.gridY + j;

					if (coordX >= 0 && coordX < m_gridX && coordY >= 0 && coordY < m_gridY)
					{
						neighbours.Add(m_grid[coordX, coordY]);
					}
				}
			}

			return neighbours;
		}

		private void OnDrawGizmos()
		{
			if (!drawGizmos)
			{
				return;
			}

			// Area
			Gizmos.DrawWireCube(transform.position, new Vector3(gridArea.x, 1f, gridArea.y));

			// Nodes
			if (m_grid != null)
			{
				foreach (Node node in m_grid)
				{
					// Green is for go, red is for obstacle				
					Gizmos.color = (node.isWalkable) ? Color.green : Color.red;
					Gizmos.DrawSphere(node.position, nodeRadius);
				}
			}
		}

		private bool CheckCollisions(Vector3 point, float radius)
		{
			Vector2 position = new Vector2(point.x, point.z);
			Vector2 objMin = position - Vector2.right * radius - Vector2.up * radius;
			Vector2 objMax = position + Vector2.right * radius + Vector2.up * radius;
			foreach (var obstacle in m_obstacles)
			{
				if (obstacle.IsColliding(objMin, objMax))
				{
					return true;
				}
			}
			return false;
		}
	}
}