using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnscaledTime : MonoBehaviour
{
    int propertyID;
    float startTime;

    void Awake()
    {
        propertyID = Shader.PropertyToID("UnscaledTime");

        //To reset time on scene change.
        SceneManager.sceneLoaded += OnSceneLoaded;
        startTime = Time.unscaledTime;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        startTime = Time.unscaledTime;
    }

    void Update()
    {
        Shader.SetGlobalFloat(propertyID, Time.unscaledTime - startTime);
    }
}
