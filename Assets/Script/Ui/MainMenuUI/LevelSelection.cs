using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private SceneDataSO sceneData;
    [SerializeField] private Color color;
    [SerializeField] private Color colorBasic;
    [SerializeField] private GameObject[] image;

    private void Start()
    {

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        //SaveSystem.newSceneLoad = true;
        SceneManager.LoadScene(sceneData.sceneIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (var item in image)
        {
            item.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (var item in image)
        {
            item.SetActive(false);
        }
    }
}
