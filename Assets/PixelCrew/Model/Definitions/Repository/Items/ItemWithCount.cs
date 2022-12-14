using System;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Repository.Items
{
        [Serializable]
        public struct ItemWithCount
        {
            [InventoryId] [SerializeField] private string _itemId;
            [SerializeField] private int _count;

            public string ItemId => _itemId;
            public int Count => _count;

            public ItemWithCount(string id, int count)
            {
                _itemId = id;
                _count = count;
            }

        }
}