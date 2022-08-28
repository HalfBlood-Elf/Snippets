using System.Collections;
using System.Collections.Generic;
using UI.SystemUI.Settings.Controlls;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.SystemUI.Settings
{
    public abstract class SettingsItemGroupBase : SettingsItem
    {
        [SerializeField] protected Transform container;

        protected readonly List<SettingsItem> activeSettingItem = new();

        public Transform Container => container;

        public void Setup(
            InputActionData inputActionData,
            GameObject bindingInProgress,
            SettingsItemRebinding.Factory settingsItemRebindingFactory,
            System.Action<InputControl, ActionRebindButton> onMatchRebinding)
        {
            Setup(inputActionData.inputAction.name);

            var length = inputActionData.bindHolders.Length;
            for (int i = 0; i < length; i++)
            {
                var item = settingsItemRebindingFactory.Create();
                item.Setup(inputActionData.bindHolders[i], bindingInProgress, onMatchRebinding);

                item.transform.SetParent(container);
                item.gameObject.SetActive(true);
                activeSettingItem.Add(item);
            }
        }

        public void AddSettingsItem(SettingsItemRebinding settingsItemRebinding,
            BindDataHolder bindHolder,
            GameObject bindingInProgress,
            System.Action<InputControl, ActionRebindButton> onMatchRebinding)
        {
            settingsItemRebinding.Setup(bindHolder, bindingInProgress, onMatchRebinding);

            settingsItemRebinding.transform.SetParent(container);
            settingsItemRebinding.gameObject.SetActive(true);
            activeSettingItem.Add(settingsItemRebinding);
        }

        public void ReturnItemsToPool(ComponentObjectPooler<SettingsItemRebinding> rebindingItemPooler)
        {
            for (int i = 0; i < activeSettingItem.Count; i++)
            {
                var item = activeSettingItem[i];
                if (item is SettingsItemRebinding itemRebinding)
                {
                    itemRebinding.transform.SetParent(rebindingItemPooler.Container);
                    rebindingItemPooler.ReturnToPool(itemRebinding);
                }
            }
            activeSettingItem.Clear();
        }
    }
}