using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI.SystemUI.Settings
{
    public class SettingsItem : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI title;

        public void Setup(string name)
        {
            title.text = name;
        }

        public abstract class BaseFactory<TValue>: PlaceholderFactory<TValue> where TValue: SettingsItem { } 

        public class SettingsFactory: BaseFactory<SettingsItem> { }
    }
}
