﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Enumeration for player stats
 */
public enum Stat {
  Health, Moves, Attack, Armor, Magic
}

/*
 * Class for the dungeon crawling Character
 */
public class Character : MonoBehaviour {

#region Public Variables

  public Vector2 pos;
  [HideInInspector]
  public Vector3 offset= new Vector3(0,0.5f,0.5f);
  public Camera cam;
  public float camy = 2.5f;

#endregion
#region Delegates and Events

  // Create a delegate named StatsUpdatEvent
  public delegate void StatsUpdateEvent(); 
  // Create an event OnStatsUpdate
  public event StatsUpdateEvent OnStatsUpdate;

#endregion
#region Private Variables

  protected Dictionary<Stat, int>characterStats;

#endregion
#region Unity Methods

	void Start() {
    characterStats = new Dictionary<Stat, int>()
      {{Stat.Health, 1}, {Stat.Moves, 1}, {Stat.Attack, 1},
       {Stat.Armor, 0}, {Stat.Magic, 0}};
	}
	
	void Update() {
    UpdateTransforms();
	}

#endregion
#region Accessor Methods

  public int health   {get {return characterStats[Stat.Health];}}
  public int moves    {get {return characterStats[Stat.Moves];}}
  public int attack   {get {return characterStats[Stat.Attack];}}
  public int armor    {get {return characterStats[Stat.Armor];}}
  public int magic    {get {return characterStats[Stat.Magic];}}
  public float attack_range = 2f;
  public int GetStat(Stat s) {
    return characterStats[s];
  }

#endregion
#region Public Methods

  public bool IsDead() {
    return (health <= 0);
  }

  public bool EnemyInRange(Character c) {
    return DistanceToCharacter(c) < attack_range;
  }

  public bool Attack(Character c) {
    // Also check if player in sight (not through walls).
    if (EnemyInRange(c)) {
      c.DecrementStat(Stat.Health);
      return true;
    }
    return false;
  }

  public void Move(Vector2 dir) {
    
      pos += dir;
  }
  
  public void MoveTo(Vector2 dir) {
      pos = dir;
  }

  public virtual void UpdateTransforms () {
	  this.gameObject.transform.localPosition = 
      new Vector3(pos.x, 0, pos.y) + offset;
    cam.transform.localPosition = 
      new Vector3(pos.x, camy, pos.y) + offset;
  }

  public void IncrementStat(Stat s) {
    characterStats[s] += 1;
    // Trigger Event OnStatsUpdate
    if (OnStatsUpdate != null) OnStatsUpdate();
  }

  public void DecrementStat(Stat s) {
    characterStats[s] -= 1;
    // Trigger Event OnStatsUpdate
    if (OnStatsUpdate != null) OnStatsUpdate();
  }

  public void FullReset() {
    characterStats = new Dictionary<Stat, int>()
      {{Stat.Health, 1}, {Stat.Moves, 1}, {Stat.Attack, 1},
       {Stat.Armor, 0}, {Stat.Magic, 0}};
    OnStatsUpdate();
  }

  public void TurnReset() {
    characterStats[Stat.Moves] = 1;
    characterStats[Stat.Attack] = 1;
    characterStats[Stat.Armor] = 0;
    characterStats[Stat.Magic] = 0;
    OnStatsUpdate();
  }

  public float DistanceToObject(Vector3 obj_location) {
    Vector3 self = this.gameObject.transform.localPosition;
    float dist = Vector3.Distance(self, obj_location);
    return dist;
  }

  public float DistanceToCharacter(Character c) {
    return DistanceToObject(c.transform.localPosition);
  }

#endregion
}
