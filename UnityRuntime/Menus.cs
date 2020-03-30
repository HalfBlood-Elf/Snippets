using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Menus : SceneSingleton<Menus>
{
	[System.Serializable]
	private class Menu
	{
		public Panels type;
		public GameObject panelGo;
		public bool isOvelay;
	}

	[SerializeField] private GameObject loadingSpin;
	[SerializeField] private List<Menu> menus = new List<Menu>();


	public static bool canChangeMenu = true;

	public enum Panels
	{
		MainMenu,
		Message,
		Settings,
		Exit,
		Splash,

	}
	private void Awake()
	{
		var v0 = Vector3.zero;
		foreach (var menu in menus)
		{
			menu.panelGo.transform.localPosition = v0;
		}
	}
	private void Start()
	{
		SelectPanel(Panels.Splash);
		

	}
	private void AskQuit()
	{
		SelectPanel(Panels.Exit);
	}

	public void OnYesExitButton()
	{
		Application.Quit();
	}

	public void ShowLoading(bool enable)
	{
		loadingSpin.transform.localPosition = Vector3.zero;
		loadingSpin.SetActive(enable);
		BackButtonManager.Instance.CanBack = !enable;
	}

	public void SelectPanel(int panelIndex) { SelectPanel((Panels)panelIndex); }

	public void SelectPanel(Panels panel)
	{
		//Debug.Log("selectPanel: "+ panel);
		ShowLoading(false);
		var openMenu = menus.Where((m) => m.type == panel).First();
		if (openMenu.isOvelay)
		{
			openMenu.panelGo.SetActive(true);
		}
		else
		{
			foreach (var menu in menus)
			{
				menu.panelGo.SetActive(menu.type == panel);
			}
		}
	}

	
}
