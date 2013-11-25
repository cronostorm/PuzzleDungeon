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

  public void InitStats (int level) {
    characterStats = new Dictionary<Stat, int>()
      {{Stat.Health, Random.Range(1,5 * level)},
       {Stat.Moves, Random.Range(1,1 * level + 1)},
       {Stat.Attack, Random.Range(1,5 * level)},
       {Stat.Armor, Random.Range(0,2 * level)},
       {Stat.Magic, Random.Range(0,2 * level)}};
  }

  public bool AttackPlayer(Character player) {
    // Also check if player in sight (not through walls).
    if (DistanceToCharacter(player) < attack_range) {
      player.DecrementStat(Stat.Health);
      return true;
    }
    return false;
  }
}
