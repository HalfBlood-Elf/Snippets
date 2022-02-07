using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class StartVideoPlay : NetworkBehaviour
{
	[SerializeField] private VideoPlayerManager videoPlayerManager;
	[SerializeField] private FadeOutSceen fadeOutSceen;
	[SerializeField] private AudioSource preFlightSource;

	[SerializeField] private UnityEngine.Events.UnityEvent OnVideoEndedOnServer;

	[ServerCallback]
	public void PlayVideoAndTriggerEvent()
	{
		RpcStartVideo();
		StartCoroutine(WaitForVideo(() =>
			{
				OnVideoEndedOnServer?.Invoke();
				Debug.Log("PlayVideoAndTriggerEvent WaitForVideo done");
			}
		));
	}


	[ClientRpc]
	public void RpcStartVideo()
	{
		StartCoroutine(VideoPlaying());
	}

	private IEnumerator VideoPlaying()
	{
		preFlightSource.Play();
		yield return new WaitForSeconds(preFlightSource.clip.length);
		yield return fadeOutSceen.Fade(true);

		//fadeOutSceen.SetBlockerState(false);
		yield return videoPlayerManager.PlayVideo();
	}

	[ClientRpc]
	public void RpcDisableBlockers()
	{
		fadeOutSceen.SetBlockerState(false);
		videoPlayerManager.SetScreenState(false);
	}

	private IEnumerator WaitForVideo(System.Action onWaitEnd)
	{
		yield return new WaitForSeconds(preFlightSource.clip.length);
		yield return fadeOutSceen.WaitForAnim();
		yield return videoPlayerManager.WaitForVideoLength();
		onWaitEnd();
	}
}
