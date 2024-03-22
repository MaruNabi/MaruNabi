using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UnscaledTime))]
public class UnscaledTimeEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.richText = true;
        EditorGUILayout.LabelField("Have a <b>single</b> instance of this script active at all times.", style);
        EditorGUILayout.LabelField("Or the <b>Unscaled Time</b> shader option will not work.", style);
    }

}
