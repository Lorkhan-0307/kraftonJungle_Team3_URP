using UnityEngine;
using UnityEngine.Playables;

public class IntroCutscene : MonoBehaviour
{
    [SerializeField] private PlayableDirector PD;

    private void Start()
    {
        PD = gameObject.GetComponent<PlayableDirector>();
        // 타임라인의 종료 이벤트에 함수 연결
        PD.stopped += OnTimelineStopped;
    }

    public void Play()
    {
        Debug.Log("인트로 컷신 시작");
        PD.Play();
        Debug.Log("인트로 컷신 종료");
    }

    private void OnTimelineStopped(PlayableDirector PD)
    {
        Debug.Log("서버 로직 마저 실행");
        // 타임라인이 끝나면 실행될 코드
        NEGameStart.AfterCutScene();
    }

    private void OnDestroy()
    {
        // 메모리 누수를 방지하기 위해 이벤트 구독 해제
        PD.stopped -= OnTimelineStopped;
    }
}
