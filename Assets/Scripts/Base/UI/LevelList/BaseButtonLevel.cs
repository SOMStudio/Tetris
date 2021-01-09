using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Base.UI.LevelList
{
    public class BaseButtonLevel : MonoBehaviour
    {
        [Header("Main")]
        [SerializeField] private int levelNumber;
        [SerializeField] private TMP_Text refText;
        [SerializeField] private Button refButton;

        public virtual void Init(int numberLevel, string nameLevel = "")
        {
            levelNumber = numberLevel;
            LevelName = string.IsNullOrEmpty(nameLevel) ? (numberLevel+1).ToString() : nameLevel;
        }
        
        public int LevelNumber
        {
            get => levelNumber;
            set => levelNumber = value;
        }

        public string LevelName
        {
            get => refText.text;
            set => refText.text = value;
        }

        public void AddClickAction(UnityAction action)
        {
            refButton.onClick.AddListener(action);
        }

        public void RemoveClickAction(UnityAction action)
        {
            refButton.onClick.RemoveListener(action);
        }
    }
}
