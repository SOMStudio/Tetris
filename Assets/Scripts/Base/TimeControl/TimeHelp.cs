namespace Base.TimeControl
{
	public static class TimeHelp
	{		
		public static string GetHours(float value)
		{
			int aHour = (int) value / 3600;
			aHour = aHour % 24;

			int tmp = (int) aHour;
			string hour = tmp.ToString();
			if (hour.Length < 2)
				hour = $"0{hour}";

			return hour;
		}
	
		public static string GetMinutes(float value)
		{
			int aMinute = (int) value / 60;
			aMinute = aMinute % 60;

			int tmp = (int) aMinute;
			string minutes = tmp.ToString();
			if (minutes.Length < 2)
				minutes = $"0{minutes}";

			return minutes;
		}
	
		public static string GetSeconds(float value)
		{
			int aSecond = (int) value % 60;

			int tmp = (int) aSecond;
			string seconds = tmp.ToString();
			if (seconds.Length < 2)
				seconds = $"0{seconds}";

			return seconds;
		}
	
		public static string GetMills(float value)
		{
			int aMillis = (int) (value * 100) % 100;

			int tmp = (int) aMillis;
			string mills = tmp.ToString();
			if (mills.Length < 2)
				mills = $"0{mills}";

			return mills;
		}

		/// <summary>
		/// Gets the formatted time (MM:SS:MS).
		/// </summary>
		/// <returns>The formatted time.</returns>
		public static string GetFormattedTime(float time, string format = "MM:SS:MS")
		{
			string res = format;
			string[] splitSt = format.Split(':');

			foreach (var stFormat in splitSt)
			{
				if (stFormat == "HH" || stFormat == "H")
				{
					res = res.Replace(stFormat, GetHours(time));
				} else if (stFormat == "MM" || stFormat == "M")
				{
					res = res.Replace(stFormat, GetMinutes(time));
				} else if (stFormat == "SS" || stFormat == "S")
				{
					res = res.Replace(stFormat, GetSeconds(time));
				} else if (stFormat == "MS" || stFormat == "ms")
				{
					res = res.Replace(stFormat, GetMills(time));
				}
			}
		
			return res;
		}
	}
}
