using System;
using UnityEngine;
public static class EventManager
{
    public static Action<ModuleType, ModuleState> ModuleChangeState;
    public static Action UpdateUI;
    public static Action CalledLoad;
    public static Action Sacrifice;
    public static Action DestroyModule;
    public static Action<ModuleType> ModuleUsed;
    public static Action<bool> OnCameraVolumeChanged;
    public static Func<bool> OnReturnCameraVolumeValue;
    public static Action OnCameraShake;
    public static Action<string, int, int, int, int, string, string, Color, Color> SetResultScreen;
}
