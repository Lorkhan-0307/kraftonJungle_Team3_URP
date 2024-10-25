using System;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Runtime;
using UnityEngine;

public class DynamoDBManager : MonoBehaviour
{
    // AWS 자격 증명 - Access Key, Secret Key
    private static string accessKey = "AKIA5FTZBVKDTJYXT35R";
    private static string secretKey = "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";
    private static string region = "ap-northeast-2"; // DynamoDB 리전

    private static AmazonDynamoDBClient client;

    void Start()
    {
        // AWS 자격 증명 사용하여 클라이언트 생성
        var credentials = new BasicAWSCredentials(accessKey, secretKey);
        client = new AmazonDynamoDBClient(credentials, Amazon.RegionEndpoint.GetBySystemName(region));

        // 예제: 클라이언트 접속 시 확인/저장/불러오기 로직
        string UserId = "jangwoo"; // 예제 플레이어 ID
        string playerNickname = "Player123";
        string UserNum = "1";

        ConnectPlayer(UserId, UserNum, playerNickname);
    }

    // 클라이언트 접속 로직
    public async Task ConnectPlayer(string UserId, string UserNum, string playerNickname)
    {
        // 클라이언트가 서버에 접속 시 키값 확인
        var playerData = await GetPlayerData(UserId, UserNum);

        if (playerData == null)
        {
            // 키값이 없으므로 새로운 데이터를 생성하여 저장
            string newKey = GenerateHashKey(UserId);
            await SavePlayerData(UserId,UserNum, playerNickname);
            Debug.Log($"New player data created with key: {newKey}");
        }
        else
        {
            // 키값이 있으므로 닉네임 출력
            Debug.Log($"Player nickname found: {playerData["Nickname"].S}");
        }
    }

    // DynamoDB에 플레이어 데이터를 저장하는 함수
    public static async Task SavePlayerData(string UserId, string UserNum, string nickname)
    {
        var request = new PutItemRequest
        {
            TableName = "player", // DynamoDB 테이블 이름
            Item = new Dictionary<string, AttributeValue>
            {
                { "UserID", new AttributeValue { S = UserId } },  // Primary Key
                { "UserNum", new AttributeValue { N = UserNum } }, // Sort Key 추가
                { "Nickname", new AttributeValue { S = nickname } }   // 닉네임 속성
            }
        };

        try
        {
            var response = await client.PutItemAsync(request);
            Debug.Log("Player data saved successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save player data: " + e.Message);
        }
    }

    // DynamoDB에서 플레이어 데이터를 가져오는 함수
    public static async Task<Dictionary<string, AttributeValue>> GetPlayerData(string UserId, string userNum)
    {
        var request = new GetItemRequest
        {
            TableName = "player", // 테이블 이름
            Key = new Dictionary<string, AttributeValue>
        {
            { "UserID", new AttributeValue { S = UserId } }, // Primary Key
            { "UserNum", new AttributeValue { N = userNum } } // Sort Key
        }
        };

        try
        {
            var response = await client.GetItemAsync(request);
            if (response.Item != null && response.Item.Count > 0)
            {
                return response.Item; // 플레이어 데이터 반환
            }
            else
            {
                Debug.Log("Player data not found.");
                return null;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to retrieve player data: " + e.Message);
            return null;
        }
    }


    // 고유한 키를 생성하는 해시 함수 (예시)
    private string GenerateHashKey(string UserId)
    {
        return "KEY_" + UserId + "_" + DateTime.UtcNow.Ticks.ToString(); // 간단한 예시
    }
}
