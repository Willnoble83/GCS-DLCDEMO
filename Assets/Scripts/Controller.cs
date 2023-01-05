using System.Collections;
using System.IO;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public static Controller reference;

    public TMP_Text DisplayText;
    Downloader downloader;
    public Button DLC1Button;
    public Button DLC2Button;

	const string bucket = "gcsdlcdemo-files";

	private void Awake()
	{
        reference = this;
	}

	void Start()
    {
        UpdateText("");
        downloader = new Downloader();
        DLC1Button.onClick.AddListener(delegate
        {
            StartEncryptedDownload("DLC1.txt", "dlc1");
        });
        DLC2Button.onClick.AddListener(delegate
        {
            StartEncryptedDownload("DLC2.txt", "dlc2");
        });

    }

    public void AdjustButtonInteractable()
	{
        DLC1Button.interactable = false;
        DLC1Button.GetComponentInChildren<TMP_Text>().text = "DLC1 (Not Found)";
        DLC2Button.interactable = false;
        DLC2Button.GetComponentInChildren<TMP_Text>().text = "DLC2 (Not Found)";
        foreach(Key key in KeyLoader.keys)
		{
            if(key.id == "dlc1")
			{
                DLC1Button.interactable = true;
                DLC1Button.GetComponentInChildren<TMP_Text>().text = "DLC1 (Encrypted)";
			}
            else if (key.id == "dlc2")
			{
                DLC2Button.interactable = true;
                DLC2Button.GetComponentInChildren<TMP_Text>().text = "DLC2 (Encrypted)";
			}
		}
	}

    /*
     * Starts a download of an unencrypted object and updates the displayed text
     * Takes the name of the object to download from Google Cloud Storage
     */
    public void StartDownload(string objectName)
	{
        Debug.Log($"Downloading {objectName}");
        StreamReader reader = new StreamReader(downloader.DownloadObjectIntoMemory(bucket, objectName));
        string data = reader.ReadToEnd();
        UpdateText(data);
	}

    /*
     * Starts a download of an encrypted piece of data and updates the text
     * Takes the name of the object to download from Google Cloud Storage and the 
     * ID of the encryption key to use
     */
    public void StartEncryptedDownload(string objectName, string keyID)
	{
        Debug.Log($"Downloading {objectName} with key {keyID}");
        if(KeyLoader.keys == null)
		{
            Debug.LogWarning($"Tried to download encrypted object before keys were loaded");
            UpdateText("DLC Keys aren't loaded, please wait");
            return;
		}
        string key = null;
        foreach(Key LoadedKey in KeyLoader.keys)
		{
            Debug.Log($"Testing {LoadedKey.id}");
            if(LoadedKey.id == keyID)
			{
                key = LoadedKey.key;
                break;
            }
		}
        if(key == null) 
		{
            UpdateText("DLC was not found.");
            return;
		}

        StreamReader reader = new StreamReader(downloader.DownloadObjectIntoMemory(bucket, objectName, key));
        string data = reader.ReadToEnd();
        UpdateText(data);
    }

    /*
     * Updates the text displayed to the user
     */
    void UpdateText(string newText)
	{
        if(DisplayText == null)
		{
            Debug.LogError($"No Display Text Set for Controller");
            return;
		}
        DisplayText.text = ($"Display: '{newText}'");
	}
}
