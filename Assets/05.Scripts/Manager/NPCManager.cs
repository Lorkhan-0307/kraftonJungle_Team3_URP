using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NPCManager : MonoBehaviour
{
    public GameObject npcPrefab;
    // ������ NPC ��
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
            // �������� ��ġ ���
            Vector3 randomPosition = GetRandomNavMeshPosition();

            // NPC ����
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

    // NavMesh �� ���� ��ȿ ��ġ ã��
    Vector3 GetRandomNavMeshPosition()
    {
        // ��� NavMeshSurface ã��
        NavMeshSurface[] navMeshSurfaces = FindObjectsOfType<NavMeshSurface>();
        if (navMeshSurfaces.Length == 0)
        {
            // NavMeshSurface�� ���� ���
            Debug.LogWarning("No NavMeshSurface found.");
            return Vector3.zero;
        }

        // NavMeshSurface ���� ����
        NavMeshSurface selectedSurface = navMeshSurfaces[Random.Range(0, navMeshSurfaces.Length)];

        // ���õ� NavMeshSurface�� ����
        Bounds bounds = selectedSurface.GetComponent<Collider>().bounds;

        // bounds ������ ���� ��ġ ����
        Vector3 randomPoint = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            bounds.min.y,
            Random.Range(bounds.min.z, bounds.max.z)
        );

        NavMeshHit hit;
        // ���� ����� ��ȿ�� ��ġ ã��
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            // ��ȿ�� ��ġ ��ȯ
            return hit.position;
        }

        // ��ȿ�� NavMesh ��ġ�� ������
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
