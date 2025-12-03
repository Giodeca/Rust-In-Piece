using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameMenu : MonoBehaviour
{
    [SerializeField] private GameObject credit;


    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void OpenCred()
    {
        if (credit.activeSelf)
        {
            credit.SetActive(false);
        }
        else
        { credit.SetActive(true); }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
