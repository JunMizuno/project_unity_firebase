using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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

        Texture2D texture = new Texture2D(width, height);
        texture.LoadImage(readBinary);

        return texture;
    }
}
