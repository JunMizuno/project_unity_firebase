using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TryFunction : MonoBehaviour {



    /// <summary>
    /// 開始時
    /// </summary>
	void Start () {
        //Utility.OpenUri("https://www.yahoo.co.jp");

        string result = "";
        StartCoroutine(Utility.RequestFirebaseFunction("https://us-central1-testproject-e2271.cloudfunctions.net/selectUserData" + "?pass=test01", r => {
            result = r;
            Debug.Log("<color=cyan>" + result + "</color>");
        }));
    }
}
