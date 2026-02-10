using System;
using UnityEngine;

public class Timer
{
    private float _time;
    private float _maxTime;
    public float CurrentTime => _time;

    public enum TimerReset { Automatic, Manual }
    private TimerReset _timerReset;

    public event Action OnTimerDone;
    private bool _done = false;

    public void CountTimer()
    {
        if (_time > 0)
        {
            _time -= Time.deltaTime;
        }
        else if (_time <= 0)
        {
            if (_timerReset == TimerReset.Automatic) ResetTimer();
            if (!_done)
            {   
                _done = true;
                OnTimerDone?.Invoke();
            }          
        }
    }
    public void ResetTimer()
    {
        _time = _maxTime;
        _done = false;
    }

    public Timer(float time, TimerReset timerReset = TimerReset.Automatic)
    {
        _maxTime = time;
        _time = time;
        _timerReset = timerReset;
    }

    public void setNewMax(float newMax)
    {
        _maxTime = newMax;
    }
}