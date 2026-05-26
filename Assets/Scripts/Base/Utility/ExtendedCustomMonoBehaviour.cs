using UnityEngine;

namespace Base.Utility
{
	public class ExtendedCustomMonoBehaviour : MonoBehaviour
	{
		protected int id;

		protected Transform myTransform;
		protected GameObject myGameObject;
		protected Rigidbody myBody;

		protected virtual void Start()
		{
			Init();
		}

		protected virtual void Init()
		{
			if (!myTransform)
			{
				myTransform = transform;
			}

			if (!myGameObject)
			{
				myGameObject = gameObject;
			}

			if (!myBody)
			{
				myBody = GetComponent<Rigidbody>();
			}
		}

		public virtual void SetId(int anId)
		{
			id = anId;
		}

		public int GetId()
		{
			return id;
		}
	}
}
