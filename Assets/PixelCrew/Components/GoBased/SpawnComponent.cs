using UnityEngine;
using PixelCrew.Utils;
using System.Collections;

namespace PixelCrew.Components
{

    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _prefab;
        [Space][Header("MultipleSpawn")]
        [SerializeField] public int _numberToSpawn = 1;
        [SerializeField] private float _xScatter = 0;
        [SerializeField] private float _interval = 0.1f;

        private Coroutine _coroutine;

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            SpawnInstance();
        }

        [ContextMenu("MultipleSpawn")]
        public void MultipleSpawn()
        {
            if (_coroutine != null) return;
            _coroutine = StartCoroutine(SpawnRoutine(_numberToSpawn, _interval));
        }

        public GameObject SpawnInstance()
        {
            var xPosition = Random.Range(_target.position.x - _xScatter, _target.position.x + _xScatter);
            Vector3 position = new Vector3(xPosition, _target.position.y, _target.position.z);

            var instance = SpawnUtils.Spawn(_prefab, position);

            var scale = _target.lossyScale;
            instance.transform.localScale = scale;

            instance.SetActive(true);
            return instance;
        }

        private IEnumerator SpawnRoutine(int count, float interval)
        {
            for (int i = 0; i < count; i++)
            {
                SpawnInstance();
                yield return new WaitForSeconds(interval);
            }
        }

        public void SetPrefab(GameObject prefab)
        {
            _prefab = prefab;
        }

        public void SetSpawnPosition(Transform transform)
        {
            _target = transform;
        }

        private void OnDestroy()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }
}
