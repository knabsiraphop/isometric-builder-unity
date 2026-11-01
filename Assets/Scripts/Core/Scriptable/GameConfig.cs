using UnityEngine;

namespace IsoBuilder.Core
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Scriptable Objects/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [SerializeField] private GridSystemConfig gridSystemConfig;
        [SerializeField] private ItemConfig itemConfig;
        [SerializeField] private QuizConfig quizConfig;
        [SerializeField] private StationConfig stationConfig;

        public GridSystemConfig GridSystemConfig => gridSystemConfig;
        public ItemConfig ItemConfig => itemConfig;
        public QuizConfig QuizConfig => quizConfig;
        public StationConfig StationConfig => stationConfig;
    }
}
