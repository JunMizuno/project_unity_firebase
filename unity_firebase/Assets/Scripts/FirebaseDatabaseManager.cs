using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class FirebaseDatabaseManager : MonoBehaviour
{
    private const string FIREBASE_ACCOUNT_URL = "https://testproject-e2271.firebaseio.com/";
    private const string FIREBASE_PARENT_PATH = "users";
    private DatabaseReference _databaseReference;

    /// <summary>
    /// 開始時
    /// </summary>
    void Start()
    {
        CheckAvailableGooglePlayDeveloperService();

        // リアルタイムデータベースのAPIを呼び出す前に必ず設定する
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(FIREBASE_ACCOUNT_URL);

        // テスト実行
        WriteUserData();
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
    /// データベースに書き込む
    /// 20181016現時点でテスト用
    /// </summary>
    private void WriteUserData()
    {
        // データベースのルートリファレンスを取得
        // 指定した場所までのパスを取得している
        _databaseReference = FirebaseDatabase.DefaultInstance.GetReference(FIREBASE_PARENT_PATH);

        // @todo. 以下テスト用、実用的でなない
        var userData = new UserData(3);
        userData.IsNew = true;
        userData.UserName = "もう一度やってみたさん";
        string json = JsonUtility.ToJson(userData);

        // 書き込むデータを設定
        // @memo. 同名のカラムが既に存在していた場合は上書きされる模様
        // @memo. この場合の「test02」などは仮ユーザーID、正式な場合はUDIUなどを組み合わせて生成させたものを指定することが望ましい
        _databaseReference.Child("test03").SetRawJsonValueAsync(json);
    }

    /// <summary>
    /// データベースから読み込む
    /// 20181016現時点でテスト用
    /// </summary>
    private void LoadUserData()
    {
        // @todo. 以下テスト用、実用的ではない
    }
}
