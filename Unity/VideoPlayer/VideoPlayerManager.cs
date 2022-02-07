using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerManager : MonoBehaviour
{
	[SerializeField] private VideoPlayer videoPlayer;
	[SerializeField] private GameObject videoImageObject;


	public IEnumerator PlayVideo()
	{
		SetScreenState(true);
		videoPlayer.Play();

		yield return new WaitForSeconds((float)videoPlayer.clip.length);

		//todo: videoSkiping
	}

	public void SetScreenState(bool state)
	{
		videoImageObject.SetActive(state);

	}

	public IEnumerator WaitForVideoLength()
	{
		yield return new WaitForSeconds((float)videoPlayer.clip.length);
	}
}
