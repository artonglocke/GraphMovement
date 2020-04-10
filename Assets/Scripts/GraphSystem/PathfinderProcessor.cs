using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphSystem
{
	public class PathfinderProcessor : MonoBehaviour
	{
		struct Request
		{
			public Vector3 start;
			public Vector3 end;
			public Action<Vector3[], bool> callback;

			public Request(Vector3 startPosition, Vector3 endPosition, Action<Vector3[], bool> action)
			{
				start = startPosition;
				end = endPosition;
				callback = action;
			}
		}

		private Queue<Request> m_requests = new Queue<Request>();
		private Request m_currentRequest;

		private static PathfinderProcessor s_instance; //  Convert to global messaging system instead of singleton
		private Pathfinder m_pathfinder;
		private bool m_isProcessing;

		private void Awake()
		{
			s_instance = this;
			m_pathfinder = GetComponent<Pathfinder>();
		}

		public static void RequestPath(Vector3 start, Vector3 end, Action<Vector3[], bool> action)
		{
			Request request = new Request(start, end, action);
			s_instance.m_requests.Enqueue(request);
			s_instance.ProcessNext();
		}

		private void ProcessNext()
		{
			if (!m_isProcessing && m_requests.Count > 0)
			{
				m_currentRequest = m_requests.Dequeue();
				m_isProcessing = true;
				m_pathfinder.Begin(m_currentRequest.start, m_currentRequest.end);
			}
		}

		public void FinishProcessing(Vector3[] path, bool success)
		{
			m_currentRequest.callback(path, success);
			m_isProcessing = false;
			ProcessNext();
		}
	}
}