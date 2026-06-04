using Base;
using Base.Resource;
using Base.SaveSystem;
using Base.SaveSystem.Interfaces;
using Base.Utility;
using UnityEngine;

namespace Game
{
	public class UserManager : BaseUserManager
	{
		[Header("Main")]
		[SerializeField] private bool useDontDestroy = true;
		[SerializeField] protected IntResource wave;

		private ISaveSystem fileSaveSystem;

		private bool dataWasRead;
		private bool dataNeedWrite;

		private bool highScoreShowInLevel;

		private BaseGameController gameController;

		private SingletonComposition<UserManager> singletonComponent;
		
		[System.NonSerialized] public static UserManager Instance;

		private void Awake()
		{
			singletonComponent = new SingletonComposition<UserManager>(Instance, 
				() => Instance = this,
				() => Destroy(this.gameObject));

			string fileName = $"{Application.persistentDataPath}/playerData_{gamePrefsName}.dat";
			
			fileSaveSystem = new FileSaveSystem(fileName);
		}

		private void Start()
		{
			if (useDontDestroy)
				DontDestroyOnLoad(this.gameObject);

			score.AddListener(CheckHighScore);
			
			if (!gameController)
			{
				gameController = GameController.Instance;

				health.AddListener(gameController.CheckLifePlayer);
			}
		}

		public override void GetDefaultData()
		{
			base.GetDefaultData();

			wave.Set(0);
		}

		public void VisitLevel(int value)
		{
			if (dataWasRead)
			{
				if (GetLevel() < value)
				{
					SetLevel(value);

					dataNeedWrite = true;
				}

				ResetHighScoreShowFlag();
			}
			else
			{
				LoadPrivateDataPlayer();
			}
		}

		private void CheckHighScore(int value)
		{
			if (dataWasRead)
			{
				if (value > GetHighScore())
				{
					if (!highScoreShowInLevel)
					{
						highScoreShowInLevel = true;

						MenuManager.Instance?.ShowAdviceGameWindow("You improve Best Score!");
					}

					SetHighScore(GetScore(), true);

					dataNeedWrite = true;
				}
			}
			else
			{
				LoadPrivateDataPlayer();
			}
		}

		private void ResetHighScoreShowFlag()
		{
			highScoreShowInLevel = false;
		}

		public void SetWave(int value, bool withEvent = false)
		{
			if (withEvent)
				wave.Change(value);
			else
				wave.Set(value);
		}
		
		public void AddWave()
		{
			wave.Add(1);
		}

		public int GetWave()
		{
			return wave.Get();
		}
		
		public void SavePrivateDataPlayer()
		{
			if (dataWasRead)
			{
				if (dataNeedWrite)
				{
					PlayerData data = new PlayerData();
					data.playerName = playerName;
					data.bestScore = GetHighScore();
					data.level = GetLevel();

					fileSaveSystem.Save(data);

					dataNeedWrite = false;
				}
			}
			else
			{
				LoadPrivateDataPlayer();
			}
		}
		
		public void LoadPrivateDataPlayer()
		{
			if (!dataWasRead)
			{
				PlayerData data = new PlayerData();

				if (fileSaveSystem.Load(out data))
				{
					playerName = data.playerName;
					SetHighScore(data.bestScore);
					SetLevel(data.level);
				}
				else
				{
					GetDefaultData();
				}

				dataWasRead = true;
			}
		}
	}

	[System.Serializable]
	public class PlayerData
	{
		public string playerName;
		public int bestScore;
		public int level;
	}
}
