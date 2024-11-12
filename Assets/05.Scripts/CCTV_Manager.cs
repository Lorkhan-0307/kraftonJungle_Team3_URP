using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTV_Manager : MonoBehaviour
{
/*
 *  CCTV 매니저 사용법
 *
 * 1. 원하는 위치에 Cinemachine - virtual camera를 생성
 * 2. Virtual Camera Object에 Camera Component를 추가
 * 3. Manager에 cctv_cameras에 현재 카메라를 추가
 * 4. 원하는 TV 오브젝트(prefab)를 원하는 위치에 생성한 뒤, 같은 index 번호를 가지도록 cctvs에 추가.
 * 5. 완료!
 *
 * 주의: 반드시 cctvs와 cctv_cameras의 갯수가 동일할 것. cameras의 length에 의존하므로, 이를 주의. 
 */

    [SerializeField] private CCTV tv_screen;
    [SerializeField] private Camera[] cctv_cameras;

    private RenderTexture cRenderTexture;   

    private int currentCameraIndex;

    private void Start()
    {
        if (tv_screen == null) tv_screen = GetComponent<CCTV>();

        currentCameraIndex = 0;
        SetActiveCamera(currentCameraIndex);
    }

    public void SwitchCamera(int direction)
    {
        // 현재 카메라 비활성화
        cctv_cameras[currentCameraIndex].targetTexture = null;
        cctv_cameras[currentCameraIndex].gameObject.SetActive(false);

        // 인덱스 업데이트
        currentCameraIndex += direction;

        
        if (currentCameraIndex < 0)
            currentCameraIndex = cctv_cameras.Length - 1;
        else if (currentCameraIndex >= cctv_cameras.Length)
            currentCameraIndex = 0;

        // 새 카메라 활성화
        SetActiveCamera(currentCameraIndex);
    }
    
    private void SetActiveCamera(int index)
    {
        if (cRenderTexture != null)
        {
            cRenderTexture.Release();
            Destroy(cRenderTexture);
        }
        
        cRenderTexture = new RenderTexture(1024, 512, 24);
        cRenderTexture.graphicsFormat = UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8A8_UNorm;
        cRenderTexture.depthStencilFormat = UnityEngine.Experimental.Rendering.GraphicsFormat.D24_UNorm_S8_UInt;
        
        cctv_cameras[index].gameObject.SetActive(true);

        cctv_cameras[index].targetTexture = cRenderTexture;
        
        tv_screen.ApplyScreen(cRenderTexture);
    }
}
