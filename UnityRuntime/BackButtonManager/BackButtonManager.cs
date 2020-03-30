using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButtonManager : MonoBehaviour
{
	public GameObject ExitMenu = null;
	public bool CanBack = true;

	public static BackButtonManager Instance;
	private List<BackButtonStateSaver> stateSavers = new List<BackButtonStateSaver>();

	#region singleton

	private void Awake()
	{
		if (!Instance)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	#endregion singleton

	public void AddState(BackButtonStateSaver menu)
	{
		stateSavers.Insert(0, menu);
	}
	public void CloseMenu(BackButtonStateSaver menu)
	{
		if (stateSavers.Contains(menu))
		{
			stateSavers.Remove(menu);
		}
	}

	public bool CheckLastMenu(BackButtonStateSaver menu)
	{
		var bbs = stateSavers[0];

		return bbs == menu;
	}

	public void CloseLastMenu()
	{
		if (!CanBack) return;
		if (GetClosableCount() == 0
			/*&& !stateSavers.Peek()*/)
		{
			PauseOrQuit();
		}
		else
		{
			var bbs = stateSavers[0];
			stateSavers.RemoveAt(0);
			bbs.CloseMenu();
		}
	}

	private void PauseOrQuit()
	{
		Menus.Instance.SelectPanel(Menus.Panels.Exit);
	}

	public int GetClosableCount()
	{
		return stateSavers.Count;
		int count = stateSavers.Count;

		var bbs = stateSavers[0];

		if (count > 0 && !bbs)
		{
			EmptyStack();
			count = 0;
		}
		return count;
	}

	public void EmptyStack()
	{
		stateSavers.Clear();
		return;
		for (int i = 0; i < stateSavers.Count; i++)
		{
			var bbs = stateSavers[0];
			stateSavers.RemoveAt(0);
		}
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			CloseLastMenu();
		}
	}
}