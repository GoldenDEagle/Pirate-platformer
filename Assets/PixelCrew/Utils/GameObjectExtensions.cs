using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Utils
{
    public static class GameObjectExtensions
    {
        public static bool IsInLayer(this GameObject go, LayerMask layer)
        {
            return layer == (layer | 1 << go.layer);
        }

        public static Transform FindChildTransform(this GameObject parent, string name)
        {
            Transform child = null;

            foreach (Transform trans in parent.transform)
            {
                if (trans.name == name)
                {
                    child = trans;
                    if (child != null)
                        return child;
                }
                else
                {
                    child = FindChildTransform(trans.gameObject, name);
                    if (child != null)
                        return child;
                }
            }

            return child;
        }

    }
}
