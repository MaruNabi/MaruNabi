using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class LoadingScene : BaseScene
{
    public static string nextScene;
    Dictionary<string, LoadingSceneData> loadingDataDict = new Dictionary<string, LoadingSceneData>();

    [SerializeField]
    TMP_Text loadingText;

    void Start()
    {
        StartCoroutine(WaitForDataLoading());

        StartCoroutine(LoadSceneProcess());
    }

    private IEnumerator WaitForDataLoading()
    {
        yield return new WaitUntil(() => Managers.Data.loadingDict != null);
        loadingDataDict = Managers.Data.loadingDict;
        loadingDataDict.TryGetValue("Dialogue_001", out LoadingSceneData loadingSceneData);
        Debug.Log(loadingSceneData.DIALOGUE);
    }

    protected override void Init()
    {
        base.Init();

        SceneType = ESceneType.LoadingScene;
    }

    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0f;
        float fakeLoadingTime = 0f;

        loadingText.DOFade(0.0f, 0.5f).SetLoops(-1, LoopType.Yoyo);

        while(!op.isDone)
        {
            yield return null;

            if (op.progress < 0.9f)
            {
                fakeLoadingTime = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                fakeLoadingTime = Mathf.Lerp(0.9f, 1f, timer);
                if(fakeLoadingTime >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    public override void Clear()
    {
        nextScene = "";
    }
}
