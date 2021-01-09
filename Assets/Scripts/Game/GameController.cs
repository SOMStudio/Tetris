using Base;
using Base.Utility;
using Game.Menu;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    [AddComponentMenu("Base/GameController")]
    public class GameController : BaseGameController
    {
        [Header("Main")]
        [SerializeField] private bool useDontDestroy = true;

        private SingletonComposition<GameController> _singletonComponent;

        private IMainMenu mainMenu;
        private IGameMenu gameMenu;
        private BaseUserManager userManager;
        
        [System.NonSerialized] public static GameController Instance;

        private void Awake()
        {
            _singletonComponent = new SingletonComposition<GameController>(Instance, 
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
            userManager = UserManager.Instance;
            
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

                mainMenu?.ConsoleWinMessage_Show("Game over!", action);
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
                    gameMenu?.ShowAdviceGameWindow("Yes!");
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
                Invoke(nameof(WinMessage), GameData.Data.DelayelayForWinMessage);
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
