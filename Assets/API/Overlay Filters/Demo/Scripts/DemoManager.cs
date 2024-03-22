using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace OverlayFilters.Demo
{
    public class DemoManager : MonoBehaviour
    {
        public static DemoManager c;
        public static float currentFade;

        //References:
        Text text;
        Text description;
        Transform overlaysParent;
        List<DemoExample> overlays;

        //Interface:
        Toggle fullscreen;
        Slider fade;
        Dropdown layer;

        //Internal:
        int index;
        Transform currentOverlay;

        void Awake()
        {
            c = this;
            overlaysParent = GameObject.Find("Overlays").transform;
            text = transform.Find("Text").GetComponent<Text>();
            description = transform.Find("Description").GetComponent<Text>();

            fade = transform.Find("Fade").GetComponent<Slider>();
            fullscreen = transform.Find("Fullscreen").GetComponent<Toggle>();
            layer = transform.Find("Layer").GetComponent<Dropdown>();

            overlays = new List<DemoExample>();
            for(int n = 0; n < overlaysParent.childCount; n++)
            {
                overlays.Add(overlaysParent.GetChild(n).GetComponent<DemoExample>());
                overlaysParent.GetChild(n).gameObject.SetActive(true);
            }
        }

        private void Start()
        {
            index = 0;
            UpdateIndex();
        }

        void Update()
        {
            if (Input.mouseScrollDelta.y < 0)
            {
                CycleNext();
            }

            if (Input.mouseScrollDelta.y > 0)
            {
                CycleBack();
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                fullscreen.isOn = !fullscreen.isOn;
            }
        }

        void CycleNext()
        {
            index++;
            if(index >= overlaysParent.childCount)
            {
                index = 0;
            }

            UpdateIndex();
        }
        void CycleBack()
        {
            index--;
            if(index < 0)
            {
                index = overlaysParent.childCount - 1;
            }

            UpdateIndex();
        }
        void UpdateIndex()
        {
            currentOverlay = overlaysParent.GetChild(index);
            UpdateText();

            fullscreen.isOn = !overlays[index].notFullscreen;
            currentFade = fade.value = overlays[index].defaultFade;
            layer.value = overlays[index].GetLayerValue();

            description.text = overlays[index].description;

            foreach (DemoExample example in overlays)
            {
                example.FadeOut();
            }
            overlays[index].FadeIn();

            fade.interactable = !overlays[index].leftClickCreate && !overlays[index].fadeParticles;
            fullscreen.interactable = !overlays[index].leftClickCreate && !overlays[index].fadeParticles;
        }
        void UpdateText()
        {
            text.text = "<color=#FFFFFFaa>Current: </color>" + currentOverlay.name + "<color=#FFFFFFaa>  ("+ (index + 1) + "/" + overlays.Count + ")</color><size=15>\n\nScroll <color=#FFFFFFaa>to cycle through different examples.\nUse </color>A,W,S,D <color=#FFFFFFaa>to move the player around.</color></size>";
        }

        public void SetLayer()
        {
            overlays[index].SetLayerValue(layer.value);

            if(EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
        public void SetFade()
        {
            currentFade = fade.value;

            if (EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }

        public bool IsFullscreen()
        {
            return fullscreen.isOn;
        }

        public int GetIndex()
        {
            return index;
        }
    }

}