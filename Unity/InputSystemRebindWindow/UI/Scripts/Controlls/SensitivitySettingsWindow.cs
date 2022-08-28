using Argentics.Package.UISystem.Window;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.SystemUI.Settings.Controlls
{
    public class SensitivitySettingsWindow : Window
    {
        [SerializeField] private FloatSettingsItem mouseSensitivitySlider;

        private void Start()
        {
            mouseSensitivitySlider.onValueChanged.AddListener(SetMouseSensitivity);
        }

        public override void OnAppeared()
        {
            GameInputSystem.InputSettingsOverrides.Changed += InputSettingsOverrides_Changed;
            InputSettingsOverrides_Changed();
        }

        public override void OnDisappeared()
        {
            GameInputSystem.InputSettingsOverrides.Changed -= InputSettingsOverrides_Changed;
        }

        private void InputSettingsOverrides_Changed()
        {
            mouseSensitivitySlider.SetValueWithoutNotify(GameInputSystem.InputSettingsOverrides.MouseSensitivity);
        }

        private void SetMouseSensitivity(float sensitivity)
        {
            GameInputSystem.InputSettingsOverrides.MouseSensitivity = sensitivity;
        }
    }
}
