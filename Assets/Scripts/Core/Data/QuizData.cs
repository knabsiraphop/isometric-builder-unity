using System;

namespace IsoBuilder.Core
{
    [Serializable]
    public class QuizData
    {
        public int id;
        public string question;
        public string[] choices;
        public int correctAnswerIndex;
        public string answerExplanation;
    }
}
