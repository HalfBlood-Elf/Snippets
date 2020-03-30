using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UnityEngine.UI
{
	public class CustomDropdown : Dropdown
	{
		[Space()]
		public GameObject blocker = null;

		protected override GameObject CreateBlocker(Canvas rootCanvas)
		{
			if(blocker == null)
				blocker = base.CreateBlocker(rootCanvas);
			blocker.SetActive(true);
			var blockerButton = blocker.GetComponent<Button>();
			blockerButton.onClick.RemoveAllListeners();
			blockerButton.onClick.AddListener(Hide);
			return blocker;
		}

		protected override void DestroyBlocker(GameObject blocker)
		{
			blocker.SetActive(false);
		}

	}
}