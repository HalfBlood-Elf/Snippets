using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnimatedToggle : Selectable, IPointerClickHandler, IEventSystemHandler, ISubmitHandler, ICanvasElement
{
	[System.Serializable]
	public class ToggleEvent : UnityEvent<bool> { }
	public enum ToggleTransition
	{
		None,
		Animate
	}

	public Image background;
	public Image fillImage;
	public float minFill = 0, maxFill = 1;
	public Image knob;
	public float minPosX = -70, maxPosX = 70;

	[Space()]
	[SerializeField]
	private bool m_IsOn;
	public ToggleTransition toggleTransition = ToggleTransition.Animate;
	public float speed = 10;
	[Space()]
	public ToggleEvent onValueChanged = new ToggleEvent();
	public bool isOn
	{
		get { return m_IsOn; }

		set
		{
			Set(value);
		}
	}

	Vector2 anchorMin = Vector2.zero;
	Vector2 anchorMax = Vector2.one;
	private float threshold = 0.1f;
	private IEnumerator routine;



	protected override void Awake()
	{
		base.Awake();
		if(background)
			background.raycastTarget = true;
		if (fillImage)
			fillImage.raycastTarget = false;
		if (knob)
			knob.raycastTarget = false;

		//var toggle = gameObject.AddComponent<Toggle>();
		//toggle.onValueChanged

	}


	private void InternalToggle()
	{
		if (!IsActive() || !IsInteractable())
			return;

		isOn = !isOn;
	}
	/// <summary>
	/// Set isOn without invoking onValueChanged callback.
	/// </summary>
	/// <param name="value">New Value for isOn.</param>
	public void SetIsOnWithoutNotify(bool value)
	{
		Set(value, false);
	}

	void Set(bool value, bool sendCallback = true)
	{
		m_IsOn = value;
		PlayEffect(toggleTransition == ToggleTransition.None);
		if (m_IsOn == value)
			return;
		if (sendCallback)
		{
			onValueChanged.Invoke(m_IsOn);
		}
	}

	private void PlayEffect(bool instant)
	{
		if (!gameObject.activeSelf || !gameObject.activeInHierarchy) return;
#if UNITY_EDITOR
		if (!Application.isPlaying)
		{
			instant = true;
		}
#endif
		if (instant)
		{
			if (fillImage != null)
				fillImage.fillAmount = m_IsOn ? maxFill : minFill;
			if (knob != null)
			{
				var pos = knob.transform.localPosition;
				pos.x = m_IsOn ? maxPosX : minPosX;
				knob.transform.localPosition = pos;
			}
		}
		else
		{
			if (routine != null)
			{
				if (fillImage != null)
					fillImage.fillAmount = !m_IsOn ? maxFill : minFill;
				if (knob != null)
				{
					var pos = knob.transform.localPosition;
					pos.x = !m_IsOn ? maxPosX : minPosX;
					knob.transform.localPosition = pos;
				}
				StopCoroutine(routine);
				routine = null;
			}
			routine = Animate(m_IsOn);
			StartCoroutine(routine);
		}
	}
	private IEnumerator Animate(bool targetState)
	{
		float normalisedValue = 0;
		float targetPosX = (targetState) ? maxPosX : minPosX;
		float targetFill = (targetState) ? maxFill : minFill;
		while (Mathf.Abs(1 - normalisedValue) > threshold)
		{
			if (fillImage != null)
				fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, targetFill, normalisedValue);
			if(knob != null)
			{
				var pos = knob.transform.localPosition;
				pos.x = Mathf.Lerp(pos.x , targetPosX, normalisedValue);
				knob.transform.localPosition = pos;
			}
			normalisedValue = Mathf.Lerp(0, 1, 1 - Mathf.Exp(-speed * Time.deltaTime));
			yield return null;
		}
	}

	/// <summary>
	/// Assume the correct visual state.
	/// </summary>
	protected override void Start()
	{
		PlayEffect(true);
	}




	/// <summary>
	/// React to clicks.
	/// </summary>
	public virtual void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left)
			return;

		InternalToggle();
	}

	public virtual void OnSubmit(BaseEventData eventData)
	{
		InternalToggle();
	}

#if UNITY_EDITOR
	protected override void OnValidate()
	{
		base.OnValidate();
		Set(m_IsOn, false);
		if (!UnityEditor.PrefabUtility.IsPartOfPrefabAsset(this) && !Application.isPlaying)
			CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
	}

	public void Rebuild(CanvasUpdate executing)
	{
#if UNITY_EDITOR
		if (executing == CanvasUpdate.Prelayout)
			onValueChanged.Invoke(isOn);
#endif
	}

	public void LayoutComplete() { }

	public void GraphicUpdateComplete() { }

#endif // if UNITY_EDITOR
}
