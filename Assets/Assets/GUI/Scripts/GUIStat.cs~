using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Class for updating Stat text on the gui
 * Uses delegates and events 
 */
public class GUIStat : MonoBehaviour {

#region Public Variables

  public Character character;
  public Stat s;

#endregion
#region Private Variables

  private Dictionary<Stat, string> HeaderText = 
    new Dictionary<Stat, string>()
      {{Stat.Health, "Health\t"}, {Stat.Armor, "Armor\t"}, 
       {Stat.Moves, "Moves\t"}, {Stat.Attack, "Attack\t"}, 
       {Stat.Magic, "Magic\t"}};

#endregion
#region Unity Methods

  void Start () {
    UpdateText();
  }

  void OnEnable() {
    // Add UpdateText from character's OnStatsUpdate event
    character.OnStatsUpdate += () => UpdateText();
  }

  void OnDisable() {
    // Remove UpdateText from character's OnStatsUpdate event
    character.OnStatsUpdate -= () => UpdateText();
  }
  
  void Update () {
  }

#endregion
#region Public Methods

  void UpdateText() {
    GUIText label = this.gameObject.GetComponent<GUIText>();
    label.text = HeaderText[s] + character.GetStat(s);
  }

#endregion
}
