
#region Manager
public enum ESceneType
{
    TitleScene,
    TutorialScene, 
    GameScene,
    LoadingScene,
    TestScene,
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

enum EMonsterName
{
    Mouse = 0,      // ��
    Scholar = 1,  // ����
    Empty = 2     // ��ĭ
}
#endregion