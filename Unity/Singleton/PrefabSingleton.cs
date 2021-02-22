using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	protected Singleton() { }
	private static T instance;

	public static T Instance
	{
		get
		{
			if (instance == null)
				instance = FindObjectOfType<T>();
			if (instance == null)
			{
				UnityEngine.Object manager = Resources.Load(typeof(T).Name);
				if (manager != null)
					Instantiate(manager);
				else Debug.LogErrorFormat("Cant find prefab for {0} Singleton", name);
			}
			return instance;
		}
	}

}