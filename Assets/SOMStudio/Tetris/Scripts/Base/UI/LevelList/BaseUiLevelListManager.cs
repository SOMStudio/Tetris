using System.Collections.Generic;
using Base.Sound;
using Game;
using UnityEngine;
using Base.UI.Content;

namespace Base.UI.LevelList
{
    public class BaseUiLevelListManager : MonoBehaviour
    {
        [Header("Main")]
        [SerializeField] private UiChildContentManager contentManager;
        [SerializeField] private List<BaseButtonLevel> buttonLevelList;

        public int CountLevel => buttonLevelList.Count;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            if (!contentManager)
            {
                contentManager = gameObject.GetComponent<UiChildContentManager>();
            }
        }

        public virtual void AddLevel(string nameLevel = "")
        {
            var newObj = contentManager.AddChild();

            BaseButtonLevel newBaseButtonLevel = newObj.GetComponent<BaseButtonLevel>();
            if (newBaseButtonLevel != null)
            {
                newBaseButtonLevel.Init(buttonLevelList.Count);
                newBaseButtonLevel.AddClickAction(() => { StartLevel(buttonLevelList.Count - 1); });

                buttonLevelList.Add(newBaseButtonLevel);
            }

            RenameLevel(buttonLevelList.Count - 1, nameLevel);
        }

        public virtual void RemoveLevel(int numberLevel)
        {
            contentManager.RemoveChild(numberLevel);

            buttonLevelList.Remove(buttonLevelList[numberLevel]);
        }

        public virtual void RemoveLevelLast()
        {
            contentManager.RemoveChildLast();

            if (buttonLevelList.Count > 0)
                RemoveLevel(buttonLevelList.Count - 1);
        }

        public void RenameLevel(int numberLevel, string nameLevel)
        {
            buttonLevelList[numberLevel].LevelName = nameLevel;
        }

        public void StartLevel(int levelNumber)
        {
            GameController.Instance.RunLevel(levelNumber);

            SoundManager.Instance?.PlaySoundByIndex(1, Vector3.zero);
        }
    }
}
