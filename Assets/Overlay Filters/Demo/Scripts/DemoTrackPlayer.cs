using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverlayFilters.Demo
{
    public class DemoTrackPlayer : MonoBehaviour
    {
        public string propertyName;

        Material[] materials;

        int propertyID;
        bool fixedUpdated;

        void Start()
        {
            propertyID = Shader.PropertyToID(propertyName);

            SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>(true);
            materials = new Material[sprites.Length];

            for (int i = 0; i < sprites.Length; i++)
            {
                materials[i] = sprites[i].material;
            }
        }


        void FixedUpdate()
        {
            fixedUpdated = true;
        }
        void LateUpdate()
        {
            if (fixedUpdated)
            {
                fixedUpdated = false;

                foreach(Material mat in materials)
                {
                    mat.SetVector(propertyID, DemoPlayer.c.transform.position + Vector3.up * 0.6f);
                }
            }
        }
    }

}