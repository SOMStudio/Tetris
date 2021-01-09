using System;
using UnityEngine;

namespace Tetris
{
    public class PointFieldManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject pointObject;

        private Renderer rend;
        
        private void Awake()
        {
            pointObject = this.gameObject;

            rend = GetComponent<Renderer>();
        }

        public void SetPosition(PointField point)
        {
            var setPos = point.GetV2Pos();
        
            transform.position = new Vector3(setPos.x, setPos.y, .0f);
        }

        public void SetColor(Color color)
        {
            rend.material.color = color;
        }
    }
}
