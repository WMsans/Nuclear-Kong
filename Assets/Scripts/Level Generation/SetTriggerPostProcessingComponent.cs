using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Edgar.Unity;
using UnityEngine;

public class SetTriggerPostProcessingComponent : DungeonGeneratorPostProcessingComponentGrid2D
{
    [SerializeField] private SerializedDictionary<string, bool> triggerRemap;
    public override void Run(DungeonGeneratorLevelGrid2D level)
    {
        var tilemaps = level.GetSharedTilemaps();
        foreach(var kvp in triggerRemap)
        {
            var wallTiles = tilemaps.Where(x => x.gameObject.name == kvp.Key);
            foreach (var x in wallTiles) x.GetComponent<CompositeCollider2D>().isTrigger = kvp.Value;
        }
    }
}
