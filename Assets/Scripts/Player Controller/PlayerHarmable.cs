using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class PlayerHarmable : MonoBehaviour
{
    [Button]
    public void OnDead()
    {
        //Debug.Log("Player dead");
        RespawnManager.Instance.RespawnPlayer();
    }
}
