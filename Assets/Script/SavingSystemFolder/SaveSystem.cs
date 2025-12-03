using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class SaveSystem
{
    private static SaveData saveData = new SaveData();
    //public SerializableDictionary<string, float> soundValues = new SerializableDictionary<string, float>();
    //public static bool newSceneLoad;


    [System.Serializable]
    public struct SaveData
    {
        public PlayerSaveData playerSaveData;
        //Fare la stessa cosa per enemy e altri valori da salvare tipo abilita e robe varie
        public SceneSaveData sceneSaveData;
        public SerializableDictionary<string, DataSoundSlider> soundValues;
    }
    public static void SoundSave()
    {
        saveData.soundValues = new SerializableDictionary<string, DataSoundSlider>();
    }

    public static SerializableDictionary<string, DataSoundSlider> GetValueDictionary() => saveData.soundValues;
    public static string SaveFileName()
    {
        string saveFile = Application.persistentDataPath + "/save" + ".save";
        return saveFile;
    }
    public static void SaveSoundSetUp()
    {
        if (saveData.soundValues == null)
        {
            saveData.soundValues = new SerializableDictionary<string, DataSoundSlider>();
        }

        foreach (UI_VolumeSlider slider in SlidlerManager.Instance.ui_slider)
        {
            slider.SaveDataSound(ref saveData.soundValues);
        }


    }

    public static void Save()
    {
        HandleSaveData();
        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(saveData, true));
    }
    //public static void SaveOnlyScene()
    //{
    //    HandleDataSceneSave();
    //    File.WriteAllText(SaveFileName(), JsonUtility.ToJson(saveData, true));
    //}
    public static void HandleDataSceneSave()
    {
        GameManager.Instance.sceneData.Save(ref saveData.sceneSaveData);
    }
    private static void HandleSaveData()
    {
        if (PlayerManager.instance != null)
            PlayerManager.instance.player.Save(ref saveData.playerSaveData);

        if (GameManager.Instance != null)
            GameManager.Instance.sceneData.Save(ref saveData.sceneSaveData);

        /*if (SlidlerManager.Instance != null)
            SaveSoundSetUp();*/
    }

    public static async Task SaveAsynchronously()
    {
        await SaveAsync();
    }
    private static async Task SaveAsync()
    {
        HandleSaveData();

        await File.WriteAllTextAsync(SaveFileName(), JsonUtility.ToJson(saveData, true));
    }
    public static void Load()
    {
        string saveContent = File.ReadAllText(SaveFileName());
        saveData = JsonUtility.FromJson<SaveData>(saveContent);
        HandleLoadData();
    }


    public static void ClearSave()
    {
        if (File.Exists(SaveFileName()))
            File.Delete(SaveFileName());
        else
            Debug.Log("NoFileExist");


    }
    public static async Task LoadAsync()
    {
        if (File.Exists(SaveFileName()))
        {
            string saveContent = File.ReadAllText(SaveFileName());

            saveData = JsonUtility.FromJson<SaveData>(saveContent);

            await HandleLoadDataAsync();

        }
        else
        {
            Debug.Log("NoLoadData");
        }
    }
    public static void LoadPlayerData()
    {
        if (PlayerManager.instance.player != null)
        {
            PlayerManager.instance.player.Load(saveData.playerSaveData);
        }
    }

    public static void LoadSoundData()
    {

        try
        {
            foreach (UI_VolumeSlider slider in SlidlerManager.Instance.ui_slider)
            {
                slider.LoadSlider(PlayerPrefs.GetFloat(slider.parameter));


                /*if (saveData.soundValues.TryGetValue(slider.parameter, out DataSoundSlider dataSound))
                {
                    Debug.LogWarning($"pORCO IL CLERO");
                    slider.LoadSlider(dataSound);
                }
                else
                {
                    Debug.LogWarning($"No saved data found for parameter: {slider.parameter}");
                }*/
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error loading sound data: {ex.Message}");
        }
    }
    private static async Task HandleLoadDataAsync()
    {

        await GameManager.Instance.sceneData.LoadAsync(saveData.sceneSaveData);

        await GameManager.Instance.sceneData.WaitForSceneToBeFullyLoaded();
        if (PlayerManager.instance.player != null)
            PlayerManager.instance.player.Load(saveData.playerSaveData);

        if (SlidlerManager.Instance != null)
            LoadSoundData();

    }
    private static void HandleLoadData()
    {
        PlayerManager.instance.player.Load(saveData.playerSaveData);

        GameManager.Instance.sceneData.Load(saveData.sceneSaveData);

        if (SlidlerManager.Instance != null)
            LoadSoundData();
    }

}
