using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField]
    private string entityId;

    public string EntityID => entityId;

    public virtual void Initialize()
    {
    }
}