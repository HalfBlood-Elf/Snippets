using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManagerEffectSource : MonoBehaviour
{
	public bool isAvailable = true;
	public event System.Action OnPlayEnded;

	[SerializeField] private AudioSource source;

	private Coroutine playingRoutine;

	private AudioSource Source { 
		get
		{
			if (!source) source = GetComponent<AudioSource>();
			return source; 
		} 
	}

	public void PlayWholeClip(SoundManager.Sound sound)
	{
		if(isAvailable && playingRoutine == null)
		{
			playingRoutine = StartCoroutine(PlayingClip(sound));
		}
		else
		{
			Debug.LogError("Cant play clip on already busy source");
		}
	}

	private IEnumerator PlayingClip(SoundManager.Sound sound)
	{
		isAvailable = false;
		gameObject.SetActive(true);

		Source.clip = sound.clip;
		Source.volume = SoundManager.Instance.GetVolume(sound.type) * source.volume;
		Source.Play();

		yield return new WaitForSeconds(sound.clip.length);

		Source.Stop();
		isAvailable = true;
		gameObject.SetActive(false);
		OnPlayEnded?.Invoke();

	}

	public void StopPlaying()
	{
		if (playingRoutine != null) StopCoroutine(playingRoutine);
		isAvailable = true;
		OnPlayEnded?.Invoke();
	}
}
