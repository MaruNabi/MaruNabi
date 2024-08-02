using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningSign : MonoBehaviour
{
    [SerializeField] private GameObject earthEnergyBeam;

    private void OnEnable()
    {
        StartCoroutine(SpawnEarthEnergyBeam());
    }

    private IEnumerator SpawnEarthEnergyBeam()
    {
        yield return new WaitForSeconds(0.75f);
        earthEnergyBeam.SetActive(true);
        Managers.Sound.PlaySFX("Mask_Earth");
        gameObject.SetActive(false);
    }
}
