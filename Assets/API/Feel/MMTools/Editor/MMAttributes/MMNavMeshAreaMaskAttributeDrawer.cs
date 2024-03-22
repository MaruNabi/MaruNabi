#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
 
namespace MoreMountains.Tools
{
	[CustomPropertyDrawer(typeof(MMNavMeshAreaMaskAttribute))]
	public class MMNavMeshAreaMaskAttributeDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty serializedProperty, GUIContent label)
		{
			string[] navMeshAreaNames  = GameObjectUtility.GetNavMeshAreaNames();
			float positionWidth = position.width;
			int maskValue = serializedProperty.intValue;
			
			position.width = EditorGUIUtility.labelWidth;
			EditorGUI.PrefixLabel(position, label);
         
			position.x += EditorGUIUtility.labelWidth;
			position.width = positionWidth - EditorGUIUtility.labelWidth;
 
			EditorGUI.BeginChangeCheck();
			maskValue = EditorGUI.MaskField(position, maskValue, navMeshAreaNames);
			
			if (EditorGUI.EndChangeCheck())
			{
				serializedProperty.intValue = maskValue;
			}
		}
	}
}
#endif