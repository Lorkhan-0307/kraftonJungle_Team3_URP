using Photon.Pun;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NPCManager : Singleton<NPCManager>
{
    public string npcPrefab;
    // 스폰할 NPC 수
    public int npcCount = 10;

    public string npcGroupName = "AllNPC";

    private GameObject NPCGroup;
    public List<GameObject> allNPC = new List<GameObject>();

    public float moveRadius = 20f;

    private float waitTimer;
    public float minWaitTime = 0.02f;
    public float maxWaitTime = 0.05f;

    void Start()
    {
        NPCGroup = new GameObject(npcGroupName);
    }

    // 호스트만 호출
    public void SpawnNPC()
    {
        for (int i = 0; i < npcCount; i++)
        {
            // 랜덤으로 위치 계산
            Vector3 randomPosition = GetRandomNavMeshPosition();

            // NPC 생성
            GameObject npc = PhotonNetwork.Instantiate(npcPrefab, randomPosition, Quaternion.identity);
            allNPC.Add(npc);
            NavMeshAgent agent = npc.GetComponent<NavMeshAgent>();
            // 우선순위를 NPC마다 다르게 설정
            agent.avoidancePriority = Random.Range(0, 100);
            npc.transform.parent = NPCGroup.transform;
            SetNewDestination(npc.GetComponent<NavMeshAgent>());
        }
    }

    void Update()
    {
        // 호스트만 동작
        // 낮일 때만 작동
        if (NetworkManager.Instance.IsServer() && TimeManager.instance.GetisDay())
        {
            foreach (GameObject npc in allNPC)
            {
                NavMeshAgent agent = npc.GetComponent<NavMeshAgent>();
                if (agent.remainingDistance < 0.1f)
                {
                    SetNewDestination(agent);
                }
            }
        }
    }

    public void FindAllNPC()
    {
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");
        Debug.Log($"NPC Count : {npcs.Length}");

        // 리스트 초기화 후 NPC 추가
        allNPC.Clear();
        allNPC.AddRange(npcs);
    }

    // NavMesh 위 랜덤 유효 위치 찾기
    public static Vector3 GetRandomNavMeshPosition()
    {
        // 모든 NavMeshSurface 찾기
        NavMeshSurface[] navMeshSurfaces = FindObjectsOfType<NavMeshSurface>();
        if (navMeshSurfaces.Length == 0)
        {
            // NavMeshSurface가 없을 경우
            Debug.LogWarning("No NavMeshSurface found.");
            return GetRandomFallbackPos();
        }

        int maxAttempts = 5;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
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
        }

        // 유효한 NavMesh 위치가 없으면
        Debug.LogWarning($"Failed to find a valid NavMesh position after {maxAttempts} attempts.");
        return GetRandomFallbackPos();
    }

    private static Vector3 GetRandomFallbackPos()
    {
        // Fallback 위치 범위
        float minX = 3f, maxX = 40f;
        float minZ = 3f, maxZ = 27f;
        float y = 0f;

        return new Vector3(
            Random.Range(minX, maxX),
            y,
            Random.Range(minZ, maxZ)
        );
    }

    void SetNewDestination(NavMeshAgent agent)
    {
        Vector3 randomDirection = Random.insideUnitSphere * moveRadius;
        // NPC의 현재 위치 더하기
        randomDirection += agent.transform.position;

        NavMeshPath path = new NavMeshPath();
        if (NavMesh.CalculatePath(agent.transform.position, randomDirection, NavMesh.AllAreas, path))
        {
            //Debug.DrawLine(agent.transform.position, randomDirection, Color.red);
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                agent.SetDestination(randomDirection);
                return;
            }
            //Debug.LogWarning("Path can't be completed");
        }
        // 유효한 경로를 찾지 못했을 경우
        //Debug.LogWarning("Could not find valid path");
    }

    public void SetAble()
    {
        // 각 NPC 오브젝트를 활성화
        foreach (GameObject npc in allNPC)
        {
            // NPC 활성화
            npc.SetActive(true);
        }
    }

    public void SetDisable()
    {
        FindAllNPC();
        // 각 NPC 오브젝트를 비활성화
        foreach (GameObject npc in allNPC)
        {
            // NPC 비활성화
            npc.SetActive(false);
        }
    }
}
