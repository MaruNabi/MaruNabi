using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class LoadingScene : MonoBehaviour
{
    public static string nextScene;
    Dictionary<int, LoadingSceneData> loadingDataDict = new Dictionary<int, LoadingSceneData>();
    private int textRandomCount;

    [SerializeField] TMP_Text loadingText;
    [SerializeField] TMP_Text loadingPhrases;

    void Start()
    {
        Managers.Pool.Clear();

        Managers.Sound.PlayBGM("LoadingScene");

        StartCoroutine(WaitForDataLoading());

        StartCoroutine(LoadSceneProcess());
    }

    private IEnumerator WaitForDataLoading()
    {
        yield return new WaitUntil(() => Managers.Data.loadingDict != null);
        loadingDataDict = Managers.Data.loadingDict;
        textRandomCount = UnityEngine.Random.Range(1, loadingDataDict.Count + 1);
        loadingDataDict.TryGetValue(textRandomCount, out LoadingSceneData loadingSceneData);
        loadingText.text = loadingSceneData.DIALOGUE;
    }

    /*protected override void Init()
    {
        base.Init();

        SceneType = ESceneType.LoadingScene;
    }*/

    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0f;
        float fakeLoadingTime = 0f;

        loadingPhrases.DOFade(0.0f, 0.5f).SetLoops(-1, LoopType.Yoyo);

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

    /*public override void Clear()
    {
        nextScene = "";
        textRandomCount = 0;
    }*/
}
