[System.Serializable]
public abstract class Action
{
    public abstract string Execute(Fighter source, Fighter target, out bool output);
    public abstract string ExecuteSafe(Fighter source, Fighter target, out bool output);
    public abstract string GetDescription();
}
