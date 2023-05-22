using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quiz Question", fileName = "New Question")]
public class QuestionSO : ScriptableObject
{
    [TextArea(2,6)]
    [SerializeField] string question = "Enter new question here";
    [SerializeField] string[] answers = new string[4];
    [SerializeField] int correctAns;
    public string GetQuestion()
    {
        return question;
    }
    public string GetAns(int idx)
    {
        return answers[idx];
    }
    public int GetCorrectAns()
    {
        return correctAns;
    }
}
