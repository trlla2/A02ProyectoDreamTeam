using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUpEffect : ScriptableObject //scriptable object to inheritance multiple types of power ups
{

    public Sprite powerUpSprite;
    public abstract void Apply(GameObject target);  //apply power ups to target

}
