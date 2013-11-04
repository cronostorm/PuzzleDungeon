using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dungeon {
  public Tile[] map;
  private int _width;
  private int _height;
  int width {get {return _width;}}
  int height {get {return _height;}}

  private List<Tile> tiles = 
    new List<Tile>() {Tile.Wall, Tile.Floor, Tile.Water, Tile.Lava};
  
	// Use this for initialization
	public Dungeon() {
	  map = new Tile[_width * _height];
    for (int i = 0; i < _width * _height; i++) {
      map[i] = new Tile();
    }
	}
	
	// Update is called once per frame
	public Dungeon (int i, int j) {
	  _width = i;
    _height = j;
	  map = new Tile[_width * _height];
    for (int k = 0; k < _width * _height; k++) {
      int type = Random.Range(0, tiles.Count);
      map[k] = tiles[type]; 
    }
	}

  public Tile GetTile (int i, int j) {
    return map[i + j * _width];
  }

  public Tile GetTile (Vector2 idx) {
    return GetTile((int) idx.x, (int) idx.y);
  }

  public bool CanMove (Vector2 idx) {
    int i = (int) idx.x;
    int j = (int) idx.y;
    if (i < 0 || i >= _width || j < 0 || j >= _height) {
      return false;
    }
    return !(map[i + j * _width] == Tile.Wall);
  }

  public Vector2 GetIdx (Vector3 pos) {
    return new Vector2((int) (pos.x + _width/2), (int) (pos.z + _height/2));
  }

  public Vector2 GetCenter() {
    return new Vector2((int) _width/2, (int) _height/2);
  }

  public Vector3 GetTilePos(Vector2 idx) {
    return new Vector3(idx.x, 0, idx.y);
  }
}
