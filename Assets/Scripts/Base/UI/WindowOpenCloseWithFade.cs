using System;
using Base.Utility;
using DG.Tweening;
using UnityEngine;

namespace Base.UI
{
    public class WindowOpenCloseWithFade : WindowOpenClose
    {
        [SerializeField] private float timeForFade = 0.4f;
    
        private CanvasGroup canvasGrope;

        private Tweener openTween;
        private Tweener closeTween;

        protected override void Init()
        {
            base.Init();

            canvasGrope = GetComponent<CanvasGroup>();
        
            if (!canvasGrope) throw new NullReferenceException();

            if (IsOpen())
            {
                canvasGrope.alpha = 1;
                canvasGrope.interactable = true;
                canvasGrope.blocksRaycasts = true;
            }
            else
            {
                canvasGrope.alpha = 0;
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
                    openTween = canvasGrope
                        .DOFade(1, timeForFade)
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
                    closeTween = canvasGrope
                        .DOFade(0, timeForFade)
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
