using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Dropper : MonoBehaviour
{
    [SerializeField] private List<string> gifts;
    [SerializeField] private Transform dropPoint;

    public void DropGift()
    {
        var handler = Addressables.InstantiateAsync(gifts.PeekRandom());
        handler.Completed += op =>
        {
            var giftObj = op.Result;
            giftObj.transform.position = dropPoint.position;
            
            var destroyBy = giftObj.AddComponent<DestroyByPosition>();
            destroyBy.coordinate = DestroyByPosition.Coordinate.Y;
            destroyBy.direction = DestroyByPosition.Direction.Down;
            destroyBy.worldY = -10f;
            
            // var linearMovement = giftObj.AddComponent<LinearMovement>();
            // linearMovement.direction = Vector3.left;
            // linearMovement.speed = 150f;
            // var destroyByX = giftObj.AddComponent<DestroyByX>();
        };
    }
    
}
