using System;
using UnityEngine;

public class ObstacleObject : CellObject
{
    public int pointsToDestroy;
    
    private int _damagePoints;
    private PlayerController _playerController;

    private void Start()
    {
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public override void OnPlayerEntered()
    {
        _playerController.Attack();
        _damagePoints++;
        if (_damagePoints < pointsToDestroy)
        {
            return;
        }
        Destroy(gameObject);
        GameManager.Instance.RemoveCellObject(cellPosition);
    }
}
