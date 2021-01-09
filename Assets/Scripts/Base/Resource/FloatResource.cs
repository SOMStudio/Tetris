using System;
using Base.Resource.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Base.Resource
{
    [Serializable]
    public class FloatResource : IResource<float>
    {
        [SerializeField] private float value;
        [Header("Events")]
        [SerializeField] private FloatEvent changeEvent; 
        
        public void Set(float value)
        {
            this.value = value;
        }

        public float Get()
        {
            return value;
        }

        public void Add(float value)
        {
            this.value += value;
            
            changeEvent?.Invoke(this.value);
        }

        public void Reduce(float value)
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
        
        public void Change(float value)
        {
            this.value = value;
            
            changeEvent?.Invoke(this.value);
        }

        public void AddListener(UnityAction<float> value)
        {
            changeEvent.AddListener(value);
        }

        public void RemoveListener(UnityAction<float> value)
        {
            changeEvent.RemoveListener(value);
        }
        
        [Serializable]
        public class FloatEvent : UnityEvent<float>{}
    }
}
