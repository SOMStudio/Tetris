using System;
using System.Collections.Generic;
using SOMStudio.Tetris.Scripts.Base.Optimization;
using SOMStudio.Tetris.Scripts.Game.Field;
using UnityEngine;
using UnityEngine.Events;

namespace SOMStudio.Tetris.Scripts.Game
{
    public class PlayFieldManager : MonoBehaviour
    {
        [SerializeField]
        private Vector2Int size = new Vector2Int(10, 15);

        private readonly Dictionary<PointField, PointFieldManager> pointList = new();

        private readonly ObjectField[] nextDropObjectList = new ObjectField[3];

        private PlayField playField;

        private GameController gameController;

        private global::SOMStudio.Tetris.Scripts.Game.GameData.DevScripts.GameData gameDate;
        private PlayerManager playerManager;

        private ObjectPool objectPool;

        public event UnityAction<IEnumerable<ObjectField>> UpdateNextDropObjectListEvent;
        public event UnityAction LoseLifeEvent;

        private void Start()
        {
            gameController = GameController.Instance;

            gameDate = (global::SOMStudio.Tetris.Scripts.Game.GameData.DevScripts.GameData) gameController.GameData.Data;
            playerManager = LevelManager.Instance.PlayerManager;

            objectPool = ObjectPool.GetPoolByName("ObjectPoints");

            playField = new PlayField(size);
            
            playField.DestroyLineEvent += LineDestroy;
            playField.DropLineEvent += LineDrop;

            playField.CreateObjectEvent += ObjectCreate;
            playField.ChangeObjectEvent += ObjectUpdate;
            playField.FixObjectEvent += ObjectFix;

            playField.CantCreateObjectEvent += CannotCreateDropObject;
        }
        
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                ObjectMoveLeft();
            } else if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                ObjectMoveRight();
            } else if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                ObjectRotate();
            } else if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                ObjectMoveDown();
            }
        }

        public void Clear()
        {
            if (!playField.IsEmpty)
            {
                playField.Clear();

                foreach (var point in pointList)
                {
                    objectPool.LiberationObject(point.Value.transform);
                }

                pointList.Clear();
            }
        }

        public void MobileControl()
        {
            if (Math.Abs(playerManager.Horizontal) > 0.3f)
            {
                if (playerManager.Horizontal > 0)
                    ObjectMoveRight();
                else
                    ObjectMoveLeft();
            }

            if (Math.Abs(playerManager.Vertical) > 0.3f)
            {
                if (playerManager.Vertical > 0)
                    ObjectRotate();
                else
                    ObjectMoveDown();
            }
        }
        
        public void InitNextDropObjectList()
        {
            playField.SetRandomDropObject();
            
            for (int i = 0; i < nextDropObjectList.Length; i++)
            {
                nextDropObjectList[i] = playField.GetRandomDropObject();
            }
            
            UpdateNextDropObjectListEvent?.Invoke(nextDropObjectList);
        }
        
        private void SetNextDropObject()
        {
            playField.SetDropObject(nextDropObjectList[0]);

            for (int i = 1; i < nextDropObjectList.Length; i++)
            {
                nextDropObjectList[i - 1] = nextDropObjectList[i];
            }

            nextDropObjectList[^1] = playField.GetRandomDropObject();
            
            UpdateNextDropObjectListEvent?.Invoke(nextDropObjectList);
        }
        
        public void SetDestroyRayCountListener(UnityAction<int> action)
        {
            playField.DestroyLineCountEvent += action;
        }

        public void ObjectMoveLeft()
        {
            if (playField.IsDropObjectActive)
            {
                playField.MoveLeftDropObject();
            }
        }

        public void ObjectMoveRight()
        {
            if (playField.IsDropObjectActive)
            {
                playField.MoveRightDropObject();
            }
        }

        public void ObjectMoveDown()
        {
            if (playField.IsDropObjectActive)
            {
                playField.MoveDownDropObject();
            }
        }
        
        public void ObjectRotate()
        {
            if (playField.IsDropObjectActive)
            {
                playField.RotateDropObject();
            }
        }

        private void ObjectCreateRandom()
        {
            playField.SetRandomDropObject();
        }

        private Color GetRandomColor()
        {
            return  gameDate.RandomColor;
        }
        
        private void LineDestroy(IEnumerable<PointField> pos)
        {
            foreach (var point in pos)
            {
                if (pointList.ContainsKey(point))
                {
                    var pointFieldManager = pointList[point];
                    pointList.Remove(point);
                    
                    objectPool.LiberationObject(pointFieldManager.transform);
                }
            }
        }

        private void LineDrop(IEnumerable<PointField> pos)
        {
            foreach (var point in pos)
            {
                if (pointList.ContainsKey(point))
                {
                    var pointFieldManager = pointList[point];
                    pointList.Remove(point);

                    var newPoint = point - new PointField(0, 1);
                    pointList.Add(newPoint, pointFieldManager);
                    pointFieldManager.SetPosition(newPoint);
                }
            }
        }

        private void ObjectCreate(IEnumerable<PointField> points)
        {
            var color = GetRandomColor();
            
            foreach (var point in points)
            {
                var newGameObject = objectPool.GetObject(point.GetV3Pos()).gameObject;
                var pointFieldManager = newGameObject.GetComponent<PointFieldManager>();
                
                pointFieldManager.SetColor(color);
                
                pointList.Add(point, pointFieldManager);
            }
        }

        private void ObjectFix(IEnumerable<PointField> pos)
        {
            SetNextDropObject();
        }

        private void ObjectUpdate(IEnumerable<PointField> oldPoints, IEnumerable<PointField> newPoints)
        {
            List<PointFieldManager> pointsFieldManager = new List<PointFieldManager>();
            
            foreach (var point in oldPoints)
            {
                if (pointList.ContainsKey(point))
                {
                    pointsFieldManager.Add(pointList[point]);
                    
                    pointList.Remove(point);
                }
            }

            int i = 0;
            foreach (var point in newPoints)
            {
                pointsFieldManager[i].SetPosition(point);
                
                pointList.Add(point, pointsFieldManager[i]);

                i++;
            }
        }

        private void CannotCreateDropObject()
        {
            LoseLifeEvent?.Invoke();
        }
    }
}
