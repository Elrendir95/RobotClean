using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    public abstract void OnCollect(GameObject collector);
}
