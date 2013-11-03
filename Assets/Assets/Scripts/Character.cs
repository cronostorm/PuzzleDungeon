using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {
  public Vector2 pos;
  public Vector3 offset= new Vector3(0,0.5f,0.5f);
  public Camera cam;
  public float camy = 2.5f;

	// Use this for initialization
	void Start() {
	}
	
	// Update is called once per frame
	void Update() {
    UpdateTransforms();
	}

  public void Move(Vector2 dir) {
    pos += dir;
  }
  
  public void MoveTo(Vector2 dir) {
    pos = dir;
  }

  public void UpdateTransforms () {
	  this.gameObject.transform.localPosition = 
      new Vector3(pos.x, 0, pos.y) + offset;
    cam.transform.localPosition = 
      new Vector3(pos.x, camy, pos.y) + offset;
  }
}
