using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphSystem
{
	public class PathfindingAgent : MonoBehaviour
	{
		public float speed = 3.5f;

		private Vector3[] m_path;
		private int m_targetIndex;

		public void MoveTo(Vector3 target)
		{
			PathfinderProcessor.RequestPath(transform.position, target, OnPathFound);
		}

		private void OnPathFound(Vector3[] newPath, bool pathFound)
		{
			if (pathFound)
			{
				m_path = newPath;
				StopCoroutine("OnMoveAlongPath");
				StartCoroutine("OnMoveAlongPath");
			}
		}

		private IEnumerator OnMoveAlongPath()
		{
			Vector3 current = m_path[0];
			m_targetIndex = 0;
			while (true)
			{
				if (transform.position == current)
				{
					++m_targetIndex;
					if (m_targetIndex < m_path.Length)
					{
						current = m_path[m_targetIndex];
					}
					else
					{
						yield break;
					}
				}
				transform.position = Vector3.MoveTowards(transform.position, current, speed * Time.deltaTime);
				yield return null;
			}
		}

		private void OnDrawGizmos()
		{
			if (m_path != null)
			{
				for (int i = m_targetIndex; i < m_path.Length; i++)
				{
					if (i == m_targetIndex)
					{
						Gizmos.DrawLine(transform.position, m_path[i]);
					}
					else
					{
						Gizmos.DrawLine(m_path[i - 1], m_path[i]);
					}
				}
			}
		}
	}
}