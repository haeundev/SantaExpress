using System.Collections;
using UniRx;
using UnityEngine;

public class DestroyByX : MonoBehaviour
{
    [SerializeField] private float worldX = -51.7f;
    
    private void Start()
    {
        Observable.FromCoroutine(WaitDestroy).Subscribe().AddTo(this);
    }

    private IEnumerator WaitDestroy()
    {
        while (true)
        {
            yield return YieldCache.WaitUntil(() => transform.position.x < worldX);
            Destroy(gameObject);
            yield break;
        }
    }
}