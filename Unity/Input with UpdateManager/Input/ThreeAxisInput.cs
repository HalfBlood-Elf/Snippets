using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Argentics.Package.Cinemachine.Samples.InputHandlers
{
	public class ThreeAxisInput : InputListener
	{
		[SerializeField] private KeyCode _forwardPositive = KeyCode.W;
		[SerializeField] private KeyCode _forwardNegative = KeyCode.S;
		[SerializeField] private KeyCode _sidewaysPositive = KeyCode.A;
		[SerializeField] private KeyCode _sidewaysNegative = KeyCode.D;
		[SerializeField] private KeyCode _verticalPositive = KeyCode.LeftShift;
		[SerializeField] private KeyCode _verticalNegative = KeyCode.LeftControl;

		public event UnityAction<Vector3> OnInputUpdated
		{
			add => _onInputUpdated.AddListener(value);
			remove => _onInputUpdated.RemoveListener(value);
		}


		[SerializeField] private UnityEvent<Vector3> _onInputUpdated;
		public override void OnUpdate()
		{
			var input = new Vector3(GetKeyInput(_sidewaysPositive, _sidewaysNegative),
			   GetKeyInput(_verticalPositive, _verticalNegative),
			   GetKeyInput(_forwardPositive, _forwardNegative));

			_onInputUpdated.Invoke(input);
		}


		private static float GetKeyInput(KeyCode positive, KeyCode negative)
		{
			var input = 0f;

			if (Input.GetKey(positive)) input++;
			if (Input.GetKey(negative)) input--;

			return input;
		}
	}
}
