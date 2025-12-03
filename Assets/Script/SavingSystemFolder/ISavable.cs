public interface ISavable<T> where T : struct
{
    public void Save(ref T data);
    public void Load(T data);
}
