using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argentics.Package.Cinemachine.Samples.InputHandlers
{
	public class InputListenerUpdateHandler : UpdateManager
	{
		[SerializeField] private InputListener[] _updateManagerEntries;
		protected override IUpdateble[] UpdateManagerEntries => _updateManagerEntries;
	}
}
