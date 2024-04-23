using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public Slider nabiSlider;
    public Slider maruSlider;
    private float currentUltimateGuageNabi = 0.0f;
    private float currentUltimateGuageMaru = 0.0f;

    void Start()
    {

    }

    void Update()
    {
        if (currentUltimateGuageNabi != PlayerNabi.ultimateGauge)
        {
            SliderUpdateNabi();
        }
        if (currentUltimateGuageMaru != PlayerMaru.ultimateGauge)
        {
            SliderUpdateMaru();
        }
    }

    public void SliderUpdateNabi()
    {
        nabiSlider.value = PlayerNabi.ultimateGauge;
        currentUltimateGuageNabi = PlayerNabi.ultimateGauge;
    }

    public void SliderUpdateMaru()
    {
        maruSlider.value = PlayerMaru.ultimateGauge;
        currentUltimateGuageMaru = PlayerMaru.ultimateGauge;
    }
}
