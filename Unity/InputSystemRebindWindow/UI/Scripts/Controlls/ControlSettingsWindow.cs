using Argentics.GameInput;
using Argentics.Package.UISystem.Queuing;
using Argentics.Package.UISystem.Router;
using Argentics.Package.UISystem.Switcher;
using Argentics.Package.UISystem.Window;
using Argentics.Package.UISystem.WindowFactory;
using System.Collections;
using System.Collections.Generic;
using UI.SystemUI.Settings.Controlls;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UnityEngine.InputSystem;

namespace UI.SystemUI.Settings.Controlls
{
    public class ControlSettingsWindow : Window, IInitializable
    {
        [SerializeField] private KeyBindingsSettingsWindow keyBindingsSettingsWindow;
        [SerializeField] private SensitivitySettingsWindow sensitivitySettingsWindow;

        [SerializeField] private Button defaultSettingsButton;
        [SerializeField] private Button revertChangesButton;

        protected Canvas canvas;
        protected Router<PopupSwitcher, LastQueuingSingleWindow> router;


        private bool SettingsChanged => !GameInputSystem.InputSettingsOverrides.Equals(GameInputSystem.InputSettings);

        public bool IsInitialized { get; private set; }

        private GameInputActions gameInputActions;

        [Inject]
        private void Construct(DiContainer diContainer)
        {
            gameInputActions = diContainer.Resolve<GameInputActions>();
        }

        private void Start()
        {
            if (IsInitialized) return;

            canvas = GetComponent<Canvas>();
            revertChangesButton.onClick.AddListener(RevertCurrentSettingsChanges);
            defaultSettingsButton.onClick.AddListener(ResetControllSettingsToDefault);

            router = new(canvas.sortingOrder + 1)
            {
                new Argentics.Package.UISystem.WindowFactory.IFactory[]
                {
                    new SceneReferenceFactory<KeyBindingsSettingsWindow>(keyBindingsSettingsWindow),
                    new SceneReferenceFactory<SensitivitySettingsWindow>(sensitivitySettingsWindow)
                }
            };
            IsInitialized = true;
        }


        public override void OnAppeared()
        {
            if (!IsInitialized) Start();

            GameInputSystem.InputSettingsOverrides.CopyFrom(GameInputSystem.InputSettings);
            GameInputSystem.InputSettingsOverrides.Changed += OnInputSettingsOverridesChanged;

            router.New<KeyBindingsSettingsWindow>().Run();
            router.New<SensitivitySettingsWindow>().Run();
            OnInputSettingsOverridesChanged();
        }
        
        public override void OnDisappeared()
        {
            TrySaveChanges();
            GameInputSystem.InputSettingsOverrides.Changed -= OnInputSettingsOverridesChanged;
        }

        private void ResetControllSettingsToDefault()
        {
            var defaultSettings = new GameInputSystem.InputSettingData();
            ApplySettings(defaultSettings); // we need to apply overrides befor Changed event for proper update of ui (for example)
            GameInputSystem.InputSettingsOverrides.CopyFrom(defaultSettings);
            //GameInputSystem.InputActions.RemoveAllBindingOverrides();
        }

        private void OnInputSettingsOverridesChanged()
        {
            revertChangesButton.interactable = SettingsChanged;
        }

        private void RevertCurrentSettingsChanges()
        {
            ApplySettings(GameInputSystem.InputSettings); // we need to apply overrides befor Changed event for proper update of ui (for example)
            GameInputSystem.InputSettingsOverrides.CopyFrom(GameInputSystem.InputSettings);
        }

        private void ApplySettings(GameInputSystem.InputSettingData settings)
        {
            gameInputActions.LoadBindingOverridesFromJson(settings.InputBindingOverridesAsJson);
        }

        private void TrySaveChanges()
        {
            if (SettingsChanged)
            {
                GameInputSystem.InputSettings.CopyFrom(GameInputSystem.InputSettingsOverrides);
                GameInputSystem.SaveInputSettings();
            }
        }

    }
}
