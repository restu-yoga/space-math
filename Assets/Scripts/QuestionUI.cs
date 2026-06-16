using UnityEngine;
using TMPro;

public class QuestionUI : MonoBehaviour
{
    public QuestionGenerator generator; // assign di inspector
    public TMP_Text questionText;       // assign QuestionText TMP UI

    void Start()
    {
        if (generator == null)
            generator = FindObjectOfType<QuestionGenerator>();

        // initial display
        RefreshUI();
    }

    public void RefreshUI()
    {
        if (generator != null && questionText != null)
        {
            questionText.text = generator.currentQuestion;
        }
    }

    // helper to request a new question then refresh UI
    public void NewQuestionAndRefresh()
    {
        if (generator != null)
        {
            generator.NewQuestion();
            RefreshUI();
        }
    }
}
