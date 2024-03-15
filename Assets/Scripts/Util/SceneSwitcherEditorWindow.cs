// Author: rpopic2 (github.com/rpopic2/unity-snippets)
// Last Modified: 2023-02-13
// Description: A simple scene switcher window for Unity Editor
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

public class SceneSwitcherEditorWindow : EditorWindow
{
    private Dictionary<string, string> scenes = new Dictionary<string, string>();
    [MenuItem("Tools/SceneSwitcher")]
    private static void Init()
    {
        EditorWindow.GetWindow(typeof(SceneSwitcherEditorWindow), false, "Scene Switcher", true);
    }
    private void OnEnable()
    {
        Refresh();
    }
    private void OnGUI()
    {
        foreach (var (name, path) in scenes)
        {
            if (GUILayout.Button(name, GUILayout.Height(30)))
            {
                EditorSceneManager.OpenScene(path);
            }
        }
        if (GUILayout.Button("Refresh", GUILayout.Width(100)))
        {
            Refresh();
        }
    }
    private void Refresh()
    {
        scenes.Clear();
        foreach (var s in EditorBuildSettings.scenes)
        {
            var path = s.path;
            var slashLastIndex = path.LastIndexOf('/');
            var sceneName = path.Substring(slashLastIndex + 1, path.LastIndexOf('.') - slashLastIndex - 1);
            scenes.Add(sceneName, path);
        }
    }
}
