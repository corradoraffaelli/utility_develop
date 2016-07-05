using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Linq;
using System.Collections;
using System.Reflection;
using System;

[System.Serializable]
[CustomEditor(typeof(ChangeSortingLayer))]
public class ChangeSortingLayerEditor : Editor {
	
	string[] sortingLayerNames;

	public ChangeSortingLayerEditor()
	{
		sortingLayerNames = GetSortingLayerNames();
	}
	
	public override void OnInspectorGUI()
	{
		ChangeSortingLayer obj = target as ChangeSortingLayer;

		if (obj != null)
		{
			obj.indexString = EditorGUILayout.Popup("Sorting Layer", obj.indexString, sortingLayerNames, EditorStyles.popup);
			
			obj.sortingLayerOrder = EditorGUILayout.IntField("Sorting Order", obj.sortingLayerOrder);
			
			obj.applyToChildren = EditorGUILayout.Toggle("Apply To Children", obj.applyToChildren);

			obj.sortingLayerString = sortingLayerNames[obj.indexString];

			obj.Change();


			if(GUI.changed) {
				EditorUtility.SetDirty(obj);
			}
		}
	}

	// Get the sorting layer names
	public string[] GetSortingLayerNames()
	{
		Type internalEditorUtilityType = typeof(InternalEditorUtility);
		PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
		return (string[])sortingLayersProperty.GetValue(null, new object[0]);
	}
}
