public class ObstacleObject : CellObject
{
    public int pointsToDestroy;
    
    private int _damagePoints;

    public override void OnPlayerEntered()
    {
        _damagePoints++;
        if (_damagePoints >= pointsToDestroy)
        {
            Destroy(gameObject);
            GameManager.Instance.RemoveCellObject(cellPosition);
        }
    }
}
