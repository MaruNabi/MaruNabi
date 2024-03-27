using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public Slider nabiSlider;
    public Slider maruSlider;
    private float currentUltimateGuage = 0.0f;

    public Image[] maruLife;
    public Image[] nabiLife;
    public Sprite blankHP, fillHP;

    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            maruLife[i].sprite = fillHP;
            nabiLife[i].sprite = fillHP;
        }
    }

    void Update()
    {
        if (currentUltimateGuage != Player.ultimateGauge)
        {
            SliderUpdate();
        }
    }

    public void SliderUpdate()
    {
        nabiSlider.value = Player.ultimateGauge;
        currentUltimateGuage = Player.ultimateGauge;
    }
}
