using UnityEngine;

namespace IsoBuilder.Core
{
    [CreateAssetMenu(fileName = "StationConfig", menuName = "Scriptable Objects/StationConfig")]
    public class StationConfig : ScriptableObject
    {
        public StationData[] StationDatas;
    }
}
