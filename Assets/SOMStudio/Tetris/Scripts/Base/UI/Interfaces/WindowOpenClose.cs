using UnityEngine;

namespace SOMStudio.Tetris.Scripts.Base.UI.Interfaces
{
	[AddComponentMenu("SOMStudio/Tetris/Utility/Window Open-Close")]
	public class WindowOpenClose : MonoBehaviour
	{
		[SerializeField]
		private bool defaultOpen;
		
		private bool isOpen;

		private void Awake()
		{
			Init();
		}

		protected virtual void Init()
		{
			isOpen = defaultOpen;
		}
		
		public void Click()
		{
			if (IsOpen())
			{
				Close();
			}
			else
			{
				Open();
			}
		}

		public virtual void Open()
		{
			if (!IsOpen())
			{
				isOpen = true;
			}
		}

		public virtual void Close()
		{
			if (IsOpen())
			{
				isOpen = false;
			}
		}

		public bool IsOpen()
		{
			return isOpen;
		}
	}
}
