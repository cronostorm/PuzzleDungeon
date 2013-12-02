using UnityEngine;
using UnityEditor;
using System.Collections;

public class GameOverPopup : MonoBehaviour {
  //public bool doWindow0 = true;
  public bool toggle = false;
  public bool click = false;
  void DoWindow(int windowID) {
    click = GUI.Button(new Rect(10, 30, 80, 20), "New Game");
    if (click) {
      print ("Clicked");
    }
  }

  public void ShowPopup(bool show) {
    toggle = show;
  }

  void OnGUI() {
    if (toggle) {
      GUI.Window(0, new Rect(110, 10, 200, 60), DoWindow, "Game Over");
      // print ("Window on");
      click = false;
      toggle = false;
    }
  }

  public bool StartOver() {
    return click;
  }

}