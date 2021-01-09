using System;
using Base.Data.GameData.DevScripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tetris.GameData.DevScripts
{
    [Serializable]
    public class GameData : BaseData
    {
        [Header("Colors")]
        [SerializeField] private Color[] colorList;

        [Header("Time")]
        [SerializeField]
        private float controlStep = 0.25f;
        [SerializeField]
        private float dropStep = 1.0f;
        [SerializeField]
        private float reduceDropStepPeriod = 60.0f;
        [SerializeField]
        private float reduceDropStepMagnitude = 0.05f;

        public Color GetDropObjectColor(int index)
        {
            return colorList[index];
        }

        public int ColorCount => colorList.Length;

        public Color RandomColor => colorList[Random.Range(0, colorList.Length)];

        public float ControlStep => controlStep;

        public float DropStep => dropStep;

        public float ReduceDropStepPeriod => reduceDropStepPeriod;

        public float ReduceDropStepMagnitude => reduceDropStepMagnitude;

        public string GetStringTask()
        {
            return "Good luck!";
        }
    }
}
