using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab;
    // 스폰할 NPC 수
    [SerializeField]
    private int npcCount = 10;

    public Vector3 spawnAreaMin;  // 스폰 영역의 최소 좌표
    public Vector3 spawnAreaMax;  // 스폰 영역의 최대 좌표

    public string npcGroupName = "AllNPC";

    private GameObject alllNPC;

    public Vector3 spawnAreaCenter;
    public float spawnAreaRadius = 10f;

    public NavMeshSurface navMeshSurface;

    void Start()
    {
        alllNPC = new GameObject(npcGroupName);
        SpawnNPC();
    }

    void SpawnNPC()
    {
        for (int i = 0; i < npcCount; i++)
        {
            // 랜덤으로 위치 계산
            Vector3 randomPosition = GetRandomNavMeshPosition();

            // NPC 생성
            GameObject npc = Instantiate(npcPrefab, randomPosition, Quaternion.identity);
            npc.transform.parent = alllNPC.transform;
        }
    }

    // NavMesh 위 랜덤 유효 위치 찾기
    Vector3 GetRandomNavMeshPosition()
    {
        // 모든 NavMeshSurface 찾기
        NavMeshSurface[] navMeshSurfaces = FindObjectsOfType<NavMeshSurface>();
        if (navMeshSurfaces.Length == 0)
        {
            // NavMeshSurface가 없을 경우
            Debug.LogWarning("No NavMeshSurface found.");
            return Vector3.zero;
        }

        // NavMeshSurface 랜덤 선택
        NavMeshSurface selectedSurface = navMeshSurfaces[Random.Range(0, navMeshSurfaces.Length)];

        // 선택된 NavMeshSurface의 영역
        Bounds bounds = selectedSurface.GetComponent<Collider>().bounds;

        // bounds 내에서 랜덤 위치 생성
        Vector3 randomPoint = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            bounds.min.y, 
            Random.Range(bounds.min.z, bounds.max.z)
        );

        NavMeshHit hit;
        // 가장 가까운 유효한 위치 찾기
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            // 유효한 위치 반환
            return hit.position;
        }

        // 유효한 NavMesh 위치가 없으면
        return Vector3.zero;
    }
}
