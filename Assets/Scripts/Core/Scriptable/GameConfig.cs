using UnityEngine;

namespace IsoBuilder.Core
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Scriptable Objects/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [field: SerializeField] public GridSystemConfig GridSystemConfig { get; private set; }
        [field: SerializeField] public PlaceableConfig PlaceableConfig { get; private set; }
        [field: SerializeField] public QuizConfig QuizConfig { get; private set; }
        [field: SerializeField] public StationConfig StationConfig { get; private set; }
        [field: SerializeField] public MixerConfig MixerConfig { get; private set; }
        [field: SerializeField] public InventoryItemMasterData InventoryItemMasterData { get; private set; }
        [field: SerializeField] public StartingData StartingData { get; private set; }
    }
}
