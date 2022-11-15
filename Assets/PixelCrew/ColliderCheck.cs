﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew
{
    public class ColliderCheck : LayerCheck
    {
        private Collider2D _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            _isTouchingLayer = _collider.IsTouchingLayers(_layer);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            _isTouchingLayer = _collider.IsTouchingLayers(_layer);
        }

    }
}
