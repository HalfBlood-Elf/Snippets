using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argentics.Package.Cinemachine.Samples.InputHandlers
{
	public abstract class InputListener : MonoBehaviour, IUpdateble
	{
		public abstract void OnUpdate();
	}
}
