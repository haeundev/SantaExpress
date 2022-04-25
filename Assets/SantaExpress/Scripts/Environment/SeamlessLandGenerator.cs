using System.Collections.Generic;
using UnityEngine;

namespace SantaExpress.Scripts
{
    public class SeamlessLandGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject meshGenerator;
        [SerializeField] private Material material;
        [SerializeField] private int xSize = 1;
        [SerializeField] private int zSize = 20;
        [SerializeField] private float perlinMin = .5f;
        [SerializeField] private float perlinMax = 2f;

        private LandGenerator _lastCreated;

        private void Start()
        {
            for (var i = 0; i < 5; i++)
            {
                var newPiece = Instantiate(meshGenerator, transform).GetComponent<LandGenerator>();
                //var pos = newPiece.transform.position;
                //newPiece.transform.position = new Vector3(pos.x * i, pos.y, pos.z);
                newPiece.SetPrev(_lastCreated);
                newPiece.SetSize(xSize, zSize);
                newPiece.SetPerlin(perlinMin, perlinMax);
                _lastCreated = newPiece;
            }
        }
    }
}