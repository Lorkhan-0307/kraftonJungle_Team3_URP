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
    
    
    [SerializeField] private CCTV[] cctvs;
    [SerializeField] private Camera[] cctv_cameras;

    [SerializeField] private RenderTexture originalRenderTexture;

    private void Start()
    {
        for (int index = 0; index < cctv_cameras.Length; index++)
        {
            Debug.Log("doing " + index + " Camera");
            RenderTexture currentRenderTexture = new RenderTexture(originalRenderTexture);
            cctv_cameras[index].targetTexture = currentRenderTexture;
            cctvs[index].ApplyScreen(currentRenderTexture);
        }
    }
}
