using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] List<QuestionSO> questionsList = new List<QuestionSO>();
    QuestionSO currentQuestionSO;
    
    [Header("Answers")]
    [SerializeField] GameObject[] answerButtons;
    int correctAnswerIndex;
    public bool hasAnsweredInTime = true;

    [Header("Button Colors")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;

    [Header("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;

    [Header("Scoring")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scoreKeeper;

    [Header("ProgressBar")]
    //[SerializeField] Slider progressBar;
    public Slider progressBar;

    public bool isGameOver;

    void Awake()
    {
        timer = FindObjectOfType<Timer>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        progressBar.maxValue = questionsList.Count;
        progressBar.value = 0;
    }

    void Update()
    {
        timerImage.fillAmount = timer.fillFraction;

        if (!timer.isAnsweringQuestion && !hasAnsweredInTime)
        {
            DisplayAnswer(-1);
            SetButtonState(false);
        }
    }

    public void OnAnswerSelected(int index)
    {
        DisplayAnswer(index);

        SetButtonState(false);
        timer.CancelTimer();
        hasAnsweredInTime = true;

        //if (progressBar.value == progressBar.maxValue)
        //{
        //    isGameOver = true;
        //}
    }

    void DisplayAnswer(int index)
    {
        Image buttonImg;

        if (index == currentQuestionSO.GetCorrectAnswerIndex())
        {
            questionText.text = "Correct!";
            buttonImg = answerButtons[index].GetComponent<Image>();
            buttonImg.sprite = correctAnswerSprite;
            scoreKeeper.IncrementCorrectAnswers();
        }
        else
        {
            correctAnswerIndex = currentQuestionSO.GetCorrectAnswerIndex();
            string correctAnswer = currentQuestionSO.GetAnswer(correctAnswerIndex);
            questionText.text = "Sorry, the correct answer was:\n" + correctAnswer;
            //TextMeshProUGUI buttonText = answerButtons[correctAnswerIndex].GetComponentInChildren<TextMeshProUGUI>();
            //questionText.text = buttonText.text;

            buttonImg = answerButtons[correctAnswerIndex].GetComponent<Image>();
            buttonImg.sprite = correctAnswerSprite;
        }

        scoreText.text = "Score: " + scoreKeeper.CalculateScore() + "%";
    }

    public void GetNextQuestion()
    {
        if (questionsList.Count > 0)
        {
            SetButtonState(true);
            SetDefaultButtonSprites();
            GetRandomQuestion();
            PopulateQuestionAndAnswers();
            hasAnsweredInTime = false;
            scoreKeeper.IncrementQuestionsSeen();
            progressBar.value++;
        }
    }

    void GetRandomQuestion()
    {
        int index = Random.Range(0, questionsList.Count);
        currentQuestionSO = questionsList[index];
        
        if (questionsList.Contains(currentQuestionSO))
            questionsList.Remove(currentQuestionSO);
    }

    void PopulateQuestionAndAnswers()
    {
        questionText.text = currentQuestionSO.GetQuestion();

        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = currentQuestionSO.GetAnswer(i);
        }
    }

    void SetButtonState(bool state)
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            Button button = answerButtons[i].GetComponent<Button>();
            button.interactable = state;
        }
    }

    void SetDefaultButtonSprites()
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            Image buttonImg = answerButtons[i].GetComponent<Image>();
            buttonImg.sprite = defaultAnswerSprite;
        }
    }
}
