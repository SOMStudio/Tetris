using System;
using Base.Utility;
using DG.Tweening;
using UnityEngine;

namespace Base.UI
{
    public class WindowOpenCloseWithShift : WindowOpenClose
    {
        [SerializeField] private Vector2 directionShift;
        [SerializeField] private float timeForShift = 0.4f;
    
        private CanvasGroup _canvasGrope;
        private RectTransform _rectTransform;

        private Tweener _openTween;
        private Tweener _closeTween;

        protected override void Init()
        {
            base.Init();

            _canvasGrope = GetComponent<CanvasGroup>();
        
            if (!_canvasGrope) throw new NullReferenceException();

            _rectTransform = GetComponent<RectTransform>();

            if (IsOpen())
            {
                _rectTransform.anchoredPosition = Vector2.zero;
                
                _canvasGrope.interactable = true;
                _canvasGrope.blocksRaycasts = true;
            }
            else
            {
                _rectTransform.anchoredPosition = directionShift;
                
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
                    _openTween = _rectTransform
                        .DOAnchorPos(Vector2.zero, timeForShift)
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
                    _closeTween = _rectTransform
                        .DOAnchorPos(directionShift, timeForShift)
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
