using System;
using System.Collections.Generic;
using System.Linq; // 여기를 추가하세요
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
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

        string userId = "user1";
        PlayerData playerData = new PlayerData
        {
            UserID = userId
        };

        await ConnectPlayer(playerData);
    }

    private void InitializeDynamoDBClient()
    {
        if (client == null)
        {
            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            client = new AmazonDynamoDBClient(credentials, Amazon.RegionEndpoint.GetBySystemName(region));
        }
    }

    public async Task SavePlayerData(PlayerData playerData)
    {
        var request = new PutItemRequest
        {
            TableName = "UserData",
            Item = new Dictionary<string, AttributeValue>
            {
                { "UserID", new AttributeValue { S = playerData.UserID } },
                { "Nickname", new AttributeValue { S = playerData.Nickname } },
                { "UserToken", new AttributeValue { S = playerData.UserToken } }
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

    public async Task<List<Dictionary<string, AttributeValue>>> GetPlayerDataByUserID(string UserID)
    {
        var request = new QueryRequest
        {
            TableName = "UserData",
            KeyConditionExpression = "UserID = :uid",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":uid", new AttributeValue { S = UserID } }
            }
        };

        try
        {
            var response = await client.QueryAsync(request);
            if (response.Items != null && response.Items.Count > 0)
            {
                Debug.Log("Player data retrieved successfully by UserID.");
                return response.Items;
            }
            else
            {
                Debug.Log("No player data found for UserID.");
                return null;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to retrieve player data: " + e.Message);
            return null;
        }
    }

    public async Task ConnectPlayer(PlayerData playerData)
    {
        // UserID로 모든 UserToken을 조회
        var existingPlayerData = await GetPlayerDataByUserID(playerData.UserID);

        if (existingPlayerData != null && existingPlayerData.Count > 0)
        {
            // UserToken이 이미 존재하는지 확인
            var existingToken = existingPlayerData.FirstOrDefault()?["UserToken"].S;
            var existingNickname = existingPlayerData.FirstOrDefault()?["Nickname"].S;

            if (!string.IsNullOrEmpty(existingToken))
            {
                // 기존 UserToken이 있으면 그 UserToken을 사용
                playerData.UserToken = existingToken; // 기존 UserToken을 사용
                playerData.Nickname = existingNickname;
                Debug.Log($"Using existing UserToken: {playerData.UserToken}");
                Debug.Log($"Existing Nickname: {playerData.Nickname}");
            }
            else
            {
                // UserToken이 존재하지 않으면 새로운 UserToken 생성
                playerData.UserToken = GenerateHashKey(playerData.UserID);
                await SavePlayerData(playerData);
                Debug.Log("New UserToken created and player data saved.");
            }

        }
        else
        {
            // UserID가 존재하지 않을 경우 새로운 UserToken 생성
            playerData.UserToken = GenerateHashKey(playerData.UserID);
            await SavePlayerData(playerData);
            Debug.Log("New player data created.");
        }
    }



    private string GenerateHashKey(string UserID)
    {
        return $"KEY_{UserID}_{DateTime.UtcNow.Ticks}";
    }

    //public PlayerData Func()
    //{
    //    PlayerData playerData = new PlayerData();

    //    playerData.Nickname = 

    //    return new PlayerData();
    //}
}

//return형이 player data.
