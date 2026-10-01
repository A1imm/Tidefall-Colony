using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BuildingPlotInteractor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera targetCamera;
    [SerializeField]
    private BuildingPlotTooltipUI tooltipUI;

    [Header("Raycast")]
    [SerializeField]
    private LayerMask buildingPlotLayer;

    [SerializeField]
    private float maxRayDistance = 500f;

    private BuildingPlot currentHoveredPlot;

    private void Awake()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    private void Update()
    {
        if (Mouse.current == null ||
            targetCamera == null)
        {
            return;
        }

        if (EventSystem.current != null &&
            EventSystem.current.IsPointerOverGameObject())
        {
            ClearHoveredPlot();
            return;
        }

        Vector2 mousePosition =
            Mouse.current.position.ReadValue();

        Ray ray =
            targetCamera.ScreenPointToRay(
                mousePosition
            );

        if (Physics.Raycast(
                ray,
                out RaycastHit hit,
                maxRayDistance,
                buildingPlotLayer,
                QueryTriggerInteraction.Ignore))
        {
            BuildingPlot plot =
                hit.collider
                    .GetComponentInParent<BuildingPlot>();

            if (plot != null)
            {
                SetHoveredPlot(plot);

                if (tooltipUI != null)
                {
                    tooltipUI.Show(plot, targetCamera);
                }

                if (Mouse.current
                    .leftButton
                    .wasPressedThisFrame)
                {
                    plot.Select();
                }

                return;
            }
        }

        ClearHoveredPlot();
    }

    private void SetHoveredPlot(
        BuildingPlot plot)
    {
        if (currentHoveredPlot == plot)
            return;

        if (currentHoveredPlot != null)
        {
            currentHoveredPlot.SetHovered(false);
        }

        currentHoveredPlot = plot;

        currentHoveredPlot.SetHovered(true);
    }

    private void ClearHoveredPlot()
    {
        if (currentHoveredPlot != null)
        {
            currentHoveredPlot.SetHovered(false);
            currentHoveredPlot = null;
        }

        if (tooltipUI != null)
        {
            tooltipUI.Hide();
        }
    }

    private void OnDisable()
    {
        ClearHoveredPlot();
    }
}