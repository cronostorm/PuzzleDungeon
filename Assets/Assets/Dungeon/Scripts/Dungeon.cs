using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dungeon {
#region Public Variables

  public int MinRooms;
  public int MaxRooms;
  public int MinRoomSize;
  public int MaxRoomSize;
  public Tile[] map;

#endregion
#region Private Variables

  private int _width;
  private int _height;
  private List<Tile> tiles = 
    new List<Tile>() {Tile.Wall, Tile.Floor, Tile.Water, Tile.Lava};
  
#endregion
#region Constructors

	public Dungeon() {
	  map = new Tile[_width * _height];
    for (int i = 0; i < _width * _height; i++) {
      map[i] = new Tile();
    }
	}
	
	public Dungeon (int i, int j) {
	  _width = i;
    _height = j;
	  map = new Tile[_width * _height];
    for (int k = 0; k < _width * _height; k++) {
      int type = Random.Range(0, tiles.Count);
      map[k] = tiles[type]; 
    }
	}

#endregion
#region Accessor Methods

  public int width {get {return _width;}}
  public int height {get {return _height;}}

  public Tile GetTile (int i, int j) {
    return map[i + j * _width];
  }

  public Tile GetTile (Vector2 idx) {
    return GetTile((int) idx.x, (int) idx.y);
  }

  public Vector2 GetIdx (Vector3 pos) {
    return new Vector2((int) (pos.x + _width/2), (int) (pos.z + _height/2));
  }

  public Vector2 GetVec2 (int idx) {
    return new Vector2(idx % width, idx / width);
  }

  public Vector2 GetCenter() {
    return new Vector2((int) _width/2, (int) _height/2);
  }

  public Vector3 GetTilePos(Vector2 idx) {
    return new Vector3(idx.x, 0, idx.y);
  }

#endregion
#region Public Methods

  public bool CanMove (Vector2 idx) {
    int i = (int) idx.x;
    int j = (int) idx.y;
    if (i < 0 || i >= _width || j < 0 || j >= _height) {
      return false;
    }
    return map[i + j * _width].IsPassable();
  }

#endregion
}
