using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingPlotInteractor : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private LayerMask buildingPlotLayer;

    private BuildingPlot hoveredPlot;

    private void Update()
    {
        UpdateHover();

        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame &&
            hoveredPlot != null)
        {
            hoveredPlot.Select();
        }
    }

    private void UpdateHover()
    {
        if (Mouse.current == null || targetCamera == null)
            return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Ray ray = targetCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 500.0f, buildingPlotLayer))
        {
            BuildingPlot plot = hit.collider.GetComponent<BuildingPlot>();

            if (plot != hoveredPlot)
            {
                ClearCurrentHover();

                hoveredPlot = plot;

                if (hoveredPlot != null)
                    hoveredPlot.SetHovered(true);
            }

            return;
        }

        ClearCurrentHover();
    }

    private void ClearCurrentHover()
    {
        if (hoveredPlot != null)
        {
            hoveredPlot.SetHovered(false);
            hoveredPlot = null;
        }
    }
}