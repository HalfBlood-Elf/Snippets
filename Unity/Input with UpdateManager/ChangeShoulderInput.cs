using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace InputHandlers
{
	public class ChangeShoulderInput : InputListener
	{
		[SerializeField] private KeyCode _keyCode = KeyCode.Q;
		[SerializeField] private UnityEvent _changeShoulderInputEvent;

		public event UnityAction OnChangeShoulderInput
		{
			add => _changeShoulderInputEvent.AddListener(value);
			remove => _changeShoulderInputEvent.RemoveListener(value);
		}

		public override void OnUpdate()
		{
			if(Input.GetKeyDown(_keyCode))
				_changeShoulderInputEvent?.Invoke();
		}

	}
}
