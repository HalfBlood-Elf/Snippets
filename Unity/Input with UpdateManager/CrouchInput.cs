using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace InputHandlers
{
	public class CrouchInput : InputListener
	{
		[SerializeField] private KeyCode _keyCode = KeyCode.C;
		[SerializeField] private UnityEvent _crouchDownEvent;
		[SerializeField] private UnityEvent _crouchUpEvent;

		public event UnityAction CrouchDownInput
		{
			add => _crouchDownEvent.AddListener(value);
			remove => _crouchDownEvent.RemoveListener(value);
		}

		public event UnityAction CrouchUpInput
		{
			add => _crouchUpEvent.AddListener(value);
			remove => _crouchUpEvent.RemoveListener(value);
		}

		public override void OnUpdate()
		{
			if (Input.GetKeyDown(_keyCode)) _crouchDownEvent?.Invoke();
			else if (Input.GetKeyUp(_keyCode)) _crouchUpEvent?.Invoke();
		}
	}
}
