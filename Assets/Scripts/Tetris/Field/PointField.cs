using System.Data.Common;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Tetris
{
    public struct PointField
    {
        public int x;
        public int y;

        public PointField(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public PointField(Vector2Int xy)
        {
            x = xy.x;
            y = xy.y;
        }

        public Vector2Int GetV2Pos()
        {
            return new Vector2Int(x, y);
        }

        public Vector3Int GetV3Pos()
        {
            return new Vector3Int(x, y, 0);
        }
        
        public static PointField operator +(PointField point1, PointField point2)
        {
            return new PointField(point1.GetV2Pos() + point2.GetV2Pos());
        }
        
        public static PointField operator -(PointField point1, PointField point2)
        {
            return new PointField(point1.GetV2Pos() - point2.GetV2Pos());
        }

        public static bool operator ==(PointField point1, PointField point2)
        {
            return point1.Equals(point2);
        }

        public static bool operator !=(PointField point1, PointField point2)
        {
            return !(point1 == point2);
        }

        public override bool Equals(object obj)
        {
            var point = (PointField) obj;

            if (point == null) return false;

            if (point.x == this.x && point.y == this.y)
                return true;
            else
                return false;
        }
        
        public bool Equals(PointField point)
        {
            if (point.x == this.x && point.y == this.y)
                return true;
            else
                return false;
        }
    }
}