using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Utils.ObjectPool
{
    public class Pool : MonoBehaviour
    {
        private readonly Dictionary<int, Queue<PoolItem>> _items = new Dictionary<int, Queue<PoolItem>>();

        public void Get(GameObject go, Vector3 Position)
        {

        }

        public void Release(int id, PoolItem poolItem)    
        {

        }
    }
}