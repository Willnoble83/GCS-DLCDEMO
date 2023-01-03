using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string assetBundleDirectoryWin = "Assets/AssetBundles/win";
        string assetBundleDirectoryMac = "Assets/AssetBundles/mac";
        if (!Directory.Exists(assetBundleDirectoryWin))
        {
            Directory.CreateDirectory(assetBundleDirectoryWin);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectoryWin,
                                        BuildAssetBundleOptions.None,
                                        BuildTarget.StandaloneWindows);
        if (!Directory.Exists(assetBundleDirectoryMac))
        {
            Directory.CreateDirectory(assetBundleDirectoryMac);
        }
        //BuildPipeline.BuildAssetBundles(assetBundleDirectoryMac,
        //BuildAssetBundleOptions.None,
        //BuildTarget.StandaloneOSX);
    }
}