using TMPro;
using UnityEngine;

namespace Base.UI.HudPanel
{
    public class HudLifeScoreManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text textLeft;
        [SerializeField] private TMP_Text textRight;
        
        public void UpdateLeftText(int value)
        {
            textLeft.text = value.ToString();
        }
        
        public void UpdateLeftText(string value)
        {
            textLeft.text = value;
        }
        
        public void UpdateRightText(int value)
        {
            textRight.text = value.ToString();
        }
        
        public void UpdateRightText(string value)
        {
            textRight.text = value;
        }
    }
}
