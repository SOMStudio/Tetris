using Base;
using Base.Sound;
using Base.Utility;
using Game.Menu;
using Game.SaveSystem;
using Tetris;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game
{
    public class MenuManager : BaseMenuManager, IMainMenu, IGameMenu
    {
        [Header("Main")]
        [SerializeField] private bool useDontDestroy = true;
        
        [Header("Level List")]
        [SerializeField] protected bool useLevelListWindow = false;
        [SerializeField] private int numberLevelListWindow = -1;

        [Header("Left Panel")]
        [SerializeField] private int numberGameWindowLeftPanel = -1;
        [SerializeField] private Image muteUnmuteIntersectImage;
        
        [Header("Control panel")]
        [SerializeField] private int numberGameWindowControlPanel = -1;

        private SingletonComposition<MenuManager> _singletonComponent;
        
        private bool _cursorIsOverGameUi = false;
        
        [System.NonSerialized] public static MenuManager Instance;

        private void Awake()
        {
            _singletonComponent = new SingletonComposition<MenuManager>(Instance, 
                () => Instance = this,
                () => Destroy(this.gameObject));
        }
        
        private void Start()
        {
            InitMenu();
        }

        private void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ClickEscapeEvent();
            }
        }

        protected override void InitMenu()
        {
            base.InitMenu();
            
            OpenMenu();
            
            if (useDontDestroy)
                DontDestroyOnLoad(this.gameObject);
            
            PrefabSaveSystem.Instance?.AddListenerMuteEvent(ChangeMuteUnmuteButton);
        }
        
        #region OverrideEvents
        protected override void ActivateWindowEvent()
        {
            base.ActivateWindowEvent();
            
            DisActivateGameInterface();
        }

        protected override void ChangeWindowEvent(int number)
        {
            base.ChangeWindowEvent(number);
            
            SoundManager.Instance?.PlaySoundByIndex(0, Vector3.zero);
        }

        protected override void DisActivateWindowEvent()
        {
            base.DisActivateWindowEvent();

            if (!IsMenuActive())
                ActivateGameInterface();
        }

        protected override void ActivateConsoleWEvent()
        {
            base.ActivateConsoleWEvent();
            
            DisActivateGameInterface();
        }

        protected override void ChangeConsoleWEvent(int number)
        {
            base.ChangeConsoleWEvent(number);
            
            SoundManager.Instance?.PlaySoundByIndex(0, Vector3.zero);
        }

        protected override void DisActivateConsoleWEvent()
        {
            base.DisActivateConsoleWEvent();
            
            if (!IsMenuActive())
                ActivateGameInterface();
        }

        protected override void ConsoleWinMessage_ButtonOk()
        {
            base.ConsoleWinMessage_ButtonOk();
            
            SoundManager.Instance?.PlaySoundByIndex(1, Vector3.zero);
        }

        protected override void ConsoleWinYesNo_ButtonNo()
        {
            base.ConsoleWinYesNo_ButtonNo();
            
            SoundManager.Instance?.PlaySoundByIndex(1, Vector3.zero);
        }

        protected override void ConsoleWinYesNo_ButtonYes()
        {
            base.ConsoleWinYesNo_ButtonYes();
            
            SoundManager.Instance?.PlaySoundByIndex(1, Vector3.zero);
        }
        
        protected override void ActivateGameWEvent()
        {
            base.ActivateGameWEvent();
            
            WindowActivator_Open(numberGameWindowLeftPanel);
            
            #if (INPUT_MOBILE)
            WindowActivator_Open(numberGameWindowControlPanel);
            #endif
        }

        protected override void DisActivateGameWEvent()
        {
            base.DisActivateGameWEvent();

            StopWatchPanelClose();
            WindowActivator_Close(numberGameWindowLeftPanel);
            
            #if (INPUT_MOBILE)
            WindowActivator_Close(numberGameWindowControlPanel);
            #endif
        }
        
        protected override void ExitGame ()
        {
            SoundManager.Instance?.PlaySoundByIndex(1, Vector3.zero);
            
            UserManager.Instance?.SavePrivateDataPlayer();

            base.ExitGame ();
        }
        #endregion

        // main menu
        public void OpenMenu()
        {
            ActivateWindow(12);
        }
        
        // game menu
        public bool IsCursorOverGameUi()
        {
            return _cursorIsOverGameUi;
        }

        public void SetOverUiState(bool value)
        {
            _cursorIsOverGameUi = value;
        }
        
        private void ClickEscapeEvent() {
            if (consoleWindowActive == -1) {
                if (windowActive == -1) {
                    ExitGameConsoleWindow_Button ();
                }
            }
        }
        
        // stopWatch panel
        public void StopWatchPanelOpen()
        {
            WindowActivator_Open(0);
        }

        public void StopWatchPanelClose()
        {
            WindowActivator_Close(0);
        }
        
        // left panel
        private void ChangeMuteUnmuteButton(bool value)
        {
            muteUnmuteIntersectImage.enabled = value;
        }
        
        // right panel
        public void GameRightPanelOpen()
        {
            WindowActivator_Open(3);
        }
        
        public void GameRightPanelClose()
        {
            WindowActivator_Close(3);
        }

        //button action
        public void MainMenu_Button()
        {
            OpenMenu();
            
            SoundManager.Instance?.PlaySoundByIndex(1, Vector3.zero);
        }
        
        public void RunLevel_Button()
        {
            if (useLevelListWindow)
            {
                ActivateWindow(numberLevelListWindow);
            }
            else
            {
                DisActivateWindow();

                GameController.Instance?.RunLevel();

                SoundManager.Instance?.PlaySoundByIndex(1, Vector3.zero);
            }
        }

        public void StopLevel_Button()
        {
            GameController.Instance?.StopLevel();
            
            OpenMenu();
            
            SoundManager.Instance?.PlaySoundByIndex(1, Vector3.zero);
        }

        public void MenuOptions_Button()
        {
            GameController.Instance?.PauseLevel();
            
            ActivateWindow(13);
            
            SoundManager.Instance?.PlaySoundByIndex(1, Vector3.zero);
        }
        
        public void MuteUnMuteVolume_Button()
        {
            if (!PrefabSaveSystem.Instance) return;
            
            if (PrefabSaveSystem.Instance.MutedVolume)
            {
                PrefabSaveSystem.Instance.UnmuteVolume();
            }
            else
            {
                PrefabSaveSystem.Instance.MuteVolume();
            }
            
            SoundManager.Instance?.PlaySoundByIndex(1, Vector3.zero);
        }

        public void ExitGameConsoleWindow_Button() {
            if (IsMenuActive())
            {
                ConsoleWinYesNo_Show ("Do you want to Exit?", ExitGame);
            }
            else
            {
                GameController.Instance.PauseLevel();

                ConsoleWinYesNo_Show("Do you want to exit Level?",
                    () => { GameController.Instance.StopLevel(); OpenMenu();}
                    , () => GameController.Instance.RunLevel());
            }
        }
    }
}