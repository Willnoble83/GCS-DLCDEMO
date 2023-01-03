using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;

using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;

using UnityEngine;

public class Downloader
{
    static private StorageClient storage;
    static private string serviceAccountID = "unityaccess@gcs-dlc-demo-371515.iam.gserviceaccount.com";
    private const string bucket = "";

    /*
     * Creates an instance of the Downloader using a specified private key
     */
    public Downloader()
    {
        //  Loading private key from resources as a TextAsset
        string key = Resources.Load<TextAsset>("Keys/service").ToString();
        // Creating a  ServiceAccountCredential.Initializer
        // ref: https://googleapis.dev/dotnet/Google.Apis.Auth/latest/api/Google.Apis.Auth.OAuth2.ServiceAccountCredential.Initializer.html
        ServiceAccountCredential.Initializer initializer = new ServiceAccountCredential.Initializer(serviceAccountID);

        // Getting ServiceAccountCredential from the private key
        // ref: https://googleapis.dev/dotnet/Google.Apis.Auth/latest/api/Google.Apis.Auth.OAuth2.ServiceAccountCredential.html
        ServiceAccountCredential servicecredentials = new ServiceAccountCredential(
            initializer.FromPrivateKey(key)
        );
        GoogleCredential credentials = GoogleCredential.FromServiceAccountCredential(servicecredentials);
        storage = StorageClient.Create(credentials);

    }

    /*
     * Downloads an unencrypted object into memory.
     */
    public Stream DownloadObjectIntoMemory(string bucketName, string objectName)
    {

        Stream stream = new MemoryStream();

        Debug.Log("Downloading: " + objectName + " from " + bucketName);
        storage.DownloadObject(bucketName, objectName, stream);
        Debug.Log("Downloaded: " + objectName + " from " + bucketName);
        stream.Seek(0, SeekOrigin.Begin);
        return stream;

    }

    /*
     * Downloads an encrypted object into memory
     */

    public Stream DownloadObjectIntoMemory(string bucketName, string objectName, string encryptionKey)
    {
        Stream stream = new MemoryStream();
        Debug.Log($"Downloading Encrypted Object: {objectName} from {bucketName}");
        storage.DownloadObject(bucketName, objectName, stream, new DownloadObjectOptions
        {
            EncryptionKey = EncryptionKey.Create(Convert.FromBase64String(encryptionKey)),

        });
        Debug.Log($"Downloaded Encrypted Object: {objectName} from {bucketName}");
        stream.Seek(0, SeekOrigin.Begin);
        return stream;
    }
}
