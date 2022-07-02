using System;
using Base;
using Base.Data.GameData.DevScripts;
using Base.Data.GameLogic.DevScripts;
using Base.Data.LevelListData.DevScripts;
using Game;
using UnityEngine;

namespace Tetris.GameLogic.DevScripts
{
    [Serializable]
    public class GameLogicScript : BaseLogicScript
    {
        private Tetris.GameData.DevScripts.GameData gameData;
        private Tetris.LevelManager levelManager;

        private BaseGameController gameController;
        
        private int taskScore;

        public override void InitGameRules(BaseGameController gameControllerSet, BaseDataTemplate gameDataSet)
        {
            this.gameController = gameControllerSet;
            
            gameData = (Tetris.GameData.DevScripts.GameData)gameDataSet.Data;
        }
        
        public override void InitLevelRules(BaseLevelManager levelManagerSet, BaseLevelData levelDataNew = null)
        {
            levelManager = (LevelManager)levelManagerSet;

            UserManager.Instance.SetHealth(1, true);
            UserManager.Instance.SetScore(0, true);
            UserManager.Instance.SetWave(1, true);
        }

        public override void CheckLocalTask(GameObject obj, EventArgs args = null)
        {
            EvenArgsReward argsReward = (EvenArgsReward) args;
            int bonusRes = CalculateBonusCount(argsReward.countLine);

            UserManager.Instance.AddScore(bonusRes);

            gameController.CheckLocalTask(bonusRes);
        }

        private int CalculateBonusCount(int countLineReduce)
        {
            return (countLineReduce + Mathf.FloorToInt(countLineReduce / 2)) * gameData.BonusForItem; ;
        }

        public override void CheckGlobalTask()
        {
            
        }
    }
}
