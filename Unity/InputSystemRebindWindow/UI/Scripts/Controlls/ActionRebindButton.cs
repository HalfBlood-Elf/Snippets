using Argentics.GameInput;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

namespace UI.SystemUI.Settings
{
    public class ActionRebindButton : MonoBehaviour, IPointerClickHandler
    {
        private const string BINDING_IN_PROGRESS = nameof(ActionRebindButton)+ ".bindingInProgress";
        private const float TIME_OUT_REBINDING = 30f;

        [SerializeField] protected TextMeshProUGUI title; // todo: mouse button images / keyboard buttons better (as in controll toolitps)
        [SerializeField] private InputButtonDisplayer buttonDisplayer;
        [SerializeField] private Button button;

        private Controlls.BindData _bind;
        private bool _isMatchInvoked;
        private Action<InputControl, ActionRebindButton> onMatchRebinding;
        private GameObject bindingInProgress;
        private EventSystem eventSystem;
        private InputActionDisplayConfig displayConfig;

        public Controlls.BindData Bind => _bind;

        [Inject]
        private void Construct(DiContainer diContainer)
        {
            displayConfig = diContainer.Resolve<InputActionDisplayConfig>();
        }

        // button component only for visual feedback
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!button.interactable) return;

            if (eventData.button == PointerEventData.InputButton.Left)
            {
                Select();
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                Erase();
            }
        }

        public void Setup(
            Controlls.BindData bind,
            bool canRebind,
            GameObject bindingInProgress,
            Action<InputControl, ActionRebindButton> onMatchRebinding)
        {
            this.onMatchRebinding = onMatchRebinding;
            this.bindingInProgress = bindingInProgress;
            button.interactable = canRebind;
            _bind = bind;
            _bind.BindUpdated += BindUpdated;
            UpdateButtonVisual();
        }

        private void UpdateButtonVisual()
        {
            title.text = _bind.Binding.ToDisplayString();
            buttonDisplayer.Setup(displayConfig.GetDisplayConfig(_bind.Binding));
        }

        private void BindUpdated()
        {
            UpdateButtonVisual();
        }

        private void Select()
        {
            _isMatchInvoked = false;
            _bind.inputAction.Disable();
            eventSystem = EventSystem.current;
            eventSystem.enabled = false;
            InputActionRebindingExtensions.RebindingOperation rebindingOperation = _bind.inputAction
                .PerformInteractiveRebinding(_bind.bindIndex)
                .WithTimeout(TIME_OUT_REBINDING)
                .WithTargetBinding(_bind.bindIndex)
                .OnPotentialMatch(OnPotentialMatch)
                .OnMatchWaitForAnother(-1)
                .OnComplete(RebindingComplete)
                .OnCancel(OnRebindingCanceled)
                .Start();
            bindingInProgress.SetActive(true);
            GameInputSystem.GameInputRestrictions.SettingsMenuExitInputBlocked.SetSourceIsActive(true, BINDING_IN_PROGRESS);
        }

        public void Erase()
        {
            _bind.inputAction.ApplyBindingOverride(_bind.bindIndex, string.Empty);
            OnRebindFinish();
        }

        private void OnPotentialMatch(InputActionRebindingExtensions.RebindingOperation rebindingOperation)
        {
            // TODO: Remove when unity fix inputSystem
            // https://forum.unity.com/threads/withcancelingthrough-keyboard-escape-also-cancels-with-keyboard-e.1233400/
            // bug is about canceling with e (as well as with esc)
            if (rebindingOperation.selectedControl.path is "/Keyboard/escape" || 
                InputSystemExtentions.Matches(_bind.Binding.effectivePath, rebindingOperation.candidates[0]))
            {
                rebindingOperation.Cancel();
                rebindingOperation.Dispose();
                return;
            }

            // check cancel is done properly
            //cancel and bind after dialog stright forward

            //rebindingOperation.OnMatchWaitForAnother(-1);
            onMatchRebinding?.Invoke(rebindingOperation.candidates[0], this);

            rebindingOperation.Cancel();
            rebindingOperation.Dispose();
        }

        private void OnRebindingCanceled(InputActionRebindingExtensions.RebindingOperation rebindingOperation)
        {
            _bind.inputAction.Enable();
            rebindingOperation.Dispose();
            bindingInProgress.SetActive(false);
            CoroutineWorker.Instance.StartCoroutine(EnableUiInput());
            GameInputSystem.GameInputRestrictions.SettingsMenuExitInputBlocked.SetSourceIsActive(false, BINDING_IN_PROGRESS);
        }

        private void RebindingComplete(InputActionRebindingExtensions.RebindingOperation rebindingOperation)
        {
            rebindingOperation.Dispose();
            _bind.inputAction.Enable();
            bindingInProgress.SetActive(false);
            CoroutineWorker.Instance.StartCoroutine(EnableUiInput());
            OnRebindFinish();
        }

        private void OnRebindFinish()
        {
            _bind.UpdateBind();
            SaveInputs();
        }

        private void SaveInputs()
        {
            GameInputSystem.InputSettingsOverrides.SaveInputOverridesFromInputActions(GameInputSystem.InputActions);
        }

        private IEnumerator EnableUiInput()
        {
            yield return new WaitForSecondsRealtime(0.1f);
            eventSystem.enabled = true;
        }

        private void OnDestroy()
        {
            if(_bind != null)
                _bind.BindUpdated -= BindUpdated;
        }
    }
}
