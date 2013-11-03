using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile {
  public enum TileType {Wall, Floor, Water, Lava};
  Dictionary<TileType, Color> _TileColors = 
    new Dictionary<TileType, Color> 
    {{TileType.Wall,  Color.black},
     {TileType.Floor, Color.grey},
     {TileType.Water, Color.blue},
     {TileType.Lava,  Color.red}};


  TileType _type;
  public TileType type {get {return _type;}}
  public Dictionary<TileType, Color> TileColors {get {return _TileColors;}}
  public Color color {set; get;}

  public static Tile Wall   = new Tile(TileType.Wall);
  public static Tile Floor  = new Tile(TileType.Floor);
  public static Tile Water  = new Tile(TileType.Water);
  public static Tile Lava   = new Tile(TileType.Lava);

  public Tile() {
    _type = TileType.Wall;
    color = _TileColors[type];
  }
  
  public Tile(TileType t) {
    _type = t;
    color = _TileColors[type];
  }

  public override bool Equals(System.Object obj) {
    Tile tile = obj as Tile;
    if (tile == null) return false;
    else return tile.type == this.type;
  }

  public override int GetHashCode() {
    return (int) this.type;
  }
}
