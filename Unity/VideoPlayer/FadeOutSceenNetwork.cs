using UnityEngine;
using Mirror;
class FadeOutSceenNetwork: NetworkBehaviour
{
	public FadeOutSceen fadeOutSceen;

	[ClientRpc]
	public void RpcFadeScreen(bool targetIsBlocked)
	{
		fadeOutSceen.StartFade(targetIsBlocked);
	}

	[TargetRpc]
	public void TargetFadeScreen(NetworkConnection nc, bool targetIsBlocked)
	{
		fadeOutSceen.StartFade(targetIsBlocked);

	}
}
