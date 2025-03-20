using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.ShaderKeywordFilter;

#if UNITY_EDITOR
using UnityEditor;
#endif

[DefaultExecutionOrder(1000)]
public class MenuHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] TextMeshProUGUI nameField;
    [SerializeField] TextMeshProUGUI HighscoreText;
    private void Awake()
    {
        DataManger.Instance.LoadHighScore();
        if (DataManger.Instance.HighScoreName != null && DataManger.Instance.HighScoreName != "")
        {
            HighscoreText.text = $"Current Highscore: {DataManger.Instance.HighScoreName} with {DataManger.Instance.HighScore} Points";

        }
        else 
        {
            HighscoreText.text = "No highscore yet";
        }
    }

    public void btn_StartGame_Click() 
    {
        if (nameField.text != "")
        {
            DataManger.Instance.PlayerName = nameField.text;
        }
        else 
        {
            DataManger.Instance.PlayerName = "Player";
        }
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        //DataManger.Instance.SaveColor();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else   
        Application.Quit(); // original code to quit Unity player
#endif
    }
}
