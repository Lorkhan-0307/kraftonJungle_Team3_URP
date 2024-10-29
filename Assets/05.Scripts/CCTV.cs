using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CCTV : Interact
{
    [SerializeField] private MeshRenderer screenMeshRenderer;
    [SerializeField] private Material originalMaterial;
    [SerializeField] private CinemachineVirtualCamera cvc;
    [SerializeField] private int vc_original_priority = 5;
    [SerializeField] private int vc_lookat_priority = 5;

    private RenderTexture _screenTexture;

    public void ApplyScreen(RenderTexture screenTexture)
    {
        _screenTexture = screenTexture;
        Material curScreenMat = new Material(originalMaterial);
        curScreenMat.SetTexture("_BaseMap", _screenTexture);

        Material[] materials = screenMeshRenderer.materials;
        materials[1] = curScreenMat;
        screenMeshRenderer.materials = materials;

    }

    public override void Interaction()
    {
        base.Interaction();
        
        // 여기에서는 이제, Interact 하면 화면 안으로 쭉 크게 보고, 아니면 바꾼다...?
        
        // 근데 TV 화면을 무슨수로 앞으로 땡길꺼지? 똑같이? 아니면 그냥 캠으로 바꿔야하나?
        // 하긴 virtual camera pos 를 바꾸면 되기는 할꺼야...
        // 먼저 현재 virtual camera의 priority가 10 이니까, 11로 높혔다가 5로 낮춘다.
        // Ease in out 시간이 현재 2초로 되어있다.(이는 Cinemachine Blend에 있다)
        
        SwitchCamera();
        
        
    }

    private void SwitchCamera()
    {
        //StartCoroutine(TransitionCameraCoroutine());
        
        GetComponentInParent<CCTV_Manager>().SwitchCamera(1);
    }


    private IEnumerator TransitionCameraCoroutine()
    {
        isInteractable = false;
        if (cvc.Priority == vc_original_priority)
        {
            // 이 경우, 이미 interact해서 안에 들어와 있는 경우이다.
            cvc.Priority = vc_lookat_priority;
        }
        else
        {
            cvc.Priority = vc_original_priority;
        }
        
        CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();
        float blendDuration = brain.m_DefaultBlend.BlendTime;

        // Blend Duration만큼 대기
        yield return new WaitForSeconds(blendDuration);

        // Transition 완료 후 isInteractable을 다시 true로 설정
        isInteractable = true;
        
    }
}
