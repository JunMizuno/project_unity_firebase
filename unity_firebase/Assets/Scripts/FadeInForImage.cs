using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInForImage : MonoBehaviour {
    // UI->Imageをアタッチ
    [SerializeField]
    public GameObject object_ = null;

    private bool isFinished = false;
    public bool IsFinished
    {
        get
        {
            return isFinished;
        }
    }

    private float alpha_ = 1.0f;

    /// <summary>
    /// 起動時
    /// </summary>
    void Awake()
    {
        if (object_)
        {
            object_.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, alpha_);
        }
    }

    /// <summary>
    /// 開始時
    /// </summary>
	void Start () 
    {
		
	}
	
    /// <summary>
    /// 更新時
    /// </summary>
	void Update () 
    {
        if (alpha_ <= 0.0f)
        {
            alpha_ = 0.0f;
        }

        alpha_ -= 0.01f;

        if (object_)
        {
            object_.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, alpha_);
        }
    }
}
