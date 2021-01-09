using UnityEngine;

namespace Base
{
    public abstract class BaseLevelManager : MonoBehaviour
    {
        public abstract bool IsLevelStarted();
        public abstract bool IsLevelPaused();
        
        public abstract void RunLevel(int number = 0);
        public abstract void PauseLevel();
        public abstract void StopLevel();
        public abstract GameObject GetPlayer(int number = 0);
        public abstract GameObject GetEnemy(int number = 0);
    }
}
