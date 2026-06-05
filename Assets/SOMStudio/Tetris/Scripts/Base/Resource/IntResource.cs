using System;
using SOMStudio.Tetris.Scripts.Base.Resource.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace SOMStudio.Tetris.Scripts.Base.Resource
{
    [Serializable]
    public class IntResource : IResource<int>
    {
        [SerializeField] private int value;
        
        [Header("Events")]
        [SerializeField] private IntEvent changeEvent; 
        
        public void Set(int setValue)
        {
            value = setValue;
        }

        public int Get()
        {
            return value;
        }

        public void Add(int setValue)
        {
            value += setValue;
            
            changeEvent?.Invoke(value);
        }

        public void Reduce(int setValue)
        {
            if (value > 0)
            {
                value -= setValue;
                if (value < 0)
                {
                    value = 0;
                }
                
                changeEvent?.Invoke(value);
            }
        }

        public void Change(int setValue)
        {
            value = setValue;
            
            changeEvent?.Invoke(value);
        }

        public void AddListener(UnityAction<int> setValue)
        {
            changeEvent.AddListener(setValue);
        }

        public void RemoveListener(UnityAction<int> setValue)
        {
            changeEvent.RemoveListener(setValue);
        }
    }

    [Serializable]
    public class IntEvent : UnityEvent<int>{}
}
