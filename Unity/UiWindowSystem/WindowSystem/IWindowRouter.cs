
using System;

namespace WindowSystem
{
    public interface IWindowRouter
    {
        public T Show<T>(Action callback = null) where T : Window;
        public Window Show(string windowIdentity, Action callback = null);
        public T Hide<T>(Action callback = null) where T : Window;
        public Window Hide(string windowIdentity, Action callback = null);
        public T Get<T>() where T : Window;
    }
}