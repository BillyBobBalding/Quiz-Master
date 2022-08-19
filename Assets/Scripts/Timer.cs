using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] float timeToCompleteQuestion = 30f;
    [SerializeField] float timeToShowCorrectAnswer = 10f;

    public bool loadNextQuestion;
    public bool isAnsweringQuestion = false;
    public float fillFraction;
    float timerValue;
    Quiz quiz;

    private void Start()
    {
        quiz = FindObjectOfType<Quiz>();
    }

    void Update()
    {
        UpdateTimer();
    }

    void UpdateTimer()
    {
        timerValue -= Time.deltaTime;

        if (timerValue <= 0)
        {
            if (!isAnsweringQuestion)
            {
                //loadNextQuestion = true;
                isAnsweringQuestion = true;
                timerValue = timeToCompleteQuestion;
                quiz.GetNextQuestion();
            }
            else
            {
                isAnsweringQuestion = false;
                timerValue = timeToShowCorrectAnswer;
            }
        }
        else
        {
            if (isAnsweringQuestion)
            {
                fillFraction = timerValue / timeToCompleteQuestion;
            }
            else
            {
                fillFraction = timerValue / timeToShowCorrectAnswer;
            }

                
        }

        Debug.Log(isAnsweringQuestion + ": " + timerValue + " = " + fillFraction);
    }

    public void CancelTimer()
    {
        timerValue = 0;
    }
}
