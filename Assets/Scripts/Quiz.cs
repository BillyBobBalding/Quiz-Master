using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] QuestionSO questionSO;

    [Header("Answers")]
    [SerializeField] GameObject[] answerButtons;
    int correctAnswerIndex;

    [Header("Button Colors")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;

    [Header("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;

    void Start()
    {
        timer = FindObjectOfType<Timer>();
        GetNextQuestion();
        //PopulateQuestionAndAnswers();
    }

    void Update()
    {
        timerImage.fillAmount = timer.fillFraction;
    }

    public void OnAnswerSelected(int index)
    {
        Image buttonImg;

        if (index == questionSO.GetCorrectAnswerIndex())
        {
            questionText.text = "Correct!";
            buttonImg = answerButtons[index].GetComponent<Image>();
            buttonImg.sprite = correctAnswerSprite;
        }
        else
        {
            correctAnswerIndex = questionSO.GetCorrectAnswerIndex();
            string correctAnswer = questionSO.GetAnswer(correctAnswerIndex);
            questionText.text = "Sorry, the correct answer was:\n" + correctAnswer;
            //TextMeshProUGUI buttonText = answerButtons[correctAnswerIndex].GetComponentInChildren<TextMeshProUGUI>();
            //questionText.text = buttonText.text;

            buttonImg = answerButtons[correctAnswerIndex].GetComponent<Image>();
            buttonImg.sprite = correctAnswerSprite;
        }

        SetButtonState(false);
    }

    void GetNextQuestion()
    {
        SetButtonState(true);
        SetDefaultButtonSprites();
        PopulateQuestionAndAnswers();
    }
    
    void PopulateQuestionAndAnswers()
    {
        questionText.text = questionSO.GetQuestion();

        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = questionSO.GetAnswer(i);
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
