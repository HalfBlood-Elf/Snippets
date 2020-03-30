using UnityEngine;
public class SceneSingleton<T> : MonoBehaviour where T : SceneSingleton<T>
{
	[SerializeField] private bool isPersistent;
	private static T m_Instance = null;
	public static T Instance
	{
		get
		{
			if (m_Instance == null)
			{
				m_Instance = FindObjectOfType<T>();
			}
			return m_Instance;
		}
	}
	protected virtual void Awake()
	{
		if (m_Instance)
		{
			Destroy(gameObject);
		}
		else
		{
			m_Instance = GetComponent<T>();
			if (isPersistent)
				DontDestroyOnLoad(m_Instance.gameObject);
		}
	}
}