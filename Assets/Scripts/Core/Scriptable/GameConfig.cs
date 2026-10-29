using UnityEngine;

namespace IsoBuilder.Core
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Scriptable Objects/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [SerializeField] private GridSystemConfig gridSystemConfig;
        [SerializeField] private ItemConfig itemConfig;

        public GridSystemConfig GridSystemConfig => gridSystemConfig;
        public ItemConfig ItemConfig => itemConfig;
    }
}
