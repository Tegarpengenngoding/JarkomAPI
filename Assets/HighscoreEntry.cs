using System;

[System.Serializable]
public class HighscoreEntry
{
    public int id;
    public string playerName; // Pastikan ini 'playerName' (camelCase)
    public int score;
    public DateTime createdAt;
}