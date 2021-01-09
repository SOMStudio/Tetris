using System;
using UnityEngine;

namespace Base.Music
{
	[AddComponentMenu("Base/Music Clip")]
	public class MusicClipManager : MonoBehaviour {

		[Header("Main")]
		[SerializeField] private string gamePrefsName= "DefaultGame";
		
		[SerializeField] private AudioClip music;
		[SerializeField] private bool loopMusic = false;
		[SerializeField] private float fadeTime = 5f;
	
		private AudioSource source;
		private GameObject sourceGO;

		private float volumePrefs;
		private float targetVolume;

		private bool playClip = false;

		[Header("Start game")]
		public bool playAtStart = false;

		[Header("Information")]
		[SerializeField] [Range(0, 1)]  private float volume;

		private void Awake ()
		{
			string stKey = string.Format ("{0}_MusicVol", gamePrefsName);
			if (PlayerPrefs.HasKey (stKey)) {
				volumePrefs = PlayerPrefs.GetFloat (stKey);
			} else {
				volumePrefs = 0.5f;
			}
			
			sourceGO = new GameObject ("Music_" + music.name);
			source = sourceGO.AddComponent<AudioSource> ();
			source.name = "Music_" + music.name;
			source.playOnAwake = playAtStart;
			source.clip = music;
			source.volume = volume;
			DontDestroyOnLoad (sourceGO);
			
			playClip = playAtStart;
			
			FadeIn();
		}

		private void Update ()
		{
			if (playClip)
			{
				if (loopMusic)
				{
					if (!source.isPlaying)
						source.Play();
				}
				else
				{
					playClip = false;
				}
			}
			
			if (fadeTime > 0.0f)
			{
				if (Math.Abs(volume - targetVolume) > 0.01f)
				{
					volume = Mathf.Lerp(volume, targetVolume, Time.deltaTime * fadeTime);
					source.volume = volume;
				}
			}
		}
	
		public void UpdateVolume () {
			if (source) {
				volumePrefs = PlayerPrefs.GetFloat (string.Format ("{0}_MusicVol", gamePrefsName));
				
				volume = source.volume;
				targetVolume = volumePrefs;
			}
		}

		public void PlayMusic()
		{
			if (!playClip)
			{
				playClip = true;
				
				FadeIn();
			}
		}

		public void StopMusic()
		{
			if (playClip)
			{
				playClip = false;
				
				FadeOut();
			}
		}

		private void FadeIn ()
		{
			if (fadeTime > 0.0f)
				volume = 0.0f;
			else
				volume = volumePrefs;

			targetVolume = volumePrefs;
			source.volume = volume;
		}

		private void FadeOut ()
		{
			if (fadeTime > 0.0f)
				volume = source.volume;
			else
				volume = 0.0f;

			targetVolume = 0.0f;
			source.volume = volume;
		}

		public bool IsPlaying () {
			return source.isPlaying;
		}
	}
}
