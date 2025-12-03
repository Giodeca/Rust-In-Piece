using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //Animazioni se le abbiamo
    [SerializeField] private Color color;
    [SerializeField] private Color colorFrame;
    private Color baseColor;
    private Color baseColorFrame;
    [SerializeField] private GameObject animationHover;
    [SerializeField] private bool isContinue;
    [SerializeField] private Image colorBackground;
    [SerializeField] private Image colorFrameUI;
    [SerializeField] private Color colorDeactive;

    private void Start()
    {
        baseColor = colorBackground.color;
        baseColorFrame = colorFrameUI.color;

        if (isContinue && !File.Exists(SaveSystem.SaveFileName()))
        {
            colorBackground.color = colorDeactive;
        }
    }

    private void Update()
    {
        if (isContinue && !File.Exists(SaveSystem.SaveFileName()))
        {
            colorBackground.color = colorDeactive;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        animationHover.SetActive(true);
        colorBackground.color = color;
        colorFrameUI.color = colorFrame;

        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(23, null);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animationHover.SetActive(false);
        colorBackground.color = baseColor;
        colorFrameUI.color = baseColorFrame;
        //AudioManager.instance.StopSFX(23);
    }
}
