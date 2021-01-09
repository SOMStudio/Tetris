using Base.Music;
using Base.SaveSystem;
using Base.Sound;
using UnityEngine.Events;

namespace Game.SaveSystem
{
    public class PrefabSaveSystem : BasePrefabSaveSystem
    {
        private bool _mutedVolume = false;
        private float _soundVolumeSave = 0.0f;
        private float _musicVolumeSave = 0.0f;

        public event UnityAction<bool> MuteUnmuteVolumeEvent;
    
        [System.NonSerialized] public static PrefabSaveSystem Instance;
    
        void Awake()
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

        public bool MutedVolume
        {
            get { return _mutedVolume; }
        }

        private void MuteVolumeRestore()
        {
            if (audioSoundSliderValue == 0.0f)
            {
                _mutedVolume = true;
            }
        
            MuteUnmuteVolumeEvent?.Invoke(_mutedVolume);
        }
    
        private void MuteVolumeReset()
        {
            if (_mutedVolume)
            {
                _mutedVolume = false;
                MuteUnmuteVolumeEvent?.Invoke(_mutedVolume);
            }
        }
    
        public void MuteVolume()
        {
            if (!_mutedVolume)
            {
                if (audioSoundSliderValue > 0.0f)
                {
                    _soundVolumeSave = audioSoundSliderValue;
                }
                if (audioMusicSliderValue > 0.0f)
                {
                    _musicVolumeSave = audioMusicSliderValue;
                }
            
                ChangeSoundVal(0f);
                ChangeMusicVal(0f);
            
                _mutedVolume = true;
                MuteUnmuteVolumeEvent?.Invoke(_mutedVolume);
            }
        }
        
        public void UnmuteVolume()
        {
            if (_mutedVolume)
            {
                if (audioSoundSliderValue == 0.0f)
                {
                    _soundVolumeSave = 0.4f;
                }
                if (audioMusicSliderValue == 0.0f)
                {
                    _musicVolumeSave = 0.2f;
                }
            
                ChangeSoundVal(_soundVolumeSave);
                ChangeMusicVal(_musicVolumeSave);
            
                _mutedVolume = false;
                MuteUnmuteVolumeEvent?.Invoke(_mutedVolume);
            }
        }

        public void AddListenerMuteEvent(UnityAction<bool> value)
        {
            MuteUnmuteVolumeEvent += value;
        }
    }
}
