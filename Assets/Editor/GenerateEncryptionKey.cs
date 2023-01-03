using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Google.Cloud.Storage.V1;
using UnityEditor;

public class GenerateEncryptionKey : EditorWindow
{
	public string popupText = "Choose a name for the file";
	public string inputText = "";
	void OnGUI()
	{
		inputText = EditorGUILayout.TextField(popupText, inputText);
		if(GUILayout.Button("Create Key"))
		{
			GenerateKey(inputText);
		}
		if(GUILayout.Button("Abort"))
		{
			Close();
		}
	}

	[MenuItem("Keys/Create New Key")]
	static void CreateKeyWindow()
	{
		GenerateEncryptionKey window = new GenerateEncryptionKey();
		window.ShowPopup();
	}
	//[MenuItem("Keys/Rotate Keys")]
	static void RotateKeys()
	{

	}

	void GenerateKey(string fileName)
	{
		string path = Path.Combine(Path.GetFullPath("./"), "Assets", "Resources", "Keys");
		fileName = fileName + ".txt";
		var encryptionKey = EncryptionKey.Generate().Base64Key;
		Debug.Log($"Generated Base64-encoded AES-256 Encryption Key: {encryptionKey}. Saved to file {fileName}");
		File.WriteAllText(Path.Combine(path, fileName), encryptionKey);
	}
}
