using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SantaExpress.Scripts
{
    public class EndlessLandGenerator : MonoBehaviour
    {
        // [SerializeField] private GameObject meshGenerator;
        [SerializeField] private Material material;
        [SerializeField] private int xSize = 1;
        [SerializeField] private int zSize = 20;
        [SerializeField] private float perlinMin = .5f;
        [SerializeField] private float perlinMax = 2f;

        private int _landNum = 0;

        private LandGenerator _lastCreated;
        private readonly Queue<LandGenerator> _lands = new Queue<LandGenerator>();
        public Bounds areaBounds;

        private void Start()
        {
            for (var i = 0; i < 7; i++)
                Instantiate();
        }

        private void Update()
        {
            foreach (var land in _lands)
            {
                Debug.Log($"{land.GetLastVerticePosition()}");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Destroy(_lands.Dequeue().gameObject);
            Instantiate();
        }
        
        private async void Instantiate()
        {
            var handle = Addressables.InstantiateAsync("Environment/Ground/GroundPiece.prefab");
            var go = await handle;
            go.name += _landNum++;
            var land = go.GetComponent<LandGenerator>();
            land.transform.parent = transform;
            land.SetPrev(_lastCreated);
            land.SetSize(xSize, zSize);
            land.SetPerlin(perlinMin, perlinMax);
            land.Create();
            
            if (_lands.Count > 0)
                land.transform.position = _lands.Last().GetLastVerticePosition();
            
            _lastCreated = land;
            _lands.Enqueue(land);
        }
    }
}