using System.Collections;
using System.Collections.Generic;
using Edgar.Unity;
using UnityEngine;

public class SetPlayerStartPosProcessingComponent : DungeonGeneratorPostProcessingComponentGrid2D
{
    [SerializeField] private Transform player;
    public override void Run(DungeonGeneratorLevelGrid2D level)
    {
        var startPosObject = GameObject.Find("StartPos");
        if(!startPosObject) return;
        player.position = startPosObject.transform.position;
    }
}
