using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.SystemUI.Settings
{
    public class FloatSettingsItem : SettingsItem, IInitializable
    {
        [SerializeField] protected TextMeshProUGUI valueText;
        [SerializeField] protected string numberFormat = "F0";
        [SerializeField] protected Slider slider;
        [SerializeField] protected float minValue = 0;
        [SerializeField] protected float maxValue = 1;
        [SerializeField] protected bool wholeNumbers = false;

        [SerializeField] protected Button minusButton;
        [SerializeField] protected Button plusButton;
        [SerializeField] protected float buttonChangeValue = 0.1f;
        public UnityEvent<float> onValueChanged;

        public bool IsInitialized { get; protected set; }
        public Slider Slider => slider;

        private void Start()
        {
            if (IsInitialized) return;

            slider.onValueChanged.AddListener(OnSliderValueChanged);
            slider.minValue = minValue;
            slider.maxValue = maxValue;
            slider.wholeNumbers = wholeNumbers;
            SetVisuals();

            minusButton.onClick.AddListener(OnMinusButtonClick);
            plusButton.onClick.AddListener(OnPlusButtonClick);

            IsInitialized = true;
        }

        private void OnSliderValueChanged(float newValue)
        {
            onValueChanged?.Invoke(newValue);
            valueText.text = newValue.ToString(numberFormat);
        }

        private void OnMinusButtonClick()
        {
            slider.value += -buttonChangeValue;
        }

        private void OnPlusButtonClick()
        {
            slider.value += buttonChangeValue;
        }

        public void SetValueWithoutNotify(float newValue)
        {
            if (!IsInitialized) Start();
            slider.SetValueWithoutNotify(newValue);
            SetVisuals();
        }

        private void SetVisuals()
        {
            valueText.text = slider.value.ToString(numberFormat);
        }

        #if UNITY_EDITOR
        private void OnValidate()
        {
            slider.minValue = minValue;
            slider.maxValue = maxValue;
            slider.wholeNumbers = wholeNumbers;
            SetVisuals();
        }
        #endif
    }
}
