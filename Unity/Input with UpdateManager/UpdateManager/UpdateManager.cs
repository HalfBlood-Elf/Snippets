using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UpdateManager
{
	public abstract class UpdateManager: MonoBehaviour
	{
		protected abstract IUpdateble[] UpdateManagerEntries { get; }


		protected virtual void Update()
		{
			foreach (var entry in UpdateManagerEntries)
			{
				entry.OnUpdate();
			}
		}
	}
}
