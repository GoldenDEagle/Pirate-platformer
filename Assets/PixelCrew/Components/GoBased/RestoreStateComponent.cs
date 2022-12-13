﻿using PixelCrew.Model;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Components.GoBased
{
    public class RestoreStateComponent : MonoBehaviour
    {
        [SerializeField] private string _id;
        public string Id => _id;

        private GameSession _session;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();

            var isDestroyed = _session.RestoreState(Id);
            if (isDestroyed)
                Destroy(gameObject);
        }
    }
}