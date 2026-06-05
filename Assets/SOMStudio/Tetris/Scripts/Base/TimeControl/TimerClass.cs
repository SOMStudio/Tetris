using UnityEngine;

namespace SOMStudio.Tetris.Scripts.Base.TimeControl
{
	public class TimerClass
	{
		public bool isTimerRunning;

		private float timeElapsed;
		private float currentTime;
		private float lastTime;
		private float timeScaleFactor = 1.0f;
		
		public void UpdateTimer()
		{
			timeElapsed = Mathf.Abs(Time.realtimeSinceStartup - lastTime);
			
			if (isTimerRunning)
			{
				currentTime += timeElapsed * timeScaleFactor;
			}
			
			lastTime = Time.realtimeSinceStartup;
		}
		
		public void StartTimer()
		{
			isTimerRunning = true;
			lastTime = Time.realtimeSinceStartup;
		}
		
		public void StopTimer()
		{
			isTimerRunning = false;
			
			UpdateTimer();
		}
		
		public void ResetTimer()
		{
			timeElapsed = 0.0f;
			currentTime = 0.0f;
			lastTime = Time.realtimeSinceStartup;
			
			UpdateTimer();
		}
		
		public float GetTime()
		{
			return currentTime;
		}
		
		public string GetFormattedTime(string format)
		{
			return TimeHelp.GetFormattedTime(currentTime);
		}
	}
}