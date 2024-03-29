﻿using UnityEngine;
using UnityEngine.Events;
using PixelCrew.Model;
using PixelCrew.Model.Data;

namespace PixelCrew.Components.Interactions
{
    public class RequireItemComponent : MonoBehaviour
    {
        [SerializeField] private InventoryItemData[] _required;
        [SerializeField] private bool _removeAfterUse;

        [SerializeField] private UnityEvent _onSuccess;
        [SerializeField] private UnityEvent _onFail;

        public void Check()
        {
            var areAlRequirementsMet = true;
            foreach (var item in _required)
            {
                var numItems = GameSession.Instance.Data.Inventory.Count(item.Id);
                if (numItems < item.Value)
                    areAlRequirementsMet = false;
            }

            if (areAlRequirementsMet)
            {
                if (_removeAfterUse)
                {
                    foreach (var item in _required)
                    {
                        GameSession.Instance.Data.Inventory.Remove(item.Id, item.Value);
                    }
                }

                _onSuccess?.Invoke();
            }
            else
            {
                _onFail?.Invoke();
            }
        }
    }
}