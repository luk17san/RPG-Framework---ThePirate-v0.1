using UnityEngine;

[CreateAssetMenu(
    fileName = "ShipData",
    menuName = "Game/Ships/Ship Data"
)]
public class ShipData : ScriptableObject
{
    [Header("Identity")]
    public string shipId;
    public string displayName;

    [Header("Movement")]
    public float maxSpeed = 10f;
    public float acceleration = 2f;
    public float turnSpeed = 30f;

    [Header("Combat")]
    public float maxHealth = 100f;

    [Header("Economy")]
    public int purchasePrice = 1000;
}