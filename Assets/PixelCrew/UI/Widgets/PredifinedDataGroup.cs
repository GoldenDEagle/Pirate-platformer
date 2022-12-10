using System;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.UI.Widgets
{
    public class PredifinedDataGroup<TDataType, TItemType> : DataGroup<TDataType, TItemType>
        where TItemType : MonoBehaviour, IItemRenderer<TDataType>
    {
        public PredifinedDataGroup(Transform container) : base(null, container)
        {
            var items = container.GetComponentsInChildren<TItemType>();
            CreatedItems.AddRange(items);
        }

        public override void SetData(IList<TDataType> data)
        {
            if (data.Count > CreatedItems.Count)
                throw new IndexOutOfRangeException();

            base.SetData(data);
        }
    }
}