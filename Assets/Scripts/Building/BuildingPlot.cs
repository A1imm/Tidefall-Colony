using UnityEngine;

public class BuildingPlot : MonoBehaviour
{
    [Header("Plot")]
    [SerializeField] private string plotId;
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

    private Collider plotCollider;
    private bool isBuilt;

    private void Awake()
    {
        plotCollider = GetComponent<Collider>();
        isUnlocked = startsUnlocked;

        if (buildingVisual != null)
        {
            buildingVisual.SetActive(false);
        }
    }

    private void Reset()
    {
        plotRenderer = GetComponent<Renderer>();
    }

    public void SetHovered(bool isHovered)
    {
        if (plotRenderer == null || isBuilt)
            return;

        plotRenderer.sharedMaterial =
            isHovered ? hoverMaterial : defaultMaterial;
    }

    public void Select()
    {
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

        Debug.Log($"Built: {plotId}");
    }

    public void Unlock()
    {
        isUnlocked = true;
    }
}