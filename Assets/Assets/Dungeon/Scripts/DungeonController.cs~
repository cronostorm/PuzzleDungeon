using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Class that controlls dungeon behavior
 */
public class DungeonController : MonoBehaviour {
#region Public Variables
  
  public DungeonPlane plane;
  public int minRooms; 
  public int maxRooms;
  public int minRoomSize; 
  public int maxRoomSize;
  public int maxExtraTunnels;
  public Dungeon dungeon;
  public Character player;
  public float move_rate = 0.01f;

#endregion
#region Private Variables

  private Vector3 offset;
  private float lastTime;
  private Dictionary <KeyCode, Vector2> keys = 
    new Dictionary <KeyCode, Vector2>()
     {{KeyCode.UpArrow,    new Vector2(0,+1)},
      {KeyCode.DownArrow,  new Vector2(0,-1)},
      {KeyCode.RightArrow, new Vector2(+1,0)},
      {KeyCode.LeftArrow,  new Vector2(-1,0)}};
  private Vector2 pathtohere;

#endregion
#region Unity Methods
	
  void Start() {
    offset = new Vector3(0,3,0);
    lastTime = Time.time;
    GenerateDungeon();
	}
	
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
    if (Input.GetKeyDown("1")) {
      plane.Revert();
      pathtohere = player.pos;
      plane.Highlight(pathtohere);
    }
    if (Input.GetKeyDown("2")) {
      plane.Revert();
      List<int> path = AStar.Pathfind(dungeon, player.pos, pathtohere);
      foreach (int i in path) {
        plane.Highlight(dungeon.GetVec2(i));
      }
    }
	}

#endregion
#region Public Variables

  public void MovePlayer(Vector2 dir) {
    if (dungeon.CanMove(player.pos + dir)) {
      player.Move(dir);
      offset += new Vector3(dir.x, 0, dir.y);
    }
  }

  public void GenerateDungeon() {
    plane.Init();
    dungeon = new Dungeon(plane.width, plane.height);
    dungeon.SetStats(minRooms, maxRooms, minRoomSize, maxRoomSize, maxExtraTunnels);
    dungeon.Init();
    player.offset = 
      new Vector3((dungeon.width + 1) % 2 * 0.5f, 0.5f, (dungeon.height + 1) % 2 * 0.5f);
    player.MoveTo(dungeon.GetStart());
    player.UpdateTransforms();
    plane.BuildTexture(dungeon);
  }
  
#endregion
}
