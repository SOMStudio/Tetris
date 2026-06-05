using SOMStudio.Tetris.Scripts.Base.TimeControl;
using UnityEngine;
using UnityEngine.Events;

namespace SOMStudio.Tetris.Scripts.Game.Spawn
{
    public class TimeManager : MonoBehaviour
    {
        private float controlTime;
        private float timeControlCheck;
    
        private float dropStep;
        private float timeDropCheck;
    
        private float reduceDropStepPeriod;

        private float reduceDropStepMagnitude = .05f;
        private float timeReduceDropCheck;


        private float waveLengthPeriod;

        private int numberWave = 1;
        private float timeChangeWaveCheck;

        private SOMStudio.Tetris.Scripts.Game.GameData.DevScripts.GameData gameData;
    
        private TimerClass timer;

        [Header("Events")]
        public UnityEvent controlEvent;
        public UnityEvent dropEvent;
        public UnityEvent reduceDropEvent;
        public UnityEvent waveChangeEvent;

        public int NumberWave => numberWave;

        private void Start()
        {
            gameData = (SOMStudio.Tetris.Scripts.Game.GameData.DevScripts.GameData) GameController.Instance.GameData.Data;
        
            timer = new TimerClass();
            timer.ResetTimer();

            Init();
        }

        private void Init()
        {
            controlTime = gameData.ControlStep;
            dropStep = gameData.DropStep;
            reduceDropStepPeriod = gameData.ReduceDropStepPeriod;
            reduceDropStepMagnitude = gameData.ReduceDropStepMagnitude;
            waveLengthPeriod = gameData.GetWaveLength(numberWave);

            timeControlCheck += controlTime;
            timeDropCheck += dropStep;
            timeReduceDropCheck += reduceDropStepPeriod;
            timeChangeWaveCheck += waveLengthPeriod;
        }

        private void Reset()
        {
            timeControlCheck = .0f;
            timeDropCheck = .0f;
            timeReduceDropCheck = .0f;
            numberWave = 1;

            Init();
        }

        private void ChangeWave()
        {
            dropStep = gameData.DropStep;
        }
    
        private void Update()
        {
            timer.UpdateTimer();

            if (timer.GetTime() >= timeControlCheck)
            {
                timeControlCheck += controlTime;
            
                controlEvent?.Invoke();
            }

            if (timer.GetTime() >= timeReduceDropCheck && dropStep >= controlTime)
            {
                dropStep -= reduceDropStepMagnitude;
            
                timeReduceDropCheck += reduceDropStepPeriod;
            
                reduceDropEvent?.Invoke();
            }
        
            if (timer.GetTime() >= timeDropCheck)
            {
                timeDropCheck += dropStep;
            
                dropEvent?.Invoke();
            }

            if (timer.GetTime() >= timeChangeWaveCheck)
            {
                numberWave++;

                timeChangeWaveCheck += gameData.GetWaveLength(numberWave);

                ChangeWave();

                waveChangeEvent?.Invoke();
            }
        }

        public void PauseTime()
        {
            timer.StopTimer();
        }

        public void StopTime()
        {
            timer.StopTimer();
            timer.ResetTimer();
        
            Reset();
        }

        public void StartTime()
        {
            timer.StartTimer();
        }
    }
}
