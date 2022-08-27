using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WindowSystem
{
    public class RouterDontCloseAnyPrevious : MonoBehaviour, IWindowRouter
    {
        protected readonly Dictionary<string, Window> _windows = new();

        public RouterDontCloseAnyPrevious(IEnumerable<Window> windowsList)
        {
            foreach (var window in windowsList)
            {
                AddWindow(window);
            }
        }

        public virtual void AddWindow(Window window)
        {
            _windows.Add(window.GetType().Name, window);
            window.gameObject.SetActive(false);
        }

        public virtual T Show<T>(Action callback = null) where T : Window
        {
            return (T)Show(typeof(T).Name);
        }

        public virtual Window Show(string windowIdentity, Action callback = null)
        {
            if (_windows.TryGetValue(windowIdentity, out Window foundWindow))
            {
                foundWindow.Show(callback);
                return foundWindow;
            }
            else
            {
                Debug.LogError($"{windowIdentity} not registered");
                return null;
            }
        }

        public virtual T Hide<T>(Action callback = null) where T : Window
        {
            return (T)Hide(typeof(T).Name);
        }

        public virtual Window Hide(string windowIdentity, Action callback = null)
        {
            if (_windows.TryGetValue(windowIdentity, out Window foundWindow))
            {
                foundWindow.Hide(callback);
                return foundWindow;
            }
            else
            {
                Debug.LogError($"{windowIdentity} not registered");
                return null;
            }
        }

        public virtual T Get<T>() where T : Window
        {
            if (_windows.TryGetValue(typeof(T).Name, out Window foundWindow))
            {
                return (T)foundWindow;
            }
            return null;
        }
    }
}