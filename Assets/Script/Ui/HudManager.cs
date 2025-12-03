using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HudManager : Singleton<HudManager>
{

    public List<ModuleUI> modulesUI = new List<ModuleUI>();
    [SerializeField] private TMP_Text currencyText;

    [SerializeField] private GameObject panelSac;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject commandSettings;
    [SerializeField] private GameObject ResultsPanel;
    public void SetDataUp(List<ModuleData> dataList)
    {
        for (int i = 0; i < dataList.Count; i++)
        {
            modulesUI[i].SetModuleUI();
        }

    }

    private void Start()
    {
        //currencyText.text = PlayerManager.instance.playerCogCounter.ToString();
    }
    protected override void Awake()
    {
        base.Awake();
        //SaveSystem.LoadSoundData();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EventManager.UpdateUI += OnUpdateUI;
        EventManager.Sacrifice += OnSacrifice;
    }

    private void OnDisable()
    {
        EventManager.UpdateUI -= OnUpdateUI;
    }

    public void OnUpdateUI()
    {
        currencyText.text = PlayerManager.instance.playerCogCounter.ToString();
    }

    public void ResultsMenu()
    {
        if (!ResultsPanel.activeSelf)
        {
            ResultsPanel.SetActive(true);
        }
        else
        {
            ResultsPanel.SetActive(false);
        }
    }

    public void SoundMenu()
    {
        if (!settingsMenu.activeSelf)
        {
            settingsMenu.SetActive(true);
        }
        else
        {
            settingsMenu.SetActive(false);
            SaveSound();
        }
    }

    public void CommandSettings()
    {
        if (!commandSettings.activeSelf)
            commandSettings.SetActive(true);
        else
            commandSettings.SetActive(false);
    }
    private void OnSacrifice()
    {
        Time.timeScale = 0;
        GameManager.Instance.IsGamePaused = true;

        if (panelSac != null)
            panelSac.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu != null)
            {
                if (pauseMenu.activeSelf == false)
                {
                    AudioManager.instance.PlaySFX(22, PlayerManager.instance.player.transform);
                }
                pauseMenu.SetActive(true);
            }

            Time.timeScale = 0;
            GameManager.Instance.IsGamePaused = true;

        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        GameManager.Instance.IsGamePaused = false;

        pauseMenu.SetActive(false);
        AudioManager.instance.PlaySFX(23, PlayerManager.instance.player.transform);
    }

    public void Restart()
    {
        AudioManager.instance.PlaySFX(23, PlayerManager.instance.player.transform);
        //SaveSystem.newSceneLoad = true;
        GameManager.Instance.LoadAsync();

    }

    //public void QuitGame()
    //{
    //    Application.Quit();
    //    AudioManager.instance.PlaySFX(23, PlayerManager.instance.player.transform);
    //}

    public void MainMenu()
    {
        AudioManager.instance.PlaySFX(23, PlayerManager.instance.player.transform);
        //GameManager.Instance.SaveAsync();
        SceneManager.LoadScene(0);

    }

    public void SaveSound()
    {
        SaveSystem.Save();
    }
}
