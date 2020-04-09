using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphSystem
{
	public class Grid : MonoBehaviour
	{
		public Transform trackableObject;
		public LayerMask unwalkableLayer;
		public Vector2 gridArea;
		public float nodeRadius;

		private Node[,] m_grid;
		private float m_nodeDiameter;
		private int m_gridX;
		private int m_gridY;

		void Start()
		{
			// Calculating the grid area for node placement
			m_nodeDiameter = nodeRadius * 2f;
			m_gridX = Mathf.RoundToInt(gridArea.x / m_nodeDiameter);
			m_gridY = Mathf.RoundToInt(gridArea.y / m_nodeDiameter);
			InitializeGrid();
		}

		public Node GetNodeFromPosition(Vector3 point)
		{
			float ratioX = Mathf.Clamp01((point.x + gridArea.x / 2f) / gridArea.x); // Clamping values so the ratio never falls offgrid
			float ratioZ = Mathf.Clamp01((point.z + gridArea.y / 2f) / gridArea.y);


			int coordX = Mathf.RoundToInt((gridArea.x - 1) * ratioX);
			int coordY = Mathf.RoundToInt((gridArea.x - 1) * ratioZ);

			return m_grid[coordX, coordY];
		}

		private void InitializeGrid()
		{
			m_grid = new Node[m_gridX, m_gridY];
			Vector3 bottomLeft = transform.position - Vector3.right * gridArea.x / 2f - Vector3.forward * gridArea.y / 2f; // Bottom left point of the grid area

			for (int i = 0; i < m_gridX; i++)
			{
				for (int j = 0; j < m_gridY; j++)
				{
					// Calculate node point, starting with bottom left corner
					Vector3 point = bottomLeft + Vector3.right * (i * m_nodeDiameter + nodeRadius) + Vector3.forward * (j * m_nodeDiameter + nodeRadius);
					
					// Check whether or not an there is an obstacle at the point
					bool isWalkable = !Physics.CheckSphere(point, nodeRadius, unwalkableLayer);

					// Create node
					m_grid[i, j] = new Node(isWalkable, point);
				}
			}
		}

		private void OnDrawGizmos()
		{
			// Area
			Gizmos.DrawWireCube(transform.position, new Vector3(gridArea.x, 1f, gridArea.y));

			// Nodes
			if (m_grid != null)
			{
				Node trackabeNode = GetNodeFromPosition(trackableObject.position);
				foreach (Node node in m_grid)
				{
					// Green is for go, red is for obstacle				
					Gizmos.color = (node.isWalkable) ? Color.green : Color.red;
					if (node == trackabeNode)
					{
						Gizmos.color = Color.yellow;
					}
					Gizmos.DrawWireSphere(node.position, nodeRadius);
				}
			}
		}
	}
}