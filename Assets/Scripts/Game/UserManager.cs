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

		private ISaveSystem _fileSaveSystem;

		private bool _dataWasRead = false;
		private bool _dataNeedWrite = false;

		private bool _highScoreShowInLevel = false;

		private BaseGameController gameController;

		private SingletonComposition<UserManager> _singletonComponent;
		
		[System.NonSerialized] public static UserManager Instance;
		
		void Awake()
		{
			_singletonComponent = new SingletonComposition<UserManager>(Instance, 
				() => Instance = this,
				() => Destroy(this.gameObject));

			string fileName = $"{Application.persistentDataPath}/playerData_{gamePrefsName}.dat";
			
			_fileSaveSystem = new FileSaveSystem(fileName);
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
			if (_dataWasRead)
			{
				if (GetLevel() < value)
				{
					SetLevel(value);

					_dataNeedWrite = true;
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
			if (_dataWasRead)
			{
				if (value > GetHighScore())
				{
					if (!_highScoreShowInLevel)
					{
						_highScoreShowInLevel = true;

						MenuManager.Instance?.ShowAdviceGameWindow("You improve Best Score!");
					}

					SetHighScore(GetScore(), true);

					_dataNeedWrite = true;
				}
			}
			else
			{
				LoadPrivateDataPlayer();
			}
		}

		private void ResetHighScoreShowFlag()
		{
			_highScoreShowInLevel = false;
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

		/// <summary>
		/// save player data in file with encrypting, not use for Web-application (web can't write file)
		/// </summary>
		public void SavePrivateDataPlayer()
		{
			if (_dataWasRead)
			{
				if (_dataNeedWrite)
				{
					PlayerData data = new PlayerData();
					data.playerName = playerName;
					data.bestScore = GetHighScore();
					data.level = GetLevel();

					_fileSaveSystem.Save(data);

					_dataNeedWrite = false;
				}
			}
			else
			{
				LoadPrivateDataPlayer();
			}
		}

		/// <summary>
		/// restore player data from encrypting file.
		/// </summary>
		public void LoadPrivateDataPlayer()
		{
			if (!_dataWasRead)
			{
				PlayerData data = new PlayerData();

				if (_fileSaveSystem.Load(out data))
				{
					playerName = data.playerName;
					SetHighScore(data.bestScore);
					SetLevel(data.level);
				}
				else
				{
					GetDefaultData();
				}

				_dataWasRead = true;
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
