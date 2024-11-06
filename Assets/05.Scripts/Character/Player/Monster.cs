using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Monster : Player
{
    private GameObject hungerParticle;
    private GameObject hungerCanvas;

    [SerializeField] private AudioSource monster_kill_sound;
    [SerializeField] private GameObject scientistObj;
    [SerializeField] private GameObject monsterObj;

    private PlayerMovement playerMovement;

    //private bool isAttacking = false;
    //private bool isDayShiftedWhileAttacking = false;

    [SerializeField] AnimationSync aniSync;
    [SerializeField] private CinemachineVirtualCamera cvc;
    [SerializeField] private int vc_original_priority = 5;
    [SerializeField] private int vc_lookat_priority = 20;


    [SerializeField] private PlayableDirector transformationDirector;
    [SerializeField] private PlayableDirector transformationDirectorWithoutCam;

    private MouseComponent mc;

    private bool isAttacking = false;

    private GameObject _victim;


    private bool wasDay = false;

    private void Start()
    {
        playerMovement = GetComponentInChildren<PlayerMovement>();
        mc = GetComponentInChildren<MouseComponent>();
    }

    public override void OnAttack(GameObject victim)
    {
        base.OnAttack(victim);
        _victim = victim;

        OnTransformation(TimeManager.instance.GetisDay());

        if(playerMovement != null) playerMovement.isMovable = false;
        //TransitionCamera(true);

        // Todo: hunger time reset
        // Monster 에게만
        switch (victim.GetComponent<Player>().type)
        {
            case CharacterType.NPC:

                Debug.Log("Your gauge is Max");
                FindObjectOfType<HungerSlider>().SetHungerMax();
                NEHungerGauge.HungerEvent(false);

                break;
        }

        //MonsterKillSoundPlay();
        //GameManager.instance.OnKilled += MonsterKillSoundPlay();

    }

    public void TransitionCamera(bool isThird)
    {
        if (isThird)
        {
            cvc.Priority = vc_lookat_priority;
            playerMovement.SetLayerRecursive(monsterObj, 0);
            // Monster FPS 팔 끄기
            playerMovement.monsterFPS.SetActive(false);
        }
        else
        {
            cvc.Priority = vc_original_priority;
            playerMovement.SetLayerRecursive(monsterObj, 3);
        }
    }

    public override void OnDamaged(GameObject attacker)
    {
        base.OnDamaged(attacker);
        
        // 여기서 Destroy 결과 전송
        if (GetComponent<PhotonView>().AmOwner)
        {
            //PhotonNetwork.Destroy(this.gameObject);
            NEPlayerDeath.PlayerDeath();
            SpectatorManager.instance.StartSpectating();
        }
        else
        {
            SpectatorManager.instance.RemoveRemainingPlayer(this.gameObject);
        }
    }

    public override void OnDead()
    {
        base.OnDead();
    }

    public void OnNightVisibleScientist()
    {
        MonsterOutlineEffect moe = GetComponentInChildren<MonsterOutlineEffect>();
        moe.EnableOutlineEffect();
    }

    // 괴물의 밤시야 끄기
    public void OnDayUniteVisibilityScientist()
    {
        MonsterOutlineEffect moe = GetComponentInChildren<MonsterOutlineEffect>();
        moe.DisableOutlineEffect();
    }

    public void OnTransformationTimelineFinished()
    {
        if(transformationDirector.gameObject.activeInHierarchy) transformationDirector.gameObject.SetActive(false);
        if(transformationDirectorWithoutCam.gameObject.activeInHierarchy) transformationDirectorWithoutCam.gameObject.SetActive(false);
        bool isDay = TimeManager.instance.isDay;
        scientistObj.SetActive(isDay);
        monsterObj.SetActive(!isDay);
        if(playerMovement != null) playerMovement.isMovable = true;
        
        
        // 애니메이터 변환
        if (playerMovement)
        {
            playerMovement.animator = monsterObj.GetComponent<Animator>();
            playerMovement.OnMonsterFPS(false);
        }
        aniSync.ani = monsterObj.GetComponent<Animator>();
    }

    // 괴물 모습 변환
    public void OnTransformation(bool isAttackingInDay)
    {
        // 연구원 모습 비활성화
        scientistObj.SetActive(false);
        monsterObj.SetActive(true);
        // 애니메이터 변환
        if (playerMovement)
        {
            playerMovement.animator = monsterObj.GetComponent<Animator>();
            playerMovement.OnMonsterFPS(isAttackingInDay);
        }
        aniSync.ani = monsterObj.GetComponent<Animator>();
    }

    public void OffTransformation()
    {
        // 연구원 모습 활성화
        scientistObj.SetActive(true);
        monsterObj.SetActive(false);
        if (playerMovement)
        {
            // 애니메이터 변환
            playerMovement.animator = scientistObj.GetComponent<Animator>();
            playerMovement.OffMonsterFPS();
        }
        aniSync.ani = scientistObj.GetComponent<Animator>();
    }

    // Use this when Hunger Gauge reach 0
    public void OnHunger()
    {
        hungerParticle = GetComponentInChildren<ParticleSystem>(true).GameObject();
        hungerParticle.SetActive(true);
        if (NetworkManager.Instance.IsMonster())
        {
            hungerCanvas = GetComponentInChildren<HungerCanvasEffect>(true).GameObject();
            hungerCanvas.SetActive(true);
        }
        else
        {
            hungerCanvas = GetComponentInChildren<HungerCanvasEffect>(true).GameObject();
            hungerCanvas.SetActive(false);
        }
    }

    // Use this when Hunger Gauge reset
    public void NoHunger()
    {
        hungerParticle = GetComponentInChildren<ParticleSystem>(true).GameObject();
        hungerParticle.SetActive(false);
    }

    public override bool AttackDetection(GameObject target)
    {
        if (TimeManager.instance.GetisDay())
        {
            if (target.CompareTag("NPC")) return true;
        }
        else
        {
            if (target.CompareTag("Player")) return true;
        }
        return false;
    }

    public override void PlayKillSound()
    {
        base.PlayKillSound();
        monster_kill_sound.Play();
    }

    public void OnTransformationTimeline(bool isNeededCam)
    {
        // 만약 공격중이었다면 공격 끝나고 한다... 그거는 코드가 있으니까...
        if (isAttacking) return;
        
        scientistObj.SetActive(false);
        monsterObj.SetActive(false);
        
        
        if (isNeededCam)
        {
            transformationDirector.gameObject.SetActive(true);
            transformationDirector.Play();
            if (playerMovement != null)
            {
                playerMovement.isMovable = false;
                playerMovement.OffAllFPS();
            }
        }
        else
        {
            transformationDirectorWithoutCam.gameObject.SetActive(true);
            transformationDirectorWithoutCam.Play();
        }
        
    }

    public void SetupCinemachinBrainOnPlayableAssets()
    {
        TimelineAsset ta = transformationDirector.playableAsset as TimelineAsset;
        IEnumerable<TrackAsset> temp = ta.GetOutputTracks();
        foreach (var track in temp)
        {
            if (track is CinemachineTrack)
                transformationDirector.SetGenericBinding(track, FindObjectOfType<CinemachineBrain>());
        }

        ta = attackDirector.playableAsset as TimelineAsset;
        temp = ta.GetOutputTracks();
        foreach (var track in temp)
        {
            if (track is CinemachineTrack)
                attackDirector.SetGenericBinding(track, FindObjectOfType<CinemachineBrain>());
        }
    }


    #region AttackAni
    [SerializeField] private PlayableDirector attackDirector;
    [SerializeField] private PlayableDirector attackDirectorWithoutCam;

    public void OnAttackTimeLine(bool isNeededCam, GameObject victim)
    {
        isAttacking = true;
        scientistObj.SetActive(false);
        monsterObj.SetActive(false);

        wasDay = TimeManager.instance.isDay;

        _victim = victim;
        
        // Victim의 렌더러를 잠시 껐다가, 종료 후에 Victim의 시체를 보여준다.
        _victim.GetComponentInChildren<Animator>().transform.GetChild(0).gameObject.SetActive(false);




        if (isNeededCam)
        {
            attackDirector.gameObject.SetActive(true);
            attackDirector.Play();
            if (playerMovement != null)
            {
                playerMovement.isMovable = false;
                playerMovement.OffAllFPS();
            }
        }
        else
        {
            attackDirectorWithoutCam.gameObject.SetActive(true);
            attackDirectorWithoutCam.Play();
        }
    }

    // 공격 애니메이션이 끝난 시점에 시그널로 실행합니다.
    public void OnAttackFinished()
    {
        isAttacking = false;
        Debug.Log("1111");
        if (attackDirector.gameObject.activeInHierarchy) attackDirector.gameObject.SetActive(false);

        Debug.Log("2222");
        if (attackDirectorWithoutCam.gameObject.activeInHierarchy) attackDirectorWithoutCam.gameObject.SetActive(false);

        if (playerMovement)
        {
            Debug.Log("3333");
            playerMovement.isMovable = true;
            playerMovement.MonsterFPSOnTime();
            playerMovement.animator = scientistObj.GetComponent<Animator>();
        }
        //aniSync.ani = monsterObj.GetComponent<Animator>();

        Debug.Log("4444");

        bool _isDay = TimeManager.instance.isDay;

        if((wasDay && _isDay) || (!wasDay && _isDay))
        {
            // 낮 -> 낮
            // 인간으로 변신
            scientistObj.SetActive(true);
            monsterObj.SetActive(false);
            aniSync.ani = scientistObj.GetComponent<Animator>();
        }
        else if(wasDay && !_isDay){
            // 낮 -> 밤
            // TransformationTimeline 실행
            OnTransformationTimeline(NetworkManager.Instance.IsMonster());
        }
        
        _victim.GetComponentInChildren<Animator>().transform.GetChild(0).gameObject.SetActive(true);
        _victim.GetComponentInChildren<Animator>().Play("Standing React Death Left", 0, 95);
        _victim.GetComponent<BloodEffect>().OnBloodEffect();
        //OnTransformation(TimeManager.instance.isDay);
    }
    #endregion
}
