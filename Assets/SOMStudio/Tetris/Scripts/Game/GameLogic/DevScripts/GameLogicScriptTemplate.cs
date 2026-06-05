using SOMStudio.Tetris.Scripts.Base.Data.GameLogic.DevScripts;
using UnityEngine;

namespace SOMStudio.Tetris.Scripts.Game.GameLogic.DevScripts
{
    [CreateAssetMenu(fileName = "New GameLogicDataTemplate", menuName = "SOMStudio/Tetris/Create Game logic Data Template")]
    public class GameLogicScriptTemplate : BaseLogicScriptTemplate
    {
        [SerializeField] private GameLogicScript script;

        public override BaseLogicScript Script => script;
    }
}
