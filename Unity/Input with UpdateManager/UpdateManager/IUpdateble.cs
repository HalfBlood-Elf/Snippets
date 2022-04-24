using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UpdateManager
{
	public interface IUpdateble
	{
		/// <summary>
		/// Function to call in update manager instead of <see cref="MonoBehaviour"/>.Update()
		/// </summary>
		public void OnUpdate();
	}
}
