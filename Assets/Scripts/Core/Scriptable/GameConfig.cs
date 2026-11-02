using UnityEngine;

namespace IsoBuilder.Core
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Scriptable Objects/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [SerializeField] private GridSystemConfig gridSystemConfig;
        [SerializeField] private PlaceableConfig placeableConfig;
        [SerializeField] private QuizConfig quizConfig;
        [SerializeField] private StationConfig stationConfig;

        public GridSystemConfig GridSystemConfig => gridSystemConfig;
        public PlaceableConfig PlaceableConfig => placeableConfig;
        public QuizConfig QuizConfig => quizConfig;
        public StationConfig StationConfig => stationConfig;
    }
}
