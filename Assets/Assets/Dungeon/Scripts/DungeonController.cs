using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Class that controlls _dungeon behavior
 */
public class DungeonController : Controller {
#region Public Variables
  
  public DungeonPlane plane;
  public int minRooms; 
  public int maxRooms;
  public int minRoomSize; 
  public int maxRoomSize;
  public int maxExtraTunnels;
  public Character player;
  public float move_rate = 0.01f;

  public MonsterController mController;

#endregion
#region Private Variables

  private Dungeon _dungeon;
  private Vector3 offset;
  private float lastTime;
  private Dictionary <KeyCode, Vector2> keys = 
    new Dictionary <KeyCode, Vector2>()
     {{KeyCode.UpArrow,    new Vector2(0,+1)},
      {KeyCode.DownArrow,  new Vector2(0,-1)},
      {KeyCode.RightArrow, new Vector2(+1,0)},
      {KeyCode.LeftArrow,  new Vector2(-1,0)}};
  private Vector2 pathtohere;
  private bool gameOver = false; 

#endregion
#region Delegates and Events

  // Create a delegate
  public delegate void DungeonUpdateEvent(); 
  // Create an event 
  public event DungeonUpdateEvent OnDungeonUpdate;

#endregion
#region Unity Methods
	
  void Start() {
    offset = new Vector3(0,3,0);
    lastTime = Time.time;
    GenerateDungeon();
  }
	
  void Update() {
    if (turn == true) {
      foreach (KeyCode k in keys.Keys) {
        if (Input.GetKeyDown(k) || 
            (Input.GetKey(k) && Time.time - lastTime > move_rate)) {
          if (player.GetStat(Stat.Moves) > 0) {
            player.DecrementStat(Stat.Moves);
            MovePlayer(keys[k]);
            lastTime = Time.time;
          }
        }
      }
      if (player.GetStat(Stat.Moves) == 0) {
        EndTurn();
      }


      if (Input.GetKeyDown("1")) {
        plane.Revert();
        pathtohere = player.pos;
        plane.Highlight(pathtohere);
      }
      if (Input.GetKeyDown("2")) {
        plane.Revert();
        List<int> path = AStar.Pathfind(_dungeon, player.pos, pathtohere);
        foreach (int i in path) {
          plane.Highlight(_dungeon.GetVec2(i));
        }
      }
			
	  if (player.IsDead()) {
        gameOver = true;
        EndTurn();
      }

    }
    plane.Revert();
	}

#endregion
#region Public Variables
  public Dungeon dungeon {get {return _dungeon;}}

  public void MovePlayer(Vector2 dir) {
    if (_dungeon.CanMove(player.pos + dir)) {
      if (!mController.IsMonsterAt(player.pos + dir)) {
        player.Move(dir);
        offset += new Vector3(dir.x, 0, dir.y);
      }
    }
  }

  public void GenerateDungeon() {
    plane.Init();
	  gameOver = false;
    _dungeon = new Dungeon(plane.width, plane.height);
    _dungeon.SetStats(minRooms, maxRooms, minRoomSize, maxRoomSize, maxExtraTunnels);
    _dungeon.Init();
    player.offset = 
      new Vector3((_dungeon.width + 1) % 2 * 0.5f, 0.5f, (_dungeon.height + 1) % 2 * 0.5f);
    player.MoveTo(_dungeon.GetStart());
    player.UpdateTransforms();
    player.FullReset();
    // Clear old monsters.
    plane.BuildTexture(_dungeon);
    OnDungeonUpdate();
  }
  
  public void EditorDungeon() {
    plane.Init();
	  gameOver = false;
    _dungeon = new Dungeon(plane.width, plane.height);
    _dungeon.SetStats(minRooms, maxRooms, minRoomSize, maxRoomSize, maxExtraTunnels);
    _dungeon.Init();
    player.offset = 
      new Vector3((_dungeon.width + 1) % 2 * 0.5f, 0.5f, (_dungeon.height + 1) % 2 * 0.5f);
    player.MoveTo(_dungeon.GetStart());
    player.UpdateTransforms();
    // Clear old monsters.
    plane.BuildTexture(_dungeon);
  }
	
  public bool GameOver(){
    return gameOver;
  }

  public bool WinGame() {
    return mController.MonstersLeft() == 0;
  }

#endregion
}
