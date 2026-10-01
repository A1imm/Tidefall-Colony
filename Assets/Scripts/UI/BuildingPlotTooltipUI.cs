using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildingPlotTooltipUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private TMP_Text statusText;

    [Header("Position")]
    [SerializeField]
    private Vector2 screenOffset =
    new Vector2(20f, -20f);

    [SerializeField] private bool followPlot = true;

    private RectTransform rectTransform;
    private RectTransform parentRect;

    private BuildingPlot currentPlot;
    private Camera targetCamera;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentRect = rectTransform.parent as RectTransform;

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!gameObject.activeSelf)
            return;

        if (!followPlot)
            return;

        if (currentPlot == null || targetCamera == null)
            return;

        UpdatePositionFromPlot();
    }

    public void Show(BuildingPlot plot, Camera cameraToUse)
    {
        if (plot == null || cameraToUse == null)
        {
            Hide();
            return;
        }

        currentPlot = plot;
        targetCamera = cameraToUse;

        gameObject.SetActive(true);

        titleText.text = plot.DisplayName.ToUpper();

        if (plot.IsDamaged)
        {
            costText.text =
                "Repair: " +
                FormatCost(
                    plot.RepairWoodCost,
                    plot.RepairStoneCost,
                    plot.RepairFoodCost
                );

            statusText.text = "DAMAGED - REPAIR REQUIRED";
            statusText.color = new Color32(255, 166, 77, 255);
        }
        else
        {
            costText.text =
                "Cost: " +
                FormatCost(
                    plot.WoodCost,
                    plot.StoneCost,
                    plot.FoodCost
                );

            if (!plot.IsUnlocked)
            {
                statusText.text = "LOCKED";
                statusText.color = new Color32(160, 170, 178, 255);
            }
            else if (!CanAfford(plot))
            {
                statusText.text = "NOT ENOUGH RESOURCES";
                statusText.color = new Color32(235, 90, 90, 255);
            }
            else
            {
                statusText.text = "AVAILABLE";
                statusText.color = new Color32(100, 220, 140, 255);
            }
        }

        UpdatePositionFromPlot();
    }

    public void Hide()
    {
        currentPlot = null;
        targetCamera = null;
        gameObject.SetActive(false);
    }

    private bool CanAfford(BuildingPlot plot)
    {
        if (ResourceManager.Instance == null)
            return false;

        if (plot.IsDamaged)
        {
            return ResourceManager.Instance.CanAfford(
                plot.RepairWoodCost,
                plot.RepairStoneCost,
                plot.RepairFoodCost
            );
        }

        return ResourceManager.Instance.CanAfford(
            plot.WoodCost,
            plot.StoneCost,
            plot.FoodCost
        );
    }

    private string FormatCost(
        int wood,
        int stone,
        int food)
    {
        List<string> parts = new List<string>();

        if (wood > 0)
            parts.Add($"{wood} Wood");

        if (stone > 0)
            parts.Add($"{stone} Stone");

        if (food > 0)
            parts.Add($"{food} Food");

        if (parts.Count == 0)
            return "Free";

        return string.Join(" | ", parts);
    }

    private void UpdatePositionFromPlot()
    {
        if (currentPlot == null || targetCamera == null || parentRect == null)
            return;

        Vector3 worldPos = currentPlot.GetTooltipAnchorPosition();
        Vector3 screenPos = targetCamera.WorldToScreenPoint(worldPos);

        if (screenPos.z < 0f)
        {
            Hide();
            return;
        }

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRect,
                screenPos,
                null,
                out Vector2 localPoint))
        {
            return;
        }

        localPoint += screenOffset;

        float tooltipWidth = rectTransform.rect.width;
        float tooltipHeight = rectTransform.rect.height;

        float minX = parentRect.rect.xMin;
        float maxX = parentRect.rect.xMax - tooltipWidth;

        float minY = parentRect.rect.yMin + tooltipHeight;
        float maxY = parentRect.rect.yMax;

        localPoint.x = Mathf.Clamp(localPoint.x, minX, maxX);
        localPoint.y = Mathf.Clamp(localPoint.y, minY, maxY);

        rectTransform.anchoredPosition = localPoint;
    }
}