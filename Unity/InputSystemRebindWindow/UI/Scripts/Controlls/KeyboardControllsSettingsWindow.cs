using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Argentics.Package.UISystem.Window;
using UI.SystemUI.Settings.Controlls;
using UnityEngine.UI;
using System.Linq;
using System;
using System.Text;
using Zenject;

namespace UI.SystemUI.Settings.Controlls
{
    public class KeyboardControllsSettingsWindow : Window
    {
        private const string BINDING_IN_PROGRESS = nameof(KeyboardControllsSettingsWindow)+ ".bindingInProgress";
        private const string MATCH_REBINDING_MESSAGE_FORMAT = "Do you want to unbind {0} from <b>{1}</b> ?";
        [SerializeField] private MatchBindingPopup matchBindingPopup;
        [SerializeField] private GameObject bindingInProgress;
        [SerializeField] private ButtonBindingConfig config;
        [SerializeField] private Transform container;

        private ActionRebindButton _currentActionRebindButton;
        private InputControl _inputControlToRebind;
        private IReadOnlyList<BindData> _matchedDatas;
        
        private SettingsItemGroup.Factory settingsItemGroupFactory;
        private SettingsItemHeaderGroup.Factory settingsItemHeaderGroupFactory;
        private SettingsItemRebinding.Factory settingsItemRebindingFactory;
        private AllInputActionData allInputActionData;

        [Inject]
        private void Construct(DiContainer diContainer)
        {
            settingsItemGroupFactory = diContainer.Resolve<SettingsItemGroup.Factory>();
            settingsItemHeaderGroupFactory = diContainer.Resolve<SettingsItemHeaderGroup.Factory>();
            settingsItemRebindingFactory = diContainer.Resolve<SettingsItemRebinding.Factory>();

            allInputActionData = diContainer.Resolve<AllInputActionData>();

        }

        public override void OnAppeared()
        {
            UpdateInputOverrides();
            GameInputSystem.InputSettingsOverrides.Changed += OnInputSettingsOverridesChanged;
        }

        public override void OnDisappeared()
        {
            GameInputSystem.InputSettingsOverrides.Changed -= OnInputSettingsOverridesChanged;
        }

        private void Start()
        {
            matchBindingPopup.gameObject.SetActive(false);
            List<InputActionData> _unsortedKeyboardControlsToShow = new();

            var settingItems = new SettingItemGroupNode();
            foreach (var item in allInputActionData.keyboardInputActionDatas)
            {
                var id = item.Key.ToString();
                BindingConfig bindingConfig = null;
                if (config.ActionToMenuItems.ContainsKey(id)) bindingConfig = config.ActionToMenuItems[id];
                if (bindingConfig != null)
                {
                    settingItems.Add(item.Value, bindingConfig.PathSplited);
                }
                else
                {
                    _unsortedKeyboardControlsToShow.Add(item.Value);
                }
            }

            settingItems.SpawnObjects(
                container, 
                settingsItemGroupFactory,
                settingsItemHeaderGroupFactory,
                settingsItemRebindingFactory,
                bindingInProgress,
                OnPotentialMatchRebinding);

            // inputActions not in config
            foreach (InputActionData item in _unsortedKeyboardControlsToShow)
            {
                SetupItem(item, null);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(container as RectTransform);
        }

        private void SetupItem(InputActionData inputData, BindingConfig menuItemConfig)
        {
            SettingsItem settingItem;
            if (inputData.bindHolders.Length == 0) return;

            if (inputData.bindHolders.Length > 1)
            {
                var compositeRebindingItem = settingsItemGroupFactory.Create();
                compositeRebindingItem.Setup(inputData, bindingInProgress, settingsItemRebindingFactory, OnPotentialMatchRebinding);
                settingItem = compositeRebindingItem;
            }
            else
            {
                var rebindingItem = settingsItemRebindingFactory.Create();
                rebindingItem.Setup(inputData.bindHolders[0], bindingInProgress, OnPotentialMatchRebinding);
                settingItem = rebindingItem;
            }
            settingItem.transform.SetParent(container);
            settingItem.gameObject.SetActive(true);
        }

        private void OnPotentialMatchRebinding(InputControl inputControl, ActionRebindButton button)
        {
            _currentActionRebindButton = button;
            _inputControlToRebind = inputControl;
            _matchedDatas = FindMatchedBindActionName(inputControl);
            if (_matchedDatas.Count==0)
            {
                button.Bind.inputAction.ApplyBindingOverride(button.Bind.bindIndex, inputControl.path);
                button.Bind.UpdateBind();
                SaveInputs();
                return;
            }

            GameInputSystem.GameInputRestrictions.SettingsMenuExitInputBlocked.SetSourceIsActive(true, BINDING_IN_PROGRESS);
            bindingInProgress.SetActive(false);
            KeyboardControlsOnMatchRebinding(inputControl, _matchedDatas);
        }

        private IReadOnlyList<BindData> FindMatchedBindActionName(InputControl inputControl)
        {
            List<BindData> matchData = new();
            foreach (var item in allInputActionData.keyboardInputActionDatas.Values)
            {
                foreach (var bind in item.bindHolders)
                {
                    if (bind.alternativeBind is not null && InputSystemExtentions.Matches(bind.primaryBind.Binding.effectivePath, inputControl)) matchData.Add(bind.primaryBind);
                    if (bind.alternativeBind is not null && InputSystemExtentions.Matches(bind.alternativeBind.Binding.effectivePath, inputControl)) matchData.Add(bind.alternativeBind);
                }
            }
            return matchData;
        }
        private void KeyboardControlsOnMatchRebinding(InputControl inputControl, IReadOnlyList<BindData> matchedDatas)
        {
            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine();
            for (int i = 0; i < matchedDatas.Count; i++)
            {
                if (i > 0)
                {
                    stringBuilder.Append(",");
                    stringBuilder.AppendLine();
                }
                var data = matchedDatas[i];
                stringBuilder.Append(data.holder.bindingConfig.path);
            }
            string message = string.Format(MATCH_REBINDING_MESSAGE_FORMAT, inputControl.displayName, stringBuilder.ToString());
            matchBindingPopup.Setup(message, OnMatchPopupResult);
        }

        private void OnMatchPopupResult(bool isSuccess, bool leaveOld)
        {
            if (isSuccess)
            {
                _currentActionRebindButton.Bind.inputAction.ApplyBindingOverride(_currentActionRebindButton.Bind.bindIndex, _inputControlToRebind.path);
                _currentActionRebindButton.Bind.UpdateBind();

                if (!leaveOld)
                {
                    foreach (var item in _matchedDatas)
                    {
                        item.inputAction.ApplyBindingOverride(item.bindIndex, string.Empty);
                        item.UpdateBind();
                    }
                    SaveInputs();
                }
            }

            GameInputSystem.GameInputRestrictions.SettingsMenuExitInputBlocked.SetSourceIsActive(false, BINDING_IN_PROGRESS);
            _currentActionRebindButton = null;
            _inputControlToRebind = null;
            _matchedDatas = null;
        }

        private void OnInputSettingsOverridesChanged()
        {
            UpdateInputOverrides();
        }

        public void UpdateInputOverrides()
        {
            foreach (var item in allInputActionData.keyboardInputActionDatas.Values)
            {
                foreach (var holder in item.bindHolders)
                {
                    holder.primaryBind?.UpdateBind();
                    holder.alternativeBind?.UpdateBind();
                }
            }
        }

        private void SaveInputs()
        {
            GameInputSystem.InputSettingsOverrides.SaveInputOverridesFromInputActions(GameInputSystem.InputActions);
        }

        [System.Serializable]
        private class SettingItemGroupNode
        {
            private const int ROOT_INDEX = 0;
            private const string ROOT_ITEM_NAME = "root";
            private InputActionData inputActionData;
            public Dictionary<string, SettingItemGroupNode> nestedItems;

            private readonly int nestingIndex = 0;

            public SettingItemGroupNode() : this(ROOT_INDEX, ROOT_ITEM_NAME) { }

            public SettingItemGroupNode(int nestingIndex, string name)
            {
                this.nestingIndex = nestingIndex;
            }

            public void Add(InputActionData item, string[] path)
            {
                if (path is null || path.Length == 0) return;

                if (nestingIndex >= path.Length)
                {
                    this.inputActionData = item;
                }
                else
                {
                    var groupName = path[nestingIndex];

                    if (nestedItems is null) nestedItems = new();
                    if (!nestedItems.ContainsKey(groupName))
                    {
                        nestedItems.Add(groupName, new SettingItemGroupNode(nestingIndex + 1, groupName));
                    }
                    nestedItems[groupName].Add(item, path);
                }
            }

            public void SpawnObjects(
                Transform container,
                SettingsItemGroup.Factory settingsItemGroupFactory,
                SettingsItemHeaderGroup.Factory settingsItemHeaderGroupFactory,
                SettingsItemRebinding.Factory settingsItemRebindingFactory,
                GameObject bindingInProgress,
                Action<InputControl, ActionRebindButton> onMatchRebinding)
            {
                if(nestedItems is not null && nestedItems.Count > 0)
                {
                    foreach (var item in nestedItems)
                    {
                        var nextContainer = container;
                        if (item.Value.nestedItems is not null && item.Value.nestedItems.Count > 0)
                        {
                            SettingsItemGroupBase group = null;
                            if (nestingIndex == ROOT_INDEX)
                            {
                                var header = settingsItemHeaderGroupFactory.Create();
                                header.Setup(item.Key);
                                group = header;
                            }
                            else
                            {
                                var compositeRebindingItem = settingsItemGroupFactory.Create();
                                compositeRebindingItem.Setup(item.Key);
                                group = compositeRebindingItem;
                            }
                            group.transform.SetParent(container);
                            group.gameObject.SetActive(true);
                            nextContainer = group.Container;
                        }
                        
                        item.Value.SpawnObjects(
                            nextContainer,
                            settingsItemGroupFactory,
                            settingsItemHeaderGroupFactory,
                            settingsItemRebindingFactory,
                            bindingInProgress,
                            onMatchRebinding);
                    }
                }
                else if(inputActionData != null)
                {
                    SettingsItem settingItem;
                    if (inputActionData.bindHolders.Length == 0)
                    {
                        Debug.LogError("InputData Have 0 bindHolders. InputAction: " + inputActionData.inputAction.name);
                        return;
                    }
                    if (inputActionData.bindHolders.Length > 1)
                    {
                        var compositeRebindingItem = settingsItemGroupFactory.Create();
                        compositeRebindingItem.Setup(inputActionData, bindingInProgress, settingsItemRebindingFactory, onMatchRebinding);
                        settingItem = compositeRebindingItem;
                    }
                    else
                    {
                        var rebindingItem = settingsItemRebindingFactory.Create();
                        rebindingItem.Setup(inputActionData.bindHolders[0], bindingInProgress, onMatchRebinding);
                        settingItem = rebindingItem;
                    }
                    settingItem.transform.SetParent(container);
                    settingItem.gameObject.SetActive(true);
                }
            }
        }
    }
}

