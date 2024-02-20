using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public Slider nabiSlider;
    public Slider maruSlider;
    private float currentUltimateGuage = 0.0f;

    void Start()
    {

    }

    void Update()
    {
        if (currentUltimateGuage != PlayerNabi.ultimateGauge)
        {
            SliderUpdate();
        }
    }

    private void SliderUpdate()
    {
        nabiSlider.value = PlayerNabi.ultimateGauge;
    }
}
