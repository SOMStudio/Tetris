using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Tetris
{
    public class PlayField
    {
        private Vector2Int size;
        private Vector2Int spawnPoint;
        
        private Dictionary<PointField, bool> playField = new Dictionary<PointField, bool>();
        
        public ObjectField objectField;
        private Vector2Int positionObjectField;

        public event UnityAction<IEnumerable<PointField>> DestroyLineEvent;
        public event UnityAction<IEnumerable<PointField>> DropLineEvent;
        public event UnityAction<int> DestroyLineCountEvent;
        
        public event UnityAction<IEnumerable<PointField>> CreateObjectEvent;
        public event UnityAction<IEnumerable<PointField>, IEnumerable<PointField>> ChangeObjectEvent;
        public event UnityAction<IEnumerable<PointField>> FixObjectEvent;

        public event UnityAction CantCreateObjectEvent;
        
        public PlayField(Vector2Int newSize)
        {
            size = newSize;
            spawnPoint = new Vector2Int(newSize.x/2, newSize.y);
        }

        public bool IsEmpty => objectField == null && playField.Count == 0;
        
        public void Clear()
        {
            objectField = null;
            playField.Clear();
        }

        public void SetPoint(PointField point)
        {
            playField.Add(point, true);
        }

        private bool HasPoint(PointField point)
        {
            if (playField.ContainsKey(point))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerable<PointField> GetPoints()
        {
            return playField.Select(el => el.Key).ToArray();
        }
        
        private IEnumerable<PointField> GetPointsWithX(int xCoordinate)
        {
            return playField.Where(el => el.Key.x == xCoordinate).Select(el => el.Key).OrderBy(el => el.y).ToArray();
        }
        
        private IEnumerable<PointField> GetPointsWithY(int yCoordinate)
        {
            return playField.Where(el => el.Key.y == yCoordinate).Select(el => el.Key).OrderBy(el => el.x).ToArray();
        }

        public ObjectField GetRandomDropObject()
        {
            //random object
            var newDropObject = ObjectField.GetRandomObject();

            //rotate random
            int rotateCount = Random.Range(0, 4);
            for (int i = 0; i < rotateCount; i++)
            {
                newDropObject = newDropObject.RotateRight();
            }
            
            return newDropObject;
        }
        
        public void SetRandomDropObject()
        {
            SetDropObject(ObjectField.GetRandomObject());
        }
        
        public void SetDropObject(ObjectField newObject)
        {
            var newObjectPoints = newObject.GetRelativePoints()
                .Select(el => el + new PointField(spawnPoint - new Vector2Int(0, newObject.Size.y / 2))).ToArray();

            var occupiedPoints = newObjectPoints.Where(HasPoint).ToArray();

            if (occupiedPoints.Length > 0)
            {
                CantCreateObjectEvent?.Invoke();
                return;
            }
            
            objectField = newObject;
            
            positionObjectField = spawnPoint - new Vector2Int(0, newObject.Size.y/2);

            CreateObjectEvent?.Invoke(GetDropObjectRelativePoints());
        }

        public bool IsDropObjectActive => objectField != null;

        private PointField GetDropObjectRelativePoint(PointField point)
        {
            return new PointField(positionObjectField) + point;
        }

        public IEnumerable<PointField> GetDropObjectRelativePoints()
        {
            return objectField.GetRelativePoints().Select(el => new PointField(positionObjectField) + el).ToArray();
        }

        private bool CanDropObjectMoveLeft(int step = 1)
        {
            if (GetDropObjectRelativePoint(objectField.GetLeftDownRelativePointField()).x > (spawnPoint.x - size.x / 2))
            {
                var leftRelativePoints = objectField.GetLeftRelativePoints().Select(el => new PointField(positionObjectField) + el);

                var occupiedPoints = leftRelativePoints.Any(el => HasPoint(el - new PointField(step, 0)));
                if (occupiedPoints)
                    return false;
                    
                return true;
            }

            return false;
        }

        public void MoveLeftDropObject(int step = 1)
        {
            if (IsDropObjectActive)
            {
                if (CanDropObjectMoveLeft())
                {
                    var oldPoints = GetDropObjectRelativePoints();

                    positionObjectField -= new Vector2Int(step, 0);

                    var newPoints = GetDropObjectRelativePoints();

                    ChangeObjectEvent?.Invoke(oldPoints, newPoints);
                }
            }
        }

        private bool CanDropObjectMoveRight(int step = 1)
        {
            if (GetDropObjectRelativePoint(objectField.GetRightDownRelativePointField()).x <
                (spawnPoint.x + size.x / 2))
            {
                var rightRelativePoints = objectField.GetRightRelativePoints().Select(el => new PointField(positionObjectField) + el);

                var occupiedPoints = rightRelativePoints.Any(el => HasPoint(el + new PointField(step, 0)));
                if (occupiedPoints)
                    return false;
                
                return true;
            }

            return false;
        }

        public void MoveRightDropObject(int step = 1)
        {
            if (IsDropObjectActive)
            {
                if (CanDropObjectMoveRight())
                {
                    var oldPoints = GetDropObjectRelativePoints();

                    positionObjectField += new Vector2Int(step, 0);

                    var newPoints = GetDropObjectRelativePoints();

                    ChangeObjectEvent?.Invoke(oldPoints, newPoints);
                }
            }
        }

        private bool CanDropObjectMoveDown(int step = 1)
        {
            if (GetDropObjectRelativePoint(objectField.GetLeftDownRelativePointField()).y > 0)
            {
                var downRelativePoints = objectField.GetDownRelativePoints().Select(el => new PointField(positionObjectField) + el);

                var occupiedPoints = downRelativePoints.Any(el => HasPoint(el - new PointField(0, step)));
                if (occupiedPoints)
                    return false;
                
                return true;
            }

            return false;
        }

        private void FixDropObject(IEnumerable<PointField> points)
        {
            Array.ForEach(points.ToArray(), SetPoint);

            objectField = null;
                    
            FixObjectEvent?.Invoke(points);
        }

        private void CheckForDestroyLines(IEnumerable<PointField> points)
        {
            var pointsArray = points.ToArray().Reverse();
            List<int> destroyLine = new List<int>();
            
            foreach (var point in pointsArray)
            {
                var pointsWithY = GetPointsWithY(point.y);
                        
                if (pointsWithY.ToArray().Length == size.x)
                {
                    destroyLine.Add(point.y);

                    foreach (var pointY in pointsWithY)
                    {
                        playField.Remove(pointY);
                    }

                    DestroyLineEvent?.Invoke(pointsWithY);
                }
            }

            foreach (var lineNumber in destroyLine)
            {
                for (int y = lineNumber + 1; y < size.y; y++)
                {
                    var pointsWithY = GetPointsWithY(y);

                    if (pointsWithY.ToArray().Length > 0)
                    {
                        foreach (var pointY in pointsWithY)
                        {
                            playField.Remove(pointY);
                            playField.Add(pointY - new PointField(0, 1), true);
                        }

                        DropLineEvent?.Invoke(pointsWithY);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (destroyLine.Count > 0)
                DestroyLineCountEvent?.Invoke(destroyLine.Count);
        }
        
        public void MoveDownDropObject(int step = 1)
        {
            if (IsDropObjectActive)
            {
                if (CanDropObjectMoveDown())
                {
                    var oldPoints = GetDropObjectRelativePoints();

                    positionObjectField -= new Vector2Int(0, step);

                    var newPoints = GetDropObjectRelativePoints();

                    ChangeObjectEvent?.Invoke(oldPoints, newPoints);
                }
                else
                {
                    var points = GetDropObjectRelativePoints();
                    var leftPoints = objectField.GetLeftRelativePoints().Select(el => new PointField(positionObjectField) + el).ToArray();

                    FixDropObject(points);

                    CheckForDestroyLines(leftPoints);
                }
            }
        }

        private bool CanDropObjectRotate()
        {
            if (IsDropObjectActive)
            {
                if (!objectField.CanRotate) return false;

                bool moveRight = true;
                int moveStep = 0;
                var rotateObject = objectField.RotateRight();
                
                if (objectField.Size.y > objectField.Size.x)
                {
                    var centerObject = new PointField(positionObjectField);

                    var rightDownPointInRotateObject =
                        GetDropObjectRelativePoint(rotateObject.GetRightDownRelativePointField());
                    var pointsInFieldWithX = GetPointsWithY(rightDownPointInRotateObject.y).ToList();

                    if (pointsInFieldWithX.Count == 0)
                    {
                        if (centerObject.x + rotateObject.GetRightDownRelativePointField().x > size.x)
                        {
                            moveStep = (centerObject.x + rotateObject.GetRightDownRelativePointField().x) - size.x;
                            moveRight = false;
                        } else if (centerObject.x + rotateObject.GetLeftDownRelativePointField().x < 0)
                        {
                            moveStep = 0 - (centerObject.x + rotateObject.GetLeftDownRelativePointField().x);
                            moveRight = true;
                        }
                    }
                    else
                    {
                        pointsInFieldWithX.Insert(0, new PointField(-1, rightDownPointInRotateObject.y));
                        pointsInFieldWithX.Add(new PointField(size.x, rightDownPointInRotateObject.y));

                        var prevPoint = new PointField();
                        var nextPoint = new PointField();

                        var countCheck = pointsInFieldWithX.Count - 1;
                        for (int i = 0; i < countCheck; i++)
                        {
                            prevPoint = pointsInFieldWithX[i];
                            nextPoint = pointsInFieldWithX[i + 1];
                            
                            if (centerObject.x > prevPoint.x
                                && centerObject.x < nextPoint.x)
                            {
                                break;
                            }
                        }
                        
                        var distance = nextPoint.x - prevPoint.x - 1;

                        if (distance < rotateObject.Size.x)
                        {
                            return false;
                        }
                        else
                        {
                            if (centerObject.x + rotateObject.GetRightDownRelativePointField().x >= nextPoint.x)
                            {
                                moveStep = (centerObject.x + rotateObject.GetRightDownRelativePointField().x) - nextPoint.x;
                                moveRight = false;
                            } else if (centerObject.x + rotateObject.GetLeftDownRelativePointField().x <= prevPoint.x)
                            {
                                moveStep = prevPoint.x - (centerObject.x + rotateObject.GetLeftDownRelativePointField().x) + 1;
                                moveRight = true;
                            }
                        }
                    }
                }
                
                var moveStepPoint = new PointField(moveRight ? moveStep : -moveStep, 0);
                var rotateRelativePoints = rotateObject.GetRelativePoints()
                    .Select(el => new PointField(positionObjectField) + el + moveStepPoint).ToArray();

                var occupiedPoints = rotateRelativePoints.Where(HasPoint).ToArray();
                if (occupiedPoints.Length > 0)
                    return false;

                if (moveStep > 0)
                {
                    if (moveRight)
                        MoveRightDropObject(moveStep);
                    else
                        MoveLeftDropObject(moveStep);
                }

                return true;
            }

            return false;
        }

        public void RotateDropObject()
        {
            if (IsDropObjectActive)
            {
                if (CanDropObjectRotate())
                {
                    var oldPoints = GetDropObjectRelativePoints();

                    objectField = objectField.RotateRight();

                    var newPoints = GetDropObjectRelativePoints();

                    ChangeObjectEvent?.Invoke(oldPoints, newPoints);
                }
            }
        }
    }
}
