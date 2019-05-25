using UnityEngine;

public class MapLoader
{
    private const string mapPath = @"Assets\Maps\";

    public static void LoadMap(string name) {
        var tiles = MapParser.GetMap(mapPath + name);


    }
}
