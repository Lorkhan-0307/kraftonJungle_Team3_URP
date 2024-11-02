using UnityEngine;

public class BloodEffect : MonoBehaviour
{
    [SerializeField] private GameObject bloodDecalPrefab;

    public void OnBloodEffect()
    {
        //Debug.Log("On Blood Effect");
        // 현재 위치 위에서 Raycast
        RaycastHit hit;
        Vector3 pos = transform.position + Vector3.up * 0.1f;

        if (Physics.Raycast(pos, Vector3.down, out hit))
        {
            // 바닥에 Decal 생성
            Quaternion rotation = Quaternion.LookRotation(-hit.normal);
            Instantiate(bloodDecalPrefab, hit.point, rotation);
        }
    }
}