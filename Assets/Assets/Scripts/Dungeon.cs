﻿using UnityEngine;
using System.Collections;

public class Dungeon {
  public Tile[] map;
  private int _width;
  private int _height;
  int width {get {return _width;}}
  int height {get {return _height;}}
  
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
      map[k] = new Tile(0.0f, new Color(Random.Range(0.0f,1.0f), 
            Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f)));
    }
	}

  public Tile getTile (int i, int j) {
    return map[i + j * _width];
  }

}
