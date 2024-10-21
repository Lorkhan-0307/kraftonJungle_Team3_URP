using UnityEngine;
using UnityEngine.Rendering;

public class MonsterOutlineEffect : MonoBehaviour
{
    public Camera monsterCamera; // 괴물 플레이어의 카메라
    public Material[] outlineMaterials; // 여러 아웃라인 마테리얼 배열
    private CommandBuffer commandBuffer; // Command Buffer 객체

    private void Start()
    {
        // 부모의 부모 오브젝트에서 Monster 스크립트를 확인
        if (transform.parent != null && transform.parent.parent != null)
        {
            Monster monsterScript = transform.parent.parent.GetComponent<Monster>();
            if (monsterScript != null)
            {
                // Monster 스크립트가 있을 때만 Command Buffer를 설정할 준비
                commandBuffer = new CommandBuffer();
                commandBuffer.name = "Monster Outline Effect";
            }
            else
            {
                Debug.LogWarning("Monster 스크립트가 부모의 부모 오브젝트에 없습니다.");
            }
        }
    }

    // Command Buffer를 활성화하여 아웃라인 효과를 켭니다
    public void EnableOutlineEffect()
    {
        if (commandBuffer != null && monsterCamera != null)
        {
            GameObject[] scientistOutlineEmmisions = GameObject.FindGameObjectsWithTag("ScientistOutline");
            foreach (GameObject soe in scientistOutlineEmmisions)
            {
                soe.SetActive(true);
            }
            // 모든 씬의 Renderer를 검색하여 조건에 맞는 마테리얼 찾기
            Renderer[] allRenderers = FindObjectsOfType<Renderer>();
            foreach (Renderer renderer in allRenderers)
            {
                // 모든 마테리얼 검사
                foreach (Material mat in renderer.sharedMaterials)
                {
                    // 아웃라인 마테리얼 배열에 포함된 경우만 커맨드 버퍼에 추가
                    if (IsOutlineMaterial(mat))
                    {
                        commandBuffer.DrawRenderer(renderer, mat);
                        break; // 첫 번째 아웃라인 마테리얼을 찾으면 중단
                    }
                }
            }

            // 카메라에 Command Buffer 추가
            monsterCamera.AddCommandBuffer(CameraEvent.AfterForwardOpaque, commandBuffer);
            Debug.Log("아웃라인 효과가 활성화되었습니다.");
        }
    }

    // Command Buffer를 비활성화하여 아웃라인 효과를 끕니다
    public void DisableOutlineEffect()
    {
        if (commandBuffer != null && monsterCamera != null)
        {
            GameObject[] scientistOutlineEmmisions = GameObject.FindGameObjectsWithTag("ScientistOutline");
            foreach (GameObject soe in scientistOutlineEmmisions)
            {
                soe.SetActive(false);
            }
            // 커맨드 버퍼를 카메라에서 제거
            monsterCamera.RemoveCommandBuffer(CameraEvent.AfterForwardOpaque, commandBuffer);
            Debug.Log("아웃라인 효과가 비활성화되었습니다.");
        }
    }

    // 특정 마테리얼이 아웃라인 마테리얼 배열에 포함되어 있는지 확인
    private bool IsOutlineMaterial(Material material)
    {
        foreach (Material outlineMaterial in outlineMaterials)
        {
            if (material == outlineMaterial)
            {
                return true;
            }
        }
        return false;
    }
}
