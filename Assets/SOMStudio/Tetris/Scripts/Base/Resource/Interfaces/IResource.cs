using UnityEngine.Events;

namespace SOMStudio.Tetris.Scripts.Base.Resource.Interfaces
{
	interface IResource<T> where T : struct
	{
		void Set(T setValue);
		T Get();

		void Add(T setValue);
		void Reduce(T setValue);

		void AddListener(UnityAction<T> setValue);
		void RemoveListener(UnityAction<T> setValue);
	}
}
