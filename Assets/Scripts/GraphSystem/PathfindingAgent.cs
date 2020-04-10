using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphSystem
{
	public class PathfindingAgent : MonoBehaviour
	{
		public float speed = 3.5f;

		private Vector3[] path;
		private int targetIndex;

		public void MoveTo(Vector3 target)
		{
			PathfinderProcessor.RequestPath(transform.position, target, OnPathFound);
		}

		private void OnPathFound(Vector3[] newPath, bool pathFound)
		{
			if (pathFound)
			{
				path = newPath;
				StopCoroutine("OnMoveAlongPath");
				StartCoroutine("OnMoveAlongPath");
			}
		}

		private IEnumerator OnMoveAlongPath()
		{
			Vector3 current = path[0];
			targetIndex = 0;
			while (true)
			{
				if (transform.position == current)
				{
					++targetIndex;
					if (targetIndex < path.Length)
					{
						current = path[targetIndex];
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
			if (path != null)
			{
				for (int i = targetIndex; i < path.Length; i++)
				{
					if (i == targetIndex)
					{
						Gizmos.DrawLine(transform.position, path[i]);
					}
					else
					{
						Gizmos.DrawLine(path[i - 1], path[i]);
					}
				}
			}
		}
	}
}