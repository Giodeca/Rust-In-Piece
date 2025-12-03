using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Entity, ISavable<PlayerSaveData>
{
    public Vector3 MoveInput { get; set; }
    public InputManager InputManager { get; set; }
    private float attackCooldownTimer;

    // QUI IL CODICE BRUTTO
    // EH SI PROPRIO QUI
    [Header("Transitions")]
    private GameObject startTransition;
    public GameObject endTransition { get; set; }

    [Header("Damaged VFX")]
    public GameObject walkVFX;
    public GameObject repairVFX;

    [Header("Dash Collision Infos")]
    public LayerMask maskToExclude;
    public LayerMask defaultExclude;

    [Header("Animator")]
    public Animator weaponAnim;
    [SerializeField] private float attackCooldown;

    [Header("JumpModule")]
    public float jumpForce;
    public bool hasDoubleJump;
    public float descendCoefficient;
    public float coyoteTime;
    //public float jumpBufferTime;
    //public float jumpBufferCounter { get; set; }
    public float coyoteTimeCounter { get; set; }
    public float originalGravityScale { get; set; }
    //public bool JumpCancelled { get; set; }

    [Header("Shoot State")]
    public Transform projSpawner;
/*    [SerializeField] private Projectile projectile;
    [SerializeField] private Projectile chargedProj;*/
    [SerializeField] private float shootCooldown;
    [SerializeField] private float chargedShotTime;
    private float shootCooldownTimer;
    [HideInInspector] public float chargedShotTimeCounter;
    public float ChargedShotTime { get => chargedShotTime; }


    [Header("Dash Module")]
    [SerializeField] private int maxDashUses;
    [HideInInspector] public int dashUses;
    public float dashSpeed;
    public float dashDuration;
    public float dashCooldown;
    [HideInInspector] public float dashCooldownTimer;
    public float dashDir { get; private set; }

    [Header("Repair Module")]
    public float repairTime;
    public float RepairCounter { get; set; }
    [SerializeField] private float repairCostNormal;
    [SerializeField] private float repairCostDamaged;
    public float RepairCost { get; set; }



    [SerializeField] private Animator anim;


    [Header("ModuleSystem")]
    [SerializeField] private List<ModuleSystem> moduleSystems;

    public List<IModule> playerModules;
    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerAirState AirState { get; private set; }
    public PlayerShootState ShootState { get; private set; }
    public PlayerChargedShotState ChargedShotState { get; private set; }
    public PlayerRepairState RepairState { get; private set; }
    public PlayerStoppedState StoppedState { get; private set; }
    public PlayerDamagedState DamagedState { get; private set; }
    private bool playFirstSound = true;
    public float stepInterval = 0.5f;

    private bool isSound1Played = false;
    private bool isSound2Played = false;
    private bool isSound3Played = false;
    public int damagedModuleCounter { get; set; }

    protected override void Awake()
    {
        base.Awake();


        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, "Idle");
        MoveState = new PlayerMoveState(this, StateMachine, "Move");
        DashState = new PlayerDashState(this, StateMachine, "Dash");
        JumpState = new PlayerJumpState(this, StateMachine, "Jump");
        AirState = new PlayerAirState(this, StateMachine, "Jump");

        ShootState = new PlayerShootState(this, StateMachine, "Shoot");
        ChargedShotState = new PlayerChargedShotState(this, StateMachine, "Shoot");

        RepairState = new PlayerRepairState(this, StateMachine, "Repair");
        StoppedState = new PlayerStoppedState(this, StateMachine, "Stopped");
        DamagedState = new PlayerDamagedState(this, StateMachine, "Damaged");

        playerModules = new List<IModule>() { DashState, JumpState, ShootState, ChargedShotState, RepairState };

    }
    private void OnEnable()
    {

    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        startTransition = GameObject.FindGameObjectWithTag("StartTransition");
        startTransition.SetActive(false);
        endTransition = GameObject.FindGameObjectWithTag("EndTransition");
        endTransition.SetActive(false);

        StartCoroutine(StartTransition());

        InputManager = InputManager.Instance;

        StateMachine.Initialize(IdleState);
        dashUses = maxDashUses;

        originalGravityScale = Rb.gravityScale;



        if (File.Exists(SaveSystem.SaveFileName()))
            EventManager.CalledLoad?.Invoke();

        HudManager.Instance.OnUpdateUI();
        GameManager.Instance.SaveAsync();
        // Da sistemare logica livelli
        //GameManager.Instance.SetModulesStates(playerModules);
        ////GameManager.Instance.SaveAsync();
        ///
        SetPlayerRepairCost();
        ClearAndAssignDamagedModuleCounter();

    }

    private void ClearAndAssignDamagedModuleCounter()
    {
        damagedModuleCounter = 0;

        foreach (IModule module in playerModules)
        {
            if (module.ModuleState != ModuleState.Normal)
                damagedModuleCounter++;
        }

        //Debug.LogError($"DAMAGED MODULES COUNTER {damagedModuleCounter}");
    }

    protected override void Update()
    {
        if (GameManager.Instance.IsGamePaused)
            return;

        base.Update();

        if (Input.GetKeyDown(KeyCode.O) && !GameManager.Instance.isLoading)
        {
            GameManager.Instance.LoadAsync();
        }
        //Look();

        dashCooldownTimer -= Time.deltaTime;
        attackCooldownTimer -= Time.deltaTime;
        shootCooldownTimer -= Time.deltaTime;
        if (dashUses <= 0)
        {
            dashUses = maxDashUses;
            dashCooldownTimer = dashCooldown;

            //HUDManager.Instance.dashImages.ForEach(di => StartCoroutine(HUDManager.Instance.CheckCooldownOf(di, dashCooldown)));
        }

        StateMachine.CurrentState.Update();
        CheckForDashInput();

        /* if (InputManager.LongJump())
         {
             jumpBufferCounter = jumpBufferTime;
             JumpCancelled = false;
         }
         else
         {
             jumpBufferCounter -= Time.deltaTime;
         }

         if (InputManager.ShortJump())
         {
             JumpCancelled = true;
         }*/

        if (InputManager.ChargeShot())
        {
            chargedShotTimeCounter += Time.deltaTime;

            if (!isSound1Played && chargedShotTimeCounter >= chargedShotTime * 0.2f)
            {
                weaponAnim.SetBool("Charging", true);
                AudioManager.instance.PlaySFX(8, PlayerManager.instance.player.transform);
                isSound1Played = true;
            }

            if (!isSound2Played && chargedShotTimeCounter >= chargedShotTime * 0.5f)
            {
                AudioManager.instance.StopSFX(8);
                AudioManager.instance.PlaySFX(9, PlayerManager.instance.player.transform);
                isSound2Played = true;
            }


            if (!isSound3Played && chargedShotTimeCounter >= chargedShotTime)
            {
                AudioManager.instance.StopSFX(9);
                AudioManager.instance.PlaySFX(10, PlayerManager.instance.player.transform);
                isSound3Played = true;
            }
        }
        else
            ResetChargeShotSounds();

    }

    public void ResetChargeShotSounds()
    {
        chargedShotTimeCounter = 0;
        weaponAnim.SetBool("Charging", false);
        AudioManager.instance.StopSFX(8);
        AudioManager.instance.StopSFX(9);
        AudioManager.instance.StopSFX(10);
        isSound1Played = false;
        isSound2Played = false;
        isSound3Played = false;
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.FixedUpdate();
    }

    public void AnimationTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    private void CheckForDashInput()
    {

        if (InputManager.Dash() && dashCooldownTimer < 0 && DashState.moduleState == ModuleState.Normal)
        {
            if (dashUses > 0)
            {
                dashDir = MoveInput.x;

                if (dashDir == 0)
                    dashDir = facingDir;
                StateMachine.ChangeState(DashState);
            }
        }
    }

    public bool CanHeal()
    {
        IModule module;
        bool CanRepair;

        try
        {
            module = playerModules.First(m => m.ModuleState == ModuleState.Damaged);
            CanRepair = true;
        }
        catch
        {
            CanRepair = false;
        }

        Debug.Log($"Can Repair?: {PlayerManager.instance.playerCogCounter - RepairCost >= 0}");
        return PlayerManager.instance.playerCogCounter - RepairCost >= 0 && CanRepair;
    }

    private void RemoveCogCounter()
    {
        Debug.Log($"Before: {PlayerManager.instance.playerCogCounter}");
        PlayerManager.instance.playerCogCounter -= RepairCost;
        EventManager.UpdateUI?.Invoke();
        Debug.Log($"After: {PlayerManager.instance.playerCogCounter}");
    }

    public override void TakeDamage(bool isModuleDamaged, Vector2 knockbackPower, Transform direction)
    {
        if (IsInvincible)
            return;

        if (!IsDead)
        {
            Debug.LogError("ENTRATA TAKE DAMAGE");

            base.TakeDamage(isModuleDamaged, knockbackPower, direction);

            StateMachine.ChangeState(StoppedState);

            if (isModuleDamaged)
                DamageRandomModule();
        }

    }

    /*private IEnumerator DamagedVFX(float duration)
    {
        GetComponentInChildren<MeshRenderer>().material = redMat;
        yield return new WaitForSeconds(duration);
        GetComponentInChildren<MeshRenderer>().material = defaultMat;
    }*/

    /*public void HealToFull()
    {
        CurrentHP = base.MaxHealth;
        EventManager.HealthPlayerChange?.Invoke(CurrentHP);
        //HUDManager.Instance.UpdateHealthBar();
    }*/

    /*public void Look()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.LookAt(mousePosition);

        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
    }*/

    /* public void SetAttackCooldownTimer()
     {
         attackCooldownTimer = attackCooldown;
     }*/


    public void SetShootCooldownTimer()
    {
        shootCooldownTimer = shootCooldown;
    }

    public bool CanAttack()
    {
        if (attackCooldownTimer < 0)
            return true;

        return false;
    }

    public void NormalShot()
    {
        Projectile go = ObjectPoolManager.Instance.PlayerProjectilePool.UseObject();
        //Projectile go = Instantiate(projectile, projSpawner.position, projSpawner.rotation);
        go.gameObject.SetActive(true);
        go.transform.position = projSpawner.position;
        go.transform.rotation = projSpawner.rotation;

        if (ShootState.ModuleState == ModuleState.Normal)
        {
            go.moduleIsDamaged = false;
        }
        else
        {
            go.moduleIsDamaged = true;
        }
        AudioManager.instance.PlaySFX(11, PlayerManager.instance.player.transform);
    }
    public void ChargedShot()
    {

        //Projectile go = Instantiate(chargedProj, projSpawner.position, projSpawner.rotation);
        Projectile go = ObjectPoolManager.Instance.PlayerChargedShotPool.UseObject();
        go.gameObject.SetActive(true);

        go.transform.position = projSpawner.position;
        go.transform.rotation = projSpawner.rotation;

        if (ShootState.ModuleState == ModuleState.Normal)
        {
            go.moduleIsDamaged = false;
        }
        else
        {
            go.moduleIsDamaged = true;
        }
        AudioManager.instance.PlaySFX(12, PlayerManager.instance.player.transform);
    }

    public bool CanShoot()
    {
        if (shootCooldownTimer < 0)
            return true;

        return false;
    }


    /*public void InvincibilityCoroutineVFX(float duration)
    {
        StartCoroutine(StartInvincibilityVFX(duration));
    }
    private IEnumerator StartInvincibilityVFX(float duration)
    {
        invincibilityVFX.Play();
        yield return new WaitForSeconds(duration);
        invincibilityVFX.Stop();
    }*/
    public void DamageClickedModule(ModuleType moduleType, IModule module)
    {

    }
    private void DamageRandomModule()
    {
        if (IsDead)
            return;

        int random = Random.Range(1, 101);
        ModuleType moduleType;
        IModule module;

        try
        {
            Debug.LogError("ENTRATA DAMAGE MODULE");


            moduleType = moduleSystems.Find(mt => mt.startRange <= random && random <= mt.endRange).moduleType;
            module = playerModules.Find(m => m.ModuleType == moduleType);

            if (damagedModuleCounter < playerModules.Count)
            {
                Debug.LogWarning(damagedModuleCounter);

                if (module.ModuleState == ModuleState.Normal)
                {
                    module.WhenDamaged();
                    Debug.Log($"module name: {moduleType}, Status: {module.ModuleState}");

                }
                else
                    DamageRandomModule();
            }
            else
                MoveState.WhenDamaged();

            AudioManager.instance.PlaySFX(13, PlayerManager.instance.player.transform);
            return;

        }
        catch
        {
            Debug.LogError("ENTRATA DAMAGE MOVEMENT");

        }
    }

    public void Heal()
    {
        RepairRandomModule();
    }

    public void PlayStepSound()
    {
        if (playFirstSound)
            AudioManager.instance.PlaySFX(6, PlayerManager.instance.player.transform);
        else
            AudioManager.instance.PlaySFX(7, PlayerManager.instance.player.transform);

        playFirstSound = !playFirstSound;

    }
    public void SetPlayerRepairCost()
    {
        if (RepairState.ModuleState == ModuleState.Normal)
            RepairCost = repairCostNormal;
        else
            RepairCost = repairCostDamaged;
    }

    private void RepairRandomModule()
    {
        int random = Random.Range(1, 101);
        ModuleType moduleType;
        IModule module;

        try
        {
            moduleType = moduleSystems.Find(mt => mt.startRange <= random && random <= mt.endRange).moduleType;
            module = playerModules.Find(m => m.ModuleType == moduleType);

            if (module.ModuleState == ModuleState.Damaged)
            {
                module.WhenRepaired();
                Debug.Log($"module name: {moduleType}, Status: {module.ModuleState}");
                RemoveCogCounter();
            }
            else
                RepairRandomModule();
        }
        catch
        {
            Debug.LogError("no more modules to repair");
        }
        //AudioManager.instance.PlaySFX(21, PlayerManager.instance.player.transform);
    }

    private void OnDisable()
    {

    }

    public void Save(ref PlayerSaveData data)
    {
        //data.position = transform.position;
        data.playerCog = PlayerManager.instance.playerCogCounter;

        data.moduleDataList = new List<ModuleData>();
        foreach (var module in playerModules)
        {
            ModuleData moduleData = new ModuleData();
            module.Save(ref moduleData);
            data.moduleDataList.Add(moduleData);
        }
        //Debug.Log(data.moduleDataList.Count);

    }

    public void Load(PlayerSaveData data)
    {
        //Debug.Log(SaveSystem.newSceneLoad);
        //if (SaveSystem.newSceneLoad)
        //{
        //    SaveSystem.newSceneLoad = false;
        //    Debug.Log(SaveSystem.newSceneLoad);
        //}
        //else
        //{
        //    if (data.position != Vector3.zero)
        //        transform.position = data.position;
        //}


        PlayerManager.instance.playerCogCounter = data.playerCog;
        if (data.moduleDataList == null || data.moduleDataList.Count == 0)
        {
            return;
        }

        try
        {
            foreach (var moduleData in data.moduleDataList)
            {
                var module = playerModules.Find(m => m.ModuleType == moduleData.ModuleType);
                if (module != null)
                {
                    module.Load(moduleData);
                }

                EventManager.ModuleChangeState?.Invoke(moduleData.ModuleType, moduleData.ModuleState);
            }
            HudManager.Instance.SetDataUp(data.moduleDataList);

        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FinishLevel"))
        {
            AudioManager.instance.PlaySFX(15, PlayerManager.instance.player.transform);

            if (GameManager.Instance.lastLv == false)
                EventManager.Sacrifice?.Invoke();
            else
                GameManager.Instance.EndResults();

        }
        else if (collision.CompareTag("FinishTutorial"))
        {
            //SaveSystem.newSceneLoad = true;
            //Debug.Log(SaveSystem.newSceneLoad);
            //GameManager.Instance.SaveAsync();
            GameManager.Instance.SaveAsync();

            StartCoroutine(EndTransition());
        }
    }

    // QUI IL CODICE BRUTTO
    // EH SI PROPRIO QUI
    private IEnumerator EndTransition()
    {
        endTransition.SetActive(true);
        Time.timeScale = 0;

        GameManager.Instance.IsGamePaused = true;

        float elapseTime = 0;
        float t = 0;
        while (elapseTime <= 0.75f)
        {
            elapseTime += Time.unscaledDeltaTime;
            t = elapseTime / 0.75f;
            endTransition.transform.localScale = new Vector3(Mathf.Lerp(0, 100, t), Mathf.Lerp(0, 100, t), 1);

            yield return null;
        }

        Time.timeScale = 1;
        GameManager.Instance.IsGamePaused = false;

        int netSceneIndex = GameManager.Instance.sceneData.Data.sceneIndex + 1;
        SceneManager.LoadScene(netSceneIndex);

        yield return null;
    }

    // QUI IL CODICE BRUTTO
    // EH SI PROPRIO QUI
    IEnumerator StartTransition()
    {
        startTransition.SetActive(true);
        Time.timeScale = 0;
        GameManager.Instance.IsGamePaused = true;

        float elapseTime = 0;
        float t = 0;
        while (elapseTime <= 0.75f)
        {
            elapseTime += Time.unscaledDeltaTime;
            t = elapseTime / 0.75f;
            startTransition.transform.localScale = new Vector3(Mathf.Lerp(100, 0, t), Mathf.Lerp(100, 0, t), 1);

            yield return null;
        }

        Time.timeScale = 1;
        GameManager.Instance.IsGamePaused = false;

        startTransition.SetActive(false);

        yield return null;
    }
}
[System.Serializable]

public struct PlayerSaveData
{
    //public Vector3 position;
    public List<ModuleData> moduleDataList;
    public float playerCog;

}
