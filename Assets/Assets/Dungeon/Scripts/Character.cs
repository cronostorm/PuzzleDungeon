using UnityEngine;
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

  private int _health = 0;
  private int _moves  = 0;
  private int _attack = 0;
  private int _armor  = 0;
  private int _magic  = 0;
  private Dictionary<Stat, int>characterStats;

#endregion
#region Unity Methods

	void Start() {
    characterStats = new Dictionary<Stat, int>()
      {{Stat.Health, _health}, {Stat.Moves, _moves}, {Stat.Attack, _attack},
       {Stat.Armor, _armor}, {Stat.Magic, _magic}};
	}
	
	void Update() {
    UpdateTransforms();
	}

#endregion
#region Accessor Methods

  public int health   {get {return _health;}}
  public int moves    {get {return _moves;}}
  public int attack   {get {return _attack;}}
  public int armor    {get {return _armor;}}
  public int magic    {get {return _magic;}}
  public int GetStat(Stat s) {
    return characterStats[s];
  }

#endregion
#region Public Methods

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

  public void DecerementStat(Stat s) {
    characterStats[s] -= 1;
    // Trigger Event OnStatsUpdate
    if (OnStatsUpdate != null) OnStatsUpdate();
  }

#endregion
}
