using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] List<QuestionSO> questionList = new List<QuestionSO>();
    QuestionSO question;

    [Header("Answer")]
    [SerializeField] GameObject[] answerButton;
    int correctAnsIdx;
    bool hasAnswerEarly = true;

    [Header("Button Colors")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;

    [Header("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;

    [Header("Scoring")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scoreKeeper; 

    [Header("Progressbar")]
    [SerializeField] Slider progressBar;

    public bool isComplete;

    void Awake()
    {
        timer = FindObjectOfType<Timer>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        progressBar.maxValue = questionList.Count;
        progressBar.value = 0;
    }
    void Update()
    {
        timerImage.fillAmount = timer.fillFraction;
        if(timer.loadNextQuestion)
        {
            if(progressBar.value == progressBar.maxValue)
            {
                isComplete = true;
                return;
            }
            hasAnswerEarly = false;
            GetNextQuestion();
            timer.loadNextQuestion = false;
        }
        else if(!hasAnswerEarly && !timer.isAnsweringQuestion)
        {
            DisplayAnswer(-1);
            SetButtonState(false);
        }
    }
    public void OnAnswerSelected(int idx)
    {
        hasAnswerEarly = true;
        DisplayAnswer(idx);
        SetButtonState(false);
        timer.CancelTimer();
        scoreText.text = "Score: " + scoreKeeper.CalculateScore() + "%";

        
    }
    void DisplayAnswer(int idx)
    {
        Image buttonImage;
        if(idx == question.GetCorrectAns())
        {
            questionText.text = "Correct";
            buttonImage = answerButton[idx].GetComponent<Image>();
            buttonImage.color = new Color(1/3,1,1/3,1);
            scoreKeeper.IncrementCorrectAnswers();
        }
        else
        {
            correctAnsIdx = question.GetCorrectAns();
            string correctAns = question.GetAns(correctAnsIdx);
            questionText.text = "Sorry, the correct answer was: \n" + correctAns; 
            buttonImage = answerButton[correctAnsIdx].GetComponent<Image>();
            buttonImage.color = new Color(1/3,1,1/3,1);
        }
    }
    void GetNextQuestion()
    {
        if(questionList.Count > 0)
        {
            SetButtonState(true);
            SetDefaultButtonSprites();
            GetRandomQuestion();
            DisplayQuestion();
            progressBar.value++;
            scoreKeeper.IncrementQuestionsSeen();
        }
    }
    void GetRandomQuestion()
    {
        int index = Random.Range(0, questionList.Count);
        question = questionList[index];
        if(questionList.Contains(question))
        {
            questionList.Remove(question);
        }
    }
    void DisplayQuestion()
    {
        questionText.text = question.GetQuestion();
        for(int i =0; i < answerButton.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButton[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = question.GetAns(i);
        }
    }
    void SetButtonState(bool state)
    {
        for(int i =0; i < answerButton.Length; i++)
        {
            Button button = answerButton[i].GetComponent<Button>();
            button.interactable = state;
        }
    }
    void SetDefaultButtonSprites()
    {
        for(int i = 0; i < answerButton.Length;i++)
        {
            Image buttonImage = answerButton[i].GetComponent<Image>();
            buttonImage.color = new Color(1,1,0.5f,1);
        }
    }
}
