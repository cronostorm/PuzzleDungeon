using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Class for the dungeon crawling Character
 */
public class Monster : Character {

 public override void UpdateTransforms () {
   this.gameObject.transform.localPosition =
      new Vector3(pos.x, 0, pos.y) + offset;
  }
}
