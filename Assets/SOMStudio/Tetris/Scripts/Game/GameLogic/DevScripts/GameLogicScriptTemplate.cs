using Base.Data.GameLogic.DevScripts;
using UnityEngine;

namespace Tetris.GameLogic.DevScripts
{
    [CreateAssetMenu(fileName = "New GameLogicDataTemplate", menuName = "SOMStudio/Tetris/Create Game logic Data Template")]
    public class GameLogicScriptTemplate : BaseLogicScriptTemplate
    {
        [SerializeField] private GameLogicScript script;

        public override BaseLogicScript Script => script;
    }
}
