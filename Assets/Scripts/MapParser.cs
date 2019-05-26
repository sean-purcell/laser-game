using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

/**
 * Loads a "map" (a List of Lists of TileType) from a file. Check
 * `TileTypeToChar` for available tiles. An example map file could be as simple as

3 6
>  \
W     
W W\ o

 */
public class MapParser
{
    public enum TileType { Empty, LaserLeft, LaserRight, LaserUp, LaserDown, Target, MirrorForward, MirrorBackward, Wall };

    public struct MapDescription
    {
        public int rows;
        public int cols;
        public Dictionary<(int, int), TileType> tiles;

        public MapDescription(int rows, int cols) {
            this.rows = rows;
            this.cols = cols;
            this.tiles = new Dictionary<(int, int), TileType>();
        }
    };

    // TODO(toyang): should we provide runtime checks? Check that no duplicates, no delimiter usage.
    public static readonly Dictionary<TileType, char> TileTypeToChar = new Dictionary<TileType, char>
        {
            {TileType.Empty, ' '},
            {TileType.LaserLeft, '<'},
            {TileType.LaserRight, '>'},
            {TileType.LaserUp, '^'},
            {TileType.LaserDown, 'v'},
            {TileType.Target, 'o'},
            {TileType.MirrorForward, '/'},
            {TileType.MirrorBackward, '\\'},
            {TileType.Wall, 'W'},
        };

    [Serializable()]
    public class MissingTileTypeException : Exception
    {
        public MissingTileTypeException(TileType t):
            base("TileType with enum value " + Enum.GetName(typeof(TileType), t) + " isn't found in the map.") {}
    }

    [Serializable()]
    public class InvalidTileTypeCharacterException : Exception
    {
        public InvalidTileTypeCharacterException(char c):
            base("Invalid character found in map parse: " + c) {}
    }

    private static TileType GetTileType(char c)
    {
        if (!TileTypeToChar.ContainsValue(c)) {
            throw new InvalidTileTypeCharacterException(c);
        }

        // Not entirely efficient, but the number of tiles we have shouldn't be
        // too large.
        foreach (KeyValuePair<TileType, char> tilePair in TileTypeToChar) {
            if (c == tilePair.Value) {
                return tilePair.Key;
            }
        }

        // Execution should never reach here.
        throw new InvalidTileTypeCharacterException(c);
    }

    private static char GetChar(TileType t)
    {
        if (!TileTypeToChar.ContainsKey(t)) {
            throw new MissingTileTypeException(t);
        }

        return TileTypeToChar[t];
    }

    public static MapDescription GetMap(string mapPath)
    {

        using (StreamReader streamReader = new StreamReader(mapPath))
        {
            string[] dims = streamReader.ReadLine().Split();

            var map = new MapDescription(int.Parse(dims[0]), int.Parse(dims[1]));

            for (int row = 0; row < map.rows; row++) {
                string line = streamReader.ReadLine();
                for (int col = 0; col < map.cols && col < line.Length; col++) {
                    var tile = GetTileType(line[col]);
                    if (tile == TileType.Empty) continue;

                    // The first row is the highest one
                    map.tiles.Add((map.rows - 1 - row, col), tile);
                }
            }
            return map;
        }
    }
}

