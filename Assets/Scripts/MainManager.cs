using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text HighScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    private string m_playerName;
    private int m_HighScorePoints;
    private string m_HighScoreName;

    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Awake()
    {
        if (DataManger.Instance != null)
        {
            m_playerName = DataManger.Instance.PlayerName;
            if (DataManger.Instance.HighScoreName != null && DataManger.Instance.HighScoreName != "")
            {
                m_HighScoreName = DataManger.Instance.HighScoreName;
                m_HighScorePoints = DataManger.Instance.HighScore;
                HighScoreText.text = $"Current Highscore: {m_HighScoreName} with {m_HighScorePoints} Points";

            }
            else
            {
                HighScoreText.text = "No highscore yet";
                m_HighScorePoints = 0;
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score from {m_playerName}: {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);

        if (m_Points > m_HighScorePoints) 
        {
            m_HighScoreName = m_playerName;
            m_HighScorePoints = m_Points;
            DataManger.Instance.HighScoreName = m_playerName;
            DataManger.Instance.HighScore = m_Points;
            DataManger.Instance.SaveHighscore(m_Points);
        }
        HighScoreText.text = $"Current Highscore: {m_HighScoreName} with {m_HighScorePoints} Points"; ;
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else   
        Application.Quit(); // original code to quit Unity player
#endif
    }
}
