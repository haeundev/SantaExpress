using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class HouseSpawner : MonoBehaviour
{
    [SerializeField] private List<string> housePaths;
    [SerializeField] private Vector3 destroyPoint;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            var handler = Addressables.InstantiateAsync(housePaths.PeekRandom(), transform);
            handler.Completed += op =>
            {
                var houseObj = op.Result;
                var linearMovement = houseObj.AddComponent<LinearMovement>();
                linearMovement.direction = Vector3.left;
                linearMovement.speed = 150f;
                var destroyBy = houseObj.AddComponent<DestroyByPosition>();
                destroyBy.coordinate = DestroyByPosition.Coordinate.X;
                destroyBy.direction = DestroyByPosition.Direction.Left;
                destroyBy.worldX = -51.7f;
            };

            var rnd = Random.Range(0.5f, 1.5f);
            yield return YieldCache.WaitForSeconds(rnd);
        }
    }
}
