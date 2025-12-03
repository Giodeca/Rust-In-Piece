using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneData : MonoBehaviour, ISavable<SceneSaveData>
{
    public SceneDataSO Data;

    private void Awake()
    {
        GameManager.Instance.sceneData = this;
    }
    private void Start()
    {
        Time.timeScale = 1.0f;
        GameManager.Instance.IsGamePaused = false;

        //SaveSystem.SaveOnlyScene();
        //await Task.Delay(1000);
    }

    public void Load(SceneSaveData data)
    {
        GameManager.Instance.sceneLoader.LoadSceneByIndex(data.SceneID);
    }

    public void Save(ref SceneSaveData data)
    {
        data.SceneID = Data.uniqueName;
        data.SceneIndex = Data.sceneIndex;
    }

    public async Task LoadAsync(SceneSaveData data)
    {
        await GameManager.Instance.sceneLoader.LoadSceneByIndexAsync(data.SceneID);
    }

    public Task WaitForSceneToBeFullyLoaded()
    {
        TaskCompletionSource<bool> taskCompletion = new TaskCompletionSource<bool>();
        UnityEngine.Events.UnityAction<Scene, LoadSceneMode> sceneLoaderHandler = null;

        sceneLoaderHandler = (scene, mode) =>
        {
            taskCompletion.SetResult(true);
            SceneManager.sceneLoaded -= sceneLoaderHandler;
        };

        SceneManager.sceneLoaded += sceneLoaderHandler;

        return taskCompletion.Task;
    }
}

[System.Serializable]
public struct SceneSaveData
{
    public string SceneID;
    public int SceneIndex;
}
