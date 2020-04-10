using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphSystem;
using System;
using UnityEngine.InputSystem;

namespace PlayerMovement
{
	[RequireComponent(typeof(PathfindingAgent))]
	public class AStarMovement : MonoBehaviour
	{

		private MoveControls controls;
		private PathfindingAgent agent;

		void Awake()
		{
			controls = new MoveControls();
			controls.PointDetection.Movement.performed += OnAction;
			agent = GetComponent<PathfindingAgent>();
		}

		private void OnAction(InputAction.CallbackContext context)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			int maskId = LayerMask.NameToLayer("Walkable");
			if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform.gameObject.layer == maskId)
			{
				agent.MoveTo(hit.point);
			}
		}

		private void OnEnable()
		{
			controls.Enable();
		}

		private void OnDisable()
		{
			controls.Disable();
		}
	}
}