using UnityEngine;

public class MonsterAnimation : MonoBehaviour
{
    // Monster가 Attack 애니메이션을 실행한 후 실행할 로직
    public void Ate()
    {
        // 부모 오브젝트
        Transform parentTransform = transform.parent;
        if (NetworkManager.Instance.myPlayer == parentTransform.gameObject)
        {
            Transform monsterTransform = transform.parent;
            Monster m = monsterTransform.gameObject.GetComponent<Monster>();
            // 낮이면
            if(TimeManager.instance.GetisDay())
                m.OffTransformation();
            // 항상
            m.TransitionCamera(false);

            PlayerMovement playerMovement = parentTransform.GetComponentInChildren<PlayerMovement>();
            MouseComponent mc = parentTransform.GetComponentInChildren<MouseComponent>();

            mc.isAttacking = false;
            playerMovement.isAttacking = false;
        }
    }
}
