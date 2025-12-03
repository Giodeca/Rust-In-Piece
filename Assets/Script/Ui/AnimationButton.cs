using UnityEngine;
using UnityEngine.EventSystems;

public class AnimationButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject gameObjectAnimation;
    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObjectAnimation.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObjectAnimation.SetActive(false);
    }

}
