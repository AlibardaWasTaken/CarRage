
using UnityEngine;
using UnityEngine.Pool;

public interface IPoolLinked
{
    public ObjectPool<GameObject> LinkedPool { get;  set; }
    public void LinkPool(ObjectPool<GameObject> ToLink)
    {
        LinkedPool = ToLink;
    }

    GameObject ThisGameObject();

    internal void ReturnPool()
    {
        if (LinkedPool != null)
        {
            LinkedPool.Release(ThisGameObject());
            return;
        }
        Object.Destroy(ThisGameObject());

    }


}
