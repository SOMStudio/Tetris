using Base.Input;
using Base.Input.Interfaces;
using Base.Input.Samples;
using Base.Sound;
using Base.UI.Effectors;
using Base.Utility;
using Game;
using UnityEngine;

namespace Tetris
{
    public class PlayerManager : ExtendedCustomMonoBehaviour
    {
        [Header("Input Ui Slider")]
        [SerializeField] private InputSlideUi4Way inputSlider;
        [SerializeField] private InputEffector inputEffector;

        private float hor = .0f;
        private float ver = .0f;
        
        private IInputManager _inputManager;

        public int Id => id;

        public float Horizontal => hor;
        public float Vertical => ver;

        private void Update()
        {
            CheckForInput();
        }

        protected override void Init()
        {
            base.Init();
            
            SetId(myGO.GetHashCode());

            #if (INPUT_MOBILE)
            inputSlider.InitBindings(new SampleBindings());
            _inputManager = inputSlider;
            _inputManager.AddActionToBindingKeyDown("shoot", StartControl);
            _inputManager.AddActionToBindingKeyUp("shoot", StopControl);
            #else
             _inputManager = new InputManager(new SampleBindings());
            _inputManager.AddActionToBindingKeyDown("shoot", StartControl);
            _inputManager.AddActionToBindingKeyUp("shoot", StopControl);
            #endif
        }

        private void CheckForInput()
        {
            if (!MenuManager.Instance) return;
            if (MenuManager.Instance.IsMenuActive()) return;
            if (MenuManager.Instance.IsCursorOverGameUi()) return;
            
            ver = _inputManager.GetAxis("Vertical");
            hor = _inputManager.GetAxis("Horizontal");

            #if (INPUT_MOBILE)
            inputEffector?.ActivateShiftEffect(Mathf.Abs(hor) > 0.0f, hor);
            #else
            _inputManager.CheckForInput();
            #endif
        }

        private void StartControl()
        {
            #if (INPUT_MOBILE)
            inputEffector?.ActivateClickEffect(true);
            #endif

            SoundManager.Instance?.PlaySoundByIndex(1, myTransform.position);
        }

        private void StopControl()
        {
            #if (INPUT_MOBILE)
            inputEffector?.ActivateClickEffect(false);
            #endif

            SoundManager.Instance?.PlaySoundByIndex(1, myTransform.position);
        }
    }
}

