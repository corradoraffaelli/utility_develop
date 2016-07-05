using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(CRShade))]
public class CRShadeEditor : Editor {
	
	public override void OnInspectorGUI()
	{
		var myScript = target as CRShade; 

		myScript.color1 = EditorGUILayout.ColorField("Color 1", myScript. color1);
		myScript.color2 = EditorGUILayout.ColorField("Color 2", myScript.color2);
		myScript.offset = EditorGUILayout.Slider(myScript.offset, -1, 1);

		if (myScript.colorPoints != null)
		{
			for (int i = 0; i < myScript.colorPoints.Count; i++)
			{
				myScript.colorPoints[i].color = EditorGUILayout.ColorField("Color", myScript.colorPoints[i].color);
				myScript.colorPoints[i].position = EditorGUILayout.ObjectField("Position", myScript.colorPoints[i].position, typeof(Transform), true) as Transform;

				if (GUILayout.Button("X", GUILayout.Width(30.0f)))
				{
					if (myScript.colorPoints[i].position != null)
						DestroyImmediate(myScript.colorPoints[i].position.gameObject);
					myScript.colorPoints.RemoveAt(i);
				}
			}
		}

		EditorGUILayout.Separator();

		if (GUILayout.Button("Add colorElement"))
		{
			if (myScript.colorPoints == null)
				myScript.colorPoints = new List<ColorPoint>();

			ColorPoint addedPoint = new ColorPoint();
			GameObject newGo = new GameObject();
			newGo.name = "ColorPosition "+ myScript.colorPoints.Count.ToString();
			newGo.transform.SetParent(myScript.transform);

			addedPoint.position = newGo.transform;

			addedPoint.position.localPosition = Vector3.zero;

			addedPoint.color = Color.red;

			myScript.colorPoints.Add(addedPoint);
		}

		if (GUI.changed)
		{

//			var meshFilter = myScript.GetComponent<MeshFilter>();
//			meshFilter.sharedMesh.SetVertexColors(myScript.color1, myScript.color2, myScript.offset);
//
////			SceneView.RepaintAll();
//			UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
		}

		if (GUILayout.Button("click me"))
		{
			var meshFilter = myScript.GetComponent<MeshFilter>();
			if (myScript.colorPoints != null)
				meshFilter.sharedMesh.SetMultipleVertexColors(myScript.colorPoints.ToArray());

			UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
		}


	}
}
