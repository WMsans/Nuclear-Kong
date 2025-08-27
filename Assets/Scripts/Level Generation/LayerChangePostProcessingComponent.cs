using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Edgar.Unity;
using Edgar.Unity.Examples.Gungeon;
using UnityEngine;

public class LayerChangePostProcessingComponent : DungeonGeneratorPostProcessingComponentGrid2D
{
    [SerializeField] private SerializedDictionary<string, int> layerRemap;
    public override void Run(DungeonGeneratorLevelGrid2D level)
    {
        var tilemaps = level.GetSharedTilemaps();
        foreach(var kvp in layerRemap)
        {
            var wallTiles = tilemaps.Where(x => x.gameObject.name == kvp.Key);
            foreach (var x in wallTiles) x.gameObject.layer = kvp.Value;
        }
    }
}
