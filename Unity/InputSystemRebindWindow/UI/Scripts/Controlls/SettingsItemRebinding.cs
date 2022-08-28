using System;
using System.Collections;
using System.Collections.Generic;
using UI.SystemUI.Settings.Controlls;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace UI.SystemUI.Settings
{
    public class SettingsItemRebinding : SettingsItem
    {
        [SerializeField] private Transform container;
        [SerializeField] private ActionRebindButton primaryButton;
        [SerializeField] private ActionRebindButton alternativeButton;

        private BindDataHolder currentBindDataHolder;

        internal void Setup(
            BindDataHolder bindDataHolder,
            GameObject bindingInProgress,
            Action<InputControl, ActionRebindButton> onMatchRebinding)
        {
            currentBindDataHolder = bindDataHolder;

            Setup(bindDataHolder.name);

            bool canRebind = false;
            if (bindDataHolder is not null && bindDataHolder.bindingConfig is not null) canRebind = bindDataHolder.bindingConfig.canRebind;

            SetupButton(primaryButton, bindDataHolder.primaryBind, canRebind, bindingInProgress, onMatchRebinding);
            SetupButton(alternativeButton, bindDataHolder.alternativeBind, canRebind, bindingInProgress, onMatchRebinding);
        }

        private void SetupButton(ActionRebindButton button,
            BindData bind,
            bool canRebind,
            GameObject bindingInProgress,
            Action<InputControl, ActionRebindButton> onMatchRebinding)
        {
            button.gameObject.SetActive(bind != null);
            if (bind != null)
            {
                button.transform.SetParent(container);
                button.Setup(bind, canRebind, bindingInProgress, onMatchRebinding);
                button.transform.SetAsLastSibling();
            }
        }

        public class Factory : BaseFactory<SettingsItemRebinding> { }
    }
}
