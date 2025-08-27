using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KongActionBaseState : KongBaseState
{
    public KongBaseState LastState { get; set; }

    protected void ExitAction()
    {
        Owner.ChangeState(LastState);
    }

    protected void FacePlayer()
    {
        var playerPos = PlayerController.Instance.transform.position;
        Owner.transform.rotation = Quaternion.Euler(Owner.transform.eulerAngles.x,
            playerPos.x < Owner.transform.position.x ? 180f : 0f, Owner.transform.eulerAngles.z);
    }
}
