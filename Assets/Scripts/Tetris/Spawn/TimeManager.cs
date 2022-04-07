using System;
using Base.TimeControl;
using Game;
using Tetris.GameData.DevScripts;
using UnityEngine;
using UnityEngine.Events;

public class TimeManager : MonoBehaviour
{
    //[Header("Settings")]
    //[SerializeField]
    private float controlTime = .0f;
    private float timeControlCheck = .0f;
    
    //[SerializeField]
    private float dropStep = .0f;
    private float timeDropCheck = .0f;

    //[SerializeField] 
    private float reduceDropStepPeriod = .0f;
    //[SerializeField]
    private float reduceDropStepMagnitude = .05f;
    private float timeReduceDropCheck = .0f;

    //[SerializeField]
    private float waveLengthPeriad = .0f;
    //[SerializeField]
    private int numberWave = 1;
    private float timeChangeWaveCheck = .0f;

    private GameData gameData;
    
    private TimerClass timer;

    [Header("Events")]
    public UnityEvent controlEvent;
    public UnityEvent dropEvent;
    public UnityEvent reduceDropEvent;
    public UnityEvent waveChangeEvent;

    public int NumberWave => numberWave;

    private void Start()
    {
        gameData = (GameData) GameController.Instance.GameData.Data;
        
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
        waveLengthPeriad = gameData.GetWaveLength(numberWave);

        timeControlCheck += controlTime;
        timeDropCheck += dropStep;
        timeReduceDropCheck += reduceDropStepPeriod;
        timeChangeWaveCheck += waveLengthPeriad;
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
