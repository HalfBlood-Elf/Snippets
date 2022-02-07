using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class SoundManager : NetworkBehaviour
{
	public static SoundManager Instance;
	[SerializeField] private Sound[] sounds;

	[Range(0, 1)]
	[SerializeField] private float effectsVolume = 0.8f;
	[Range(0, 1)]
	[SerializeField] private float musicVolume = 0.8f;

	[Range(0, 1)]
	[SerializeField] float defaultVolume = 0.8f;

	public float DefaultVolume { get => defaultVolume; }
	public float EffectsVolume { get => effectsVolume; set => effectsVolume = value; }
	public float MusicVolume { get => musicVolume; set => musicVolume = value; }

	[Header("Source pool")]
	[SerializeField] private uint startingObjectsInPool;
	[SerializeField] private Transform poolParent;
	[SerializeField] private GameObject poolObjectPrefab;
	private List<SoundManagerEffectSource> pool = new List<SoundManagerEffectSource>();

	private void Awake()
	{
		Instance = this;
	}

	public override void OnStartClient()
	{
		for (int i = 0; i < startingObjectsInPool - pool.Count; i++)
		{
			if (AddNewSource() == null) return;
		}
	}

	private SoundManagerEffectSource AddNewSource()
	{
		SoundManagerEffectSource poolObj = Instantiate(poolObjectPrefab, poolParent).GetComponent<SoundManagerEffectSource>();
		if (!poolObj)
		{
			Debug.LogError("SoundManager pool obj prefab dont have SoundManagerEffectSource");
			return null;
		}
		pool.Add(poolObj);
		return poolObj;
	}


	[ClientCallback]
	public SoundManagerEffectSource PlayEffect(SoundEffect effect, Transform sourcePosition)
	{
		SoundManagerEffectSource availableSource = pool.FirstOrDefault(x => x.isAvailable);
		if(!availableSource)
		{
			Debug.Log("SoundManager no available SoundManagerEffectSource, adding one more.");
			availableSource = AddNewSource();

			Debug.Log($"SoundManager pool has {pool.Count} objects");
		}

		var sound = sounds.FirstOrDefault(x => x.effect == effect);
		if(sound == null)
		{
			Debug.LogError($"SoundManager no sound for {effect.ToString()} effect");
			return null;
		}

		availableSource.PlayWholeClip(sound);

		availableSource.transform.parent = sourcePosition;
		availableSource.transform.position = Vector3.zero;
		return availableSource;
	}


	public enum SoundType
	{
		None,
		Effect,
		Music,

	}

	public float GetVolume(SoundType type)
	{
		var volume = DefaultVolume;
		switch (type)
		{
			default: case SoundType.None:
				volume = 0;
				break;
			case SoundType.Effect:
				volume = EffectsVolume;
				break;
			case SoundType.Music:
				volume = MusicVolume;
				break;
		}
		return volume;
	}

	public enum SoundEffect
	{
		None,
		PickUpTile,
		PlaceDownTileInBridge,
		BridgeCreated,
		DisconnectedBridgeTileFalls,

	}

	[System.Serializable]
	public class Sound
	{
		public AudioClip[] soundVariants;
		public SoundType type;
		public SoundEffect effect;
		[Range(0, 1)] public float clipVolume = 1;

		public AudioClip GetRandomClip()
		{
			if(soundVariants == null || soundVariants.Length == 0)
			{
				Debug.LogError($"No clips in sound with effect {effect}");
				return null;
			}
			return soundVariants[Random.Range(0, soundVariants.Length)];

		}
	}
}
