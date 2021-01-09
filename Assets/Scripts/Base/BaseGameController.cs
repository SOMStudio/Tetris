using UnityEngine;
using UnityEngine.SceneManagement;
using Base.Data.GameData.DevScripts;
using Base.Data.GameLogic.DevScripts;
using Base.Data.LevelListData.DevScripts;

namespace Base
{
	public abstract class BaseGameController : MonoBehaviour
	{
		[Header("Base")]
		[SerializeField] protected GameObject explosionPrefab;

		[Header("Level")]
		[SerializeField] protected bool menuAndLevelsDivided = false;
		[SerializeField] protected BaseLevelManager levelManager;
		
		[Header("Data")]
		[SerializeField] private BaseDataTemplate gameData;
		[SerializeField] private BaseLevelListDataTemplate levelListData;

		[Header("Logic")]
		[SerializeField] private BaseLogicScriptTemplate gameLogicScript;
		
		private bool _paused;

		private Camera _mainCamera;

		public bool MenuAndLevelsDivided => menuAndLevelsDivided;

		public Camera MainCamera
		{
			get
			{
				if (_mainCamera == null) _mainCamera = Camera.main;
				return _mainCamera;
			}
		}
		
		public BaseDataTemplate GameData => gameData;
		public BaseLogicScriptTemplate GameLogicScript => gameLogicScript;
		public BaseLevelListDataTemplate LevelListData => levelListData;

		public void Explode(Vector3 aPosition)
		{
			if (explosionPrefab)
			{
				Instantiate(explosionPrefab, aPosition, Quaternion.identity);
			}
		}

		protected virtual void InitGame()
		{
			gameLogicScript.Script.InitGameRules(this, gameData);
			
			GameData.Data.ActiveLevel = -1;
		}

		protected virtual void StopGame()
		{
			
		}

		public GameObject GetPlayer(int value = 0)
		{
			return levelManager?.GetPlayer(value);
		}

		public GameObject GetEnemy(int value = 0)
		{
			return levelManager?.GetEnemy(value);
		}

		protected virtual void PlayerDestroyed()
		{
			StopLevel();
		}

		protected virtual void EnemyDestroyed()
		{

		}

		protected virtual void BossDestroyed()
		{
			StopLevel();
		}

		public virtual void StartScene(string sceneName)
		{
			SceneManager.LoadScene(sceneName);
		}

		public virtual void RestartScene()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		public void SetLevelManager(BaseLevelManager value)
		{
			levelManager = value;
		}

		public virtual void RunLevel(int number = 0)
		{
			if (menuAndLevelsDivided)
				StartScene("Level" + number);
			else
				levelManager?.RunLevel(number);
		}

		public virtual void PauseLevel()
		{
			levelManager?.PauseLevel();
		}
		
		public virtual void StopLevel()
		{
			levelManager?.StopLevel();
		}

		public virtual void CheckLifePlayer(int life)
		{
			if (life == 0)
			{
				PlayerDestroyed();
			}
		}

		public virtual void CheckLocalTask(int percentage)
		{
			if (percentage == 100)
			{
				EnemyDestroyed();
			}
		}

		public virtual void CheckGlobalTask(int percentage)
		{
			if (percentage == 100)
			{
				BossDestroyed();
			}
		}

		private bool Paused
		{
			get { return _paused; }
			set
			{
				_paused = value;

				if (_paused)
				{
					Time.timeScale = 0f;
				}
				else
				{
					Time.timeScale = 1f;
				}
			}
		}
		
		public void PauseGame()
		{
			Paused = !Paused;
		}
	}
}
