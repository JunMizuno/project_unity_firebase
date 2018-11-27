using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Facebook.Unity;

public class FacebookManager : MonoBehaviour {
    Action<bool> signInCallback_ = null;

    public bool IsSignIn
    {
        get
        {
            return FB.IsLoggedIn;
        }
    }

    /// <summary>
    /// 起動時
    /// </summary>
    void Awake()
    {
        Initialize();
    }

    /// <summary>
    /// 初期化時
    /// </summary>
    private void Initialize()
    {
        if (!FB.IsInitialized)
        {
            FB.Init(InitializedCallback, OnHideUnity);
        }
        else 
        {
            FB.ActivateApp();
        }
    }

    /// <summary>
    /// 初期化時のコールバック
    /// </summary>
    private void InitializedCallback()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            Debug.Log("<color=red>" + "Facebook Initialize Error" + "</color>");
        }
    }

    /// <summary>
    /// バックグラウンドに回った際のコールバック
    /// </summary>
    /// <param name="_isGameShow">If set to <c>true</c> is game show.</param>
    private void OnHideUnity(bool _isGameShow)
    {
        if (_isGameShow)
        {
            // @memo. ポーズ時の処理
            // @memo. ログイン連携だけなら特に何もする必要なし？
        }
        else
        {
            // @memo. レジューム時の処理
            // @memo. ログイン連携だけなら特に何もする必要なし？
        }
    }

    /// <summary>
    /// ログイン要求
    /// </summary>
    /// <param name="_callback">Callback.</param>
    public void RequestSignIn(Action<bool> _callback)
    {
        signInCallback_ = _callback;

        if (!FB.IsLoggedIn)
        {
            // @todo. パーミッションリストは必要に応じて設定する必要がある(ログイン連携のみならば以下の2つだけで良さそう)
            FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email" }, this.HandleResult);
        }
        else
        {
            if (signInCallback_ != null)
            {
                signInCallback_(true);
                signInCallback_ = null;
            }
        }
    }

    /// <summary>
    /// ログイン時のコールバック
    /// </summary>
    /// <param name="_result">Result.</param>
    private void HandleResult(IResult _result)
    {
        if (_result == null)
        {
            if (signInCallback_ != null)
            {
                signInCallback_(false);
                signInCallback_ = null;
            }

            return;
        }

        if (!string.IsNullOrEmpty(_result.Error) || _result.Cancelled)
        {
            if (signInCallback_ != null)
            {
                signInCallback_(false);
                signInCallback_ = null;
            }
        }
        else if (!string.IsNullOrEmpty(_result.RawResult))
        {
            if (signInCallback_ != null)
            {
                signInCallback_(true);
                signInCallback_ = null;
            }
        }
        else
        {
            if (signInCallback_ != null)
            {
                signInCallback_(false);
                signInCallback_ = null;
            }
        }
    }

    /// <summary>
    /// サインアウト要求
    /// </summary>
    public void RequestSignOut()
    {
        FB.LogOut();
    }

    /// <summary>
    /// アクセストークンを取得
    /// </summary>
    /// <returns>The get access token.</returns>
    public AccessToken RequestGetAccessToken()
    {
        AccessToken accessToken = null;

        if (IsSignIn)
        {
            accessToken = AccessToken.CurrentAccessToken;
        }

        return accessToken;
    }

    /// <summary>
    /// ユーザーIDを取得
    /// </summary>
    /// <returns>The user unique identifier.</returns>
    public string RequestGetUserUniqueId()
    {
        string uniqueId = "";

        if (IsSignIn)
        {
            uniqueId = AccessToken.CurrentAccessToken.UserId;
        }

        return uniqueId;
    }
}
