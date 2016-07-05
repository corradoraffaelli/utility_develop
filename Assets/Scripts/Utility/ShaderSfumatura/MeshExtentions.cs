using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class MeshExtentions {

	public static void SetVertexColor(this Mesh mesh, Color color)
	{
		var vertColors = new Color[mesh.vertexCount];
		for (int i = 0; i < mesh.vertexCount; i++)
		{
			vertColors[i] = color;
		}

		mesh.colors = vertColors;
		mesh.RecalculateNormals();
	}

	public static void SetMultipleVertexColors(this Mesh mesh, ColorPoint[] points)
	{
		var vertColors = new List<Color>();
		bool canSetColors = true;
		
		for (int i = 0; i < points.Length; i++)
		{
			if (points[i] != null)
			{
				if (points[i].position == null)
				{
					Debug.Log ("MeshExtentions.cs problem. SetMultipleVertexColor. transform " + i + " is not setted");
					canSetColors = false;
				}
			}
		}
		
		if (!canSetColors)
			return;
		
		Debug.Log (points.Length);
		
		for (int i = 0; i < mesh.vertexCount; i++)
		{
			Debug.Log ("vertex " +  i);
			Debug.Log ("vertex_position: " + mesh.vertices[i]);

			Color vertexColor = GetColorOfPoint(mesh, points, 0, i);
			vertColors.Add (vertexColor);
		}
		
		mesh.colors = vertColors.ToArray();
		mesh.RecalculateNormals();
	}

	static Color GetColorOfPoint(Mesh mesh, ColorPoint[] points, int baseIndex, int vertexIndex)
	{
		Debug.Log ("basePointPosition: " + points[baseIndex].position.position);
		
		Color vertColor = Color.green;
		
		float[] influences = new float[points.Length];
		influences[baseIndex] = 1.0f;
		
		float influencesSum = 0.0f;

		
		if (points.Length > 1)
		{
			for (int j = 0; j < points.Length; j++)
			{
				if (j == baseIndex)
				{
					influences[j] = 1.0f;
				} else {
					float distance = Vector3.Distance(points[baseIndex].position.position, points[j].position.position);
					
					Vector3 vect1 = (points[j].position.position - points[baseIndex].position.position).normalized;
					Vector3 vect2 = mesh.vertices[vertexIndex] - points[baseIndex].position.position;
					
					float dot = Vector3.Dot(vect1, vect2);
					// TODO: WHY?!?!?
//					float magn = dot / 6.0f;
//					Debug.Log (magn + " " + Vector3.Project(vect2, vect1).magnitude);
//					float magn = Vector3.Project(vect2, vect1).magnitude;

//					Debug.Log (magn + " " + dot);
					Debug.Log ("dot " + dot);

//					if (dot < 0.0f && magn > 0.0f)
//						magn = -magn;
					
					float dotNorm = Mathf.Clamp( dot / distance, 0.0f, 1.0f);
					
					// If the base index has influence 0, must recalculate all influences.
					float tempBaseInfluence = 1.0f - dotNorm;
					if (tempBaseInfluence == 0.0)
					{
						int otherBaseIndex = baseIndex++;
						return GetColorOfPoint(mesh, points, otherBaseIndex, vertexIndex);
					}

					influences[j] = dotNorm / (1.0f - dotNorm);
				}
					
				Debug.Log ("influences " + j + " " + influences[j]);
				influencesSum += influences[j];

			}
		}

		float red = 0;
		float green = 0;
		float blue = 0;
		
		for (int j = 0; j < points.Length; j++)
		{
			influences[j] = influences[j] / influencesSum;
			
			Debug.Log ("point " + j + "influence : " + influences[j]);
			
			red += influences[j] * points[j].color.r;
			green += influences[j] * points[j].color.g;
			blue += influences[j] * points[j].color.b;
		}
		vertColor = new Color(red, green, blue);
		return vertColor;
	}

	public static void SetMultipleVertexColorsOLD(this Mesh mesh, ColorPoint[] points)
	{
		var vertColors = new List<Color>();
		bool canSetColors = true;

		for (int i = 0; i < points.Length; i++)
		{
			if (points[i] != null)
			{
				if (points[i].position == null)
				{
					Debug.Log ("MeshExtentions.cs problem. SetMultipleVertexColor. transform " + i + " is not setted");
					canSetColors = false;
				}
			}
		}

		if (!canSetColors)
			return;

		Debug.Log (points.Length);

		for (int i = 0; i < mesh.vertexCount; i++)
		{
			Debug.Log ("vertex " +  i);

			Debug.Log ("vertex_position: " + mesh.vertices[i]);
			Debug.Log ("firstPointPosition: " + points[0].position.position);

			Color vertColor = Color.green;

			float[] influences = new float[points.Length];
			influences[0] = 1.0f;

			float influencesSum = 0.0f;

			int baseIndex = 0;
			float baseInfluence = 1.0f;

			if (points.Length > 1)
			{
				for (int j = 1; j < points.Length; j++)
				{
					Debug.Log ("point " + j);

					string pointsDebug = "";
					pointsDebug += ("pointPosition: " + points[j].position.position);

					float distance = Vector3.Distance(points[0].position.position, points[j].position.position);

					Vector3 vect1 = points[j].position.position - points[0].position.position;
					Vector3 vect2 = mesh.vertices[i] - points[0].position.position;

//					Debug.Log ("point " + j + "pointPosition: " + points[j].position.position);
//					Debug.Log ("point " + j + "firstPointPosition: " + points[0].position.position);
//					Debug.Log ("point " + j + "vertices_position: " + mesh.vertices[i]);
//					Debug.Log ("point " + j + "vec1: " + vect1);
//					Debug.Log ("point " + j + "vec2: " + vect2);

					pointsDebug += (" vect1: " + vect1);
					pointsDebug += (" vect2: " + vect2);

					Debug.Log (pointsDebug);

					float dot = Vector3.Dot(vect1, vect2);
//					float magn = Vector3.Project(vect2, vect1).magnitude;
//					dot = Vector3.Project(
					float magn = dot / 6.0f;

//					if (dot < 0.0f && magn > 0.0f)
//						magn = -magn;

//					Debug.Log ("point " + j + "dot: " + dot);
//					Debug.Log ("point " + j + "magn: " + magn);

					float dotNorm = Mathf.Clamp( magn / distance, 0.0f, 1.0f);

//					Debug.Log ("point " + j + "distance: " + distance);
//
//					Debug.Log ("point " + j + "dot norm: " + dotNorm);

					Debug.Log ("dot: " + dot + " magn: " + magn + " distance: " + distance + " dot norm: " + dotNorm);

//					influences[j] = dotNorm;

					if (j == 1)
					{
						influences[0] = 1.0f - dotNorm;



						influences[j] = dotNorm;

						influencesSum += influences[0];
					}
					else
					{
						if (influences[0] != 0.0f)
							influences[j] = dotNorm * influences[0];
						else
							influences[j] = dotNorm;
					}


//					if (dotNorm != 1.0f)
//						influences[j] = dotNorm / (1.0f - dotNorm);
//					else if (dotNorm == 1.0f)
//						influences[j] = 100000.0f;
//					else
//						influences[j] = 1.0f;

//					Debug.Log ("point " + j + "inlf: " + influences[j]);
//					influences[j] = 1.0f - influences[j];

					influencesSum += influences[j];
				}
			}

			for (int j = 0; j < points.Length; j++)
			{

			}

			float red = 0;
			float green = 0;
			float blue = 0;

			for (int j = 0; j < points.Length; j++)
			{
				influences[j] = influences[j] / influencesSum;

				Debug.Log ("point " + j + "influence : " + influences[j]);

				red += influences[j] * points[j].color.r;
				green += influences[j] * points[j].color.g;
				blue += influences[j] * points[j].color.b;



			}
//			Debug.Log (red + " " + green + " " + blue);
			vertColor = new Color(red, green, blue);
//			Debug.Log ("vertex " + i + "color : " + vertColor);
			vertColors.Add (vertColor);
		}
		
		mesh.colors = vertColors.ToArray();
		mesh.RecalculateNormals();
	}

	public static void SetVertexColors(this Mesh mesh, Color topColor, Color bottomColor, float verticalOffset)
	{
		var maxY = float.MinValue;
		var minY = float.MaxValue;

		for (int i = 0; i < mesh.vertices.Length; i++)
		{
			if (mesh.vertices[i].y > maxY)
				maxY = mesh.vertices[i].y;
			if (mesh.vertices[i].y < minY)
				minY = mesh.vertices[i].y;
		}

		var meshHeight = minY + maxY;

		var vertColors = new List<Color>();

		for (int i = 0; i < mesh.vertexCount; i++)
		{
			var t = mesh.vertices[i].y / meshHeight;
			vertColors.Add (Color.Lerp(topColor, bottomColor, t - verticalOffset));
		}

		mesh.colors = vertColors.ToArray();
		mesh.RecalculateNormals();
		
	}
}
