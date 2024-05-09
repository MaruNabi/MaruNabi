
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

public enum EAnimationType
{
    Idle,
    Move,
    Attack,
    Hit,
    Laugh,
    Die,
    Angry
}

public enum EMonsterName
{
    Mouse = 0,      // ��
    Scholar = 1,  // ����
    Empty = 2     // ��ĭ
}

public enum EMousePattern
{
    HeadButt,
    SpawnRats,
    Tail,
    Rock
}
#endregion