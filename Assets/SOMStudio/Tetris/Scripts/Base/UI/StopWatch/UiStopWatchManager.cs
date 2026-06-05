using TMPro;
using UnityEngine;

namespace SOMStudio.Tetris.Scripts.Base.UI.StopWatch
{
    public class UiStopWatchManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text textTime;

        public void UpdateTime(string value)
        {
            textTime.text = value;
        }
    }
}
