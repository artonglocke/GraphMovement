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
		private MoveControls m_controls;
		private PathfindingAgent m_agent;

		void Awake()
		{
			m_controls = new MoveControls();
			m_controls.PointDetection.Movement.performed += OnAction;
			m_agent = GetComponent<PathfindingAgent>();
		}

		private void OnAction(InputAction.CallbackContext context)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			int maskId = LayerMask.NameToLayer("Walkable");
			if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform.gameObject.layer == maskId)
			{
				m_agent.MoveTo(hit.point);
			}
		}

		private void OnEnable()
		{
			m_controls.Enable();
		}

		private void OnDisable()
		{
			m_controls.Disable();
		}
	}
}