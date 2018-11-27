using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TryFunction : MonoBehaviour {



    /// <summary>
    /// 開始時
    /// </summary>
	void Start () 
    {
        // @memo. ログインテスト

        //StartCoroutine(Utility.RequestFirebaseUserLoginFunction("", "", r => {
        //    Debug.Log("<color=cyan>" + "リターンストリング:" + r + "</color>");
        //}));

        StartCoroutine(Utility.RequestFirebaseUserLoginFunction("test2@test2.co.jp", "11223344aabbccdd", r => {
            Debug.Log("<color=cyan>" + "リターンストリング:" + r + "</color>");
        }));

        // @memo. ローカルデータテスト
        var test = PlayerPrefs.GetString("user_index");
        Debug.Log("<color=yellow>" + "test:" + test + "</color>");
        if (string.IsNullOrEmpty(test))
        {
            Debug.Log("<color=red>" + "ローカルデータにない" + "</color>");
        }
        else
        {
            Debug.Log("<color=yellow>" + "保存されている" + "</color>");
        }
    }
}
