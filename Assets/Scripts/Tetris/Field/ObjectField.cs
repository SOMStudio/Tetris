using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tetris
{
    public class ObjectField
    {
        private string name = "free";
        private Vector2Int size;
        private Dictionary<PointField, bool> objectField = new Dictionary<PointField, bool>();
        private Vector2Int centerTransformation = Vector2Int.zero;
        private bool canRotate = true;

        public static ObjectField LightningLeftRight()
        {
            var objectSet = new Dictionary<PointField, bool>();
            objectSet.Add(new PointField(0,0), true);
            objectSet.Add(new PointField(0,1), true);
            objectSet.Add(new PointField(1,1), true);
            objectSet.Add(new PointField(1,2), true);
            
            return new ObjectField("LightningLeftRight", objectSet, new Vector2Int(2, 3), Vector2Int.one);
        }
        
        public static ObjectField LightningRightLeft()
        {
            var objectSet = new Dictionary<PointField, bool>();
            objectSet.Add(new PointField(1,0), true);
            objectSet.Add(new PointField(1,1), true);
            objectSet.Add(new PointField(0,1), true);
            objectSet.Add(new PointField(0,2), true);
            
            return new ObjectField("LightningRightLeft", objectSet, new Vector2Int(2, 3), Vector2Int.one);
        }
        
        public static ObjectField HookToRight()
        {
            var objectSet = new Dictionary<PointField, bool>();
            objectSet.Add(new PointField(0,0), true);
            objectSet.Add(new PointField(0,1), true);
            objectSet.Add(new PointField(0,2), true);
            objectSet.Add(new PointField(1,2), true);
            
            return new ObjectField("HookToRight", objectSet, new Vector2Int(2, 3), Vector2Int.one);
        }
        
        public static ObjectField HookToLeft()
        {
            var objectSet = new Dictionary<PointField, bool>();
            objectSet.Add(new PointField(1,0), true);
            objectSet.Add(new PointField(1,1), true);
            objectSet.Add(new PointField(1,2), true);
            objectSet.Add(new PointField(0,2), true);
            
            return new ObjectField("HookToLeft", objectSet, new Vector2Int(2, 3), Vector2Int.one);
        }
        
        public static ObjectField Line()
        {
            var objectSet = new Dictionary<PointField, bool>();
            objectSet.Add(new PointField(0,0), true);
            objectSet.Add(new PointField(0,1), true);
            objectSet.Add(new PointField(0,2), true);
            objectSet.Add(new PointField(0,3), true);
            
            return new ObjectField("Line", objectSet, new Vector2Int(1, 4), new Vector2Int(0, 1));
        }
        
        public static ObjectField Cube()
        {
            var objectSet = new Dictionary<PointField, bool>();
            objectSet.Add(new PointField(0,0), true);
            objectSet.Add(new PointField(0,1), true);
            objectSet.Add(new PointField(1,0), true);
            objectSet.Add(new PointField(1,1), true);
            
            return new ObjectField("Cube", objectSet, new Vector2Int(2, 2), new Vector2Int(1, 1), false);
        }

        public static ObjectField Tringle()
        {
            var objectSet = new Dictionary<PointField, bool>();
            objectSet.Add(new PointField(0,0), true);
            objectSet.Add(new PointField(1,0), true);
            objectSet.Add(new PointField(2,0), true);
            objectSet.Add(new PointField(1,1), true);
            
            return new ObjectField("Tringle", objectSet, new Vector2Int(3, 2), new Vector2Int(1, 0));
        }

        private static ObjectField[] objectList = new ObjectField[7] {LightningLeftRight(), LightningRightLeft(), HookToLeft(), HookToRight(), Line(), Cube(), Tringle()};

        public static ObjectField GetRandomObject()
        {
            return objectList[Random.Range(0, objectList.Length)];
        }
        
        public ObjectField(string name, Dictionary<PointField, bool> objectSet, Vector2Int objectSize, Vector2Int center, bool rotate = true)
        {
            SetObject(name, objectSet, objectSize);
            SetTransformationCenter(center, rotate);
        }
        
        public Vector2Int Size => size;

        public Vector2Int CenterTransformation => centerTransformation;

        public bool CanRotate => canRotate;

        public void SetObject(string name, Dictionary<PointField, bool> objectSet, Vector2Int objectSize)
        {
            this.name = name;
            objectField = objectSet;
            size = objectSize;
        }

        public void SetTransformationCenter(Vector2Int center, bool rotate = true)
        {
            centerTransformation = center;
            canRotate = rotate;
        }

        private PointField GetLeftDownPointField()
        {
            return new PointField(0, 0);
        }
        
        public PointField GetLeftDownRelativePointField()
        {
            return GetLeftDownPointField() - new PointField(centerTransformation);
        }

        private PointField GetLeftUpPointField()
        {
            return new PointField(0, size.y);
        }
        
        public PointField GetLeftUpRelativePointField()
        {
            return GetLeftUpPointField() - new PointField(centerTransformation);
        }
        
        private PointField GetRightDownPointField()
        {
            return new PointField(size.x, 0);
        }
        
        public PointField GetRightDownRelativePointField()
        {
            return GetRightDownPointField() - new PointField(centerTransformation);
        }
        
        private PointField GetRightUpPointField()
        {
            return new PointField(size.x, size.y);
        }
        
        public PointField GetRightUpRelativePointField()
        {
            return GetRightUpPointField() - new PointField(centerTransformation);
        }

        public IEnumerable<PointField> GetPoints()
        {
            return objectField.Select(el => el.Key).ToList();
        }

        public IEnumerable<PointField> GetRelativePoints()
        {
            return objectField.Select(el => el.Key - new PointField(centerTransformation)).ToList();
        }

        private IEnumerable<PointField> GetPointsWithX(int xCoordinate)
        {
            return objectField.Where(el => el.Key.x == xCoordinate).Select(el => el.Key).OrderBy(el => el.y).ToList();
        }
        
        private IEnumerable<PointField> GetPointsWithY(int yCoordinate)
        {
            return objectField.Where(el => el.Key.y == yCoordinate).Select(el => el.Key).OrderBy(el => el.x).ToList();
        }
        
        private IEnumerable<PointField> GetLeftPoints()
        {
            List<PointField> result = new List<PointField>();
            
            for (int y = 0; y < size.y; y++)
            {
                var resLocal = GetPointsWithY(y);
                
                result.Add(resLocal.First());
            }

            return result;
        }

        public IEnumerable<PointField> GetLeftRelativePoints()
        {
            return GetLeftPoints().Select(el => el - new PointField(centerTransformation)).ToList();
        }
        
        private IEnumerable<PointField> GetRightPoints()
        {
            List<PointField> result = new List<PointField>();
            
            for (int y = 0; y < size.y; y++)
            {
                var resLocal = GetPointsWithY(y);
                
                result.Add(resLocal.Last());
            }

            return result;
        }
        
        public IEnumerable<PointField> GetRightRelativePoints()
        {
            return GetRightPoints().Select(el => el - new PointField(centerTransformation)).ToList();
        }
        
        private IEnumerable<PointField> GetDownPoints()
        {
            List<PointField> result = new List<PointField>();
            
            for (int x = 0; x < size.x; x++)
            {
                var resLocal = GetPointsWithX(x);
                
                result.Add(resLocal.First());
            }

            return result;
        }
        
        public IEnumerable<PointField> GetDownRelativePoints()
        {
            return GetDownPoints().Select(el => el - new PointField(centerTransformation)).ToList();
        }
        
        private IEnumerable<PointField> GetUpPoints()
        {
            List<PointField> result = new List<PointField>();
            
            for (int x = 0; x < size.x; x++)
            {
                var resLocal = GetPointsWithX(x);
                
                result.Add(resLocal.Last());
            }

            return result;
        }
        
        public IEnumerable<PointField> GetUpRelativePoints()
        {
            return GetUpPoints().Select(el => el - new PointField(centerTransformation)).ToList();
        }
        
        public ObjectField RotateRight()
        {
            Vector2Int newCenterTransformation = new Vector2Int(centerTransformation.y, size.x - centerTransformation.x - 1);
            
            Dictionary<PointField, bool> rotateField = new Dictionary<PointField, bool>();

            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    var getKeyPos = new PointField(i, j);
                
                    if (objectField.ContainsKey(getKeyPos))
                    {
                        bool getPoint = objectField[getKeyPos];
                    
                        var newKeyPos = new PointField(j,size.x - i - 1);
                        rotateField[newKeyPos] = getPoint;
                    }
                }
            }

            return new ObjectField(name, rotateField, new Vector2Int(size.y, size.x), newCenterTransformation, canRotate);
        }
    }
}