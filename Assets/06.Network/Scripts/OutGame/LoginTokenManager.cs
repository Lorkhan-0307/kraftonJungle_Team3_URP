using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
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
    /// DB의 데이터를 받아 LocalPlayer 객체에 값을 넣어줍니다.
    /// 로컬에 token 값이 없거나, 불러오는데 실패할 경우 False를 반환하며,
    /// 정상적으로 불러올 경우 True를 반환합니다.
    /// </summary>
    /// <returns></returns>
    public static bool LoadDataWithToken()
    {
        string token = PlayerPrefs.GetString(tokenKey, "");

        // 아직 저장되어있는 토큰이 없음
        if (token == "")
        {
            Debug.Log("No Token In Local Directory.");
            return false;
        }

        //TODO: token 값을 통해 DB에서 가져온 데이터 현재 플레이어 정보에 담기.
        Photon.Realtime.Player currentPlayer = PhotonNetwork.LocalPlayer;
        currentPlayer.NickName = "";



        //CustomData 담을 변수 선언
        ExitGames.Client.Photon.Hashtable customData = new ExitGames.Client.Photon.Hashtable();

        //CustomData 적용
        currentPlayer.SetCustomProperties(customData);



        return true;
    }
}
