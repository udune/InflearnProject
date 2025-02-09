using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelUpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image expSlider;
    [SerializeField] private TextMeshProUGUI expText, atkText, goldText, hpText, getExpText;
    private bool isPush;
    private float timer = 0.01f;
    private Coroutine coroutine;

    private void Start()
    {
        InitExp();
    }

    private void Update()
    {
        if (isPush)
        {
            timer += Time.deltaTime;
            if (timer >= 0.01f)
            {
                timer = 0.0f;
                ExpUp();
            }
        }
    }

    public void ExpUp()
    {
        BaseManager.Player.ExpUp();
        InitExp();
        transform.DORewind();
        transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.25f);
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        ExpUp();
        coroutine = StartCoroutine(PushCoroutine());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPush = false;
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        timer = 0.0f;
    }

    private void InitExp()
    {
        expSlider.fillAmount = BaseManager.Player.EXP_Percentage();
        expText.text = $"{BaseManager.Player.EXP_Percentage() * 100:0.00}%";
        atkText.text = "+" + BaseManager.Player.Next_ATK().ToCurrencyString();
        hpText.text = "+" + BaseManager.Player.Next_HP().ToCurrencyString();
        getExpText.text = $"<color=#00FF00>EXP</color> +{BaseManager.Player.Next_Exp():0.00}%";
    }

    private IEnumerator PushCoroutine()
    {
        yield return new WaitForSeconds(1.0f);
        isPush = true;
    }
}
