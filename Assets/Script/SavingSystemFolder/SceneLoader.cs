using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private SceneDataSO[] sceneDataSOArray;
    private System.Collections.Generic.Dictionary<string, int> sceneIDToIndexMap = new System.Collections.Generic.Dictionary<string, int>();


    private void Awake()
    {
        GameManager.Instance.sceneLoader = this;
        PopulateSceneMappings();
    }
    private void PopulateSceneMappings()
    {
        foreach (var sceneDataSO in sceneDataSOArray)
        {
            sceneIDToIndexMap[sceneDataSO.uniqueName] = sceneDataSO.sceneIndex;
        }
    }

    public void LoadSceneByIndex(string savedSceneID)
    {
        if (sceneIDToIndexMap.TryGetValue(savedSceneID, out int sceneIndex))
            SceneManager.LoadScene(sceneIndex);
        else
            Debug.LogError("NoSceneFound");
    }

    public async Task LoadSceneByIndexAsync(string savedSceneID)
    {
        if (sceneIDToIndexMap.TryGetValue(savedSceneID, out int sceneIndex))
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
            asyncLoad.allowSceneActivation = false;

            while (!asyncLoad.isDone)
            {
                if (asyncLoad.progress >= 0.9f)
                {
                    asyncLoad.allowSceneActivation = true;
                    break;
                }
                await Task.Yield();
            }
        }
        else
            Debug.LogError("No Scene");
    }
}
