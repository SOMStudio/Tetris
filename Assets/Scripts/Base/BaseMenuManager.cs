using Base.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Base
{
	public abstract class BaseMenuManager : MonoBehaviour
	{
		[Header("Menu Data")]
		[SerializeField] protected int windowActive = -1;
		[SerializeField] protected int consoleWindowActive = -1;
		[SerializeField] protected bool isGameInterfaceActivated = false;
		
		[Header("Menu window list")]
		[SerializeField] private WindowOpenClose[] windowActivator;

		[Header("Blocker windows")]
		[SerializeField] private int numberWindowBlocker = -1;
		[SerializeField] private int numberConsoleWindowBlocker = -1;

		[Header("Console windows")]
		[SerializeField] private int limitCountSymbolForUseSmallWindow = 40;
		[SerializeField] private int numberConsoleWinMessageSmall = -1;
		[SerializeField] private TMP_Text consoleWinMessageSmallTextHead;
		[SerializeField] private int numberConsoleWinMessageBig = -1;
		[SerializeField] private TMP_Text consoleWinMessageBigTextHead;
		[SerializeField] private int numberConsoleWinYesNo = -1;
		[SerializeField] private TMP_Text consoleWinYesNoTextHead;

		private readonly UnityEvent consoleWinMessageActionOk = new UnityEvent();
		private readonly UnityEvent consoleWinYesNoActionYes = new UnityEvent();
		private readonly UnityEvent consoleWinYesNoActionNo = new UnityEvent();

		[Header("Game Windows")]
		[SerializeField] private int numberGameWindowHud = -1;
		[SerializeField] private int numberGameWindowAdvice = -1;
		
		[SerializeField] private TMP_Text gameWindowAdviceText;
		[SerializeField] private int _countCharForDelayOneSecond = 10;

		protected virtual void ExitGame()
		{
		#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
		#else
			Application.Quit();
		#endif
		}

		protected virtual void InitMenu()
		{
			if (numberWindowBlocker < 0)
				Debug.LogError("Not set number window blocker");
			if (numberConsoleWindowBlocker < 0)
				Debug.LogError("Not set number console-window blocker");
			if (numberGameWindowHud < 0)
				Debug.LogError("Not set number game window HUD");
			if (numberGameWindowAdvice < 0)
				Debug.LogError("Not set number game window advice");
			if (numberConsoleWinMessageSmall < 0)
				Debug.LogError("Not set number console-window small message");
			if (numberConsoleWinMessageBig < 0)
				Debug.LogError("Not set number console-window big message");
			if (numberConsoleWinYesNo < 0)
				Debug.LogError("Not set number console-window YesNo");
		}

		#region Activators
		//activators
		protected void WindowActivator_Open(int number)
		{
			if ((number >= 0) && (number < windowActivator.Length))
			{
				WindowOpenClose activeA = windowActivator[number];

				if (activeA)
				{
					if (!activeA.IsOpen())
					{
						activeA.Open();
					}
				}
			}
		}

		protected void WindowActivator_Close(int number)
		{
			if ((number >= 0) && (number < windowActivator.Length))
			{
				WindowOpenClose activeA = windowActivator[number];

				if (activeA)
				{
					if (activeA.IsOpen())
					{
						activeA.Close();
					}
				}
			}
		}

		//blocker windows activator 
		private void WindowBlocker_Open()
		{
			if (numberWindowBlocker >= 0)
			{
				WindowActivator_Open(numberWindowBlocker);
			}
		}

		private void WindowBlocker_Close()
		{
			if (numberWindowBlocker >= 0)
			{
				WindowActivator_Close(numberWindowBlocker);
			}
		}

		//blocker console Windows activator
		private void ConsoleWindowBlocker_Open()
		{
			if (numberConsoleWindowBlocker >= 0)
			{
				WindowActivator_Open(numberConsoleWindowBlocker);
			}
		}

		private void ConsoleWindowBlocker_Close()
		{
			if (numberConsoleWindowBlocker >= 0)
			{
				WindowActivator_Close(numberConsoleWindowBlocker);
			}
		}
		#endregion

		#region Events
		protected virtual void ActivateWindowEvent()
		{

		}

		protected virtual void DisActivateWindowEvent()
		{

		}

		protected virtual void ChangeWindowEvent(int number)
		{

		}

		protected virtual void ActivateConsoleWEvent()
		{

		}

		protected virtual void DisActivateConsoleWEvent()
		{

		}

		protected virtual void ChangeConsoleWEvent(int number)
		{

		}
		
		protected virtual void ActivateGameWEvent()
		{

		}

		protected virtual void DisActivateGameWEvent()
		{

		}

		protected virtual void ChangeGameWEvent(bool isActive)
		{

		}

		protected virtual void ActivateAdviceGameWEvent()
		{
			
		}
		
		protected virtual void DisActivateAdviceGameWEvent()
		{
			
		}
		#endregion

		#region Windows
		public int WindowActive
		{
			get { return windowActive; }
		}

		public void ActivateWindow(int number)
		{
			if (windowActive == number)
			{
				DisActivateWindow();
			}
			else
			{
				if (windowActive > -1)
				{
					WindowActivator_Close(windowActive);
				}

				WindowActivator_Open(number);

				if (windowActive == -1)
				{
					WindowBlocker_Open();

					ActivateWindowEvent();
				}

				windowActive = number;

				ChangeWindowEvent(number);
			}
		}

		public void DisActivateWindow()
		{
			if (windowActive > -1)
			{
				WindowActivator_Close(windowActive);

				windowActive = -1;
				
				DisActivateWindowEvent();
			}

			WindowBlocker_Close();
		}
		#endregion

		#region ConsoleWindows
		public int ConsoleWindowActive => consoleWindowActive;

		public void ActivateConsoleWindow(int number)
		{
			if (consoleWindowActive == number)
			{
				DisActivateConsoleWindow();
			}
			else
			{
				if (consoleWindowActive > -1)
				{
					WindowActivator_Close(consoleWindowActive);
				}

				WindowActivator_Open(number);

				if (consoleWindowActive == -1)
				{
					ConsoleWindowBlocker_Open();

					ActivateConsoleWEvent();
				}

				consoleWindowActive = number;

				ChangeConsoleWEvent(number);
			}
		}

		public void DisActivateConsoleWindow()
		{
			if (consoleWindowActive > -1)
			{
				WindowActivator_Close(consoleWindowActive);
				
				consoleWindowActive = -1;
				
				DisActivateConsoleWEvent();
			}
			
			ConsoleWindowBlocker_Close();
		}
		
		//console Message
		private void ConsoleWinMessageSmall_SetTxt(string val)
		{
			consoleWinMessageSmallTextHead.text = TextHelp.SpecTextChar(val);
		}
		
		private void ConsoleWinMessageBig_SetTxt(string val)
		{
			consoleWinMessageBigTextHead.text = TextHelp.SpecTextChar(val);
		}

		private void ConsoleWinMessage_SetOkAction(UnityAction val)
		{
			consoleWinMessageActionOk.AddListener(val);
		}

		private void ConsoleWinMessage_ClearOkAction()
		{
			consoleWinMessageActionOk.RemoveAllListeners();
		}

		protected virtual void ConsoleWinMessage_ButtonOk()
		{
			consoleWinMessageActionOk?.Invoke();
			
			DisActivateConsoleWindow();
			
			ConsoleWinMessage_ClearOkAction();
		}
		
		public void ConsoleWinMessage_Show(string value, UnityAction actionClick = null)
		{
			bool useSmallWindow = !(value.Length > limitCountSymbolForUseSmallWindow);

			if (useSmallWindow) 
				ConsoleWinMessageSmall_SetTxt(value);
			else
				ConsoleWinMessageBig_SetTxt(value);

			if (actionClick != null)
			{
				ConsoleWinMessage_SetOkAction(actionClick);
			}

			ActivateConsoleWindow(useSmallWindow ? numberConsoleWinMessageSmall : numberConsoleWinMessageBig);
		}

		//console YesNo
		private void ConsoleWinYesNo_SetTxt(string val)
		{
			consoleWinYesNoTextHead.text = TextHelp.SpecTextChar(val);
		}

		private void ConsoleWinYesNo_SetYesAction(UnityAction val)
		{
			consoleWinYesNoActionYes.AddListener(val);
		}

		private void ConsoleWinYesNo_SetNoAction(UnityAction val)
		{
			consoleWinYesNoActionNo.AddListener(val);
		}

		private void ConsoleWinYesNo_ClearYesNoAction()
		{
			consoleWinYesNoActionYes.RemoveAllListeners();
			consoleWinYesNoActionNo.RemoveAllListeners();
		}
		
		protected virtual void ConsoleWinYesNo_ButtonYes()
		{
			consoleWinYesNoActionYes?.Invoke();

			DisActivateConsoleWindow();

			ConsoleWinYesNo_ClearYesNoAction();
		}

		protected virtual void ConsoleWinYesNo_ButtonNo()
		{
			consoleWinYesNoActionNo?.Invoke();

			DisActivateConsoleWindow();

			ConsoleWinYesNo_ClearYesNoAction();
		}
		
		public void ConsoleWinYesNo_Show(string value, UnityAction actionYes, UnityAction actionNo = null)
		{
			ConsoleWinYesNo_SetTxt(value);
			ConsoleWinYesNo_SetYesAction(actionYes);

			if (actionNo != null)
				ConsoleWinYesNo_SetNoAction(actionNo);

			ActivateConsoleWindow(numberConsoleWinYesNo);
		}
		#endregion

		public virtual bool IsMenuActive()
		{
			return WindowActive >= 0 || ConsoleWindowActive >= 0;
		}

		#region GameWindows
		protected void ActivateGameInterface()
		{
			if (!isGameInterfaceActivated)
			{
				WindowActivator_Open(numberGameWindowHud);

				ActivateGameWEvent();

				isGameInterfaceActivated = true;
				
				ChangeGameWEvent(isGameInterfaceActivated);
			}
		}

		protected void DisActivateGameInterface()
		{
			if (isGameInterfaceActivated)
			{
				WindowActivator_Close(numberGameWindowHud);

				isGameInterfaceActivated = false;
				
				ChangeGameWEvent(isGameInterfaceActivated);
				
				DisActivateGameWEvent();
			}
		}
		
		//game window Advice
		private void CloseAdviceGameWindow()
		{
			WindowActivator_Close(numberGameWindowAdvice);

			DisActivateAdviceGameWEvent();
		}
		
		public void ShowAdviceGameWindow(string value)
		{
			gameWindowAdviceText.text = TextHelp.SpecTextChar(value);
			
			WindowActivator_Open(numberGameWindowAdvice);

			if (IsInvoking(nameof(CloseAdviceGameWindow)))
			{
				CancelInvoke();
			}

			int delayTime = value.Length / _countCharForDelayOneSecond;
			delayTime = delayTime > 0 ? delayTime : 1;
            
			Invoke(nameof(CloseAdviceGameWindow), delayTime);

			ActivateAdviceGameWEvent();
		}
		#endregion
	}
}
