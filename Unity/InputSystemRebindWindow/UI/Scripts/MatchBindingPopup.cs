using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.SystemUI.Settings
{
    public class MatchBindingPopup : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI message;

        private System.Action<bool, bool> _callback;

        private void OnEnable()
        {
            GameInputSystem.InputActions.UI.Cancel.performed += Cancel_performed;
        }


        private void OnDisable()
        {
            GameInputSystem.InputActions.UI.Cancel.performed -= Cancel_performed;
        }

        public void Setup(string msg, System.Action<bool, bool> callback)
        {
            _callback = callback;
            message.text = msg;
            gameObject.SetActive(true);
        }

        private void Cancel_performed(InputAction.CallbackContext obj) => OnCancel();
        public void OnCancel() => OnResult(false, false);
        public void OnLeaveOld() => OnResult(true, true);
        public void OnRemoveOld() => OnResult(true, false);

        private void OnResult(bool isSuccess, bool leaveOld)
        {
            _callback?.Invoke(isSuccess, leaveOld);
            gameObject.SetActive(false);
        }
    }
}