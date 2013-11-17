using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Basic Tile class for the dungeon
 * Each grid is a representation of a tile class
 */
public class Tile {

#region Public Variables

  public enum TileType {Wall, Stone, Floor, Water, Lava};
  public TileType type {get {return _type;}}
  public Dictionary<TileType, Color> TileColors {get {return _TileColors;}}
  public Color color {set; get;}

  public static Tile Stone  = new Tile(TileType.Stone);
  public static Tile Wall   = new Tile(TileType.Wall);
  public static Tile Floor  = new Tile(TileType.Floor);
  public static Tile Water  = new Tile(TileType.Water);
  public static Tile Lava   = new Tile(TileType.Lava);

#endregion
#region Private Variables

  private TileType _type;
  private Dictionary<TileType, Color> _TileColors = 
    new Dictionary<TileType, Color> 
    {{TileType.Stone, Color.black},
     {TileType.Wall,  Color.black},
     {TileType.Floor, Color.grey},
     {TileType.Water, Color.blue},
     {TileType.Lava,  Color.red}};

  private Dictionary<TileType, int> _costs = 
    new Dictionary<TileType, int> 
    {{TileType.Stone, 20},
     {TileType.Wall,  4},
     {TileType.Floor, 1},
     {TileType.Water, 1},
     {TileType.Lava,  1}};


#endregion
#region Constructors

  public Tile() {
    _type = TileType.Wall;
    color = _TileColors[type];
  }
  
  public Tile(TileType t) {
    _type = t;
    color = _TileColors[type];
  }

#endregion
#region Override Methods

  public override bool Equals(System.Object obj) {
    Tile tile = obj as Tile;
    if (tile == null) return false;
    else return tile.type == this.type;
  }

  public override int GetHashCode() {
    return (int) this.type;
  }
#endregion
#region Public Methods
  
  public bool IsPassable(int mode) {
    if (mode == 0) { 
      return true;
    }
    else { 
      if (_type == TileType.Wall || _type == TileType.Stone) {
        return false;
      }
      return true;
    }
  }

  public int Cost() {
    return _costs[type];
  }

#endregion
}
