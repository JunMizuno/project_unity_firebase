using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TryFunction : MonoBehaviour {

    /// <summary>
    /// 開始時
    /// </summary>
	void Start () {
        //Utility.OpenUri("https://www.yahoo.co.jp");
        //Utility.OpenUri("https://us-central1-testproject-e2271.cloudfunctions.net/selectDeviceIndex" + "?pass=test01");
        Utility.OpenUri("https://us-central1-testproject-e2271.cloudfunctions.net/selectUserData" + "?pass=test01");
    }
}
