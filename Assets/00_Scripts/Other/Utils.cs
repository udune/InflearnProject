using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static string String_Color_Rarity(Rarity rare)
    {
        switch (rare)
        {
            case Rarity.UnCommon:
                return "<color=#FFFFFF>";
            case Rarity.Common:
                return "<color=#00FF00>";
            case Rarity.Rare:
                return "<color=#FF0000>";
            case Rarity.Legendary:
                return "<color=#00FFFF>";
            case Rarity.Hero:
                return "<color=#00FFFF>";
        }
        return "<color=#FFFFFF>";
    }
}
