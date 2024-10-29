using System;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Runtime;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class DynamoDBManager : MonoBehaviour
{
    private static AmazonDynamoDBClient client;

    private static readonly string accessKey = "AKIA5FTZBVKDTJYXT35R";
    private static readonly string secretKey = "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";
    private static readonly string region = "ap-northeast-2"; // DynamoDB 리전


    public async Task LoadData(string token, PlayerData playerData)
    {
        InitializeDynamoDBClient();


        // UserToken이 빈 문자열일 경우 새로운 UserToken과 랜덤 닉네임 생성 및 저장
        if (string.IsNullOrEmpty(token))
        {
            string newUserToken = GenerateHashKey(); // 새로운 UserToken 생성
            string randomNickname = GenerateRandomNickname(); // 랜덤 닉네임 생성

            playerData.UserToken = newUserToken;
            playerData.Nickname = randomNickname;

            // 생성한 PlayerData를 데이터베이스에 저장
            await SavePlayerData(playerData);
            await ConnectPlayer(playerData);
        }
        else
        {
            // UserToken이 비어있지 않은 경우 기존 플레이어 데이터에 연결
            playerData.UserToken = token;

            await ConnectPlayer(playerData); // 기존 데이터와 연결
        }
    }


    private void InitializeDynamoDBClient()
    {
        if (client == null)
        {
            //var accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
            //var secretKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
            //var region = Environment.GetEnvironmentVariable("AWS_REGION");

            //// 각 변수를 로그로 출력
            //Debug.Log($"Access Key: {accessKey}");
            //Debug.Log($"Secret Key: {secretKey}");
            //Debug.Log($"Region: {region}");

            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            client = new AmazonDynamoDBClient(credentials, Amazon.RegionEndpoint.GetBySystemName(region));
        }
    }


    public async Task SavePlayerData(PlayerData playerData)
    {
        var request = new PutItemRequest
        {
            TableName = "PlayerData",
            Item = new Dictionary<string, AttributeValue>
            {
                { "UserToken", new AttributeValue { S = playerData.UserToken } },
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

    public async Task<Dictionary<string, AttributeValue>> GetPlayerDataByUserToken(string UserToken)
    {
        var request = new GetItemRequest
        {
            TableName = "PlayerData",
            Key = new Dictionary<string, AttributeValue>
            {
                { "UserToken", new AttributeValue { S = UserToken } }
            }
        };

        try
        {
            var response = await client.GetItemAsync(request);
            if (response.Item != null && response.Item.Count > 0)
            {
                Debug.Log("Player data retrieved successfully by UserToken.");
                return response.Item;
            }
            else
            {
                Debug.Log("No player data found for UserToken.");
                return null;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to retrieve player data by UserToken: " + e.Message);
            return null;
        }
    }

    public async Task ConnectPlayer(PlayerData playerData)
    {
        var existingPlayerData = await GetPlayerDataByUserToken(playerData.UserToken);

        playerData.Nickname = existingPlayerData["Nickname"].S;

        Debug.Log($"Nickname: {playerData.Nickname}");
    }

    
    // 고유한 키를 생성하는 해시 함수
    private string GenerateHashKey()
    {
        return $"KEY_{DateTime.UtcNow.Ticks}";
    }

    // 랜덤 닉네임을 생성하는 함수
    private string GenerateRandomNickname()
    {
        DateTime now = DateTime.UtcNow;
        // 현재 시간의 틱 수를 기반으로 고유한 닉네임 생성
        return $"Player_{now.Hour:D2}_{now.Minute:D2}_{now.Second:D2}";
    }

    // 닉네임을 업데이트하는 메서드
    public async Task UpdateNickname(string token, string newName)
    {
        var request = new UpdateItemRequest
        {
            TableName = "PlayerData",
            Key = new Dictionary<string, AttributeValue>
            {
                { "UserToken", new AttributeValue { S = token } }
            },
            ExpressionAttributeNames = new Dictionary<string, string>
            {
                { "#N", "Nickname" }
            },
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":newName", new AttributeValue { S = newName } }
            },
            UpdateExpression = "SET #N = :newName"
        };

        try
        {
            var response = await client.UpdateItemAsync(request);
            Debug.Log("Nickname updated successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to update nickname: " + e.Message);
        }
    }
}
