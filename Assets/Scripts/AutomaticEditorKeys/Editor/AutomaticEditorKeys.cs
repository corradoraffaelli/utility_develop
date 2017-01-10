using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.IO;
using System.Text;

public class AutomaticEditorKeys : Editor {
    
    const string pathKeyPref = "EditorKeysPath";

    [MenuItem ("Tools/Automatic Keys/Update Keys")]
	static void UpdateKeys()
	{
        string path = UpdateKeysPath();

        string fullFileString = GenerateFullFileString(path);

        SaveFullFile(fullFileString, path);
	}

    [MenuItem("Tools/Automatic Keys/Set Keys File Path")]
    static void SetKeysFilePath()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Editor Keys File",
                               "EditorKeys" + ".cs",
                               "cs",
                               "Please enter a file name to save");

        string oldPath = "";

        if (EditorPrefs.HasKey(pathKeyPref))
            oldPath = EditorPrefs.GetString(pathKeyPref);

        // An old keys file is present
        if (oldPath != path && File.Exists(oldPath))
        {
            bool deleteOld = EditorUtility.DisplayDialog("Delete Old File", "It seems that an old file with keys is present, do you want to delete it?", "Delete", "Leave");

            if (deleteOld)
                File.Delete(oldPath);
        }

        EditorPrefs.SetString(pathKeyPref, path);

        string fullFileString = GenerateFullFileString(path);

        SaveFullFile(fullFileString, path);
    }



    static string UpdateKeysPath()
    {
        string path = "";

        if (EditorPrefs.HasKey(pathKeyPref))
            path = EditorPrefs.GetString(pathKeyPref);

        if (string.IsNullOrEmpty(path))
        {
            path = EditorUtility.SaveFilePanelInProject("Save Editor Keys File",
                               "EditorKeys" + ".cs",
                               ".cs",
                               "Please enter a file name to save");

            EditorPrefs.SetString(pathKeyPref, path);
        }

        return path;
    }

    static string GenerateFullFileString(string path)
    {
        string[] tagsNames = UnityEditorInternal.InternalEditorUtility.tags;
        string[] layerNames = UnityEditorInternal.InternalEditorUtility.layers;

        //Find class name from path
        string className = path;
        int lastSlashIndex = className.LastIndexOf("/");
        if (className.Contains("/"))
            className = className.Remove(0, lastSlashIndex + 1);
        if (className.Contains(".cs"))
            className = className.Remove(className.IndexOf(".cs", 3));

        // Generate string of entire file

        string finalString = "";

        string beginningString = "public static class " + className + "{\n";
        string endingString = "\n}";

        finalString += beginningString;

        //TAGS

        finalString += "\n\t" +
            "//Tag List" +
            "\n";

        for (int i = 0; i < tagsNames.Length; i++)
        {
            string actualTagString = "\tpublic const string TAG_" + tagsNames[i].ToUpper().Replace(" ", "_") + " = \"" + tagsNames[i] + "\";";
            finalString += actualTagString + "\n";
        }

        //LAYERS

        finalString += "\n\t" +
            "//Layer List" +
            "\n";

        for (int i = 0; i < layerNames.Length; i++)
        {
            string actualLayerString = "\tpublic const string LAYER_" + layerNames[i].ToUpper().Replace(" ", "_") + " = \"" + layerNames[i] + "\";";
            finalString += actualLayerString + "\n";
        }

        finalString += endingString;

        return finalString;
    }

    static void SaveFullFile(string fullFileString, string path)
    {
        string outputPath = path;

        try
        {

            // Delete the file if it exists.
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }

            // Create the file.
            using (FileStream fs = File.Create(outputPath))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes(fullFileString);
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }

            FileInfo fileInfo = new FileInfo(outputPath);

            fileInfo.Refresh();

            AssetDatabase.Refresh();

        }
        catch (Exception ex)
        {
            EditorUtility.ClearProgressBar();
            Debug.Log("ERROR --- " + ex.ToString() + " --- writing the file " + outputPath);
        }
    }



    [Obsolete]
    static void ReadAndhangeFile()
    {
        // OLD WAY
        /*
        string tempString = "";
         
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
        */
    }
}
