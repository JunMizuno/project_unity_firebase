using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using System;

public class FirebaseAuthManager : MonoBehaviour
{
    FirebaseAuth auth_ = null;
    FirebaseUser user_ = null;

    /// <summary>
    /// 開始時
    /// </summary>
	private void Start()
    {
        InitializeFirebase();

        // @todo. 挙動テスト、後で削除のこと
        // @todo. 新規登録かログインかを分ける処理が必要(既存のユーザーで新規登録の関数を呼んだ場合は当然エラーが出る)
        // @todo. 流れとしては、CreateUserWithEmailAndPasswordAsyncでエラーが返ればSignInWithEmailAndPasswordAsyncでログイン処理？
        //RegistrationWithUserInfomation("test2@test2.co.jp", "11223344aabbccdd");
        //LoginWithUserInfomation("test2@test2.co.jp", "11223344aabbccdd");
    }

    /// <summary>
    /// FirebaseのAuthモジュールを初期化
    /// </summary>
    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        auth_ = FirebaseAuth.DefaultInstance;
        auth_.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);

        CheckAvailableGooglePlayDeveloperService();
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
                Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    /// <summary>
    /// ユーザー認証変更時のコールバックリスナー
    /// リスナーを登録することで現在ログインしているユーザー取得時にAuthオブジェクトが初期化状態などでないことをチェックしているそうです
    /// </summary>
    /// <param name="_sender">Sender.</param>
    /// <param name="_eventArgs">Event arguments.</param>
    private void AuthStateChanged(object _sender, System.EventArgs _eventArgs)
    {
        if (auth_.CurrentUser != user_)
        {
            bool signedIn = user_ != auth_.CurrentUser && auth_.CurrentUser != null;
            if (!signedIn && user_ != null)
            {
                Debug.Log("Signed out " + user_.UserId);
            }

            user_ = auth_.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user_.UserId);
            }
        }
    }

    /// <summary>
    /// 破棄時
    /// </summary>
    private void OnDestroy()
    {
        auth_.StateChanged -= AuthStateChanged;
        auth_ = null;
    }

    /// <summary>
    /// ユーザーのインデックスを取得する
    /// </summary>
    /// <returns>The current user index.</returns>
    public string GetCurrentUserIndex()
    {
        string userIndex = "";

        FirebaseUser user = auth_.CurrentUser;
        if (user != null)
        {
            userIndex = user.UserId;
        }

        return userIndex;
    }

    /// <summary>
    /// ユーザー名を取得する
    /// </summary>
    /// <returns>The current user name.</returns>
    public string GetCurrentUserName()
    {
        string userName = "";

        FirebaseUser user = auth_.CurrentUser;
        if (user != null)
        {
            userName = user.DisplayName;
        }

        return userName;
    }

    /// <summary>
    /// ユーザーの登録メールを取得する
    /// </summary>
    /// <returns>The current user email.</returns>
    public string GetCurrentUserEmail()
    {
        string userEmail = "";

        FirebaseUser user = auth_.CurrentUser;
        if (user != null)
        {
            userEmail = user.Email;
        }

        return userEmail;
    }

    /// <summary>
    /// ユーザーアイコンを取得する(URI形式)
    /// </summary>
    /// <returns>The current user photo URL.</returns>
    public Uri GetCurrentUserPhotoUrl()
    {
        Uri userUri = new Uri("");

        FirebaseUser user = auth_.CurrentUser;
        if (user != null)
        {
            userUri = user.PhotoUrl;
        }

        return userUri;
    }

    /// <summary>
    /// ユーザー新規登録
    /// </summary>
    public void RegistrationWithUserInfomation(string _email, string _password)
    {
        auth_.CreateUserWithEmailAndPasswordAsync(_email, _password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
        });
    }

    /// <summary>
    /// ユーザーログイン時
    /// </summary>
    /// <param name="_email">Email.</param>
    /// <param name="_password">Password.</param>
    public void LoginWithUserInfomation(string _email, string _password)
    {
        auth_.SignInWithEmailAndPasswordAsync(_email, _password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }

            // @memo. メールやパスワードが異なっていた場合は以下のフラグが有効になる
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
        });
    }

    /// <summary>
    /// 認証情報を作成してログイン
    /// </summary>
    /// <param name="_email">Email.</param>
    /// <param name="_password">Password.</param>
    public void SetAuthUserInformation(string _email, string _password)
    {
        Credential credential = EmailAuthProvider.GetCredential(_email, _password);

        auth_.SignInWithCredentialAsync(credential).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
    }

    /// <summary>
    /// ログアウト
    /// </summary>
    public void SignOut()
    {
        if (auth_ == null) 
        {
            return;
        }

        auth_.SignOut();
    }

    /// <summary>
    /// Googleアカウントでのログイン
    /// </summary>
    /// <param name="_googleIdToken">Google identifier token.</param>
    /// <param name="_googleAccessToken">Google access token.</param>
    public void LoginWithGoogleAccount(string _googleIdToken, string _googleAccessToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(_googleIdToken, _googleAccessToken);
        auth_.SignInWithCredentialAsync(credential).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
        });
    }
}
