﻿using UnityEngine;
using System.Collections;

public class Tile {
  float val {set; get;}
  public Color color {set; get;}

  public Tile() {
    val = 0.0f;
    color = Color.blue;
  }
  
  public Tile(float v, Color c) {
    val = v;
    color = c;
  }

}
