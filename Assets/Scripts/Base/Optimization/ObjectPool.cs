using System.Collections.Generic;
using Base.Optimization.Interfaces;
using UnityEngine;

namespace Base.Optimization
{
	public class ObjectPool : MonoBehaviour {

		private static readonly Dictionary<string, ObjectPool> namesOfObjects = new Dictionary<string, ObjectPool>();

		public static ObjectPool GetPoolByName(string name) {
			return namesOfObjects[name];
		}

		[SerializeField]
		private string nameOfYourPool = "DefaultName";

		[SerializeField]
		private Transform yourPoolPrefab = null;

		[SerializeField]
		private int initialObjectCounter = 23;

		[SerializeField]
		private bool setParentThisObject = true;

		[SerializeField]
		private bool setActiveRecursively = false;

		[SerializeField]
		private bool useAdjustLiberate = false;
		
		private readonly Queue<Transform> _yourObjectsStack = new Queue<Transform>();
		private readonly Dictionary<Transform, IPoolObject> _yourObjectsInterface = new Dictionary<Transform, IPoolObject>();

		private void Awake()
		{
			namesOfObjects[nameOfYourPool] = this;
		}

		private void Start()
		{
			for (int i = 0; i < initialObjectCounter; i++)
			{
				var t = Instantiate(yourPoolPrefab) as Transform;

				if (useAdjustLiberate)
					InitObjectInterface(t);
				
				AdjustingYourObject(t);
				LiberationObject(t);
			}
		}

		public Transform GetObject(Vector3? position = null)
		{
			Transform t = null;

			if (_yourObjectsStack.Count > 0) {
				t = _yourObjectsStack.Dequeue ();
			} else {
				t = Instantiate (yourPoolPrefab) as Transform;
				
				if (useAdjustLiberate)
					InitObjectInterface(t);
			}

			if (position != null)
				t.position = (Vector3)position;
			
			AdjustingYourObject (t);

			return t;
		}

		private void InitObjectInterface(Transform t)
		{
			var objPool = t.GetComponent<IPoolObject>();
			if (objPool != null)
				_yourObjectsInterface.Add(t, objPool);
		}

		private void OnPoolAdjusting(Transform obj)
		{
			if (_yourObjectsInterface.ContainsKey(obj))
			{
				var result = _yourObjectsInterface[obj];
				result.OnPoolAdjusting(this);
			}
		}
		
		private void OnPoolLiberation(Transform obj)
		{
			if (_yourObjectsInterface.ContainsKey(obj))
			{
				var result = _yourObjectsInterface[obj];
				result.OnPoolLiberation(this);
			}
		}

		private void AdjustingYourObject(Transform obj)
		{
			if (setParentThisObject)
				obj.parent = transform;

			if (setActiveRecursively)
				obj.gameObject.SetActiveRecursively(true);
			else
				obj.gameObject.SetActive(true);

			if (useAdjustLiberate)
				OnPoolAdjusting(obj);
		}

		public void LiberationObject(Transform obj)
		{
			if (useAdjustLiberate)
				OnPoolLiberation(obj);

			if (setActiveRecursively)
				obj.gameObject.SetActiveRecursively(false);
			else
				obj.gameObject.SetActive(false);

			_yourObjectsStack.Enqueue(obj);
		}
	}
}

/*
//example use in code
public class YourPoolExampleUsage : MonoBehaviour
{
	void Start()
	{
		ObjectPool pool = ObjectPool.GetPoolByName("Bang");
		Transform obj = pool.GetObject(Vector3.zero);
	}
}


//example using event in pooled Object
//[RequireComponent(typeof(ParticleSystem))]
public class YourPoolParticleSystem : MonoBehaviour, IPoolObject
{
	private ObjectPool yourPoolClass;

	public void OnPoolAdjusting(ObjectPool ypc)
	{
		yourPoolClass = ypc;
		particleSystem.renderer.enabled = true;
		particleSystem.time = 0;
		particleSystem.Clear(true);
		particleSystem.Play(true);
	}

	public void LiberationObject(ObjectPool ypc)
	{
		particleSystem.Stop();
		particleSystem.time = 0;
		particleSystem.Clear(true);
		particleSystem.renderer.enabled = false;
	}

	void Update()
	{
		if (!particleSystem.IsAlive(true) && particleSystem.renderer.enabled)
		{
			yourPoolClass.LiberationObject(transform);
		}
	}
}*/
