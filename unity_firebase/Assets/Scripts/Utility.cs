using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.Networking;

public class Utility : MonoBehaviour {

    /// <summary>
    /// PNG画像からバイナリデータを生成
    /// </summary>
    /// <returns>The bytes by png file.</returns>
    /// <param name="_filePath">File path.</param>
    public static byte[] CreateBytesByPngFile(string _filePath)
    {
        FileStream fileStream = new FileStream(_filePath, FileMode.Open, FileAccess.Read);

        BinaryReader bin = new BinaryReader(fileStream);
        byte[] values = bin.ReadBytes((int)bin.BaseStream.Length);

        bin.Close();

        return values;
    }

    /// <summary>
    /// PNG画像からTexture2Dを生成
    /// </summary>
    /// <returns>The texture2 DB y png.</returns>
    /// <param name="_filePath">File path.</param>
    public static Texture2D CreateTexture2DByPng(string _filePath)
    {
        byte[] readBinary = CreateBytesByPngFile(_filePath);

        // @memo. PNG画像の幅は16バイト～19バイト(長さ4バイト)、画像の高さは20バイト～23バイト(長さ4バイト)に格納されている
        int pos = 16; // 16バイトから開始

        int width = 0;
        for (int i = 0; i < 4; i++)
        {
            width = width * 256 + readBinary[pos++];
        }

        int height = 0;
        for (int i = 0; i < 4; i++)
        {
            height = height * 256 + readBinary[pos++];
        }

        return CreateTexture2DByBytes(readBinary, width, height);
    }

    /// <summary>
    /// バイナリ、幅、高さを指定してTexture2Dを生成
    /// </summary>
    /// <returns>The texture2 DB y bytes.</returns>
    /// <param name="_contents">Contents.</param>
    public static Texture2D CreateTexture2DByBytes(byte[] _contents, int _width, int _height)
    {
        Texture2D texture = new Texture2D(_width, _height);
        texture.LoadImage(_contents);

        return texture;
    }

    /// <summary>
    /// 指定されたURIに基づいてそれぞれ処理をする
    /// </summary>
    /// <param name="_uri">URI.</param>
    public static void OpenUri(string _uri)
    {
        // @memo. 仕様として「sss://popup」などを定めたとする

        var uri = new Uri(_uri);
        var scheme = uri.Scheme.ToLower();
        Debug.Log("<color=magenta>" + "scheme:" + scheme + "</color>");
        var host = uri.Host.ToLower();
        Debug.Log("<color=magenta>" + "host:" + host + "</color>");

        switch (scheme) {
            case "sss":
                {
                    switch (host)
                    {
                        case "popup":
                            break;

                        case "scene":
                            break;

                        default:
                            break;
                    }
                }
                break;

            case "http":
            case "https":
                {
#if UNITY_EDITOR
                    Application.OpenURL(_uri);
#elif UNITY_WEBGL
                    // 別ウインドウで開く
                    Application.ExternalEval(string.Format("window.open('{0}','_blank')", _uri));
#else
                    Application.OpenURL(_uri);
#endif
                }
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Firebaseに対してリクエストする場合の処理
    /// </summary>
    /// <returns>The firebase function.</returns>
    /// <param name="_uri">URI.</param>
    public static IEnumerator RequestFirebaseFunction(string _uri, Action<string> _callback)
    {
        // UnityWebRequestを生成
        // @todo. 後々のことを考えるとPostを使用した方が良いはず
        UnityWebRequest request = UnityWebRequest.Get(_uri);

        yield return request.SendWebRequest();

        if (request.responseCode == -1)
        {
            Debug.Log("<color=red>" + "error push notidication" + "</color>");
            Debug.Log("<color=red>" + request.error + "</color>");
            yield return false;
        }
        else
        {
            Debug.Log("<color=magenta>" + "RequestResponseCode:" + request.responseCode + "</color>");
            Debug.Log("<color=magenta>" + "RequestResult:" + request.downloadHandler.text + "</color>");
        }

        _callback(request.downloadHandler.text);
    }
}
