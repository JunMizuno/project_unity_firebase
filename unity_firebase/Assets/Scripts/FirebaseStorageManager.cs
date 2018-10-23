using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Firebase.Storage;
using System;
using System.Threading;

public class FirebaseStorageManager : MonoBehaviour 
{
    FirebaseStorage firebaseStorage_ = null;
    StorageReference parentRefrence_ = null;
    StorageReference resourcesReference_ = null;
    StorageReference dataReference_ = null;
    StorageReference pathReference_ = null;
    StorageReference gsReference_ = null;
    StorageReference httpsReference_ = null;

    /// <summary>
    /// 開始時
    /// </summary>
    void Start()
    {
        CheckAvailableGooglePlayDeveloperService();

        // 参照を作成
        firebaseStorage_ = FirebaseStorage.DefaultInstance;

        // 参照の取得例
        parentRefrence_ = firebaseStorage_.GetReferenceFromUrl("gs://testproject-e2271.appspot.com");
        resourcesReference_ = firebaseStorage_.GetReferenceFromUrl("gs://testproject-e2271.appspot.com/Resources");
        dataReference_ = firebaseStorage_.GetReferenceFromUrl("gs://testproject-e2271.appspot.com/Data");

        //UploadBytesData();
        //UploadFiles();
        //DownloadFiles();
        GetFilesMetaData();
        //RefreshFilesMetaData();
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

        // メタデータを追加
        // @memo. この形式で記述することで変更できる項目は書き換えが可能
        //var newMetaData = new MetadataChange();
        //newMetaData.ContentType = "png";
        //newMetaData.CacheControl = "0001";

        // アップロード処理
        // アップロード中のリスナーを追加
        var task = dataReference_.PutBytesAsync(bytesData, null, new StorageProgress<UploadState>(state => {
            Debug.Log("SessionUri:" + state.UploadSessionUri);
            Debug.Log(String.Format("Progress: {0} of {1} bytes transferred.", state.BytesTransferred, state.TotalByteCount));
        }), CancellationToken.None, null);

        task.ContinueWith((Task<StorageMetadata> resultTask) => {
            if (resultTask.IsFaulted || resultTask.IsCanceled)
            {
                Debug.Log(resultTask.Exception.ToString());
            }
            else
            {
                // Metadata contains file metadata such as size, content-type, and download URL.
                StorageMetadata metadata = resultTask.Result;

                // @memo. 以下のURLはうまく取得できていない模様…
                // @memo. コンソール上ではURLを取得でき、そのURLからダウンロードまでを確認できた
                //var download_url = dataReference_.GetDownloadUrlAsync();
                Debug.Log("Finished uploading...");
                //Debug.Log("download url = " + download_url);
            }
        });
    }

    /// <summary>
    /// ファイルを指定してアップロードする
    /// </summary>
    private void UploadFiles()
    {
        // @memo. このパスは格納するディレクトリ名/ファイル名までを指定しないといけない
        // @memo. 同名ファイルが存在する場合は上書きされる
        resourcesReference_ = firebaseStorage_.GetReferenceFromUrl("gs://testproject-e2271.appspot.com/Resources/upload_test2.png");

        if (dataReference_ == null)
        {
            return;
        }

        // ファイルパスを指定
        string localFile = "Assets/StreamingAssets/upload_test2.png";

        // メタデータを追加
        // @memo. この形式で記述することで変更できる項目は書き換えが可能
        //var newMetaData = new MetadataChange();
        //newMetaData.ContentType = "png";
        //newMetaData.CacheControl = "0001";

        // アップロード処理
        // アップロード中のリスナーを追加
        var task = resourcesReference_.PutFileAsync(localFile, null, new StorageProgress<UploadState>(state => {
            Debug.Log("SessionUri:" + state.UploadSessionUri);
            Debug.Log(String.Format("Progress: {0} of {1} bytes transferred.", state.BytesTransferred, state.TotalByteCount));
        }), CancellationToken.None, null);

        task.ContinueWith((Task<StorageMetadata> resultTask) => {
            if (resultTask.IsFaulted || resultTask.IsCanceled)
            {
                Debug.Log(resultTask.Exception.ToString());
            }
            else
            {
                // Metadata contains file metadata such as size, content-type, and download URL.
                StorageMetadata metadata = resultTask.Result;

                // @memo. 以下、上記関数と同じ
                //string download_url = metadata.DownloadUrl.ToString();
                Debug.Log("Finished uploading...");
                //Debug.Log("download url = " + download_url);
            }
        });
    }

    /// <summary>
    /// ファイルをダウンロードする
    /// </summary>
    private void DownloadFiles()
    {
        if (firebaseStorage_ == null) 
        {
            return;
        }

        // ストレージから直接パスを指定してダウンロードする場合
        pathReference_ = firebaseStorage_.GetReference("Resources/upload_test2.png");

        // GoogleCloudUriを指定してダウンロードする場合
        gsReference_ = firebaseStorage_.GetReferenceFromUrl("gs://testproject-e2271.appspot.com/Resources/upload_test2.png");

        // URLを指定してダウンロードする場合
        httpsReference_ = firebaseStorage_.GetReferenceFromUrl("https://firebasestorage.googleapis.com/v0/b/testproject-e2271.appspot.com/o/Resources%2Fupload_test2.png");

        // ダウンロードURLを取得して対応する場合
        // @memo. リファレンスを変えて同じAPIを呼んでも同じURLが返ってくる
        //string urlByPath = "";
        //StartCoroutine(GetDownloadUrl(pathReference_, urlByPath));
        //string urlByUri = "";
        //StartCoroutine(GetDownloadUrl(gsReference_, urlByUri));
        //string urlByHttps = "";
        //StartCoroutine(GetDownloadUrl(httpsReference_, urlByHttps));

        // ダウンロード先のパスを指定して対応する場合
        // @memo. 末尾の拡張子までをしっかりと指定すること
        string storagePath = Application.streamingAssetsPath + "/downloaded_test_image.png";
        Debug.Log("<color=yellow>" + "storagePath:" + storagePath + "</color>");
        GetFileFromFirebaseToLocalStorage(pathReference_, storagePath);
        //GetFileFromFirebaseToLocalStorage(gsReference_, storagePath);
        //GetFileFromFirebaseToLocalStorage(httpsReference_, storagePath);
    }

    /// <summary>
    /// ファイルをダウンロードするためのURLを取得する
    /// </summary>
    /// <returns>The download URL.</returns>
    /// <param name="_reference">Reference.</param>
    private IEnumerator GetDownloadUrl(StorageReference _reference, string _url)
    {
        _reference.GetDownloadUrlAsync().ContinueWith((Task<Uri> task) =>
        {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                _url = task.Result.ToString();
            }
        });

        while (_url == String.Empty)
        {
            yield return null;
        }

        OpenUrl(_url);
    }

    /// <summary>
    /// WWW経由などでファイルを開く(ダウンロードする)
    /// </summary>
    /// <param name="_url">URL.</param>
    private void OpenUrl(string _url)
    {
        Debug.Log("<color=red>" + "Download URL:" + _url + "</color>");
    }

    /// <summary>
    /// 保存先を指定してファイルをダウンロードする
    /// </summary>
    /// <param name="_reference">Reference.</param>
    /// <param name="_storagePath">Storage path.</param>
    private void GetFileFromFirebaseToLocalStorage(StorageReference _reference, string _storagePath)
    {
        // リスナー
        var progressFunc = new StorageProgress<DownloadState>((DownloadState state) => {
            Debug.Log(String.Format("Progress: {0} of {1} bytes transferred.", state.BytesTransferred, state.TotalByteCount));
        });

        _reference.GetFileAsync(_storagePath, progressFunc, CancellationToken.None).ContinueWith(task => {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                Debug.Log("File downloaded.");
            }
        });
    }

    /// <summary>
    /// メモリ配列にダウンロードする
    /// 最大サイズが決まっているため、あまり実用的ではないかも…
    /// </summary>
    /// <param name="_reference">Reference.</param>
    private void GetFileByBytes(StorageReference _reference)
    {
        // Download in memory with a maximum allowed size of 1MB (1 * 1024 * 1024 bytes)
        const long maxAllowedSize = 1 * 1024 * 1024;
        _reference.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task) => {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log(task.Exception.ToString());
            }
            else
            {
                byte[] fileContents = task.Result;
                Debug.Log("Finished downloading!");
            }
        });
    }

    /// <summary>
    /// ファイルのメタデータを取得
    /// </summary>
    private void GetFilesMetaData()
    {
        if (firebaseStorage_ == null)
        {
            return;
        }

        // ストレージから直接パスを指定してダウンロードする場合
        pathReference_ = firebaseStorage_.GetReference("Resources/upload_test2.png");

        // GoogleCloudUriを指定してダウンロードする場合
        gsReference_ = firebaseStorage_.GetReferenceFromUrl("gs://testproject-e2271.appspot.com/Resources/upload_test2.png");

        // URLを指定してダウンロードする場合
        httpsReference_ = firebaseStorage_.GetReferenceFromUrl("https://firebasestorage.googleapis.com/v0/b/testproject-e2271.appspot.com/o/Resources%2Fupload_test2.png");

        // @memo. それぞれリファレンスを切り替えても同じはず
        pathReference_.GetMetadataAsync().ContinueWith((Task<StorageMetadata> task) =>
        {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                StorageMetadata metaData = task.Result;
                Debug.Log("<color=yellow>" + "CacheControl:" + metaData.CacheControl + " ContentType:" + metaData.ContentType + "</color>");
            }
        });
    }

    /// <summary>
    /// ファイルのメタデータを更新
    /// </summary>
    private void RefreshFilesMetaData()
    {
        if (firebaseStorage_ == null)
        {
            return;
        }

        // ストレージから直接パスを指定してダウンロードする場合
        pathReference_ = firebaseStorage_.GetReference("Resources/upload_test2.png");

        // GoogleCloudUriを指定してダウンロードする場合
        gsReference_ = firebaseStorage_.GetReferenceFromUrl("gs://testproject-e2271.appspot.com/Resources/upload_test2.png");

        // URLを指定してダウンロードする場合
        httpsReference_ = firebaseStorage_.GetReferenceFromUrl("https://firebasestorage.googleapis.com/v0/b/testproject-e2271.appspot.com/o/Resources%2Fupload_test2.png");

        var newMetaData = new MetadataChange();
        newMetaData.CacheControl = "20181023";
        newMetaData.ContentType = "image/png";

        // 更新処理
        // @memo. それぞれリファレンスを切り替えても同じはず
        pathReference_.UpdateMetadataAsync(newMetaData).ContinueWith(task => {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                StorageMetadata metaData = task.Result;
                Debug.Log("Successed Refresh MetaData!");
            }
        });
    }
}
