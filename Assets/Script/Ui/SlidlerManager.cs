using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlidlerManager : Singleton<SlidlerManager>
{
    public List<UI_VolumeSlider> ui_slider = new List<UI_VolumeSlider>();
    public Toggle toggler;

    [SerializeField] private GameObject keyControl;
    [SerializeField] private GameObject GamePadControl;
    [SerializeField] private TMP_Text imageKey;
    [SerializeField] private TMP_Text GamePadImage;

    /*protected override void Awake()
    {
        if (File.Exists(SaveSystem.SaveFileName()))
            SaveSystem.LoadSoundData();
    }*/

    private void Start()
    {
        toggler.isOn = EventManager.OnReturnCameraVolumeValue.Invoke();
    }

    protected override void OnEnable()
    {
        SaveSystem.LoadSoundData();
    }

    public void EnableDisableCRTEffect(bool isActive)
    {
        EventManager.OnCameraVolumeChanged?.Invoke(isActive);
        toggler.isOn = isActive;
    }

    public void OpenKeyComand()
    {
        imageKey.color = Color.white;
        keyControl.SetActive(true);
        GamePadImage.color = Color.gray;
        GamePadControl.SetActive(false);
    }

    public void PadKeyComand()
    {
        GamePadImage.color = Color.white;
        keyControl.SetActive(false);
        imageKey.color = Color.gray;
        GamePadControl.SetActive(true);
    }
}
