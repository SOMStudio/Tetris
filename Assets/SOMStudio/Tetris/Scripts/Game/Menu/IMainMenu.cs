using UnityEngine.Events;

namespace Game.Menu
{
    public interface IMainMenu
    {
        bool IsMenuActive();
        
        int WindowActive { get; }
        void ActivateWindow(int number);
        void DisActivateWindow();

        int ConsoleWindowActive { get; }
        void ActivateConsoleWindow(int number);
        void DisActivateConsoleWindow();
        
        void ConsoleWinMessage_Show(string value, UnityAction actionClick = null);
        void ConsoleWinYesNo_Show(string value, UnityAction actionYes, UnityAction actionNo = null);

        void OpenMenu();
    }
}