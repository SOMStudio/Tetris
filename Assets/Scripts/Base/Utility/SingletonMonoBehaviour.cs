using UnityEngine;

namespace Base.Utility
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        [Header("Singleton")]
        [SerializeField] private bool useDontDestroy = true;

        private static readonly object Lock = new();

        [System.NonSerialized] public static T Instance;

        protected virtual void Awake()
        {
            InitSingleton();
        }

        private void InitSingleton()
        {
            lock (Lock)
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
