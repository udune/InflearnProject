using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    public static MainUI instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        TextCheck();
    }

    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI allAttackText;

    public void TextCheck()
    {
        levelText.text = $"LV.{BaseManager.Player.Level + 1}";
        allAttackText.text = BaseManager.Player.AverageAttack().ToCurrencyString();
    }
}
