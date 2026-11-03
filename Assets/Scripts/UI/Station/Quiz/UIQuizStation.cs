using IsoBuilder.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace IsoBuilder.UI
{
    public class UIQuizStation : UIBaseStation
    {
        [SerializeField] private GameObject questionPanelObject;
        [SerializeField] private GameObject answerPanelObject;
        [SerializeField] private GameObject passedPanelObject;

        [Header("Question")]
        [SerializeField] private TextMeshProUGUI questionNumberText;
        [SerializeField] private TextMeshProUGUI questionText;

        [Header("Choices")]
        [SerializeField] private UIQuizChoice[] choices;

        [Header("Answers")]
        [SerializeField] private UIQuizAnswer answers;

        private QuizConfig quizConfig;
        private QuizData currentQuizData;
        private int currentShowQuizId = -1;

        public override void Initialize(GameContext gameContext, UnityAction close)
        {
            base.Initialize(gameContext, close);

            quizConfig = gameContext.Config.QuizConfig;
            for (int i = 0; i < choices.Length; i++)
            {
                choices[i].Initialize(OnSelectChoice, i);
            }
            answers.Initialize(() =>
            {
                answerPanelObject.SetActive(false);
                ShowQuiz();
            });
        }

        public override void Show()
        {
            if (quizConfig != null)
            {
                ShowQuiz();
            }
            else
            {
                Debug.LogError("Quiz config not found.");
            }
            base.Show();
        }

        private void ShowQuiz()
        {
            if (quizConfig.QuizDatas.Length <= GameContext.SaveDataStore.SaveData.QuizProgress)
            {
                questionPanelObject.SetActive(false);
                passedPanelObject.SetActive(true);
                return;
            }
            currentQuizData = quizConfig.QuizDatas[GameContext.SaveDataStore.SaveData.QuizProgress];
            if (currentShowQuizId == currentQuizData.id)
            {
                return;
            }
            questionNumberText.text = $"ข้อที่ {GameContext.SaveDataStore.SaveData.QuizProgress + 1}/{quizConfig.QuizDatas.Length}";
            questionText.text = currentQuizData.question;
            for (int i = 0; i < choices.Length; i++)
            {
                choices[i].SetChoiceText(currentQuizData.choices[i]);
            }
            currentShowQuizId = currentQuizData.id;
            questionPanelObject.SetActive(true);
        }

        private void OnSelectChoice(int index)
        {
            if (answerPanelObject.activeSelf)
                return;
            string messageText;
            string buttonText;
            if (index == currentQuizData.correctAnswerIndex)
            {
                GameContext.SaveDataStore.SaveData.QuizProgress++;
                GameContext.SaveDataStore.Save();
                messageText = currentQuizData.answerExplanation;
                buttonText = quizConfig.nextQuizButtonMessage;
            }
            else
            {
                messageText = quizConfig.FailedMessage;
                buttonText = quizConfig.retryQuizButtonMessage;
            }
            answers.SetAnswerText(messageText, buttonText);
            answerPanelObject.SetActive(true);
        }
    }
}
