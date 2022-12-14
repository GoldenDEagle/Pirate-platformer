using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Repository.Items;
using PixelCrew.UI.Widgets;
using System.Collections.Generic;
using UnityEngine;

public class WalletController : MonoBehaviour
{
    [SerializeField] private ItemWidget _prefab;
    [SerializeField] private Transform _container;

    private DataGroup<ItemWithCount, ItemWidget> _dataGroup;
    private GameSession _session;

    private List<ItemWithCount> _createdItems = new List<ItemWithCount>();

    private void Start()
    {
        _session = FindObjectOfType<GameSession>();
        _dataGroup = new DataGroup<ItemWithCount, ItemWidget>(_prefab, _container);
        _session.Data.Inventory.OnChanged += OnInventoryChanged;

        OnInventoryChanged("", 0);
    }

    private void OnInventoryChanged(string id, int value)
    {
        _createdItems.Clear();
        var items = DefsFacade.I.Items.All;
        foreach (var item in items)
        {
            if (item.HasTag(ItemTag.Currency))
                _createdItems.Add(new ItemWithCount(item.Id, _session.Data.Inventory.Count(item.Id)));
        }
        _dataGroup.SetData(_createdItems);
    }

    private void OnDestroy()
    {
        _session.Data.Inventory.OnChanged -= OnInventoryChanged;
    }
}
