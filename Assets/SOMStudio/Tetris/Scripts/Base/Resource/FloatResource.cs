using System;
using SOMStudio.Tetris.Scripts.Base.Resource.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace SOMStudio.Tetris.Scripts.Base.Resource
{
    [Serializable]
    public class FloatResource : IResource<float>
    {
        [SerializeField] private float value;
        [Header("Events")]
        [SerializeField] private FloatEvent changeEvent; 
        
        public void Set(float setValue)
        {
            value = setValue;
        }

        public float Get()
        {
            return value;
        }

        public void Add(float setValue)
        {
            value += setValue;
            
            changeEvent?.Invoke(value);
        }

        public void Reduce(float setValue)
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
        
        public void Change(float setValue)
        {
            value = setValue;
            
            changeEvent?.Invoke(value);
        }

        public void AddListener(UnityAction<float> setValue)
        {
            changeEvent.AddListener(setValue);
        }

        public void RemoveListener(UnityAction<float> setValue)
        {
            changeEvent.RemoveListener(setValue);
        }
        
        [Serializable]
        public class FloatEvent : UnityEvent<float>{}
    }
}
