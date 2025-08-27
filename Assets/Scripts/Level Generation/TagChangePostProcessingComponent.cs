using System.Linq;
using AYellowpaper.SerializedCollections;
using Edgar.Unity;
using UnityEngine;

public class TagChangePostProcessingComponent : DungeonGeneratorPostProcessingComponentGrid2D
{
    [SerializeField] private SerializedDictionary<string, string> tagRemap;
    public override void Run(DungeonGeneratorLevelGrid2D level)
    {
        var tilemaps = level.GetSharedTilemaps();
        foreach(var kvp in tagRemap)
        {
            var wallTiles = tilemaps.Where(x => x.gameObject.name == kvp.Key);
            foreach (var x in wallTiles) x.gameObject.tag = kvp.Value;
        }
    }
}
