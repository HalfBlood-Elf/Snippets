using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace InputHandlers
{ 
	public class AimInput : InputListener
	{
		private const int mouseButtonIndex = 1;

		[SerializeField] private UnityEvent _AimDownEvent;
		[SerializeField] private UnityEvent _AimUpEvent;


		public event UnityAction OnAimDownInput
		{
			add => _AimDownEvent.AddListener(value);
			remove => _AimDownEvent.RemoveListener(value);
		}

		public event UnityAction OnAimUpInput
		{
			add => _AimUpEvent.AddListener(value);
			remove => _AimUpEvent.RemoveListener(value);
		}


		public override void OnUpdate()
		{
			if(Input.GetMouseButtonDown(mouseButtonIndex)) _AimDownEvent?.Invoke();
			else if(Input.GetMouseButtonUp(mouseButtonIndex)) _AimUpEvent?.Invoke();
		}
	}
}
