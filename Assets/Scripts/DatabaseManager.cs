using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public class DatabaseManager : MonoBehaviour
{
    private string filePath;
    private HighScores highScores;

    private void Start()
    {
        string fileName = "highScores.json";
        filePath = Path.Combine(Application.persistentDataPath, fileName);

        LoadHighScores();
    }

    public void SaveHighScore(int score)
    {
        highScores.Scores.Add(score);
        highScores.Scores = highScores.Scores.OrderByDescending(s => s).Take(3).ToList();

        string json = JsonConvert.SerializeObject(highScores);
        File.WriteAllText(filePath, json);
    }

    public List<int> LoadHighScores()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            highScores = JsonConvert.DeserializeObject<HighScores>(json);
        }
        else
        {
            highScores = new HighScores();
        }

        return highScores.Scores;
    }

    private void OnDestroy()
    {
        string json = JsonConvert.SerializeObject(highScores);
        File.WriteAllText(filePath, json);
    }
}

[System.Serializable]
public class HighScores
{
    public List<int> Scores = new List<int>();
}