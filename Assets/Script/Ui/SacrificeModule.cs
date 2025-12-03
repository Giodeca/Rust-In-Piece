using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class SacrificeModule : ModuleUI, IPointerClickHandler
{
    // QUI IL CODICE BRUTTO
    // EH SI PROPRIO QUI
    [Header("Transitions")]
    private GameObject player;
    private GameObject endTransition;

    [Header("Module")]
    [SerializeField] private ModuleUI moduleUI;

    private void Start ()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        player = PlayerManager.instance.player.gameObject;
    }

    private void OnEnable()
    {
        Assignation();
    }

    private void Assignation()
    {
        ModuleType = moduleUI.ModuleType;
        ModuleState = moduleUI.ModuleState;
        ColorSetUp();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (moduleUI.ModuleState != ModuleState.Destroyed)
        {
            foreach (IModule module in PlayerManager.instance.player.playerModules)
            {
                if (module.ModuleType == this.ModuleType)
                {
                    module.WhenDestroyed();
                }
            }
            ColorSetUp();
            //SaveSystem.newSceneLoad = true;
            //Debug.Log(SaveSystem.newSceneLoad);
            //GameManager.Instance.SaveAsync();
            GameManager.Instance.SaveAsync();

            GameManager.Instance.EndResults();
            //StartCoroutine(EndTransition());
        }
    }

    // QUI IL CODICE BRUTTO
    // EH SI PROPRIO QUI
    private IEnumerator EndTransition()
    {
        endTransition = player.GetComponent<Player>().endTransition;
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
}
