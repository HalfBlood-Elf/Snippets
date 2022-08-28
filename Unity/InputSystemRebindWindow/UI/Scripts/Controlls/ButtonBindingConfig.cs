using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.SystemUI.Settings.Controlls
{
    [CreateAssetMenu(menuName = "Config/" + nameof(ButtonBindingConfig))]
    public class ButtonBindingConfig : ScriptableObject
    {
        public BindingConfig[] bindingConfigs;

        private Dictionary<string, BindingConfig> actionToBindingConfigs;

        public Dictionary<string, BindingConfig> ActionToMenuItems
        {
            get
            {
                if (actionToBindingConfigs is not null) return actionToBindingConfigs;

                actionToBindingConfigs = new();
                for (int i = 0; i < bindingConfigs.Length; i++)
                {
                    actionToBindingConfigs.Add(bindingConfigs[i].inputActionId, bindingConfigs[i]);
                }
                return actionToBindingConfigs;
            }
        }

        private void OnValidate()
        {
            for (int i = 0; i < bindingConfigs.Length; i++)
            {
                bindingConfigs[i].OnValidate();
            }
        }

    }

    [System.Serializable]
    public class BindingConfig
    {
        [SerializeField] private string name;
        public string path;
        public bool canRebind = true;

        public string inputActionId;
        public string[] compositeNames;


        /// <summary>
        /// last item in path
        /// </summary>
        public string Name => (PathSplited.Length>0) ? PathSplited[^1] : string.Empty;

        public string[] PathSplited => path.Split('/');
        public string GroupName => PathSplited.Length > 1 ? PathSplited[0] : null;
        public string PathWithoutName => path[..^Name.Length];

        internal void OnValidate()
        {
            name = path;
        }
    }
}

