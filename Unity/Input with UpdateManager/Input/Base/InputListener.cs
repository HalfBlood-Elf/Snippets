using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UpdateManager;
namespace InputHandlers
{
	public abstract class InputListener : MonoBehaviour, IUpdateble
	{
		public abstract void OnUpdate();
	}
}
