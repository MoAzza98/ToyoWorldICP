using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] ToyoBase _base;
    [SerializeField] int level;
    [SerializeField] bool isPlayerUnit;
    [SerializeField] Transform playerSpawnPoint;
    [SerializeField] Transform enemySpawnPoint;
    public Animator playerAnim;
    public Animator enemyAnim;

    public Toyo Toyo { get; set; }

    public void Setup()
    {
        Toyo = new Toyo(_base, level);
        if(isPlayerUnit)
        {
            GameObject PlayerToyo = Instantiate(_base.ToyoPrefab, playerSpawnPoint.position, Quaternion.identity);
            playerAnim = PlayerToyo.GetComponent<Animator>();
            PlayerToyo.transform.LookAt(enemySpawnPoint);
        }
        else
        {
            GameObject EnemyToyo = Instantiate(_base.ToyoPrefab, enemySpawnPoint.position, Quaternion.identity);
            enemyAnim = EnemyToyo.GetComponent<Animator>();
            EnemyToyo.transform.LookAt(playerSpawnPoint);
        }
    }
}
