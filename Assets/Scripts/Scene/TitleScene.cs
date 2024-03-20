using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
    }

    public void OnClickGameStart()
    {
        LoadingScene.nextScene = "TestScene";

        Managers.Scene.LoadScene(ESceneType.LoadingScene);
    }

    public override void Clear()
    {
        
    }
}
