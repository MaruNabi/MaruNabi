using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTestManager : MonoBehaviour
{
    [SerializeField]
    private DialogSystem dialogSystem;

    [SerializeField]
    private SceneController sceneController;

    [SerializeField]
    private GameObject CutSceneGo;

    private bool isFirstCut = false;

    private void Awake()
    {
        dialogSystem.onDialogEnd.AddListener(this.LoadRunScene);
    }

    private void OnDestroy()
    {
        dialogSystem.onDialogEnd.RemoveListener(this.LoadRunScene);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            dialogSystem.DialogNextButtonClicked();
        }
    }

    public IEnumerator CutScene()
    {
        if(isFirstCut == false)
        {
            CutSceneGo.SetActive(true);
            yield return new WaitForSeconds(5f);
            CutSceneGo.SetActive(false);

            isFirstCut = true;
        }
    }

    private void LoadRunScene()
    {
        if(isFirstCut == true)
        {
            sceneController.LoadTestScene();
        }
    }
}
