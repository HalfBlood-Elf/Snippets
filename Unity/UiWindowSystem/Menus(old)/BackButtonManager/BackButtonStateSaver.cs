using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButtonStateSaver : MonoBehaviour
{
	protected bool menuClosed;

	protected virtual void OnEnable()
	{
		menuClosed = false;
		BackButtonManager.Instance?.AddState(this);
	
	}
	public virtual void OnBackButton()
	{
		if (BackButtonManager.Instance.CheckLastMenu(this))
		{
			BackButtonManager.Instance.CloseLastMenu();
		}
	}
	public virtual void CloseMenu()
	{
		if (BackButtonManager.Instance.CanBack)
		{
			menuClosed = true;

			gameObject.SetActive(false);
			BackButtonManager.Instance.CloseMenu(this);
		}
	}

	protected virtual void OnDisable()
	{
		if (!menuClosed)
		{
			CloseMenu();
		}
	}
}