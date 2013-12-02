﻿using UnityEngine;
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
  public int xRes = 16;
  public int yRes = 16;
  public Texture2D tileSprites;

#endregion 
#region Private Variables

  private Texture2D tex;
  private Texture2D modified;
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
    tex = new Texture2D(width * xRes, height * yRes);
    tex.filterMode =  FilterMode.Point;
    modified = new Texture2D(width * xRes, height * yRes);
    modified.filterMode =  FilterMode.Point;
  }

  public void BuildTexture(Dungeon d) {
    
    Color[][] sprites = new Color[5][];
    sprites[0] = tileSprites.GetPixels(xRes * 0, 0, xRes, yRes);
    sprites[1] = tileSprites.GetPixels(xRes * 0, 0, xRes, yRes);
    sprites[2] = tileSprites.GetPixels(xRes * 1, 0, xRes, yRes);
    sprites[3] = tileSprites.GetPixels(xRes * 2, 0, xRes, yRes);
    sprites[4] = tileSprites.GetPixels(xRes * 3, 0, xRes, yRes);

    for (int x = 0; x < width * xRes; x += xRes) {
      for (int y = 0; y < height * yRes; y+= yRes) {
        tex.SetPixels(width * xRes - x - xRes, height * yRes - yRes - y, 
            xRes, yRes, sprites[d.GetTileType(x/xRes, y/yRes)]);
      }
    }
    tex.Apply();

    MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
    mesh_renderer.sharedMaterial.mainTexture = tex;
    modified.SetPixels(tex.GetPixels());
    modified.Apply();
  }

  public void Highlight(Vector2 v) {
    modified.SetPixel(width - (int) v.x - 1, height - 1 - (int) v.y, Color.yellow);
    modified.Apply();
    MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
    mesh_renderer.sharedMaterial.mainTexture = modified;
  }

  public void Revert() {
    MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
    mesh_renderer.sharedMaterial.mainTexture = tex;
    modified.SetPixels(tex.GetPixels());
    modified.Apply();
  }
#endregion
}
