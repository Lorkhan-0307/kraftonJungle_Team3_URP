using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;  // NavMeshModifier를 포함하는 네임스페이스
using UnityEditor;

public class RemoveNavMeshModifiersEditor : MonoBehaviour
{
    [MenuItem("Tools/Remove NavMesh Modifiers")]
    public static void RemoveModifiersInSelectedObject()
    {
        // 현재 선택된 오브젝트
        GameObject selectedObject = Selection.activeGameObject;

        if (selectedObject == null)
        {
            Debug.LogWarning("오브젝트를 선택하세요.");
            return;
        }

        // 선택된 오브젝트와 모든 자식들에 대해 NavMeshModifier 삭제
        NavMeshModifier[] modifiers = selectedObject.GetComponentsInChildren<NavMeshModifier>();

        foreach (var modifier in modifiers)
        {
            Undo.DestroyObjectImmediate(modifier); // Undo 기능을 사용하여 에디터에서 바로 삭제
        }

        Debug.Log("NavMesh Modifiers가 삭제되었습니다.");
    }
}
