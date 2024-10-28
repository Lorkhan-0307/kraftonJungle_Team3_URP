using UnityEngine;
using Photon.Pun;
using UnityEngine.EventSystems;

public class AnimSync : MonoBehaviourPun, IPunObservable
{
    private Animator animator;
    private string state = "0";

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (photonView.IsMine) // 로컬 플레이어만 입력을 받음
        {
            // 로컬에서 애니메이션 파라미터를 업데이트

            // 현재 EventSystem에서 활성화된 InputModule 사용
            StandaloneInputModule inputModule = EventSystem.current.currentInputModule as StandaloneInputModule;

            if (inputModule != null)
            {
                // 현재 눌린 키코드 가져오기
                if (Input.anyKeyDown)
                {
                    string keyPressed = Input.inputString;  // 입력된 키값
                    Debug.Log("Pressed key: " + keyPressed);
                    state = keyPressed;
                }
            }
            animator.SetTrigger(state);
        }
        else
        {
            // 다른 플레이어는 동기화된 값으로 애니메이터 업데이트
            animator.SetTrigger(state);
        }
    }

    // Photon PUN의 데이터 동기화 메서드
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) // 로컬 플레이어의 데이터를 전송
        {
            stream.SendNext(state); // 동기화할 애니메이터 파라미터 전송
        }
        else // 다른 플레이어로부터 데이터를 수신
        {
            state = (string)stream.ReceiveNext(); // 받은 데이터를 로컬 변수에 저장
        }
    }
}
