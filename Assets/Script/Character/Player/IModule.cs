public enum ModuleType
{
    Movement,
    Jump,
    Shoot,
    ChargedShot,
    Dash,
    Repair
}

public interface IModule : ISavable<ModuleData>
{
    public ModuleState ModuleState { get; }
    public ModuleType ModuleType { get; }

    public void WhenAbilityUsed();
    public void WhenDamaged();
    public void WhenDestroyed();
    public void WhenRepaired();

}
[System.Serializable]
public struct ModuleData
{
    public ModuleState ModuleState;
    public ModuleType ModuleType;
}
