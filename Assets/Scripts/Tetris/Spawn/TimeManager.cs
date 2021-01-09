using System;
using Base.TimeControl;
using Game;
using Tetris.GameData.DevScripts;
using UnityEngine;
using UnityEngine.Events;

public class TimeManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float delayTime = 0.25f;
    private float timeDelayCheck = .0f;
    
    [SerializeField]
    private float dropStep = 1.0f;
    private float timeDropCheck = .0f;

    [SerializeField] 
    private float reduceDropStepPeriod = 60.0f;
    [SerializeField]
    private float reduceDropStepMagnitude = .05f;
    private float timeReduceDropCheck = .0f;

    private GameData gameData;
    
    private TimerClass timer;

    [Header("Events")]
    public UnityEvent startControlEvent;
    public UnityEvent dropEvent;
    public UnityEvent reduceDropEvent;

    private void Start()
    {
        gameData = (GameData) GameController.Instance.GameData.Data;
        
        timer = new TimerClass();
        timer.ResetTimer();

        Init();
    }

    private void Init()
    {
        delayTime = gameData.ControlStep;
        dropStep = gameData.DropStep;
        reduceDropStepPeriod = gameData.ReduceDropStepPeriod;
        reduceDropStepMagnitude = gameData.ReduceDropStepMagnitude;
        
        timeDelayCheck += delayTime;
        timeDropCheck += dropStep;
        timeReduceDropCheck += reduceDropStepPeriod;
    }

    private void Reset()
    {
        timeDelayCheck = .0f;
        timeDropCheck = .0f;
        timeReduceDropCheck = .0f;

        Init();
    }
    
    private void Update()
    {
        timer.UpdateTimer();

        if (timer.GetTime() >= timeDelayCheck)
        {
            timeDelayCheck += delayTime;
            
            startControlEvent?.Invoke();
        }

        if (timer.GetTime() >= timeReduceDropCheck)
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
