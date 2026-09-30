using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance { get; private set; }

    [SerializeField] private TMP_Text objectiveText;
    [SerializeField] private BuildingPlot watchtowerPlot;
    [SerializeField] private BuildingPlot quarryPlot;
    [SerializeField] private BuildingPlot crossingRepairPlot;

    private string currentObjectiveId;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
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
                SetObjective(
                    "SurviveFlood",
                    "Survive the first flood"
                );
                break;

            case "SurviveFlood":
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
                objectiveText.text = "Lowland unlocked";
                break;

        }
    }
}