using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;  // NavMeshModifier를 포함하는 네임스페이스
using UnityEditor;

public class RemoveNavMeshSurfaceEditor : MonoBehaviour
{
    [MenuItem("Tools/Remove NavMesh Surfaces")]
    public static void RemoveSurfacesInSelectedObject()
    {
        // 현재 선택된 오브젝트
        GameObject selectedObject = Selection.activeGameObject;

        if (selectedObject == null)
        {
            Debug.LogWarning("오브젝트를 선택하세요.");
            return;
        }

        NavMeshSurface[] surfaces = selectedObject.GetComponentsInChildren<NavMeshSurface>();

        foreach (var surface in surfaces)
        {
            // Undo 기능을 사용하여 에디터에서 바로 삭제
            Undo.DestroyObjectImmediate(surface); 
        }

        Debug.Log("NavMesh Surfaces가 삭제되었습니다.");
    }

    [MenuItem("Tools/Bake NavMesh Surfaces")]
    public static void BakeSurfacesInSelectedObject()
    {
        // 현재 선택된 오브젝트
        GameObject selectedObject = Selection.activeGameObject;

        if (selectedObject == null)
        {
            Debug.LogWarning("오브젝트를 선택하세요.");
            return;
        }

        NavMeshSurface[] surfaces = selectedObject.GetComponentsInChildren<NavMeshSurface>();

        foreach (var surface in surfaces)
        {
            surface.BuildNavMesh();
        }

        Debug.Log("NavMesh Surfaces가 빌드되었습니다.");
    }

    [MenuItem("Tools/Clean NavMesh Surfaces")]
    public static void CleanSurfacesInSelectedObject()
    {
        // 현재 선택된 오브젝트
        GameObject selectedObject = Selection.activeGameObject;

        if (selectedObject == null)
        {
            Debug.LogWarning("오브젝트를 선택하세요.");
            return;
        }

        NavMeshSurface[] surfaces = selectedObject.GetComponentsInChildren<NavMeshSurface>();

        foreach (var surface in surfaces)
        {
            surface.RemoveData();
        }

        Debug.Log("NavMesh Surfaces가 클린되었습니다.");
    }
}
