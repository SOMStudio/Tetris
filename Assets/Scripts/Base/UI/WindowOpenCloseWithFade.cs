using System;
using Base.Utility;
using DG.Tweening;
using UnityEngine;

namespace Base.UI
{
    public class WindowOpenCloseWithFade : WindowOpenClose
    {
        [SerializeField] private float timeForFade = 0.4f;
    
        private CanvasGroup _canvasGrope;

        private Tweener _openTween;
        private Tweener _closeTween;

        protected override void Init()
        {
            base.Init();

            _canvasGrope = GetComponent<CanvasGroup>();
        
            if (!_canvasGrope) throw new NullReferenceException();

            if (IsOpen())
            {
                _canvasGrope.alpha = 1;
                _canvasGrope.interactable = true;
                _canvasGrope.blocksRaycasts = true;
            }
            else
            {
                _canvasGrope.alpha = 0;
                _canvasGrope.interactable = false;
                _canvasGrope.blocksRaycasts = false;
            }
        }

        public override void Open()
        {
            if (!IsOpen())
            {
                base.Open();

                if (_openTween == null)
                    _openTween = _canvasGrope
                        .DOFade(1, timeForFade)
                        .SetAutoKill(false);
                else
                    _openTween.Restart();
                
                _canvasGrope.interactable = true;
                _canvasGrope.blocksRaycasts = true;
            }
        }

        public override void Close()
        {
            if (IsOpen())
            {
                base.Close();

                if (_closeTween == null)
                    _closeTween = _canvasGrope
                        .DOFade(0, timeForFade)
                        .SetAutoKill(false);
                else
                    _closeTween.Restart();

                _canvasGrope.interactable = false;
                _canvasGrope.blocksRaycasts = false;
            }
        }

        private void OnDestroy()
        {
            _openTween.Kill();
            _closeTween.Kill();
        }
    }
}
