using UnityEngine;

namespace Base.Utility
{
	[AddComponentMenu("Utility/Window Open-Close")]
	public class WindowOpenClose : MonoBehaviour
	{
		[SerializeField]
		private bool defaultOpen = false;
		
		private bool _isOpen = false;

		void Awake()
		{
			Init();
		}

		protected virtual void Init()
		{
			_isOpen = defaultOpen;
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
				_isOpen = true;
			}
		}

		public virtual void Close()
		{
			if (IsOpen())
			{
				_isOpen = false;
			}
		}

		public bool IsOpen()
		{
			return _isOpen;
		}
		
	}
}
