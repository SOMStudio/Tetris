using UnityEngine;

namespace Base.TimeControl
{
	public class TimerClass
	{
		public bool isTimerRunning;

		private float _timeElapsed;
		private float _currentTime;
		private float _lastTime;
		private float _timeScaleFactor = 1.0f;
		
		public void UpdateTimer()
		{
			_timeElapsed = Mathf.Abs(Time.realtimeSinceStartup - _lastTime);
			
			if (isTimerRunning)
			{
				_currentTime += _timeElapsed * _timeScaleFactor;
			}
			
			_lastTime = Time.realtimeSinceStartup;
		}
		
		public void StartTimer()
		{
			isTimerRunning = true;
			_lastTime = Time.realtimeSinceStartup;
		}
		
		public void StopTimer()
		{
			isTimerRunning = false;
			
			UpdateTimer();
		}
		
		public void ResetTimer()
		{
			_timeElapsed = 0.0f;
			_currentTime = 0.0f;
			_lastTime = Time.realtimeSinceStartup;
			
			UpdateTimer();
		}
		
		public float GetTime()
		{
			return _currentTime;
		}
		
		public string GetFormattedTime(string format)
		{
			return TimeHelp.GetFormattedTime(_currentTime);
		}
	}
}