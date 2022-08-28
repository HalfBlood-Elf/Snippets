using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.SystemUI.Settings.Controlls 
{
    public class AllInputActionData
    {
        public Dictionary<System.Guid, KeyboardInputActionData> keyboardInputActionDatas;

        private IReadOnlyDictionary<System.Guid, InputActionData> currentInputSchemeData;

        public IReadOnlyDictionary<System.Guid, InputActionData> GetCurrentInputSchemeData()
        {
            //todo: handle other devices
            if (currentInputSchemeData is null)
            {
                currentInputSchemeData = keyboardInputActionDatas.ToDictionary(k => k.Key,
                                                        v => (InputActionData)v.Value);
            }
            return currentInputSchemeData;
        }
    }


    public class KeyboardInputActionData : InputActionData
    {
        public override string TargetGroup => ControlBindingPathes.KEYBOARD_AND_MOUSE_GROUP;

        public KeyboardInputActionData(InputAction inputAction, BindingConfig bindingConfig) : base(inputAction, bindingConfig)
        {
        }

    }

    [System.Serializable]
    public abstract class InputActionData
    {
        public const string PARSING_ERROR = "Every primary (odd) binding suppose to have alternative binding. InputAction: ";

        public string inputActionId;
        [HideInInspector] public InputAction inputAction;
        public BindDataHolder[] bindHolders;

        public abstract string TargetGroup { get; }

        public InputActionData(InputAction inputAction, BindingConfig bindingConfig) 
        {
            // this constructor probably won't work for gamepad
            this.inputAction = inputAction;
            inputActionId = inputAction.id.ToString();

            List<BindDataHolder> bindDataHolders = new();

            bool? isAlternativeBind = null;
            int alternativeBindIndex = 0;

            var bindingsCount = inputAction.bindings.Count;
            for (int i = 0; i < bindingsCount; i++)
            {
                var binding = inputAction.bindings[i];
                if (binding.groups != TargetGroup && !binding.isComposite) continue;

                if (!binding.isPartOfComposite)
                {
                    if (isAlternativeBind.HasValue)
                        isAlternativeBind = !isAlternativeBind;
                    else isAlternativeBind = false;
                }

                if(binding.isComposite) continue;

                if (isAlternativeBind.Value)
                {
                    var bindDataHolder = bindDataHolders[alternativeBindIndex];

                    string bindName;
                    if (bindingConfig is null) bindName = inputAction.name;
                    else if (bindingConfig.compositeNames.Length > 0) bindName = bindingConfig.compositeNames[alternativeBindIndex];
                    else bindName = bindingConfig.Name;

                    bindDataHolder.alternativeBind = new BindData(bindName + " (Secondary)", inputAction, binding, bindDataHolder);
                    bindDataHolder.SubscribeToAlternative();
                    alternativeBindIndex++;
                }
                else
                {
                    string bindName;
                    if (bindingConfig is null) bindName = inputAction.name;
                    else if (bindingConfig.compositeNames.Length > 0) bindName = bindingConfig.compositeNames[bindDataHolders.Count];
                    else bindName = bindingConfig.Name;

                    var holder = new BindDataHolder() {bindingConfig = bindingConfig, name = bindName};
                    holder.primaryBind = new BindData(bindName + " (Primary)", inputAction, binding, holder);
                    holder.primaryBind.isPrimary = true;
                    holder.SubscribeToPrimary();
                    bindDataHolders.Add(holder);
                }
                
            }

            bindHolders = bindDataHolders.ToArray();
        }
    }

    [System.Serializable]
    public class BindDataHolder
    {
        public System.Action AnyBindUpdated;

        public string name;
        public BindingConfig bindingConfig;
        public BindData primaryBind;
        public BindData alternativeBind;

        public BindData GetBindDataWithNotEmptyBind()
        {
            if (primaryBind.Binding.effectivePath != string.Empty 
            || alternativeBind.Binding.effectivePath == string.Empty)
                return primaryBind;
            return alternativeBind;
        }

        public void SubscribeToPrimary()
        {
            primaryBind.BindUpdated += BindUpdated;
        }

        public void SubscribeToAlternative()
        {
            alternativeBind.BindUpdated += BindUpdated;
        }

        private void BindUpdated()
        {
            AnyBindUpdated?.Invoke();
        }
    }

    [System.Serializable]
    public class BindData
    {
        public event System.Action BindUpdated;

        public int bindIndex;
        public string bindId;
        public string name;
        [HideInInspector] public InputAction inputAction;
        public bool isPrimary;

        public BindDataHolder holder;
        public InputBinding Binding => inputAction.bindings[bindIndex];

        public BindData(string name, InputAction inputAction, InputBinding binding, BindDataHolder holder)
        {
            this.inputAction = inputAction;
            this.holder = holder;
            this.name = name;
            bindIndex = inputAction.bindings.IndexOf(b => b.id == binding.id);
            bindId = binding.id.ToString();
        }

        public void UpdateBind()
        {
            BindUpdated?.Invoke();
        }
    }
}
