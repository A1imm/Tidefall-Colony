using UnityEngine;

public class BuildingPlot : MonoBehaviour
{
    [Header("Plot")]
    [SerializeField] private string plotId;
    [SerializeField] private string displayName;
    [SerializeField] private string completionObjectiveId;
    [SerializeField] private bool startsUnlocked = true;
    private bool isUnlocked;

    [Header("Building Cost")]
    [SerializeField] private int woodCost;
    [SerializeField] private int stoneCost;
    [SerializeField] private int foodCost;

    [Header("Building")]
    [SerializeField] private GameObject buildingVisual;

    [Header("Visuals")]
    [SerializeField] private Renderer plotRenderer;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material hoverMaterial;
    [SerializeField] private Material lockedMaterial;
    [SerializeField] private Material damagedMaterial;

    [Header("Flood Damage")]
    [SerializeField] private bool canBeDamagedByFlood;
    [SerializeField] private ResourceProducer resourceProducer;
    [SerializeField] private GameObject damagedIndicator;

    private Collider plotCollider;
    private bool isBuilt;
    private bool isDamaged;
    private bool interactionEnabled = true;

    public string PlotId => plotId;

    public bool IsBuilt => isBuilt;
    public bool IsUnlocked => isUnlocked;
    public bool IsDamaged => isDamaged;

    public int WoodCost => woodCost;
    public int StoneCost => stoneCost;
    public int FoodCost => foodCost;

    public string DisplayName =>
    string.IsNullOrWhiteSpace(displayName)
        ? plotId
        : displayName;

    public int RepairWoodCost =>
        Mathf.CeilToInt(woodCost * 0.5f);

    public int RepairStoneCost =>
        Mathf.CeilToInt(stoneCost * 0.5f);

    public int RepairFoodCost =>
        Mathf.CeilToInt(foodCost * 0.5f);

    private void Awake()
    {
        plotCollider = GetComponent<Collider>();
        isUnlocked = startsUnlocked;

        if (buildingVisual != null)
        {
            buildingVisual.SetActive(false);
        }

        UpdateVisualState();
    }

    private void Reset()
    {
        plotRenderer = GetComponent<Renderer>();
    }

    public void SetHovered(bool isHovered)
    {
        if (plotRenderer == null || !interactionEnabled)
            return;

        if (isDamaged)
        {
            plotRenderer.sharedMaterial =
                isHovered ? hoverMaterial : damagedMaterial;

            return;
        }

        if (isBuilt)
            return;

        if (!isUnlocked)
        {
            plotRenderer.sharedMaterial = lockedMaterial;
            return;
        }

        plotRenderer.sharedMaterial =
            isHovered ? hoverMaterial : defaultMaterial;
    }

    public void Select()
    {
        if (!interactionEnabled)
            return;

        if (isDamaged)
        {
            TryRepair();
            return;
        }

        if (isBuilt)
            return;

        if (!isUnlocked)
        {
            Debug.Log($"{plotId} is not unlocked yet.");
            return;
        }

        if (ResourceManager.Instance == null)
        {
            Debug.LogError("ResourceManager not found.");
            return;
        }

        if (!ResourceManager.Instance.SpendResources(
                woodCost,
                stoneCost,
                foodCost))
        {
            Debug.Log(
                $"Not enough resources to build {plotId}."
            );

            return;
        }

        Build();
    }

    private void Build()
    {
        isBuilt = true;
        isDamaged = false;

        if (plotRenderer != null)
        {
            plotRenderer.enabled = false;
        }

        if (plotCollider != null)
        {
            plotCollider.enabled = false;
        }

        if (buildingVisual != null)
        {
            buildingVisual.transform.position = transform.position;
            buildingVisual.SetActive(true);
        }

        if (ObjectiveManager.Instance != null &&
            !string.IsNullOrEmpty(completionObjectiveId))
        {
            ObjectiveManager.Instance.CompleteObjective(
                completionObjectiveId
            );
        }

        if (damagedIndicator != null)
        {
            damagedIndicator.SetActive(false);
        }

        Debug.Log($"Built: {plotId}");
    }

    public void Unlock()
    {
        if (isBuilt)
            return;

        isUnlocked = true;
        UpdateVisualState();

        Debug.Log($"Unlocked building plot: {plotId}");
    }

    private void UpdateVisualState()
    {
        if (plotRenderer == null)
            return;

        if (isBuilt && !isDamaged)
        {
            plotRenderer.enabled = false;

            if (plotCollider != null)
            {
                plotCollider.enabled = false;
            }

            return;
        }

        plotRenderer.enabled = true;

        if (plotCollider != null)
        {
            plotCollider.enabled = interactionEnabled;
        }

        if (!interactionEnabled)
        {
            plotRenderer.sharedMaterial = lockedMaterial;
            return;
        }

        if (isDamaged)
        {
            plotRenderer.sharedMaterial = damagedMaterial;
            return;
        }

        plotRenderer.sharedMaterial =
            isUnlocked ? defaultMaterial : lockedMaterial;
    }

    private void TryRepair()
    {
        if (!isDamaged)
            return;

        if (ResourceManager.Instance == null)
        {
            Debug.LogError("ResourceManager not found.");
            return;
        }

        int repairWoodCost = Mathf.CeilToInt(woodCost * 0.5f);
        int repairStoneCost = Mathf.CeilToInt(stoneCost * 0.5f);
        int repairFoodCost = Mathf.CeilToInt(foodCost * 0.5f);

        if (!ResourceManager.Instance.SpendResources(
                repairWoodCost,
                repairStoneCost,
                repairFoodCost))
        {
            Debug.Log($"Not enough resources to repair {plotId}.");
            return;
        }

        isDamaged = false;

        if (damagedIndicator != null)
        {
            damagedIndicator.SetActive(false);
        }

        if (resourceProducer != null)
        {
            resourceProducer.SetProductionEnabled(true);
        }

        UpdateVisualState();

        Debug.Log(
            $"Repaired {plotId}: " +
            $"{repairWoodCost} Wood, " +
            $"{repairStoneCost} Stone."
        );
    }

    public void SetFlooded(bool flooded)
    {
        if (!canBeDamagedByFlood)
            return;

        interactionEnabled = !flooded;

        if (flooded && isBuilt && !isDamaged)
        {
            isDamaged = true;

            if (resourceProducer != null)
            {
                resourceProducer.SetProductionEnabled(false);
            }

            if (damagedIndicator != null)
            {
                damagedIndicator.SetActive(true);
            }

            Debug.Log($"{plotId} was damaged by the flood.");
        }

        UpdateVisualState();
    }

    public Vector3 GetTooltipAnchorPosition()
    {
        Collider col = GetComponent<Collider>();

        if (col == null)
            col = GetComponentInChildren<Collider>();

        if (col != null)
        {
            return col.bounds.center + new Vector3(0f, col.bounds.extents.y + 0.15f, 0f);
        }

        return transform.position + Vector3.up * 0.5f;
    }
}