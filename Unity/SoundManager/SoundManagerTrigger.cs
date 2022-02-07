using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SoundManagerSoundEffectTrigger : NetworkBehaviour
{
	[SerializeField] private Transform soundPosition;
	[SerializeField] private SoundManager.SoundEffect effect;

	private SoundManagerEffectSource currentSource;
	[ServerCallback]
	public void Play() => RpcPlayOnClients();
	[ServerCallback]
	public void Stop() => RpcStopOnClients();
	[ClientRpc]
	private void RpcPlayOnClients()
	{
		var source = SoundManager.Instance.PlayEffect(effect, soundPosition);
		if(source != null)
		{
			currentSource = source;
			currentSource.OnPlayEnded += CurrentSource_OnPlayEnded;
		}
	}

	private void CurrentSource_OnPlayEnded()
	{
		currentSource.OnPlayEnded -= CurrentSource_OnPlayEnded;
		currentSource = null;
	}

	[ClientRpc]
	private void RpcStopOnClients()
	{
		if (currentSource == null) return;
		currentSource.StopPlaying();
	}
}
