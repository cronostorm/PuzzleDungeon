﻿using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(DungeonController))]
public class DungeonPlaneEditor : Editor {
  public override void OnInspectorGUI() {
    DrawDefaultInspector();
    if(GUILayout.Button("new map")) { 
      ((DungeonController) target).EditorDungeon();
    }
  }
}
