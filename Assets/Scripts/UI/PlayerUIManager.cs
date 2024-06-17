using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    private GameObject maruSlider;
    private Image maruSliderImage;
    private GameObject maruSliderEdge;

    private GameObject nabiSlider;
    private Image nabiSliderImage;
    private GameObject nabiSliderEdge;

    private float currentUltimateGuageNabi = 0.0f;
    private float currentUltimateGuageMaru = 0.0f;
    private float maxUltimateGuage = 2500f;

    void Start()
    {
        maruSlider = GameObject.Find("M_Ultimate_Bar");
        maruSliderImage = maruSlider.GetComponent<Image>();
        maruSliderEdge = GameObject.Find("M_Ultimate_Edge");

        nabiSlider = GameObject.Find("N_Ultimate_Bar");
        nabiSliderImage = nabiSlider.GetComponent<Image>();
        nabiSliderEdge = GameObject.Find("N_Ultimate_Edge");

        SliderUpdateMaru();
        SliderUpdateNabi();
    }

    void Update()
    {
        if (currentUltimateGuageMaru != PlayerMaru.ultimateGauge)
        {
            SliderUpdateMaru();
        }

        if (currentUltimateGuageNabi != PlayerNabi.ultimateGauge)
        {
            SliderUpdateNabi();
        }
    }

    public void SliderUpdateNabi()
    {
        nabiSliderImage.fillAmount = PlayerNabi.ultimateGauge / maxUltimateGuage;
        currentUltimateGuageNabi = PlayerNabi.ultimateGauge;

        float edgePos = (nabiSliderImage.fillAmount - 0.5f) * 105f;
        nabiSliderEdge.transform.localPosition = new Vector3(edgePos, 0, 0);
    }

    public void SliderUpdateMaru()
    {
        //maruSliderImage.fillAmount = Mathf.Lerp(maruSliderImage.fillAmount, PlayerMaru.ultimateGauge / maxUltimateGuage, lerpSpeed * Time.deltaTime);
        maruSliderImage.fillAmount = PlayerMaru.ultimateGauge / maxUltimateGuage;
        currentUltimateGuageMaru = PlayerMaru.ultimateGauge;

        float edgePos = (maruSliderImage.fillAmount - 0.5f) * 105f;
        maruSliderEdge.transform.localPosition = new Vector3(edgePos, 0, 0);
    }
}
