using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#region Enums
public enum State {Dungeon, Puzzle, AI};
#endregion 

public class GameFlowController : MonoBehaviour {
#region Private Variables
  private State _state;
  private Dictionary<State, State> _nextState =
    new Dictionary<State, State> {{State.Puzzle, State.Dungeon}, 
                           {State.Dungeon, State.AI}, 
                           {State.AI, State.Puzzle}};
  private Dictionary<State, Controller> _controllers;
#endregion
#region Public Variables

  public State state {get {return _state;}}
  public GemGen gemController;
  public DungeonController dungeonController;
  public MonsterController monsterController;
  public GameOverPopup gameOverPopup;

#endregion
#region Unity Methods

	// Use this for initialization
	void Start () {
	   _state = State.Puzzle;
     _controllers = new Dictionary<State, Controller> 
      {{State.Puzzle,  gemController}, 
       {State.Dungeon,  dungeonController}, 
       {State.AI,  monsterController}};
    _controllers[_state].Toggle(true);

  }

  void OnEnable() {
    gemController.OnEndTurn += () => AdvanceState();
    monsterController.OnEndTurn += () => AdvanceState();
    dungeonController.OnEndTurn += () => AdvanceState();
	}

  void OnDisable() {
    dungeonController.OnEndTurn -= () => AdvanceState();
    monsterController.OnEndTurn -= () => AdvanceState();
    gemController.OnEndTurn -= () => AdvanceState();
  }
	
	// Update is called once per frame
	void Update () {
	  if (Input.GetKeyDown("space")) {
      ResetGameState();
    }

    if (dungeonController.GameOver()) {
      DisplayGameOver();

    }

	}

#endregion
#region Public Methods

  public void ResetGameState() {
      dungeonController.GenerateDungeon();
      monsterController.GenMonsters();
  }

  public void AdvanceState() {
    _controllers[_state].Toggle(false);
    _state = _nextState[_state];
    _controllers[_state].Toggle(true);
  }
  public void DisplayGameOver() {
    gameOverPopup.ShowPopup(true);
    //gameOverPopup.StartOver();
    ResetGameState();
    print("GAME OVER");

  }

#endregion
}
