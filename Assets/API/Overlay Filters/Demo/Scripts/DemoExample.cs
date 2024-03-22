using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace OverlayFilters.Demo
{
    public class DemoExample : MonoBehaviour
    {
        SortingGroup group;
        int defaultOrder;

        [Header("Settings:")]
        public bool fadeSprites = true;
        public bool fadeParticles = false;
        public bool leftClickCreate = false;
        public bool notFullscreen = false;
        public bool canSpread = true;
        [Range(0,1)]
        public float defaultFade = 1f;

        [Header("Desc:")]
        [TextArea(2,2)]
        public string description;

        //References:
        List<SpriteRenderer> sprites;
        List<Material> materials;
        List<ParticleSystem> particles;

        float alpha;
        float targetAlpha;

        int fadeID;
        int positionID;
        float currentSpread;
        Vector2 spreadPosition;
        Vector2 currentSpreadPosition;

        Transform frame;
        GameObject creation;

        void Awake()
        {
            group = gameObject.GetComponent<SortingGroup>();
            defaultOrder = group.sortingOrder;

            frame = transform.Find("Frame");
            positionID = Shader.PropertyToID("_FadeSpreadPosition");
            fadeID = Shader.PropertyToID("_FadeSpreadFade");

            sprites = new List<SpriteRenderer>();
            materials = new List<Material>();
            foreach(SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
            {
                if(sr.name != "Frame")
                {
                    sprites.Add(sr);

                    Material mat = sr.material;
                    materials.Add(mat);
                    mat.SetFloat("_FadeSpreadFade", 32f);
                    mat.SetFloat("_FadeSpreadWidth", 1f);
                    mat.SetFloat("_FadeSpreadNoiseFactor", 1f);
                    mat.SetVector("_FadeSpreadNoiseScale", new Vector4(0.07f, 0.07f));

                }
            }

            if(fadeParticles)
            {
                particles = new List<ParticleSystem>();

                foreach(ParticleSystem ps in gameObject.GetComponentsInChildren<ParticleSystem>(true))
                {
                    particles.Add(ps);
                }
            }

            alpha = targetAlpha = 0;
            UpdateFadeProcess();

            if (leftClickCreate)
            {
                creation = transform.GetChild(0).gameObject;
                creation.SetActive(false);
            }
        }

        void Update()
        {
            //Fading:
            float target = GetTargetAlpha();
            if (target != alpha)
            {
                alpha = Mathf.Lerp(alpha, target, Time.deltaTime * 4f);

                if (Mathf.Abs(alpha - target) < 0.01f)
                {
                    alpha = target;
                }

                UpdateFadeProcess();
            }

            //Frame Movement:
            if(frame != null && (targetAlpha > 0.1f || frame.localScale.x < 10f))
            {
                if(DemoManager.c.IsFullscreen() == false && targetAlpha > 0.1f)
                {
                    Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    newPos.z = 0;
                    frame.position = newPos;
                    frame.localScale = Vector3.Lerp(frame.localScale, Vector3.one * 0.25f, Time.deltaTime * 6f);
                }
                else
                {
                    if (Mathf.Abs(frame.localScale.x - 12) > 0.1f)
                    {
                        frame.localScale = Vector3.Lerp(frame.localScale, Vector3.one * 3f, Time.deltaTime * 0.8f);
                    }
                }
            }

            if (leftClickCreate && alpha > 0.5f)
            {
                if(Input.GetMouseButtonDown(0))
                {
                    GameObject newCreation = Instantiate<GameObject>(creation);
                    Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    newPos.z = Random.value * 0.1f;
                    newCreation.transform.SetParent(transform);
                    newCreation.transform.position = newPos;
                    newCreation.SetActive(true);
                    Destroy(newCreation, 4f);
                }
            }

            if(canSpread && alpha > 0.5f)
            {
                if (Input.GetMouseButton(0) && DemoManager.c.IsFullscreen() && ((float) Input.mousePosition.x / (float) Screen.width < 0.8f || (float)Input.mousePosition.y / (float)Screen.height < 0.8f))
                {
                    float speed = 1.5f + currentSpread / 16f;

                    currentSpread = Mathf.Lerp(currentSpread, -4, Time.deltaTime * speed);
                    if (currentSpread < 0) currentSpread = 0;

                    spreadPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
                else
                {
                    currentSpread += Time.deltaTime * (4f + currentSpread);
                    if (currentSpread > 32) currentSpread = 32;
                }

                currentSpreadPosition = Vector2.Lerp(currentSpreadPosition, spreadPosition, Time.deltaTime * 10f);

                foreach (Material mat in materials)
                {
                    mat.SetFloat(fadeID, currentSpread);
                    mat.SetVector(positionID, currentSpreadPosition);
                }
            }
        }

        public void FadeIn()
        {
            group.sortingOrder = defaultOrder;
            targetAlpha = 1f;
        }
        public void FadeOut()
        {
            targetAlpha = 0f;
        }

        void UpdateFadeProcess()
        {
            if (fadeSprites)
            {
                if(frame != null && frame.gameObject.activeSelf != (alpha > 0))
                {
                    frame.gameObject.SetActive(alpha > 0);

                    if(alpha == 0 && canSpread)
                    {
                        currentSpread = 32;
                        currentSpreadPosition = spreadPosition = Vector2.zero;

                        foreach(Material mat in materials)
                        { 
                            mat.SetFloat(fadeID, currentSpread);
                            mat.SetVector(positionID, spreadPosition);
                        }
                    }
                }

                foreach (SpriteRenderer sr in sprites)
                {
                    Color color = sr.color;

                    color.a = Mathf.Clamp01(alpha);
                    sr.color = color;
                }
            }

            if(fadeParticles)
            {
                bool targetState = targetAlpha > 0.5f;

                foreach(ParticleSystem ps in particles)
                {
                    if(ps.gameObject.activeSelf != targetState)
                    {
                        ps.gameObject.SetActive(targetState);
                    }
                }
            }
        }

        float GetTargetAlpha()
        {
            return targetAlpha * DemoManager.currentFade;
        }

        public void SetLayerValue(int value)
        {
            switch(value)
            {
                case (0):
                    group.sortingOrder = -50;
                    break;
                case (1):
                    group.sortingOrder = leftClickCreate ? 510 : 500;
                    break;
                case (2):
                    group.sortingOrder = 30100;
                    break;
            }
        }
        public int GetLayerValue()
        {
            if(defaultOrder > 0)
            {
                if(defaultOrder < 900)
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
            else
            {
                return 0;
            }
        }
    }

}