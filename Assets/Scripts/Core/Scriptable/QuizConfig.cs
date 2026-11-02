using UnityEngine;

namespace IsoBuilder.Core
{
    [CreateAssetMenu(fileName = "QuizConfig", menuName = "Scriptable Objects/QuizConfig")]
    public class QuizConfig : ScriptableObject
    {
        public int QuizAmount;
        public QuizData[] QuizDatas;
    }
}
