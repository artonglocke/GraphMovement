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

		private Queue<Request> requests = new Queue<Request>();
		Request currentRequest;

		private static PathfinderProcessor instance; //  Convert to global messaging system instead of singleton
		private Pathfinder pathfinder;
		private bool isProcessing;

		private void Awake()
		{
			instance = this;
			pathfinder = GetComponent<Pathfinder>();
		}

		public static void RequestPath(Vector3 start, Vector3 end, Action<Vector3[], bool> action)
		{
			Request request = new Request(start, end, action);
			instance.requests.Enqueue(request);
			instance.ProcessNext();
		}

		private void ProcessNext()
		{
			if (!isProcessing && requests.Count > 0)
			{
				currentRequest = requests.Dequeue();
				isProcessing = true;
				pathfinder.Begin(currentRequest.start, currentRequest.end);
			}
		}

		public void FinishProcessing(Vector3[] path, bool success)
		{
			currentRequest.callback(path, success);
			isProcessing = false;
			ProcessNext();
		}
	}
}