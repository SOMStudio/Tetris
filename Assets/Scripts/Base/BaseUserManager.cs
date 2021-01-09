using Base.Resource;
using UnityEngine;

namespace Base
{
	[AddComponentMenu("Base/User Manager")]
	public class BaseUserManager : MonoBehaviour
	{
		[Header("Base")]
		[SerializeField] protected string gamePrefsName = "DefaultGame";
		
		[Header("Data")]
		[SerializeField] protected string playerName = "Aanonym";
		[SerializeField] protected IntResource score;
		[SerializeField] protected IntResource highScore;
		[SerializeField] protected IntResource level;
		[SerializeField] protected IntResource health;

		protected bool isFinished;

		public virtual void GetDefaultData()
		{
			playerName = "Anonim";

			SetScore(0);
			SetLevel(0);
			SetHealth(3);
			SetHighScore(0);

			isFinished = false;
		}

		public string GetName()
		{
			return playerName;
		}

		public void SetName(string aName)
		{
			playerName = aName;
		}

		public int GetLevel()
		{
			return level.Get();
		}

		public void SetLevel(int value, bool withEvent = false)
		{
			if (withEvent)
				level.Change(value);
			else
				level.Set(value);
		}

		public int GetHighScore()
		{
			return highScore.Get();
		}

		public void SetHighScore(int value, bool withEvent = false)
		{
			if (withEvent)
				highScore.Change(value);
			else
				highScore.Set(value);
		}

		public int GetScore()
		{
			return score.Get();
		}

		public virtual void AddScore(int value)
		{
			score.Add(value);
		}

		public void LostScore(int value)
		{
			score.Reduce(value);
		}

		public void SetScore(int value, bool withEvent = false)
		{
			if (withEvent)
				score.Change(value);
			else
				score.Set(value);
		}

		public int GetHealth()
		{
			return health.Get();
		}

		public void AddHealth(int value)
		{
			health.Add(value);
		}

		public void ReduceHealth(int value)
		{
			health.Reduce(value);
		}

		public void SetHealth(int value, bool withEvent = false)
		{
			if (withEvent)
				health.Change(value);
			else
				health.Set(value);
		}

		//===============================

		public bool GetIsFinished()
		{
			return isFinished;
		}

		public void SetIsFinished(bool value)
		{
			isFinished = value;
		}
	}
}