public class ExitObject : CellObject
{
    public override void OnPlayerEntered()
    {
       GameManager.Instance.GenerateNextLevel();
    }
}
