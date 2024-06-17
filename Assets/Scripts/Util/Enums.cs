
#region Manager
public enum ESceneType
{
    TitleScene,
    TutorialScene, 
    GameScene,
    LoadingScene,
    TestScene,
    StageSelectionScene,
    StagePrepareScene,
    Stage2,
}
#endregion

#region Monster
public enum EMonsterType
{
    Normal,
    Elite,
    Boss
}

public enum EMovementType
{
    Ground,
    Flying
}

public enum EScholarAnimationType
{
    Idle,
    Move,
    Attack,
    Hit,
    Laugh,
    Die,
    Angry
}

public enum EMouseAnimationType
{
    Caught,
    Run,
    Rush,
    Tail,
    Crying,
    Dead,
    NoRush,
    Clear,
    StopCrying
}

public enum EFoxAnimationType
{
    Idle,
    Laugh,
    Die,
    Attack
}

public enum EMonsterName
{
    Mouse = 0,      // Áã
    Scholar = 1,  // ¼±ºñ
    Empty = 2     // ºóÄ­
}

public enum EMousePattern
{
    Rush,
    SpawnRats,
    Tail,
    Rock
}

public enum ETigerBitePosition
{
    Left,
    Middle,
    Right
}
#endregion