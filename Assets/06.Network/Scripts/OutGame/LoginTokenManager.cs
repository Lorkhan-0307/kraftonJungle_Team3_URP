using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class LoginTokenManager
{
    public static string tokenKey = "LoginToken";

    /// <summary>
    /// 로컬에 token 값을 저장합니다.
    /// </summary>
    /// <param name="token"></param>
    public static void SaveTokenToLocal(string token)
    {
        PlayerPrefs.SetString(tokenKey, token);
    }

    /// <summary>
    /// PlayerPrefs 안에 있는 로컬 클라이언트의 토큰 값을 불러옵니다.
    /// </summary>
    /// <returns></returns>
    public static string GetToken()
    {
        return PlayerPrefs.GetString(tokenKey, "");
    }

    public static void ResetToken()
    {
        PlayerPrefs.SetString(tokenKey, "");
    }
}
