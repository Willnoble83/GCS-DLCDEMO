using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class KeyLoader : MonoBehaviour
{
    public List<string> KeysToLoad = new List<string>();

    public static List<Key> keys = null;

    void Start()
    {
        StartCoroutine(LoadAssets());

    }

    /*
     * Loads AssetBundles and grabs the decryption key from each bundle. 
     */
    IEnumerator LoadAssets()
	{
        List<Key> loadedKeys = new List<Key>();
        foreach (string key in KeysToLoad)
        {
            Debug.Log($"Loading Key {key}");
            string path = Path.GetFullPath("./") + Path.Combine("DLC");
            Debug.Log($"Current Path: {path}");
            // Check that the AssetBundle can be found
            if (Directory.Exists(path) && File.Exists(Path.Combine(path, key)))
            {
                
                AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(Path.Combine(path, key)));
                // Wait for the Load Request to be completed
                yield return request;
                AssetBundle bundle = request.assetBundle;
                
                // Load the key data from the bundle and place it into our Key class.
                TextAsset cred = bundle.LoadAsset<TextAsset>(key);
                loadedKeys.Add(new Key(key, cred.ToString()));
            }
            else
			{
                Debug.Log($"Key '{key}' not found");
			}
        }
        if (loadedKeys.Count == 0)
        {
            Debug.LogError($"Loaded zero keys.");
        }
        else {
            keys = loadedKeys;
            Controller.reference.AdjustButtonInteractable();
        }
    }

}
