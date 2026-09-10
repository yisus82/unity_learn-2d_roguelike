using UnityEngine;

public class FoodObject : CellObject
{
    public int foodAmount;

    public override void OnPlayerEntered()
    {
        Destroy(gameObject);
        GameManager.Instance.ChangeFoodAmount(foodAmount);
    }
}