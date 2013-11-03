using UnityEngine;
using System.Collections;

public class DungeonController : MonoBehaviour {
  public DungeonPlane plane;
  public Dungeon dungeon;
  public Camera cam;
  public Character player;

  private Vector3 offset;

	// Use this for initialization
	void Start () {
    offset = new Vector3(0,3,0);
	}
	
	// Update is called once per frame
	void Update () {
    MovePlayer(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
    cam.transform.localPosition = offset;
    if (Input.GetKeyDown("space")) {
      GenerateDungeon();
    }
	}

  void MovePlayer(Vector3 dir) {
    player.Move(dir);
    offset += dir;
  }

  public void GenerateDungeon() {
    offset = new Vector3(0,3,0);
    Vector3 scale = Vector3.one;
    scale.x = plane.width * 1.6f/16;
    scale.z = plane.height * 0.9f/9;
    plane.transform.localScale = scale;
    player.MoveTo(Vector3.zero);
    player.Move(new Vector3(0, 1, 0));
    if (plane.width % 2 == 1) {
      offset += new Vector3(0.5f, 0, 0);
    }
    else {
      player.Move(new Vector3(-0.5f, 0, 0));
    }
    if (plane.height % 2 == 0) {
      offset += new Vector3(0, 0, 0.5f);
      player.Move(new Vector3(0, 0, 0.5f));
    }
    cam.transform.localPosition = offset;

    dungeon = new Dungeon(plane.width, plane.height);
    plane.BuildTexture(dungeon);
  }
}
