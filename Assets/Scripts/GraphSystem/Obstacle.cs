using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphSystem
{
	public class Obstacle : MonoBehaviour
	{
		private Vector2 m_min;
		private Vector2 m_max;

		void Awake()
		{
			Vector2 position = new Vector2(transform.position.x, transform.position.z);
			m_min = position - Vector2.right * transform.localScale.x / 2f - Vector2.up * transform.localScale.z / 2f;
			m_max = position + Vector2.right * transform.localScale.x / 2f + Vector2.up * transform.localScale.z / 2f;
		}

		public bool IsColliding(Vector2 nodeMin, Vector2 nodeMax)
		{
			// Simple AABB collision checking
			float topX = nodeMin.x - m_max.x;
			float topY = nodeMin.y - m_max.y;
			float bottomX = m_min.x - nodeMax.x;
			float bottomY = m_min.y - nodeMax.y;

			if (topX > 0f || topY > 0f || bottomX > 0f || bottomY > 0f)
			{
				return false;
			}
			return true;
		}
	}
}