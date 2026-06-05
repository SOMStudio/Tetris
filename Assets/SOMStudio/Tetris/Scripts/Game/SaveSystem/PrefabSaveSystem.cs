using SOMStudio.Tetris.Scripts.Base.Music;
using SOMStudio.Tetris.Scripts.Base.SaveSystem;
using SOMStudio.Tetris.Scripts.Base.Sound;
using UnityEngine.Events;

namespace SOMStudio.Tetris.Scripts.Game.SaveSystem
{
    public class PrefabSaveSystem : BasePrefabSaveSystem
    {
        private bool mutedVolume;
        private float soundVolumeSave;
        private float musicVolumeSave;

        public event UnityAction<bool> MuteUnmuteVolumeEvent;
    
        [System.NonSerialized] public static PrefabSaveSystem Instance;

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    
        protected override void RestoreOptionsPref()
        {
            base.RestoreOptionsPref();

            UserManager.Instance?.LoadPrivateDataPlayer();
        
            InitEvents();
        
            MuteVolumeRestore();
        }

        protected override void InitEvents()
        {
            base.InitEvents();

            ChangeSoundValueEvent += MuteVolumeReset;
            if (SoundManager.Instance)
                ChangeSoundValueEvent += SoundManager.Instance.UpdateVolume;

            ChangeMusicValueEvent += MuteVolumeReset;
            if (MusicManager.Instance)
                ChangeMusicValueEvent += MusicManager.Instance.UpdateVolume;
        }

        public bool MutedVolume => mutedVolume;

        private void MuteVolumeRestore()
        {
            if (audioSoundSliderValue == 0.0f)
            {
                mutedVolume = true;
            }
        
            MuteUnmuteVolumeEvent?.Invoke(mutedVolume);
        }
    
        private void MuteVolumeReset()
        {
            if (mutedVolume)
            {
                mutedVolume = false;
                MuteUnmuteVolumeEvent?.Invoke(mutedVolume);
            }
        }
    
        public void MuteVolume()
        {
            if (!mutedVolume)
            {
                if (audioSoundSliderValue > 0.0f)
                {
                    soundVolumeSave = audioSoundSliderValue;
                }
                if (audioMusicSliderValue > 0.0f)
                {
                    musicVolumeSave = audioMusicSliderValue;
                }
            
                ChangeSoundVal(0f);
                ChangeMusicVal(0f);
            
                mutedVolume = true;
                MuteUnmuteVolumeEvent?.Invoke(mutedVolume);
            }
        }
        
        public void UnmuteVolume()
        {
            if (mutedVolume)
            {
                if (audioSoundSliderValue == 0.0f)
                {
                    soundVolumeSave = 0.4f;
                }
                if (audioMusicSliderValue == 0.0f)
                {
                    musicVolumeSave = 0.2f;
                }
            
                ChangeSoundVal(soundVolumeSave);
                ChangeMusicVal(musicVolumeSave);
            
                mutedVolume = false;
                MuteUnmuteVolumeEvent?.Invoke(mutedVolume);
            }
        }

        public void AddListenerMuteEvent(UnityAction<bool> value)
        {
            MuteUnmuteVolumeEvent += value;
        }
    }
}
