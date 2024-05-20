using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawOparcity : MonoBehaviour
{
    ParticleSystem[] particles;
    
    public void SetOparcity(float _oparcity)
    {
        if(particles == null)
            particles = GetComponentsInChildren<ParticleSystem>();
        
        foreach (var particle in particles)
        {
            var main = particle.main;
            var color = main.startColor.color;
            color.a = _oparcity;
            main.startColor = color;
        }
    }
}