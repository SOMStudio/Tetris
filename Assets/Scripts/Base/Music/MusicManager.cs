using System.Collections.Generic;
using Base.Utility;
using UnityEngine;

namespace Base.Music
{
	[AddComponentMenu("Base/Music Manager")]
	public class MusicManager : SingletonMonoBehaviour<MusicManager> {

		[Header("Main")]
		[SerializeField] protected List<MusicClipManager> musicList;

		private void Start() {
			Init();
		}

		private void Init() {
			
		}

		public void UpdateVolume() {
			foreach (MusicClipManager item in musicList) {
				item.UpdateVolume ();
			}
		}

		public void StopMusic(int value) {
			MusicClipManager temp = musicList [value];

			if (temp) {
				temp.StopMusic();
			}
		}

		public void PlayMusic(int value) {
			MusicClipManager temp = musicList [value];

			if (temp) {
				temp.PlayMusic();
			}
		}
	
		public void PlayMusicStopAnother(int value) {
			for (int i = 0; i < musicList.Count; i++)
			{
				if (i != value)
				{
					if (musicList[i].IsPlaying() == true)
					{
						StopMusic(i);
					}
				}
				else
				{
					PlayMusic (i);
				}
			}
		}
		
		public void PlayMenuMusic()
		{
			PlayMusicStopAnother(0);
		}
	
		public void PlayLevelMusic()
		{
			PlayMusicStopAnother(1);
		}
	}
}
