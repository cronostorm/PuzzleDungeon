using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Class for the dungeon crawling Character
 */
public class Monster : Character {


  float attack_range = 2f;

  public override void UpdateTransforms () {
    this.gameObject.transform.localPosition =
      new Vector3(pos.x, 0, pos.y) + offset;
  }



  public bool AttackPlayer(Character player) {
    // Also check if player in sight (not through walls).
    if (DistanceToObject(player.transform.localPosition) < attack_range) {
      player.DecerementStat(Stat.Health);
      return true;
    }
    return false;
  }
}
