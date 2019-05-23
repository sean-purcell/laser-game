using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

/**
 * Loads a "map" (a List of Lists of TileType) from a file. Check
 * `TileTypeToChar` for available tiles. An example map file could be as simple as

>  \.
W     .
W W\ o.

 * The period '.' character is used as a visual sentinel to end a row of the
 * map.
 */
public class MapLoader
{
    public enum TileType { Empty, LaserLeft, LaserRight, LaserUp, LaserDown, Target, MirrorForward, MirrorBackward, Wall };
    private const char RowSentinel = '.';

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

    public static List<List<TileType>> GetMap(string mapPath)
    {
        List<List<TileType>> map = new List<List<TileType>>();

        using (StreamReader streamReader = new StreamReader(mapPath))
        {
            string line;
            while ((line = streamReader.ReadLine()) != null)
            {
                List<TileType> row = new List<TileType>();
                foreach (char tileChar in line)
                {
                    if (tileChar == RowSentinel) break;
                    row.Add(GetTileType(tileChar));
                }
                map.Add(row);
            }
        }

        return map;
    }

    static void Main()
    {
        const string mapPath = "map.txt";

        Console.WriteLine("Loading map from " + mapPath);

        var map = GetMap(mapPath);

        foreach (List<TileType> mapRow in map)
        {
            Console.WriteLine(String.Join(" ", mapRow.Select(tile => Enum.GetName(typeof(TileType), tile))));
        }

    }
}
