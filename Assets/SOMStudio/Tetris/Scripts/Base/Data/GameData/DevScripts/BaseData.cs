using UnityEngine;

namespace SOMStudio.Tetris.Scripts.Base.Data.GameData.DevScripts
{
    public class BaseData
    {
        [Header("Game")]
        [SerializeField] private float delayForWinMessage = 1.0f;

        [Header("Level")]
        [SerializeField] private int activeLevel = -1;
        [SerializeField] private int bunusForItem = 10;

        [Header("Sound&Music")]
        [SerializeField] private float defaulVolume = 0.5f;

        public float DelayForWinMessage => delayForWinMessage;

        public int ActiveLevel {
            get => activeLevel;
            set => activeLevel = value;
        }

        public int BonusForItem => bunusForItem;

        public float DefaultVolume => defaulVolume;
    }
}
