using System.IO;
using UnityEngine;

public class DataManger : MonoBehaviour
{
    public static DataManger Instance;

    // Saved Values
    public string PlayerName;

    public string HighScoreName;
    public int HighScore;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadHighScore();
    }

    [System.Serializable]
    class SaveData
    {
        public string HighScoreName;
        public int HighScore;
    }

    public void SaveHighscore(int score)
    {
        SaveData data = new SaveData();
        data.HighScoreName = PlayerName;
        data.HighScore = score;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            HighScoreName = data.HighScoreName;
            HighScore = data.HighScore;
        }
    }
}
