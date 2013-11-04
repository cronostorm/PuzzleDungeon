﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DungeonController : MonoBehaviour {
  public DungeonPlane plane;
  public Dungeon dungeon;
  public Character player;
  public float move_rate = 0.01f;

  private Vector3 offset;
  private float lastTime;
  private Dictionary <KeyCode, Vector2> keys = 
    new Dictionary <KeyCode, Vector2>()
     {{KeyCode.UpArrow,    new Vector2(0,+1)},
      {KeyCode.DownArrow,  new Vector2(0,-1)},
      {KeyCode.RightArrow, new Vector2(+1,0)},
      {KeyCode.LeftArrow,  new Vector2(-1,0)}};

	// Use this for initialization
	void Start() {
    offset = new Vector3(0,3,0);
    lastTime = Time.time;
    GenerateDungeon();
	}
	
	// Update is called once per frame
	void Update() {
    foreach (KeyCode k in keys.Keys) {
      if (Input.GetKeyDown(k) || 
          (Input.GetKey(k) && Time.time - lastTime > move_rate)) {
        MovePlayer(keys[k]);
        lastTime = Time.time;
      }
    }

    if (Input.GetKeyDown("space")) {
      GenerateDungeon();
    }
	}

  void MovePlayer(Vector2 dir) {
    if (dungeon.CanMove(player.pos + dir)) {
      player.Move(dir);
      offset += new Vector3(dir.x, 0, dir.y);
    }
  }

  public void GenerateDungeon() {
    plane.Init();
    dungeon = new Dungeon(plane.width, plane.height);
    player.MoveTo(dungeon.GetCenter());
    player.UpdateTransforms();
    plane.BuildTexture(dungeon);
  }
}