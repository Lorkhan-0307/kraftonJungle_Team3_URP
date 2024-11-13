using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DebugLogger : MonoBehaviour
{
    private string logFilePath;
    private StreamWriter logWriter;
    int index = 0;

    private void Awake()
    {
        index = PlayerPrefs.GetInt("DebugLogIndex", 0);
        NewFile();

        // Unity 로그 이벤트 연결
        Application.logMessageReceived += HandleLog;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type != LogType.Error) return;
        // 로그 메시지 작성
        string logEntry = $"{System.DateTime.Now} [{type}] {logString}\n{stackTrace}\n";

        // 로그 파일에 기록
        logWriter.WriteLine(logEntry);
    }

    public void AddLog(string text)
    {
        logWriter.WriteLine(text);
    }

    public void NewFile()
    {
        logWriter?.Close();

        index++;
        PlayerPrefs.SetInt("DebugLogIndex", index);

        // 로그 파일 경로 설정
        logFilePath = Path.Combine(Application.persistentDataPath, $"GameLog{index}.txt");

        // StreamWriter를 사용해 로그 파일 생성 (기존 파일은 덮어쓰기)
        logWriter = new StreamWriter(logFilePath, false);
        logWriter.AutoFlush = true; // 즉시 기록을 위해 자동 플러시 설정
    }

    private void OnDestroy()
    {
        // 로그 이벤트 연결 해제
        Application.logMessageReceived -= HandleLog;

        // 로그 파일 닫기
        logWriter?.Close();
    }
}
