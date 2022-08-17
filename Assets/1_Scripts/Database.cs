using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;

[System.Serializable]
public class PlayerData
{
    public int highscore;
}

public class Database : MonoBehaviour
{
    public PlayerData playerData;
    public string filePath;
    public string fileName;
    private string path;
    public TMP_Text highscoreText;

    private void Awake()
    {
        path = Application.persistentDataPath + filePath + fileName;

        if (!File.Exists(path))
        {
            SaveGame();
        }
        LoadGame();
        Assigner();
    }

    public void SaveGame()
    {
        if (!File.Exists(path))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(Application.persistentDataPath + filePath + fileName));
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + filePath + fileName);

        bf.Serialize(file, playerData);
        file.Close();
    }

    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + filePath + fileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + filePath + fileName, FileMode.Open);
            playerData = (PlayerData)bf.Deserialize(file);
            file.Close();
        }
    }

    public void Assigner()
    {
        highscoreText.text = $"Highscore: {playerData.highscore}";
    }
}
