using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphSystem;

namespace PlayerMovement
{
	public class PointMovement : MonoBehaviour
	{
		private MoveControls controls;
		private
		// Start is called before the first frame update
		void Awake()
		{

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