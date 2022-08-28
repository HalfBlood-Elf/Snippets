using Argentics.GameInput;
using System.Collections.Generic;
using System.Linq;
using UI.SystemUI.Settings.Controlls;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InputMonoInstaller : MonoInstaller
{
    [SerializeField] private ButtonBindingConfig buttonBindingConfig;

    public override void InstallBindings()
    {
        var inputSettings = InstallSettings();
        var inputActions = InstallInputActions(inputSettings);

        InstallInputData(inputActions, buttonBindingConfig);
        InstallRestrictions(inputActions);
    }


    private GameInputSystem.InputSettingData InstallSettings()
    {
        GameInputSystem.InputSettingData inputSettings;
        if (Serializator.FileExistsAtPersistendDataPath(GameInputSystem.INPUT_OVERRIDES_SAVE_FILE_PATH))
            inputSettings = Serializator.JsonDeSerialisationFromPersistentDataPath<GameInputSystem.InputSettingData>(GameInputSystem.INPUT_OVERRIDES_SAVE_FILE_PATH);
        else
        {
            inputSettings = new();
        }
        GameInputSystem.InputSettings = inputSettings; 
        Container.Bind<GameInputSystem.InputSettingData>().FromInstance(inputSettings);
        return inputSettings;
    }

    private GameInputActions InstallInputActions(GameInputSystem.InputSettingData inputSettings)
    {
        var inputActions = new GameInputActions();
        inputActions.LoadBindingOverridesFromJson(inputSettings.InputBindingOverridesAsJson);
        inputActions.Enable();
        Container.Bind<GameInputActions>().FromInstance(inputActions);
        GameInputSystem.InputActions = inputActions;
        return inputActions;
    }

    private void InstallInputData(GameInputActions inputActions, ButtonBindingConfig buttonBindingConfig)
    {
        Dictionary<System.Guid, KeyboardInputActionData> allKeyboardControls = new();

        foreach (InputAction inputAction in inputActions.Gameplay.Get().actions)
        {
            var id = inputAction.id.ToString();
            BindingConfig bindingConfig = null;
            if (buttonBindingConfig.ActionToMenuItems.ContainsKey(id)) bindingConfig = buttonBindingConfig.ActionToMenuItems[id];

            var data = new KeyboardInputActionData(inputAction, bindingConfig);
            allKeyboardControls.Add(inputAction.id, data);
        }
        allKeyboardControls = allKeyboardControls.Where(x=>x.Value.bindHolders.Length != 0)
                                                .ToDictionary(pair => pair.Key, pair => pair.Value);

        var allInputActionData = new AllInputActionData() { keyboardInputActionDatas = allKeyboardControls };
        Container.Bind<AllInputActionData>().FromInstance(allInputActionData);
    }

    private void InstallRestrictions(GameInputActions inputActions)
    {
        GameInputSystem.GameInputRestrictions = new();
        GameInputSystem.GameInputRestrictions.InitilizeDebugInput(inputActions);
    }
}
