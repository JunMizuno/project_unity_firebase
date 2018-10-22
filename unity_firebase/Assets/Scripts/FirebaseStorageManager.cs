using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Firebase.Storage;

public class FirebaseStorageManager : MonoBehaviour 
{
    FirebaseStorage firebaseStorage_ = null;
    StorageReference parentRefrence_ = null;
    StorageReference resourcesReference_ = null;
    StorageReference dataReference_ = null;

    /// <summary>
    /// 開始時
    /// </summary>
    void Start()
    {
        CheckAvailableGooglePlayDeveloperService();

        // 参照を作成
        firebaseStorage_ = FirebaseStorage.DefaultInstance;
        parentRefrence_ = firebaseStorage_.GetReferenceFromUrl("gs://testproject-e2271.appspot.com");
        resourcesReference_ = firebaseStorage_.GetReferenceFromUrl("gs://testproject-e2271.appspot.com/Resources");
        dataReference_ = firebaseStorage_.GetReferenceFromUrl("gs://testproject-e2271.appspot.com/Data");

        UploadBytesData();
        //UploadFiles();
        //DownloadFiles();

        //var dataTest = data_ref.Child("test.png");
    }

    /// <summary>
    /// GooglePlay開発者サービスのバージョン確認
    /// 公式ドキュメントよりそのまま抜粋
    /// </summary>
    private void CheckAvailableGooglePlayDeveloperService()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Set a flag here indiciating that Firebase is ready to use by your
                // application.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    /// <summary>
    /// メモリ内のバイトデータをアップロード
    /// </summary>
    private void UploadBytesData()
    {
        // @memo. このパスは格納するディレクトリ名/ファイル名までを指定しないといけない
        // @memo. 同名ファイルが存在する場合は上書きされる
        dataReference_ = firebaseStorage_.GetReferenceFromUrl("gs://testproject-e2271.appspot.com/Data/upload_test.png");

        if (dataReference_ == null)
        {
            return;
        }

        // メモリ内のデータを取得
        var bytesData = Utility.CreateBytesByPngFile("Assets/StreamingAssets/upload_test.png");

        // Upload the file to the path "images/rivers.jpg"
        dataReference_.PutBytesAsync(bytesData)
          .ContinueWith((Task<StorageMetadata> task) => {
              if (task.IsFaulted || task.IsCanceled)
              {
                  Debug.Log(task.Exception.ToString());
              }
              else
              {
                  // Metadata contains file metadata such as size, content-type, and download URL.
                  StorageMetadata metadata = task.Result;

                  // @memo. 以下のURLはうまく取得できていない模様…
                  // @memo. コンソール上ではURLを取得でき、そのURLからダウンロードまでを確認できた
                  //var download_url = dataReference_.GetDownloadUrlAsync();
                  Debug.Log("Finished uploading...");
                  //Debug.Log("download url = " + download_url);
              }
          });

    }

    /// <summary>
    /// 
    /// </summary>
    private void UploadFiles()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    private void DownloadFiles()
    {

    }
}
