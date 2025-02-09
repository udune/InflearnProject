using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable", menuName = "Object/Character", order = int.MaxValue)]
public class CharacterScriptable : ScriptableObject
{
    public string characterName;
    public float attackRange;
    public Rarity rarity;
}
