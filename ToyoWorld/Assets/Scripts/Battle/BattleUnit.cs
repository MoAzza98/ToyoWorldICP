using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{

    [SerializeField] bool isPlayerUnit;
    [SerializeField] Transform playerSpawnPoint;
    [SerializeField] Transform enemySpawnPoint;
    public GameObject playerToyo;
    public GameObject enemyToyo;
    public Animator playerAnim;
    public Animator enemyAnim;

    public Toyo Toyo { get; set; }

    public void Setup(Toyo toyo)
    {
        Toyo = toyo;
        if(isPlayerUnit)
        {
            playerToyo = Instantiate(toyo.Base.ToyoPrefab, playerSpawnPoint.position, Quaternion.identity);
            playerAnim = playerToyo.GetComponent<Animator>();
            playerToyo.transform.LookAt(enemySpawnPoint);
        }
        else
        {
            enemyToyo = Instantiate(toyo.Base.ToyoPrefab, enemySpawnPoint.position, Quaternion.identity);
            enemyAnim = enemyToyo.GetComponent<Animator>();
            enemyToyo.transform.LookAt(playerSpawnPoint);
        }
    }
}
