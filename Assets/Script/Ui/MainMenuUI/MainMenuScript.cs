using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject levelSelectionPanel;
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject controlsPanel;

    public void Continue()
    {
        if (File.Exists(SaveSystem.SaveFileName()))
        {
            GameManager.Instance.LoadAsync();
            //GameManager.Instance.LoadData();
        }

    }

    public void OpenControlPanel()
    {
        if (controlsPanel.activeSelf)
        {
            controlsPanel.SetActive(false);
        }
        else
            controlsPanel.SetActive(true);

    }
    public void OpenCLoseLevelPanel()
    {
        if (levelSelectionPanel.activeSelf)
            levelSelectionPanel.SetActive(false);
        else
            levelSelectionPanel.SetActive(true);
    }

    public void OpenOption()
    {
        if (optionPanel.activeSelf)
            optionPanel.SetActive(false);
        else
            optionPanel.SetActive(true);
    }

    public void OpenCredits()
    {
        if (creditsPanel.activeSelf)
            creditsPanel.SetActive(false);
        else
            creditsPanel.SetActive(true);
    }
    public void NewGame()
    {
        SaveSystem.ClearSave();
        SceneManager.LoadScene(1);
    }
    public void CloseGame()
    {
        Application.Quit();
    }

    public void SaveSound()
    {
        SaveSystem.Save();
    }




}
