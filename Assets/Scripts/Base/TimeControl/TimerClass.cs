using UnityEngine;

namespace Base.TimeControl
{
	public class TimerClass
	{
		public bool isTimerRunning = false;

		private float _timeElapsed = 0.0f;
		private float _currentTime = 0.0f;
		private float _lastTime = 0.0f;
		private float _timeScaleFactor = 1.0f; // <-- If you need to scale time, change this!

		/// <summary>
		/// Update timer.
		/// </summary>
		public void UpdateTimer()
		{
			_timeElapsed = Mathf.Abs(Time.realtimeSinceStartup - _lastTime);
			
			if (isTimerRunning)
			{
				_currentTime += _timeElapsed * _timeScaleFactor;
			}
			
			_lastTime = Time.realtimeSinceStartup;
		}

		/// <summary>
		/// Starts the timer.
		/// </summary>
		public void StartTimer()
		{
			isTimerRunning = true;
			_lastTime = Time.realtimeSinceStartup;
		}

		/// <summary>
		/// Stops the timer.
		/// </summary>
		public void StopTimer()
		{
			isTimerRunning = false;
			
			UpdateTimer();
		}

		/// <summary>
		/// ResetTimer will set the timer back to zero
		/// </summary>
		public void ResetTimer()
		{
			_timeElapsed = 0.0f;
			_currentTime = 0.0f;
			_lastTime = Time.realtimeSinceStartup;
			
			UpdateTimer();
		}

		/// <summary>
		/// GetTime. Call UpdateTimer() before trying to use this function, otherwise the time value will not be up to date.
		/// </summary>
		/// <returns>The time float</returns>
		public float GetTime()
		{
			return _currentTime;
		}

		/// <summary>
		/// GetTime in format (default MM:SS:MS)
		/// </summary>
		/// <returns>The time string</returns>
		public string GetFormattedTime(string format)
		{
			return TimeHelp.GetFormattedTime(_currentTime);
		}
	}
}