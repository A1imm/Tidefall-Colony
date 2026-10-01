using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance { get; private set; }

    [SerializeField] private TMP_Text objectiveText;
    [SerializeField] private BuildingPlot watchtowerPlot;
    [SerializeField] private BuildingPlot quarryPlot;
    [SerializeField] private BuildingPlot crossingRepairPlot;
    [SerializeField] private BuildingPlot safeFarmPlot;
    [SerializeField] private BuildingPlot lowlandFarmPlot;
    [SerializeField] private BuildingPlot lowlandFarmBPlot;
    [SerializeField] private BuildingPlot beaconPlot;
    [SerializeField] private BuildingPlot warehousePlot;
    [SerializeField] private GameObject scenarioCompletePanel;
    [SerializeField] private GameObject gameplayHud;

    private string currentObjectiveId;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        if (scenarioCompletePanel != null)
        {
            scenarioCompletePanel.SetActive(false);
        }

        if (gameplayHud != null)
        {
            gameplayHud.SetActive(true);
        }

        Instance = this;
    }

    private void Start()
    {
        SetObjective(
            "BuildLumberCamp",
            "Build a Lumber Camp"
        );
    }

    public void SetObjective(string objectiveId, string description)
    {
        currentObjectiveId = objectiveId;

        if (objectiveText != null)
        {
            objectiveText.text = $"Objective: {description}";
        }
    }

    public void CompleteObjective(string objectiveId)
    {
        if (objectiveId != currentObjectiveId)
            return;

        Debug.Log($"Objective completed: {objectiveId}");

        switch (objectiveId)
        {
            case "BuildLumberCamp":
                SetObjective(
                    "GatherWoodForWarehouse",
                    "Gather 20 Wood"
                );
                break;

            case "GatherWoodForWarehouse":
                if (warehousePlot != null)
                {
                    warehousePlot.Unlock();
                }

                SetObjective(
                    "BuildWarehouse",
                    "Build a Warehouse"
                );
                break;

            case "BuildWarehouse":
                SetObjective(
                    "GatherWood",
                    "Gather 40 Wood"
                );
                break;

            case "GatherWood":
                if (watchtowerPlot != null)
                {
                    watchtowerPlot.Unlock();
                }

                SetObjective(
                    "BuildWatchtower",
                    "Build a Flood Watchtower"
                );
                break;

            case "BuildWatchtower":
                if (quarryPlot != null)
                {
                    quarryPlot.Unlock();
                }

                SetObjective(
                    "BuildQuarry",
                    "Build a Quarry"
                );
                break;

            case "BuildQuarry":
                SetObjective(
                    "SurviveFlood",
                    "Survive the next flood"
                );
                break;

            case "SurviveFlood":
                SetObjective(
                    "GatherStone",
                    "Gather 40 Stone"
                );
                break;

            case "GatherStone":
                if (crossingRepairPlot != null)
                {
                    crossingRepairPlot.Unlock();
                }

                SetObjective(
                    "RepairCrossing",
                    "Repair the Broken Crossing"
                );
                break;

            case "RepairCrossing":
                if (safeFarmPlot != null)
                {
                    safeFarmPlot.Unlock();
                }

                if (lowlandFarmPlot != null)
                {
                    lowlandFarmPlot.Unlock();
                }

                if (lowlandFarmBPlot != null)
                {
                    lowlandFarmBPlot.Unlock();
                }

                SetObjective(
                    "BuildFarm",
                    "Establish Food Production"
                );
                break;

            case "BuildFarm":
                SetObjective(
                    "SurviveSecondFlood",
                    "Survive the next flood"
                );
                break;

            case "SurviveSecondFlood":
                SetObjective(
                    "GatherFood",
                    "Gather 30 Food"
                );
                break;

            case "GatherFood":
                if (beaconPlot != null)
                {
                    beaconPlot.Unlock();
                }

                SetObjective(
                    "ActivateBeacon",
                    "Activate the Ancient Beacon"
                );
                break;

            case "ActivateBeacon":
                currentObjectiveId = "";

                if (gameplayHud != null)
                {
                    gameplayHud.SetActive(false);
                }

                if (scenarioCompletePanel != null)
                {
                    scenarioCompletePanel.SetActive(true);
                }

                Debug.Log("Scenario completed.");
                break;

        }
    }
}