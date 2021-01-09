using UnityEngine;
using UnityEngine.Events;

namespace Base.Utility
{
	public class SingletonComposition<T> where T : MonoBehaviour
	{
		private SingletonComposition()
		{

		}

		public SingletonComposition(T instanceField, UnityAction initAction, UnityAction destroyAction)
		{
			if (instanceField)
			{
				destroyAction?.Invoke();
			} else
			{
				initAction?.Invoke();
			}
		}
	}
}
