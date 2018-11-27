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
    private DatabaseReference databaseReference_;
    private Firebase.Auth.FirebaseUser firebaseUser_;

    /// <summary>
    /// 開始時
    /// </summary>
    void Start()
    {
        CheckAvailableGooglePlayDeveloperService();

        // リアルタイムデータベースのAPIを呼び出す前に必ず設定する
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(FIREBASE_ACCOUNT_URL);

        // テスト実行
        //WriteUserData();
        //LoadUserData();
    }

    /// <summary>
    /// GooglePlay開発者サービスのバージョン確認(SDKの初期化)
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
    public void WriteUserData(UserData _userData)
    {
        // データベースのルートリファレンスを取得
        // 指定した場所までのパスを取得している
        databaseReference_ = FirebaseDatabase.DefaultInstance.GetReference(FIREBASE_PARENT_PATH);

        string json = JsonUtility.ToJson(_userData);

        // 書き込むデータを設定
        // @memo. 同名のカラムが既に存在していた場合は上書きされる模様
        // @memo. 正式な場合はUDIUなどを組み合わせて生成させたものを指定することが望ましい
        databaseReference_.Child(_userData.userIndex).SetRawJsonValueAsync(json);
    }

    /// <summary>
    /// データベースから読み込む
    /// 20181016現時点でテスト用
    /// </summary>
    public void LoadUserData()
    {
        // データベースのルートリファレンスを取得
        // 指定した場所までのパスを取得している
        databaseReference_ = FirebaseDatabase.DefaultInstance.GetReference(FIREBASE_PARENT_PATH + "/test02");

        // @todo. 以下テスト用、実用的ではない
        databaseReference_.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("<color=red>" + "データ取得エラー" + "</color>");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapShot = task.Result;

                // jsonデータを取得する場合
                // @memo. Dictionaryを使用する場合は以下の様な記述で取得可能
                // @memo. Dictionary<string, object> itemDic = Json.Deserialize(itemJson) as Dictionary<string, object>;
                string json = snapShot.GetRawJsonValue();
                Debug.Log("<color=yellow>" + "json:" + json + "</color>");
                UserData userData = JsonUtility.FromJson<UserData>(json);
                Debug.Log("<color=yellow>" + "UserIndex:" + userData.userIndex + " DeviceIndex:" + userData.deviceIndex + " IsNew:" + userData.isNew + " UserName:" + userData.userName + "</color>");

                // ループしてデータを取得する場合
                // そのままのデータを使用すると文字列が文字化け(エスケープシーケンスによるもの)していたので下記の様な記述にしています
                IEnumerable<DataSnapshot> result = snapShot.Children;
                foreach (DataSnapshot data in result)
                {
                    string key = System.Text.RegularExpressions.Regex.Unescape(data.Key);
                    string child = System.Text.RegularExpressions.Regex.Unescape(data.GetRawJsonValue());
                    Debug.Log("<color=green>" + "dataのキー:" + key + " dataのjson:" + child + "</color>");
                }
            }
        });
    }
}
