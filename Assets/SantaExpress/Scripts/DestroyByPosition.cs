using System;
using System.Collections;
using UniRx;
using UnityEngine;

public class DestroyByPosition : MonoBehaviour
{
    public enum Coordinate { X, Y, Z }
    public enum Direction { Left, Right, Up, Down }
    
    public Coordinate coordinate;
    public Direction direction;
    public float worldX;
    public float worldY;
    public float worldZ;
    
    private void Start()
    {
        Observable.FromCoroutine(WaitDestroy).Subscribe().AddTo(this);
    }

    private IEnumerator WaitDestroy()
    {
        while (true)
        {
            switch (coordinate)
            {
                case Coordinate.X:
                    switch (direction)
                    {
                        case Direction.Left:
                            yield return YieldCache.WaitUntil(() => transform.position.x < worldX);
                            break;
                        case Direction.Right:
                            yield return YieldCache.WaitUntil(() => transform.position.x > worldX);
                            break;
                    }
                    break;
                case Coordinate.Y:
                    switch (direction)
                    {
                        case Direction.Up:
                            yield return YieldCache.WaitUntil(() => transform.position.y > worldY);
                            break;
                        case Direction.Down:
                            yield return YieldCache.WaitUntil(() => transform.position.y < worldY);
                            break;
                    }
                    break;
            }
            Destroy(gameObject);
            yield break;
        }
    }
}