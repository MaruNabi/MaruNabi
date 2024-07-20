using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace UnityToolbarExtender.Examples
{
	static class ToolbarStyles
	{
		public static readonly GUIStyle commandButtonStyle;

		static ToolbarStyles()
		{
			commandButtonStyle = new GUIStyle("Command")
			{
				fontSize = 14,
				alignment = TextAnchor.MiddleCenter,
				imagePosition = ImagePosition.ImageAbove,
				fontStyle = FontStyle.Bold,
				fixedWidth = 120
			};
		}
	}

	[InitializeOnLoad]
	public class SceneSwitchLeftButton
	{
		static SceneSwitchLeftButton()
		{
			ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
		}

		static void OnToolbarGUI()
		{
			GUILayout.FlexibleSpace();

			if(GUILayout.Button(new GUIContent("Start Game", "첫 씬부터 게임을 시작합니다."), ToolbarStyles.commandButtonStyle))
			{
				// GUID로 변경해야 함.
				var startSceneData = AssetDatabase.FindAssets("Init", null);
				SceneHelper.StartScene(startSceneData[0]);
			}
		}
	}

	[InitializeOnLoad]
	public class SceneSwitchRightButton
	{
		private static GenericMenu genericMenu;
		
		static SceneSwitchRightButton()
		{
			ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI);	
		}

		static void OnToolbarGUI()
		{
			if (EditorGUILayout.DropdownButton(new GUIContent("Dev Scenes", "빠른 개발 씬 이동"), FocusType.Passive, ToolbarStyles.commandButtonStyle))
			{
				genericMenu = new GenericMenu();
				
				string[] guids = AssetDatabase.FindAssets("t:scene", new[] { "Assets/Scenes"});
				
				foreach(var guid in guids)
				{
					var scenePath = AssetDatabase.GUIDToAssetPath(guid);
					var sceneName = scenePath.Replace("Assets/Scenes/", "").Replace(".unity", "");
					
					genericMenu.AddItem(new GUIContent(sceneName), false, OnClickDropdownItem, guid);

					genericMenu.ShowAsContext();
				}
			}
			
			GUILayout.FlexibleSpace();
		}

		private static void OnClickDropdownItem(object guid)
		{
			SceneHelper.StartScene((string)guid, false);
		}
	}

	static class SceneHelper
	{
		private static string sceneGuid;
		private static bool isAutoPlay;

		public static void StartScene(string guid, bool autoPlay = true)
		{
			// Unity가 실행중이라면 종료
			if(EditorApplication.isPlaying)
			{
				EditorApplication.isPlaying = false;
			}

			sceneGuid = guid;
			isAutoPlay = autoPlay;
			
			EditorApplication.update += OnUpdate;
		}

		static void OnUpdate()
		{
			if (sceneGuid == null ||
			    EditorApplication.isPlaying || EditorApplication.isPaused ||
			    EditorApplication.isCompiling || EditorApplication.isPlayingOrWillChangePlaymode)
			{
				return;
			}

			EditorApplication.update -= OnUpdate;

			if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
			{
				string scenePath = AssetDatabase.GUIDToAssetPath(sceneGuid);
				EditorSceneManager.OpenScene(scenePath);
				EditorApplication.isPlaying = isAutoPlay;
			}
			
			sceneGuid = null;
			isAutoPlay = true;
		}
	}
}