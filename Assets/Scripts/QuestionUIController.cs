using UnityEngine;
using TMPro;

public class QuestionUIController : MonoBehaviour
{
    public TMP_Text questionText; // assign QuestionText (TMP UI) in Inspector

    void Start()
    {
        if (questionText != null)
            questionText.gameObject.SetActive(false); // default hidden
    }

    public void ShowQuestion(string q)
    {
        if (questionText == null) return;
        questionText.text = q;
        questionText.gameObject.SetActive(true);
    }

    public void HideQuestion()
    {
        if (questionText == null) return;
        questionText.gameObject.SetActive(false);
    }
}
