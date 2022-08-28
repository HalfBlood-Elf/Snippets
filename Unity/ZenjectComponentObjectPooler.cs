using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace LocalObjectPooler
{
    public class ZenjectComponentObjectPooler<T> : ComponentObjectPooler<T> where T : MonoBehaviour
    {
        protected PlaceholderFactory<T> factory;
        protected override Stack<T> PoolStack { get => objectsStack; set => objectsStack = value; }

        public ZenjectComponentObjectPooler(PlaceholderFactory<T> factory, Transform parent, uint initialPoolCount = 5)
            : base(null, parent, 0)
        {
            this.factory = factory;

            for (byte i = 0; i < initialPoolCount - this.initialPoolCount; i++)
            {
                ReturnToPool(InstantiatePrefab());
            }
        }

        protected override T InstantiatePrefab()
        {
            var obj = factory.Create();
            obj.transform.SetParent(parent);
            obj.gameObject.SetActive(false);
            return obj;
        }
    }
}
