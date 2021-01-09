using UnityEngine;

namespace Base.Utility
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        [Header("Singleton")]
        [SerializeField] private bool useDontDestroy = true;

        private static readonly object _lock = new object();

        [System.NonSerialized] public static T Instance;

        protected virtual void Awake()
        {
            InitSingleton();
        }

        private void InitSingleton()
        {
            lock (_lock)
            {
                if (!Instance)
                {
                    Instance = (T)FindObjectOfType(typeof(T));

                    if (useDontDestroy)
                        DontDestroyOnLoad(this.gameObject);
                }
                else
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
