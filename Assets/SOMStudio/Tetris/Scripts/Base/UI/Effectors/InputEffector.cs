using UnityEngine;
using UnityEngine.UI;

namespace Base.UI.Effectors
{
    public class InputEffector : MonoBehaviour
    {
        [Header("Color")]
        [SerializeField] private Color defaultColor = Color.white;
        [SerializeField] private Color clickColor = Color.white;

        [Header("Shift")]
        [SerializeField] private float rotateSpeed = 10;

        [Header("Reference")]
        [SerializeField] private RectTransform rectTransformBg;
        [SerializeField] private Image imageControl;
        [SerializeField] private Image imageBg;

        private bool rotate;
        private float multiplier = 1.0f;
        
        private void Update()
        {
            if (rotate)
            {
                rectTransformBg.Rotate(Vector3.forward, multiplier * rotateSpeed * Time.deltaTime);
            }
        }

        public void ActivateClickEffect(bool value)
        {
            if (value)
            {
                imageBg.color = clickColor;
                imageControl.color = clickColor;
            }
            else
            {
                imageBg.color = defaultColor;
                imageControl.color = defaultColor;
            }
        }

        public void ActivateShiftEffect(bool rotate, float multiplier)
        {
            this.rotate = rotate;
            this.multiplier = multiplier;
        }
        
    }
}
