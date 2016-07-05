using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PulsingScale))]
[CanEditMultipleObjects]
public class PulsingScaleEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space();

        EditorGUILayout.TextArea("It works with TextMesh component and Unity UIText. \nIf you want to use with different kind of text, \nyou must extend the code.");
    }
	
}
