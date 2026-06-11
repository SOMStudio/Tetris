using SOMStudio.Tetris.Scripts.Base;
using SOMStudio.Tetris.Scripts.Base.Utility;
using SOMStudio.Tetris.Scripts.Game.Menu;
using UnityEngine;
using UnityEngine.Events;

namespace SOMStudio.Tetris.Scripts.Game
{
    [AddComponentMenu("SOMStudio/Tetris/GameController")]
    public class GameController : BaseGameController
    {
        [Header("Main")]
        [SerializeField] private bool useDontDestroy = true;

        private SingletonComposition<GameController> singletonComponent;

        private bool showStartMessage = true;

        private IMainMenu mainMenu;
        private IGameMenu gameMenu;

        [System.NonSerialized] public static GameController Instance;

        private void Awake()
        {
            singletonComponent = new SingletonComposition<GameController>(Instance, 
                () => Instance = this,
                () => Destroy(this.gameObject));
        }

        private void Start()
        {
            InitGame();
        }

        #region OverrideMethods
        protected override void InitGame()
        {
            base.InitGame();
            
            mainMenu = MenuManager.Instance;
            gameMenu = MenuManager.Instance;

            if (useDontDestroy)
                DontDestroyOnLoad(this.gameObject);
        }

        public override void RunLevel(int number = 0)
        {
            if (menuAndLevelsDivided)
            {
                if (GameData.Data.ActiveLevel == number)
                {
                    levelManager?.RunLevel();
                }
                else
                {
                    string nameScene = LevelListData.Data[number].NameScene;
                    StartScene(nameScene);
                }
            }
            else
            {
                base.RunLevel(number);
            }

            GameData.Data.ActiveLevel = number;

            UserManager.Instance?.VisitLevel(number);
        }

        public override void StopLevel()
        {
            base.StopLevel();

            GameData.Data.ActiveLevel = -1;
            showStartMessage = true;
        }

        public override void CheckLifePlayer(int life)
        {
            base.CheckLifePlayer(life);
            
            if (life == 0)
            {
                UnityAction action;
                if (menuAndLevelsDivided)
                    action = () => StartScene("Menu");
                else
                    action = () => mainMenu?.OpenMenu();

                mainMenu?.ConsoleWinMessage_Show($"Game over!\nWave:{UserManager.Instance.GetWave()}\nScore:{UserManager.Instance.GetScore()}", action);
            }
        }

        public override void CheckLocalTask(int percentage)
        {
            base.CheckLocalTask(percentage);
            
            if (percentage > 0)
            {
                if (percentage > 10)
                    gameMenu?.ShowAdviceGameWindow("You so cool!");
                else
                {
                    if (showStartMessage)
                    {
                        showStartMessage = false;
                        gameMenu?.ShowAdviceGameWindow("Yes, continue it!");
                    }
                }
            }
            else
            {
                gameMenu?.ShowAdviceGameWindow("No-o-o-o-o!");
            }
        }

        public override void CheckGlobalTask(int percentage)
        {
            base.CheckGlobalTask(percentage);
            
            if (percentage == 100)
            {
                Invoke(nameof(WinMessage), GameData.Data.DelayForWinMessage);
            }
        }
        #endregion

        private void WinMessage()
        {
            UnityAction action;
            if (menuAndLevelsDivided)
                action = () => StartScene("Menu");
            else
                action = () => mainMenu?.OpenMenu();

            mainMenu?.ConsoleWinMessage_Show("You win!", action);
        }
    }
}
