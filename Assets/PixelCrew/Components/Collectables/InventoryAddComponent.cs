﻿using UnityEngine;
using PixelCrew.Creatures;
using PixelCrew.Model.Definitions;

namespace PixelCrew.Components.Collectables
{
    public class InventoryAddComponent : MonoBehaviour
    {
        [InventoryId] [SerializeField] private string _id;
        [SerializeField] private int _count;

        public void Add(GameObject go)
        {
            var hero = go.GetComponent<Hero>();
            if (hero != null)
                hero.AddToInventory(_id,_count);
        }
    }
}