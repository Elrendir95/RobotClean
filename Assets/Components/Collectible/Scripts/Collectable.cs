using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    public virtual void OnCollect(GameObject collector)
    {
        Destroy(gameObject);
    }
}
