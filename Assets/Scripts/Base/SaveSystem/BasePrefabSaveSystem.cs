using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Base.SaveSystem
{
    public class BasePrefabSaveSystem : MonoBehaviour
    {
        public bool didInit;

        [Header("Base Settings")] public string gamePrefsName = "DefaultGame";
        
        [SerializeField] protected float audioSoundSliderValue;
        [SerializeField] private Slider audioSoundSlider;
        public event UnityAction ChangeSoundValueEvent;

        [SerializeField] protected float audioMusicSliderValue;
        [SerializeField] private Slider audioMusicSlider;
        public event UnityAction ChangeMusicValueEvent;
        
        [SerializeField] protected float graphicsSliderValue;
        [SerializeField] private Slider graphicsSlider;
        [SerializeField] private int graphicsDefaultValue = -1;
        public event UnityAction ChangeGraphicsValueEvent;
        
        private int detailLevels = 6;

        private void Start()
        {
            RestoreOptionsPref();
            InitEvents();
        }

        protected virtual void RestoreOptionsPref()
        {
            RestoreSfxValue();

            RestoreMusicValue();

            RestoreGraphicsValue();

            didInit = true;
        }

        protected virtual void InitEvents()
        {
            ChangeGraphicsValueEvent += SetQuality;
        }

        private void RestoreSfxValue()
        {
            string stKey = $"{gamePrefsName}_SFXVol";
            if (PlayerPrefs.HasKey(stKey))
            {
                audioSoundSliderValue = PlayerPrefs.GetFloat(stKey);
            }
            else
            {
                audioSoundSliderValue = 1;
            }
            
            if (audioSoundSlider != null)
            {
                audioSoundSlider.value = audioSoundSliderValue;
            }
        }

        private void RestoreMusicValue()
        {
            string stKey = $"{gamePrefsName}_MusicVol";
            if (PlayerPrefs.HasKey(stKey))
            {
                audioMusicSliderValue = PlayerPrefs.GetFloat(stKey);
            }
            else
            {
                audioMusicSliderValue = 1;
            }
            
            if (audioMusicSlider != null)
            {
                audioMusicSlider.value = audioMusicSliderValue;
            }
        }

        private void RestoreGraphicsValue()
        {
            string stKey = $"{gamePrefsName}_GraphicsDetail";
            if (PlayerPrefs.HasKey (stKey)) {
                graphicsSliderValue = PlayerPrefs.GetFloat (stKey);
            } else {
                if (graphicsDefaultValue == -1) {
                    string[] names = QualitySettings.names;
                    detailLevels = names.Length;

                    switch (Application.platform) {
                        case RuntimePlatform.Android:
                        case RuntimePlatform.IPhonePlayer:
                            graphicsSliderValue = 2;
                            break;
                        default:
                            graphicsSliderValue = detailLevels;
                            break;
                    }
                } else {
                    graphicsSliderValue = graphicsDefaultValue;
                }
            }

            #if UNITY_EDITOR
            Debug.Log ("quality=" + graphicsSliderValue);
            #endif

            SetQuality ();
            
            if (graphicsSlider != null) {
                string[] namesQlt = QualitySettings.names;
                graphicsSlider.maxValue = namesQlt.Length - 1;

                graphicsSlider.value = graphicsSliderValue;
            }
        }

        protected virtual void SaveOptionsPref()
        {
            SaveSoundValue();

            SaveMusicValue();

            SaveGraphicsValue();
        }

        private void SetQuality()
        {
            QualitySettings.SetQualityLevel ((int)graphicsSliderValue, true);
        }

        private void SaveSoundValue()
        {
            string stKey = $"{gamePrefsName}_SFXVol";
            PlayerPrefs.SetFloat(stKey, audioSoundSliderValue);
        }

        private void SaveMusicValue()
        {
            string stKey = $"{gamePrefsName}_MusicVol";
            PlayerPrefs.SetFloat(stKey, audioMusicSliderValue);
        }

        private void SaveGraphicsValue()
        {
            string stKey = $"{gamePrefsName}_GraphicsDetail";
            PlayerPrefs.SetFloat(stKey, graphicsSliderValue);
        }

        public void ChangeSoundVal(float val)
        {
            audioSoundSliderValue = val;

            if (didInit)
            {
                SaveSoundValue();
                
                ChangeSoundValueEvent?.Invoke();
            }
        }
        
        public void ChangeMusicVal(float val) {
            audioMusicSliderValue = val;

            if (didInit)
            {
                SaveMusicValue();
                
                ChangeMusicValueEvent?.Invoke();
            }
        }

        public void ChangeGraphicsVal(float val) {
            graphicsSliderValue = val;

            if (didInit)
            {
                SaveGraphicsValue();
                
                ChangeGraphicsValueEvent?.Invoke();
            }
        }
    }
}
