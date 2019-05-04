using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButtons : MonoBehaviour
{
    public string _name;
    public bool unlocked;
    public string[] toBeUnlocked;
    public int levelToUnlock;
    public int nessesaryXPLevel;
    public int maxLevel;
    public int skillPointCost = 1;
    [HideInInspector]
    public string text;
}
