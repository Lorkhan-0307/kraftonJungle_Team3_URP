using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Photon.Pun;
using System.Linq;
public class Heartbeat : MonoBehaviour
{
    private AudioSource audioSource;
    private float distance;

    [SerializeField] private GameObject Monster;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
    }

    private void Update()
    {

        if (!NetworkManager.Instance || NetworkManager.Instance.IsMonster()) return;

        if (Monster == null)
        {
            List<GameObject> Monsters = NetworkManager.Instance.Monsters.Values.Select(monster => monster.gameObject).ToList();
            if (Monsters.Count > 0)
            {
                Monster = Monsters[0];
                Debug.Log("Hearbeat.cs :: 몬스터 찾음. 이름은 " + Monster.name);
            }
            else
            {
                Debug.Log("Hearbeat.cs :: 몬스터가 없음");
                return;
            }
        }

        // 밤이고, 관전 중이 아닐 때, 몬스터와 가까워질수록 빠르고 크게 심장소리 재생
        if (!TimeManager.instance.isDay && !SpectatorManager.instance.isSpectating)
        {
            Debug.Log("Hearbeat.cs :: 심장소리 재생");
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            else
            {
                // 거리에 따라 볼륨과 피치를 조절
                distance = Vector3.Distance(Monster.transform.position, transform.position);
                audioSource.volume = (float)(1 - 0.3 * (Mathf.Clamp(distance, 50f, 100f) - 50f) / 50f);
                audioSource.pitch = (float)(1.5 - 0.5 * (Mathf.Clamp(distance, 0f, 50f) / 50f));
            }
        }
        else
        {
            Debug.Log("Hearbeat.cs :: 심장소리 멈춤");
            audioSource.Stop();
        }




    }
}