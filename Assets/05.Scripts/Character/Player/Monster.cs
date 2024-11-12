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
    private GameObject hungerOutline;
    private GameObject hungerCanvas;

    [SerializeField] private AudioSource monster_kill_sound;
    [SerializeField] private GameObject scientistObj;
    [SerializeField] private GameObject monsterObj;

    private PlayerMovement monsterMovement;
    private PlayerMovement playerMovement;

    [SerializeField] AnimationSync aniSync;
    [SerializeField] private CinemachineVirtualCamera cvc;
    [SerializeField] private int vc_original_priority = 5;
    [SerializeField] private int vc_lookat_priority = 20;


    [SerializeField] private PlayableDirector transformationDirector;
    [SerializeField] private PlayableDirector transformationDirectorWithoutCam;

    [SerializeField] private PlayableDirector deadDirector;

    private MouseComponent mc;

    private bool isAttacking = false;

    private GameObject _victim;
    //private GameObject _attacker;


    private bool wasDay = false;

    private void Start()
    {
        monsterMovement = GetComponentInChildren<PlayerMovement>();
        mc = GetComponentInChildren<MouseComponent>();
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    public override void OnAttack(GameObject victim)
    {
        base.OnAttack(victim);
        _victim = victim;

        OnTransformation(TimeManager.instance.GetisDay());

        if(monsterMovement != null) monsterMovement.isMovable = false;
        //TransitionCamera(true);

        // victim pos 값 timeline 기준으로 변경
        Vector3 targetPos = transform.Find("MonsterKill/astronaut").position;

        victim.transform.position = targetPos;

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

    // public void TransitionCamera(bool isThird)
    // {
    //     if (isThird)
    //     {
    //         cvc.Priority = vc_lookat_priority;
    //         monsterMovement.SetLayerRecursive(monsterObj, 0);
    //         // Monster FPS 팔 끄기
    //         monsterMovement.monsterFPS.SetActive(false);
    //     }
    //     else
    //     {
    //         cvc.Priority = vc_original_priority;
    //         monsterMovement.SetLayerRecursive(monsterObj, 3);
    //     }
    // }

    public override void OnDamaged(GameObject attacker)
    {
        base.OnDamaged(attacker);
        
        // 여기서 Destroy 결과 전송
        if (GetComponent<PhotonView>().AmOwner)
        {
            //PhotonNetwork.Destroy(this.gameObject);
            OnDeadTimeline(attacker);
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
        if(monsterMovement != null) monsterMovement.isMovable = true;
        
        
        // 애니메이터 변환
        if (monsterMovement)
        {
            monsterMovement.animator = monsterObj.GetComponent<Animator>();
            monsterMovement.OnMonsterFPS(false);
        }
        aniSync.ani = monsterObj.GetComponent<Animator>();
    }

    public void OnDeadTimelineFinished()
    {
        Debug.Log("Dead Timeline Finished");
        //if (_attacker)
        //    // 공격자 모델 켜기
        //    _attacker.SetActive(false);
        if (GetComponent<PhotonView>().IsMine)
        {
            //Transform playerObjectTransform = transform.Find("PlayerObjects(Clone)");
            //if (playerObjectTransform) Destroy(playerObjectTransform.gameObject);
            // timeline이 끝나는 시점에 호출
            NEPlayerDeath.PlayerDeath();
            SpectatorManager.instance.StartSpectating();
        }
    }

    // 괴물 모습 변환
    public void OnTransformation(bool isAttackingInDay)
    {
        // 연구원 모습 비활성화
        scientistObj.SetActive(false);
        monsterObj.SetActive(true);
        // 애니메이터 변환
        if (monsterMovement)
        {
            monsterMovement.animator = monsterObj.GetComponent<Animator>();
            monsterMovement.OnMonsterFPS(isAttackingInDay);
        }
        aniSync.ani = monsterObj.GetComponent<Animator>();
    }

    public void OffTransformation()
    {
        // 연구원 모습 활성화
        scientistObj.SetActive(true);
        monsterObj.SetActive(false);
        if (monsterMovement)
        {
            // 애니메이터 변환
            monsterMovement.animator = scientistObj.GetComponent<Animator>();
            monsterMovement.OffMonsterFPS();
        }
        aniSync.ani = scientistObj.GetComponent<Animator>();
    }

    // Use this when Hunger Gauge reach 0
    public void OnHunger()
    {
        // particle systen on
        // hungerParticle = GetComponentInChildren<ParticleSystem>(true).GameObject();
        // hungerParticle.SetActive(true);
        
        hungerOutline = scientistObj.transform.Find("Renderer/Outline").gameObject;
        if(playerMovement)
            playerMovement.SetLayerRecursive(hungerOutline, 6);
        
        if (NetworkManager.Instance.IsMonster())
        {
            Debug.Log("On Hunger Canvas");
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
        Debug.Log("No Hunger");
        // particle system off
        // hungerParticle = GetComponentInChildren<ParticleSystem>(true).GameObject();
        // hungerParticle.SetActive(false);
        
        if(hungerCanvas)
            hungerCanvas.SetActive(false);

        if (hungerOutline && playerMovement){
            playerMovement.SetLayerRecursive(hungerOutline, 0);
        }

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
            if (monsterMovement != null)
            {
                monsterMovement.isMovable = false;
                monsterMovement.OffAllFPS();
            }
        }
        else
        {
            transformationDirectorWithoutCam.gameObject.SetActive(true);
            transformationDirectorWithoutCam.Play();
        }
        
    }

    public void OnDeadTimeline(GameObject attacker)
    {
        Debug.Log("Dead Timeline");
        scientistObj.SetActive(false);
        monsterObj.SetActive(false);

        // 공격자 모델 끄기
        //_attacker = attacker;
        //attacker.SetActive(false);
        attacker.GetComponentInChildren<Animator>().gameObject.SetActive(false);

        deadDirector.gameObject.SetActive(true);
        deadDirector.Play();
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

        ta = deadDirector.playableAsset as TimelineAsset;
        temp = ta.GetOutputTracks();
        foreach (var track in temp)
        {
            if (track is CinemachineTrack)
                deadDirector.SetGenericBinding(track, FindObjectOfType<CinemachineBrain>());
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
            if (monsterMovement != null)
            {
                monsterMovement.isMovable = false;
                monsterMovement.OffAllFPS();
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
        if (attackDirector.gameObject.activeInHierarchy) attackDirector.gameObject.SetActive(false);

        if (attackDirectorWithoutCam.gameObject.activeInHierarchy) attackDirectorWithoutCam.gameObject.SetActive(false);

        if (monsterMovement)
        {
            monsterMovement.isMovable = true;
            monsterMovement.MonsterFPSOnTime();
            monsterMovement.animator = scientistObj.GetComponent<Animator>();
        }


        bool _isDay = TimeManager.instance.isDay;

        if((wasDay && _isDay) || (!wasDay && _isDay))
        {
            // 낮 -> 낮
            // 인간으로 변신
            scientistObj.SetActive(true);
            monsterObj.SetActive(false);
            aniSync.ani = scientistObj.GetComponent<Animator>();
        }
        else{
            // 낮 -> 밤
            // TransformationTimeline 실행
            //OnTransformationTimeline(NetworkManager.Instance.IsMonster());
            // 연구원 모습 비활성화
            scientistObj.SetActive(false);
            monsterObj.SetActive(true);
            // 애니메이터 변환
            if (monsterMovement)
            {
                monsterMovement.animator = monsterObj.GetComponent<Animator>();
                monsterMovement.OnMonsterFPS(false);
            }
            aniSync.ani = monsterObj.GetComponent<Animator>();
        }
        
        _victim.GetComponentInChildren<Animator>().transform.GetChild(0).gameObject.SetActive(true);
        _victim.GetComponentInChildren<Animator>().Play("Standing Death Forward", 0, 95);
        _victim.GetComponent<BloodEffect>().OnBloodEffect();
    }
    #endregion
}
