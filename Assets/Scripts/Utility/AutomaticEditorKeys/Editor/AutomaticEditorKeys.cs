using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.IO;
using System.Text;

public class AutomaticEditorKeys : MonoBehaviour {

	string m_ScriptFilePath;

	[MenuItem ("Tools/Update Keys")]
	static void UpdateKeys()
	{
		string[] tagsNames = UnityEditorInternal.InternalEditorUtility.tags;
		string[] layerNames = UnityEditorInternal.InternalEditorUtility.layers;
		string path = EditorKeys.keysPath;

		string outputPath = EditorKeys.keysPath;

		string tempString = "";
		string finalString = "";
		
		try {
			
			StreamReader sr = new StreamReader (path);

			bool firstString = true;

			while ((tempString = sr.ReadLine () ) != null) {

				if (!tempString.Contains("TAG_") && !tempString.Contains("LAYER_"))
				{
					if (firstString)
					{
						firstString = false;
						finalString += tempString;
					} else {
						finalString += "\n" + tempString;
					}
				}

				if (tempString.Contains("Tag list"))
				{
					for (int i = 0; i < tagsNames.Length; i++)
					{
						string actualTagString = "\tpublic const string TAG_" + tagsNames[i].ToUpper().Replace(" ", "_") + " = \"" + tagsNames[i] + "\";";
						finalString += "\n" + actualTagString;
					}
				}

				if (tempString.Contains("Layer list"))
				{
					for (int i = 0; i < layerNames.Length; i++)
					{
						string actualLayerString = "\tpublic const string LAYER_" + layerNames[i].ToUpper().Replace(" ", "_") + " = \"" + layerNames[i] + "\";";
						finalString += "\n" + actualLayerString;
					}
				}

			}
			
			sr.Close ();
			
		} catch (Exception ex) {
			EditorUtility.ClearProgressBar();
			Debug.Log ("ERROR --- " + ex.ToString() + " --- reading the file " + path);
		}

		try
		{
			// Delete the file if it exists.
			if (File.Exists(outputPath))
			{
				// Note that no lock is put on the
				// file and the possibility exists
				// that another process could do
				// something with it between
				// the calls to Exists and Delete.
				File.Delete(outputPath);
			}
			
			// Create the file.
			using (FileStream fs = File.Create(outputPath))
			{
				Byte[] info = new UTF8Encoding(true).GetBytes(finalString);
				// Add some information to the file.
				fs.Write(info, 0, info.Length);
				
			}
			
			FileInfo fileInfo = new FileInfo(outputPath);
			
			fileInfo.Refresh();
			
			AssetDatabase.Refresh();
			
		} catch (Exception ex) {
			EditorUtility.ClearProgressBar();
			Debug.Log ("ERROR --- " + ex.ToString() + " --- writing the file " + outputPath);
		}

	}

}
