using UnityEngine;
using System;

public class QuestionGenerator : MonoBehaviour
{
    public string currentQuestion { get; private set; }
    public int currentAnswer { get; private set; }

    [Header("Range Penjumlahan & Pengurangan")]
    public int minValue = 1;
    public int maxValue = 10;

    [Header("Range Perkalian & Pembagian Dasar")]
    public int minMultiplyDivide = 1;
    public int maxMultiplyDivide = 10;

    [Header("Operasi yang Diizinkan")]
    public bool allowSubtraction = true;
    public bool allowMultiplication = true;
    public bool allowDivision = true;

    [Header("Optional - assign in Inspector")]
    public QuestionUIController uiController;

    System.Random rnd = new System.Random();

    void Start()
    {
        if (uiController == null)
            uiController = FindObjectOfType<QuestionUIController>();

        GenerateQuestion();
    }

    public void NewQuestion()
    {
        GenerateQuestion();

        if (uiController != null)
            uiController.ShowQuestion(currentQuestion);
    }

    void GenerateQuestion()
    {
        int operation = GetRandomOperation();

        int a;
        int b;

        if (operation == 0)
        {
            // Penjumlahan 1-10
            a = rnd.Next(minValue, maxValue + 1);
            b = rnd.Next(minValue, maxValue + 1);

            currentAnswer = a + b;
            currentQuestion = $"{a} + {b} = ?";
        }
        else if (operation == 1)
        {
            // Pengurangan 1-10, dibuat agar tidak negatif
            a = rnd.Next(minValue, maxValue + 1);
            b = rnd.Next(minValue, maxValue + 1);

            if (a < b)
            {
                int tmp = a;
                a = b;
                b = tmp;
            }

            currentAnswer = a - b;
            currentQuestion = $"{a} - {b} = ?";
        }
        else if (operation == 2)
        {
            // Perkalian tabel 1-10
            a = rnd.Next(minMultiplyDivide, maxMultiplyDivide + 1);
            b = rnd.Next(minMultiplyDivide, maxMultiplyDivide + 1);

            currentAnswer = a * b;
            currentQuestion = $"{a} × {b} = ?";
        }
        else if (operation == 3)
        {
            // Pembagian tabel 1-10, hasil selalu bulat
            b = rnd.Next(minMultiplyDivide, maxMultiplyDivide + 1);
            currentAnswer = rnd.Next(minMultiplyDivide, maxMultiplyDivide + 1);
            a = b * currentAnswer;

            currentQuestion = $"{a} ÷ {b} = ?";
        }
    }

    int GetRandomOperation()
    {
        int operation;

        do
        {
            operation = rnd.Next(0, 4);
        }
        while (
            (operation == 1 && !allowSubtraction) ||
            (operation == 2 && !allowMultiplication) ||
            (operation == 3 && !allowDivision)
        );

        return operation;
    }

    public void HideQuestionUI()
    {
        if (uiController != null)
            uiController.HideQuestion();
    }
}