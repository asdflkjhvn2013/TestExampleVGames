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
    public int level;
    public int numOfSetsSpawn;
    public int[] typesChessPieces;
}

[Serializable]
public class ChapterData
{
    public LevelData[] chapter;
}