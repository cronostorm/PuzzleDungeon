using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class DungeonPlane : MonoBehaviour {
  public int width = 0;
  public int height = 0;
  public Vector3 offset = new Vector3(0, 0, 0);
  public int xRes = 1;
  public int yRes = 1;
  public Dungeon dungeon;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

  public void BuildTexture(Dungeon d) {
    Texture2D texture = new Texture2D(width * xRes, height * yRes);
    for (int x = 0; x < width; x++) {
      for (int y = 0; y < height; y++) {
        texture.SetPixel(x , y , d.getTile(x, y).color);
      }
    }
    texture.Apply();

    texture.filterMode = FilterMode.Point;
    MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
    mesh_renderer.sharedMaterial.mainTexture = texture;
  }
}
