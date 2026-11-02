using UnityEngine;

namespace IsoBuilder.Core
{
    [CreateAssetMenu(fileName = "QuizConfig", menuName = "Scriptable Objects/QuizConfig")]
    public class QuizConfig : ScriptableObject
    {
        public QuizData[] QuizDatas;
        public string FailedMessage;
        public string nextQuizButtonMessage;
        public string retryQuizButtonMessage;
    }
}
