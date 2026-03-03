using UnityEngine;

[RequireComponent(typeof(Collectable))]
[RequireComponent(typeof(BoxCollider))]
public class CollectableTrigger :  MonoBehaviour
{
#if UNITY_EDITOR
    private void Reset()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }
#endif

    private Collectable _collectable;

    void Awake()
    {
        _collectable = GetComponent<Collectable>();
    }
    void OnTriggerEnter(Collider collider)
    {
        // TODO Check if it can collect
        _collectable.OnCollect(collider.gameObject);
    }
}
