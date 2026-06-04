using System.Collections.Generic;
using Base.Utility;
using UnityEngine;

namespace Base.Sound
{
	[AddComponentMenu("SOMStudio/Tetris/Base/Sound Manager")]
	public class SoundManager : SingletonMonoBehaviour<SoundManager>
	{
		[Header("Main")]
		[SerializeField] private string gamePrefsName = "DefaultGame";
		[SerializeField] protected AudioClip[] gameSounds;
		
		private List<SoundObject> soundObjectList;
		private SoundObject tempSoundObj;

		[Header("Information")]
		[SerializeField] [Range(0, 1)] private float volume = 0.5f;

		private void Start()
		{
			if (soundObjectList == null)
			{
				Init();
			}
		}

		private void Init()
		{
			string stKey = $"{gamePrefsName}_SFXVol";
			if (PlayerPrefs.HasKey(stKey))
			{
				volume = PlayerPrefs.GetFloat(stKey);
			}
			else
			{
				volume = 0.5f;
			}

			soundObjectList = new List<SoundObject>();

			foreach (AudioClip theSound in gameSounds)
			{
				tempSoundObj = new SoundObject(theSound, theSound.name, volume);
				soundObjectList.Add(tempSoundObj);

				DontDestroyOnLoad(tempSoundObj.sourceGO);
			}
		}

		public float GetVolume()
		{
			return volume;
		}

		public void UpdateVolume()
		{
			if (soundObjectList == null)
			{
				Init();
			}

			string stKey = $"{gamePrefsName}_SFXVol";
			volume = PlayerPrefs.GetFloat(stKey);

			for (int i = 0; i < soundObjectList.Count; i++)
			{
				tempSoundObj = soundObjectList[i];
				tempSoundObj.source.volume = volume;
			}
		}

		public void PlaySoundByIndex(int anIndexNumber, Vector3 aPosition)
		{
			if (anIndexNumber > soundObjectList.Count)
			{
				anIndexNumber = soundObjectList.Count - 1;
			}

			tempSoundObj = soundObjectList[anIndexNumber];
			tempSoundObj.PlaySound(aPosition);
		}
	}

	public class SoundObject
	{
		public AudioSource source;
		public GameObject sourceGO;
		public Transform sourceTR;

		public AudioClip clip;
		public string name;

		public SoundObject(AudioClip aClip, string aName, float aVolume)
		{
			sourceGO = new GameObject("AudioSource_" + aName);
			sourceTR = sourceGO.transform;
			source = sourceGO.AddComponent<AudioSource>();
			source.name = "AudioSource_" + aName;
			source.playOnAwake = false;
			source.clip = aClip;
			source.volume = aVolume;
			clip = aClip;
			name = aName;
		}

		public void PlaySound(Vector3 atPosition)
		{
			sourceTR.position = atPosition;
			source.PlayOneShot(clip);
		}
	}
}