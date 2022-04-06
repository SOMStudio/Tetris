using System;
using Base;
using Base.Optimization;
using Base.Utility;
using Tetris.GameData.DevScripts;
using Tetris.GameLogic.DevScripts;
using Game;
using Game.Menu;
using UnityEngine;

namespace Tetris
{
    public class LevelManager : BaseLevelManager
    {
        [Header("Main")] [SerializeField]
        private bool useDontDestroy = true;

        [Header("Play Field Manager")] [SerializeField]
        private PlayFieldManager playFieldManager;

        [Header("Player Manager")] [SerializeField]
        private PlayerManager playerManager;

        [Header("Spawn Manager")] [SerializeField]
        private TimeManager timeManager;

        [Header("Spawn Manager")] [SerializeField]
        private ObjectPool spawnManager;

        [Header("Drop Object Manager UI")] [SerializeField]
        private DropObjectManagerUi dropObjectManagerUi;

        private IMainMenu mainMenu;
        private IGameMenu gameMenu;
        private BaseGameController gameController;
        
        private GameData.DevScripts.GameData gameData;
        private GameLogicScript gameLogicScript;

        private bool startLevel = false;
        private bool pauseLevel = false;

        private SingletonComposition<LevelManager> singletonComponent;
        
        [System.NonSerialized] public static LevelManager Instance;

        public PlayFieldManager PlayFieldManager => playFieldManager;
        public PlayerManager PlayerManager => playerManager;
        public TimeManager TimeManager => timeManager;
        public ObjectPool SpawnManager => spawnManager;

        private void Awake()
        {
            singletonComponent = new SingletonComposition<LevelManager>(Instance, 
                () => Instance = this,
                () => Destroy(this.gameObject));
        }
        
        private void Start()
        {
            InitLevel();
        }

        private void InitLevel()
        {
            if (useDontDestroy)
                DontDestroyOnLoad(this.gameObject);
            
            mainMenu = MenuManager.Instance;
            gameMenu = MenuManager.Instance;
            
            if (!gameController)
            {
                gameController = GameController.Instance;
                gameController.SetLevelManager(this);

                gameData = (GameData.DevScripts.GameData)gameController.GameData.Data;
                gameLogicScript = (GameLogicScript)gameController.GameLogicScript.Script;

                timeManager.controlEvent.AddListener(playFieldManager.MobileControl);
                timeManager.dropEvent.AddListener(playFieldManager.ObjectMoveDown);
                timeManager.reduceDropEvent.AddListener(ReduceDropStep);
                timeManager.waveChangeEvent.AddListener(ChangeWave);

                playFieldManager.updateNextDropObjectListEvent += dropObjectManagerUi.SetDropObjectList;
                playFieldManager.SetDestroyRayCountListener(CatchLevelReward);
                playFieldManager.LoseLifeEvent += LoseLife;

                if (gameController.MenuAndLevelsDivided)
                    RunLevel(gameData.ActiveLevel);
            }
        }
        
        #region OverrideMethods
        public override bool IsLevelStarted()
        {
            return startLevel;
        }

        public override bool IsLevelPaused()
        {
            return pauseLevel;
        }

        public override void RunLevel(int number = 0)
        {
            if (number >= 0)
            {
                if (!startLevel)
                {
                    startLevel = !startLevel;

                    ActivateLevel(number);
                }
                else
                {
                    if (pauseLevel)
                    {
                        pauseLevel = !pauseLevel;
                        
                        timeManager.StartTime();
                        
                        MenuManager.Instance.GameRightPanelOpen();
                    }
                }
            }
        }

        public override void PauseLevel()
        {
            if (startLevel)
            {
                if (!pauseLevel)
                {
                    pauseLevel = !pauseLevel;

                    timeManager.PauseTime();
                    
                    MenuManager.Instance.GameRightPanelClose();
                }
            }
        }

        public override void StopLevel()
        {
            startLevel = false;
            
            timeManager.StopTime();
            
            playFieldManager.Clear();
            
            MenuManager.Instance.GameRightPanelClose();
        }

        public override GameObject GetPlayer(int number = 0)
        {
            return playerManager.gameObject;
        }

        public override GameObject GetEnemy(int number = 0)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        private void ActivateLevel(int number)
        {
            gameLogicScript.InitLevelRules(this);

            string stRules = gameData.GetStringTask();
            mainMenu?.ConsoleWinMessage_Show(stRules, StartLevel);
        }

        private void StartLevel()
        {
            if (!gameController.MenuAndLevelsDivided)
                mainMenu?.DisActivateWindow();
            
            #if (INPUT_MOBILE)
            gameMenu?.ShowAdviceGameWindow("Control (Move: left, right, down, Rotate: up)!");
            #else
            gameMenu?.ShowAdviceGameWindow("Control (Move: left, right, down -buttons, Rotate: up-button )!");
            #endif

            timeManager.StartTime();
            
            playFieldManager.InitNextDropObjectList();
            MenuManager.Instance.GameRightPanelOpen();
        }

        public void CatchLevelReward(int countLine)
        {
            gameLogicScript.CheckLocalTask(this.gameObject, new EvenArgsReward() {countLine = countLine});
        }

        private void LoseLife()
        {
            UserManager.Instance.ReduceHealth(1);
        }

        private void ReduceDropStep()
        {
            MenuManager.Instance.ShowAdviceGameWindow("Time drop object was reduced!");
        }

        private void ChangeWave()
        {
            MenuManager.Instance.ShowAdviceGameWindow("Start new wave!");
        }
    }

    class EvenArgsReward : EventArgs
    {
        public int countLine;
    }
}
