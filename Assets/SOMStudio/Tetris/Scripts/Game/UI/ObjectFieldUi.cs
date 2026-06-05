using System.Linq;
using SOMStudio.Tetris.Scripts.Game.Field;
using UnityEngine;
using UnityEngine.UI;

namespace SOMStudio.Tetris.Scripts.Game.UI
{
    public class ObjectFieldUi : MonoBehaviour
    {
        public Image[] pointList;

        private ObjectField activeObject;

        private Image GetPoint(int x, int y)
        {
            int resultNumber = y * 4 + x;

            return pointList[resultNumber];
        }

        public void ShowObject(ObjectField objectField)
        {
            activeObject = objectField;

            var points = activeObject.GetPoints();
            var center = activeObject.CenterTransformation;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    bool hasPoint = points.Contains(new PointField(i, j));

                    if (hasPoint)
                        GetPoint(i, j).color = Color.black;
                    else
                        GetPoint(i, j).color = new Color(1.0f, 1.0f, 1.0f, .01f);
                }
            }
        }
    }
}