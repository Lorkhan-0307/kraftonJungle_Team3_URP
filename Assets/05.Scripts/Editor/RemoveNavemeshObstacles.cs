using UnityEngine;
using UnityEngine.AI; // NavMeshObstacle을 사용하기 위한 네임스페이스
using UnityEditor;

public class RemoveNavMeshObstaclesEditor : MonoBehaviour
{
    [MenuItem("Tools/Remove NavMesh Obstacles")]
    public static void RemoveObstaclesInSelectedObject()
    {
        // 현재 선택된 오브젝트를 가져옴
        GameObject selectedObject = Selection.activeGameObject;

        if (selectedObject == null)
        {
            Debug.LogWarning("오브젝트를 선택하세요.");
            return;
        }

        // 선택된 오브젝트와 모든 자식들에 대해 NavMeshObstacle 삭제
        NavMeshObstacle[] obstacles = selectedObject.GetComponentsInChildren<NavMeshObstacle>();

        foreach (var obstacle in obstacles)
        {
            Undo.DestroyObjectImmediate(obstacle); // Undo 시스템을 사용하여 에디터에서 바로 삭제
        }

        Debug.Log("NavMesh Obstacles가 삭제되었습니다.");
    }
}