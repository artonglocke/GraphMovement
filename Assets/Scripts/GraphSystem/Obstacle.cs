using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphSystem
{
	public class Obstacle : MonoBehaviour
	{
		private Vector2 min;
		private Vector2 max;

		void Awake()
		{
			Vector2 position = new Vector2(transform.position.x, transform.position.z);
			min = position - Vector2.right * transform.localScale.x / 2f - Vector2.up * transform.localScale.z / 2f;
			max = position + Vector2.right * transform.localScale.x / 2f + Vector2.up * transform.localScale.z / 2f;
		}

		public bool IsColliding(Vector2 nodeMin, Vector2 nodeMax)
		{
			// Simple AABB collision checking
			float topX = nodeMin.x - max.x;
			float topY = nodeMin.y - max.y;
			float bottomX = min.x - nodeMax.x;
			float bottomY = min.y - nodeMax.y;

			if (topX > 0f || topY > 0f || bottomX > 0f || bottomY > 0f)
			{
				return false;
			}
			return true;
		}
	}
}