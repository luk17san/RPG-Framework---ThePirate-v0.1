using UnityEngine;

public class ShipSail : MonoBehaviour
{
    [Header("Sail Settings")]

    [SerializeField]
    private bool available = true;

    [SerializeField]
    private bool deployed;

    [Header("Visual Settings")]

    [SerializeField]
    private GameObject sailVisual;

    [SerializeField]
    private Vector3 foldedScale = new Vector3(1f, 0.05f, 1f);

    [SerializeField]
    private Vector3 deployedScale = Vector3.one;

    public bool IsAvailable => available;
    public bool IsDeployed => deployed;

    private void Awake()
    {
        ApplyVisualState();
    }

    public void SetAvailable(bool value)
    {
        available = value;

        if (!available)
            SetDeployed(false);
    }

    public void SetDeployed(bool value)
    {
        if (!available && value)
            return;

        deployed = value;
        ApplyVisualState();
    }

    private void ApplyVisualState()
    {
        if (sailVisual == null)
            sailVisual = gameObject;

        sailVisual.SetActive(available);

        if (!available)
            return;

        sailVisual.transform.localScale =
            deployed ? deployedScale : foldedScale;
    }
}