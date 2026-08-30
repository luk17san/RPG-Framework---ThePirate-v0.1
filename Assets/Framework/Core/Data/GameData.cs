using UnityEngine;

public abstract class GameData : ScriptableObject
{
    [SerializeField]
    private string id;

    [SerializeField]
    private string displayName;

    public string ID => id;

    public string DisplayName => displayName;
}