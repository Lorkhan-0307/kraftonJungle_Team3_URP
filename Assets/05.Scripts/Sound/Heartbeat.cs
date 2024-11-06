using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
public class Heartbeat : MonoBehaviour
{
    [SerializeField] private AudioSource heartbeatSound;

    private GameObject Monster;


    private void Update()
    {
        if (!NetworkManager.Instance || NetworkManager.Instance.IsMonster()) return;

        if (Monster == null)
        {
            List<GameObject> Monsters = NetworkManager.Instance.Monsters.Values.Select(monster => monster.gameObject).ToList();
            if (Monsters.Count > 0)
            {
                Monster = Monsters[0];
            }
            else
            {
                return;
            }
        }

        // 밤이고, 관전 중이 아닐 때, 몬스터와 가까워질수록 빠르고 크게 심장소리 재생
        if (!TimeManager.instance.isDay && !SpectatorManager.instance.isSpectating)
        {
            float distance = Vector3.Distance(Monster.transform.position, transform.position);
            if (distance < 10f)
            {
                heartbeatSound.volume = 2f;
                heartbeatSound.pitch = 2f;
            }
            else if (distance < 20f)
            {
                heartbeatSound.volume = 1f;
                heartbeatSound.pitch = 1f;
            }
            else
            {
                heartbeatSound.volume = 0.5f;
                heartbeatSound.pitch = 0.5f;
            }
        }
        else
        {
            heartbeatSound.volume = 0f;
        }
    }
}