using UnityEngine;
using System.Collections;

/*
 * Script that textures a plane to look like a dungeon
 */
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class DungeonPlane : MonoBehaviour {
#region Public Variables

  public int width = 0;
  public int height = 0;
  public Vector3 offset = new Vector3(0, 0, 0);
  public int xRes = 1;
  public int yRes = 1;

#endregion 
#region Private Variables

#endregion
#region Unity Methods

	void Start() {
	}
	
	void Update() {
	}

#endregion 
#region Public Methods

  public void Init() {
    Vector3 scale = Vector3.one;
    scale.x = width * 1.6f/16;
    scale.z = height * 0.9f/9;
    this.gameObject.transform.localScale = scale;
    this.gameObject.transform.localPosition = new Vector3(width/2, 0, height/2);
  }

  public void BuildTexture(Dungeon d) {
    Texture2D texture = new Texture2D(width * xRes, height * yRes);
    for (int x = 0; x < width; x++) {
      for (int y = 0; y < height; y++) {
        texture.SetPixel(width - x - 1, height - 1 - y, d.GetTile(x, y).color);
      }
    }
    texture.Apply();

    texture.filterMode = FilterMode.Point;
    MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
    mesh_renderer.sharedMaterial.mainTexture = texture;
  }

#endregion
}
