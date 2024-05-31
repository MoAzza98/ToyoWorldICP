using Boom;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomServices : MonoBehaviour
{
    public static BoomServices i { get; private set; }
    private void Awake()
    {
        i = this;
    }

    /// <summary>
    /// Method that handles executing the UserWonMatch 
    /// </summary>
    /// <returns></returns>
    public async UniTaskVoid UserWonMatch()
    {

        //Record on your world that the user has won the match.
        //It will increment +1 to the user's played count and +1 to the user's won count.
        var result = await ActionUtil.Guilds.UserWonMatch();

        if (result.IsOk)
        {
            Debug.Log($"{nameof(UserWonMatch)} request has been executed");
        }
        else
        {
            Debug.LogError($"Something went wrong executing {nameof(UserWonMatch)}. More details: {result.AsErr()}");
        }
    }

    /// <summary>
    /// Method that handles executing the UserLostMatch
    /// </summary>
    /// <returns></returns>
    public async UniTaskVoid UserLostMatch()
    {
        //Record on your world that the user has lost the match.
        //It will increment +1 to the user's played count and +1 to the user's lost count.
        var result = await ActionUtil.Guilds.UserLostMatch();

        if (result.IsOk)
        {
            Debug.Log($"{nameof(UserLostMatch)} request has been executed");
        }
        else
        {
            Debug.LogError($"Something went wrong executing {nameof(UserLostMatch)}. More details: {result.AsErr()}");
        }
    }
}
