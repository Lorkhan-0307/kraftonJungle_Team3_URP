using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NPCManager : MonoBehaviour
{
    public GameObject npcPrefab;
    // 스폰할 NPC 수
    [SerializeField]
    private int npcCount = 10;

    public string npcGroupName = "AllNPC";

    private GameObject NPCGroup;
    private GameObject[] allNPC;

    public float moveRadius = 10f;

    private float waitTimer;
    public float minWaitTime = 2f;
    public float maxWaitTime = 5f;

    void Start()
    {
        NPCGroup = new GameObject(npcGroupName);
        allNPC = new GameObject[npcCount];
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
            allNPC[i] = npc;
            npc.transform.parent = NPCGroup.transform;
            SetNewDestination(npc.GetComponent<NavMeshAgent>());
        }
    }

    void Update()
    {
        for (int i = 0; i < npcCount; i++)
        {
            NavMeshAgent agent = allNPC[i].GetComponent<NavMeshAgent>();
            if (agent.remainingDistance < 0.1f)
            {
                if (waitTimer <= 0)
                {
                    SetNewDestination(agent);
                    waitTimer = Random.Range(minWaitTime, maxWaitTime);
                }
                else
                {
                    waitTimer -= Time.deltaTime;
                }
            }
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

    void SetNewDestination(NavMeshAgent agent)
    {
        Vector3 randomDirection = Random.insideUnitSphere * moveRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, moveRadius, 1);
        agent.SetDestination(hit.position);
    }
}
