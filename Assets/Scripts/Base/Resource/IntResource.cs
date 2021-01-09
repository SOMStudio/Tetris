using System;
using Base.Resource.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Base.Resource
{
    [Serializable]
    public class IntResource : IResource<int>
    {
        [SerializeField] private int value;
        
        [Header("Events")]
        [SerializeField] private IntEvent changeEvent; 
        
        public void Set(int value)
        {
            this.value = value;
        }

        public int Get()
        {
            return value;
        }

        public void Add(int value)
        {
            this.value += value;
            
            changeEvent?.Invoke(this.value);
        }

        public void Reduce(int value)
        {
            if (this.value > 0)
            {
                this.value -= value;
                if (this.value < 0)
                {
                    this.value = 0;
                }
                
                changeEvent?.Invoke(this.value);
            }
        }

        public void Change(int value)
        {
            this.value = value;
            
            changeEvent?.Invoke(this.value);
        }

        public void AddListener(UnityAction<int> value)
        {
            changeEvent.AddListener(value);
        }

        public void RemoveListener(UnityAction<int> value)
        {
            changeEvent.RemoveListener(value);
        }
    }

    [Serializable]
    public class IntEvent : UnityEvent<int>{}
}
