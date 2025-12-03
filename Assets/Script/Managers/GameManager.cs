using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public SceneData sceneData;
    public SceneLoader sceneLoader;

    public bool lastLv;

    private bool isSaving;
    public bool isLoading;

    public bool IsGamePaused { get; set; }

    [SerializeField] private MilestoneSystem milestoneSystem;

    protected override void Awake()
    {
        //SaveSystem.SoundSave();
    }
    //[SerializeField] private List<ModuleDictionary> modulesStates;

    private void Start()
    {
        Debug.LogError(milestoneSystem.CalculateTime());
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EventManager.CalledLoad += OnLoadCalled;
    }

    private void OnDisable()
    {
        EventManager.CalledLoad -= OnLoadCalled;
    }

    private void Update()
    {
        milestoneSystem.Timer = Time.unscaledTime;
        if (Input.GetKeyDown(KeyCode.L) && !isLoading)
        {
            LoadAsync();

        }
        if (Input.GetKeyDown(KeyCode.P) && !isSaving)
        {
            SaveAsync();
        }
        if (Input.GetKeyDown(KeyCode.C))
            SaveSystem.ClearSave();


    }
    public void EndResults()
    {
        HudManager.Instance.ResultsMenu();
        Time.timeScale = 0;
        IsGamePaused = true;
        milestoneSystem.EndResults();
    }
    public void AddCogToMilestone()
    {
        milestoneSystem.CogCount++;
    }
    public void AddEnemyToMilestone()
    {
        milestoneSystem.DefeatedEnemiesCount++;
    }

    public async void SaveAsyncNewGame()
    {

        isSaving = true;
        await SaveSystem.SaveAsynchronously();
        isSaving = false;

    }
    public async void SaveAsync()
    {
        isSaving = true;
        await SaveSystem.SaveAsynchronously();
        isSaving = false;
    }
    public async void LoadAsync()
    {
        if (isLoading) return;

        isLoading = true;
        await SaveSystem.LoadAsync();
        isLoading = false;
    }

    public void LoadNextScene()
    {
        Time.timeScale = 1;
        IsGamePaused = false;

        int netSceneIndex = sceneData.Data.sceneIndex + 1;
        SceneManager.LoadScene(netSceneIndex);
    }

    public void LoadData()
    {
        SaveSystem.Load();
    }
    private void OnLoadCalled()
    {
        SaveSystem.LoadPlayerData();
    }

}
