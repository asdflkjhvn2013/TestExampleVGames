using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    public int CurrentLevel;
    public int CurrentGold;
    public int HighScore;
    public bool IsSoundOn;
}

[Serializable]
public class LevelData
{
    public int Level;
    public int NumOfMatch;
    public List<int> TypeOfBlock;
} 
