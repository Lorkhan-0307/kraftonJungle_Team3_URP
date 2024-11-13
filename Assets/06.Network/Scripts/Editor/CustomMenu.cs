using UnityEngine;
using UnityEditor;
using System.IO;

public class CustomMenu : MonoBehaviour
{
    // 커스텀 메뉴 항목 생성
    [MenuItem("Custom Tools/Show Game Result")]
    public static void ShowGameResult()
    {
        // PlayerPrefs에서 "GameResultDemo" 키로 string 값 불러오기
        string gameResult = PlayerPrefs.GetString("GameResultDemo", "");

        if (gameResult == "")
        {
            // 콘솔창에 출력
            Debug.Log($"No Result.");
            return;
        }

        string result = "";
        string[] lines = gameResult.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            string[] tokens = lines[i].Split(',');
            if (tokens.Length != 2) continue;

            bool isMonsterWon = bool.Parse(tokens[0]);

            result += $"{(isMonsterWon ? "Monster":"Scientist")} Won, {tokens[1]} people alive.\n";
        }

        // 콘솔창에 출력
        Debug.Log(result);
    }

    // 커스텀 메뉴 항목 생성
    [MenuItem("Custom Tools/Delete Result Logs")]
    public static void DeleteResultLogs()
    {
        // PlayerPrefs에서 "GameResultDemo" 키로 string 값 불러오기
        PlayerPrefs.SetString("GameResultDemo", "");

        // 콘솔창에 출력
        Debug.Log("Result Log Is Initialized.");
    }

    // 커스텀 메뉴 항목 생성
    [MenuItem("Custom Tools/Delete Debugging Logs")]
    public static void DeleteDebugLogs()
    {
        // PlayerPrefs에서 "GameResultDemo" 키로 string 값 불러오기
        PlayerPrefs.SetInt("DebugLogIndex", 0);

        // 콘솔창에 출력
        Debug.Log("Debug Log Is Initialized.");


        // 로그 파일 삭제
        string path = Application.persistentDataPath;
        if (Directory.Exists(path))
        {
            // 경로에 있는 모든 파일 가져오기
            string[] files = Directory.GetFiles(path);

            foreach (string file in files)
            {
                try
                {
                    File.Delete(file); // 파일 삭제
                }
                catch (IOException e)
                {
                    Debug.LogError($"Failed to delete file: {file} - {e.Message}");
                }
            }

            Debug.Log("All files in Application.persistentDataPath have been deleted.");
        }
        else
        {
            Debug.LogWarning("Application.persistentDataPath directory does not exist.");
        }
    }
}
