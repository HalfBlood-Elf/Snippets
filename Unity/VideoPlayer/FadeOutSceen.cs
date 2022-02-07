using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutSceen : MonoBehaviour
{
	public float TimeToFade { get => timeToFade; }

	[SerializeField] private Image image;
	[SerializeField] private float timeToFade = 1;

	private float blockOnAplha = 1;
	private float blockOffAplha = 0;

	private Coroutine fadeRoutine;
	public void StartFade(bool targetIsBlocked)
	{
		if (fadeRoutine != null) StopCoroutine(fadeRoutine);
		fadeRoutine = StartCoroutine(Fade(targetIsBlocked));
	}

	private void InstantFade(bool targetIsBlocked)
	{
		SetBlockerState(true);
		var targetAlpha = targetIsBlocked ? blockOnAplha : blockOffAplha;
		SetColorAlpha(image, targetAlpha);

	}

	public IEnumerator Fade(bool targetIsBlocked)
	{
		if (timeToFade <= 0)
		{
			InstantFade(targetIsBlocked);
			yield break;
		}
		SetBlockerState(true);

		var targetAlpha = targetIsBlocked ? blockOnAplha : blockOffAplha;
		if(Mathf.Abs(image.color.a - targetAlpha) > 0.001f)
		{
			SetColorAlpha(image, !targetIsBlocked ? blockOnAplha : blockOffAplha);
			var fadeSpeed = (targetAlpha - image.color.a) / timeToFade;
			var startTime = Time.time;
			while (Time.time - startTime < timeToFade && Mathf.Abs(image.color.a - targetAlpha) > 0.001f)
			{
				SetColorAlpha(image, image.color.a + (fadeSpeed * Time.deltaTime));
				yield return null;
			}
		}
		else
		{
			yield return WaitForAnim();
		}
	}

	public void SetBlockerState(bool state)
	{
		image.gameObject.SetActive(state);

	}

	private void SetColorAlpha(Image targetImage, float a)
	{
		var c = targetImage.color;
		c.a = a;
		targetImage.color = c;
	}

	public IEnumerator WaitForAnim()
	{
		yield return new WaitForSeconds(timeToFade);
	}
}
