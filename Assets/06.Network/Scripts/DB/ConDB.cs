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
    private static string accessKey = "AKIA5FTZBVKDTJYXT5R";
    private static string secretKey = "Nk+v5CCr5Xa3ZiC6UwCX4h9aTEXrKgvWBqk70VV";
    private static string region = "ap-northeast-2"; // DynamoDB가 있는 리전

    private static AmazonDynamoDBClient client;

    // 유니티 시작 시 클라이언트 초기화
    void Start()
    {
        // AWS 자격 증명 사용하여 클라이언트 생성
        var credentials = new BasicAWSCredentials(accessKey, secretKey);
        client = new AmazonDynamoDBClient(credentials, Amazon.RegionEndpoint.GetBySystemName(region));

        // 예시로 Player 데이터 저장
        string playerId = "jangwoo";
        string password = "123";

        SavePlayerData(playerId, password).Wait();
    }

    // DynamoDB에 플레이어 데이터를 저장하는 함수
    public static async Task SavePlayerData(string playerID, string password)
    {
        var request = new PutItemRequest
        {
            TableName = "PlayerTable", // DynamoDB 테이블 이름
            Item = new Dictionary<string, AttributeValue>
            {
                { "playerID", new AttributeValue { S = playerID } },  // Primary Key
                { "Password", new AttributeValue { S = password } }   // Password 속성
            }
        };

        try
        {
            // DynamoDB에 아이템 저장
            var response = await client.PutItemAsync(request);
            Debug.Log("Player data saved successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save player data: " + e.Message);
        }
    }
}
