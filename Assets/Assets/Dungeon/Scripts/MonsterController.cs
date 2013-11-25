﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MonsterController : Controller {

  public DungeonPlane plane;
  public GameObject MonsterObj;
  public Character player;
  public int minMonsters;
  public int maxMonsters;
  public int maxMonsterLevel;
  public int minMonsterLevel;
  public DungeonController dungeonController;

  private Dungeon _dungeon;
  private ArrayList monsters = new ArrayList();

	// Use this for initialization
	void Start () {
	}

  void OnEnable() {
    dungeonController.OnDungeonUpdate +=  () => SetDungeon();
    dungeonController.OnDungeonUpdate +=  () => GenMonsters();
  }

  void OnDisable() {
    dungeonController.OnDungeonUpdate -=  () => GenMonsters();
    dungeonController.OnDungeonUpdate -=  () => SetDungeon();
  }

  void SetDungeon() {
    _dungeon = dungeonController.dungeon;
  }
	
	// Update is called once per frame
	void Update () {
    if (turn == true) {
      for (int i = 0; i < monsters.Count; i++) {
        Monster m = (Monster)monsters[i];
        // Monster move
        List<int> path = AStar.Pathfind(_dungeon, m.pos, player.pos);
        // Move along path.
        if (path.Count != 0) {
          m.MoveTo(_dungeon.GetVec2(path[path.Count - 1]));
        }
        // Check if I can attack
        m.AttackPlayer(player);
      }
      EndTurn();
    }
	}

  public void DeleteMonsters() {
    foreach (Monster m in monsters) {
      Destroy(m.gameObject);
    }
  }

  public void GenMonsters() {
    DeleteMonsters();
    monsters.Clear();
    // Build monsters.
    int numMonsters = Random.Range(minMonsters, maxMonsters + 1);
    for (int i = 0; i < numMonsters; i++) {
      GameObject temp = Instantiate(MonsterObj) as GameObject;
      Monster m = temp.GetComponent<Monster>();
      int monsterLevel = Random.Range(minMonsterLevel, maxMonsterLevel + 1);
      m.InitStats(monsterLevel);
      // Generate monster in new random position.
      int numRooms = _dungeon.GetNumRooms();
      m.offset =
        new Vector3((_dungeon.width + 1) % 2 * 0.5f, 0.5f,
          (_dungeon.height + 1) % 2 * 0.5f);
      // Don't spawn in same spot as player (room 0).
      int randRoom = Random.Range (1, numRooms);
      m.MoveTo(_dungeon.GetRoom (randRoom));
      // m.UpdateTransforms();
      monsters.Add(m);
    }
  }
}
