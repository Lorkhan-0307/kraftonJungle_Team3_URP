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
    private static readonly string accessKey = "AKIA5FTZBVKDTJYXT35R";
    private static readonly string secretKey = "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";
    private static readonly string region = "ap-northeast-2"; // DynamoDB 리전

    private static AmazonDynamoDBClient client;

    private async void Start()
    {
        InitializeDynamoDBClient();

        PlayerData playerData = new PlayerData
        {
            UseToken = GenerateHashKey("user111"),
            UserID = "user111",
            Nickname = "PlayerTwo"
        };

        await ConnectPlayer(playerData);
    }

    // AWS 클라이언트 초기화 함수
    private void InitializeDynamoDBClient()
    {
        if (client == null)
        {
            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            client = new AmazonDynamoDBClient(credentials, Amazon.RegionEndpoint.GetBySystemName(region));
        }
    }

    // DynamoDB에 플레이어 데이터를 저장하는 함수
    public async Task SavePlayerData(PlayerData playerData)
    {
        var request = new PutItemRequest
        {
            TableName = "GamePlayer",
            Item = new Dictionary<string, AttributeValue>
            {
                { "UserID", new AttributeValue { S = playerData.UserID } },
                { "UseToken", new AttributeValue { S = playerData.UseToken } },
                { "Nickname", new AttributeValue { S = playerData.Nickname } }
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

    // DynamoDB에서 UserID로 플레이어 데이터를 가져오는 함수
    public async Task<Dictionary<string, AttributeValue>> GetPlayerDataByUserID(string UserID)
    {
        var request = new GetItemRequest
        {
            TableName = "GamePlayer",
            Key = new Dictionary<string, AttributeValue>
            {
                { "UserID", new AttributeValue { S = UserID } }
            }
        };

        try
        {
            var response = await client.GetItemAsync(request);
            if (response.Item != null && response.Item.Count > 0)
            {
                Debug.Log("Player data retrieved successfully by UserID.");
                return response.Item;
            }
            else
            {
                Debug.Log("No player data found for UserID.");
                return null;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to retrieve player data by UserID: " + e.Message);
            return null;
        }
    }

    // 플레이어 데이터가 존재하는지 확인하고, 존재하지 않으면 저장하는 함수
    public async Task ConnectPlayer(PlayerData playerData)
    {
        var existingPlayerData = await GetPlayerDataByUserID(playerData.UserID);

        if (existingPlayerData == null)
        {
            await SavePlayerData(playerData);
            Debug.Log("New player data created.");
        }
        else
        {
            Debug.Log($"Player data found for UserID: {existingPlayerData["UserID"].S}");
            Debug.Log($"Nickname: {existingPlayerData["Nickname"].S}");
        }
    }

    // 고유한 키를 생성하는 해시 함수
    private string GenerateHashKey(string UserID)
    {
        return $"KEY_{UserID}_{DateTime.UtcNow.Ticks}";
    }
}
