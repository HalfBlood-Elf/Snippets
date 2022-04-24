using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UpdateManager;

namespace InputHandlers
{
	public class InputListenerUpdateHandler : UpdateManager
	{
		[SerializeField] private InputListener[] _updateManagerEntries;
		protected override IUpdateble[] UpdateManagerEntries => _updateManagerEntries;
	}
}
