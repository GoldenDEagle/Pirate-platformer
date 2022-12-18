﻿using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace PixelCrew.Creatures.Bosses
{
    public class ChangeLightsComponent : MonoBehaviour
    {
        [SerializeField] private Light2D[] _lights;
        [ColorUsage(true, true)]
        [SerializeField] private Color _color;

        public void SetColor()
        {
            foreach (var light2D in _lights)
            {
                light2D.color = _color;
            }
        }
    }
}