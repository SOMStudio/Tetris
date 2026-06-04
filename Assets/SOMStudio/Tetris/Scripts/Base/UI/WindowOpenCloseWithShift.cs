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
    
        private CanvasGroup canvasGrope;
        private RectTransform rectTransform;

        private Tweener openTween;
        private Tweener closeTween;

        protected override void Init()
        {
            base.Init();

            canvasGrope = GetComponent<CanvasGroup>();
        
            if (!canvasGrope) throw new NullReferenceException();

            rectTransform = GetComponent<RectTransform>();

            if (IsOpen())
            {
                rectTransform.anchoredPosition = Vector2.zero;
                
                canvasGrope.interactable = true;
                canvasGrope.blocksRaycasts = true;
            }
            else
            {
                rectTransform.anchoredPosition = directionShift;
                
                canvasGrope.interactable = false;
                canvasGrope.blocksRaycasts = false;
            }
        }

        public override void Open()
        {
            if (!IsOpen())
            {
                base.Open();

                if (openTween == null)
                    openTween = rectTransform
                        .DOAnchorPos(Vector2.zero, timeForShift)
                        .SetAutoKill(false);
                else
                    openTween.Restart();

                canvasGrope.interactable = true;
                canvasGrope.blocksRaycasts = true;
            }
        }

        public override void Close()
        {
            if (IsOpen())
            {
                base.Close();

                if (closeTween == null)
                    closeTween = rectTransform
                        .DOAnchorPos(directionShift, timeForShift)
                        .SetAutoKill(false);
                else
                    closeTween.Restart();
            
                canvasGrope.interactable = false;
                canvasGrope.blocksRaycasts = false;
            }
        }

        private void OnDestroy()
        {
            openTween.Kill();
            closeTween.Kill();
        }
    }
}
