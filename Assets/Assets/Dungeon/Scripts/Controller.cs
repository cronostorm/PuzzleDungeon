using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
  protected bool turn = false;

  // Create a delegate
  public delegate void EndTurnEvent(); 
  // Create an event 
  public event EndTurnEvent OnEndTurn;

  
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

  public void Toggle(bool newturn) {
    turn = newturn;
  }

  protected virtual void EndTurn() {
    EndTurnEvent endturn = OnEndTurn;
    if (endturn != null) {
      endturn();
    }
  }
}
