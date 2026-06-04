using UnityEngine.Events;

namespace Game.Menu
{
    public interface IGameMenu
    {
        bool IsMenuActive();
        
        int ConsoleWindowActive { get; }
        void ActivateConsoleWindow(int number);
        void DisActivateConsoleWindow();
        
        void ConsoleWinMessage_Show(string value, UnityAction actionClick = null);
        void ConsoleWinYesNo_Show(string value, UnityAction actionYes, UnityAction actionNo = null);

        bool IsCursorOverGameUi();
        void ShowAdviceGameWindow(string value);
        void StopWatchPanelOpen();
        void StopWatchPanelClose();
    }
}