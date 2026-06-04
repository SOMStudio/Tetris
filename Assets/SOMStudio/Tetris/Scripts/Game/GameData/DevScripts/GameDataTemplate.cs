using Base.Data.GameData.DevScripts;
using UnityEngine;

namespace Tetris.GameData.DevScripts
{
    [CreateAssetMenu(fileName = "New GameDataTemplate", menuName = "SOMStudio/Tetris/Create Game Data Template")]
    public class GameDataTemplate : BaseDataTemplate
    {
        [SerializeField] private GameData data;

        public override BaseData Data => data;
    }
}
