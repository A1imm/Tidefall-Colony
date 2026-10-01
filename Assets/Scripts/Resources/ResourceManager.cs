using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    [Header("Starting Resources")]
    [SerializeField] private int wood = 50;
    [SerializeField] private int stone = 0;
    [SerializeField] private int food = 0;

    public int Wood => wood;
    public int Stone => stone;
    public int Food => food;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public bool CanAfford(int woodCost, int stoneCost, int foodCost)
    {
        return wood >= woodCost &&
               stone >= stoneCost &&
               food >= foodCost;
    }

    public bool SpendResources(int woodCost, int stoneCost, int foodCost)
    {
        if (!CanAfford(woodCost, stoneCost, foodCost))
        {
            return false;
        }

        wood -= woodCost;
        stone -= stoneCost;
        food -= foodCost;

        Debug.Log(
            $"Resources spent: Wood -{woodCost}, Stone -{stoneCost}, Food -{foodCost}"
        );

        Debug.Log(
            $"Remaining: Wood {wood}, Stone {stone}, Food {food}"
        );

        return true;
    }

    public void AddWood(int amount)
    {
        wood += amount;
        CheckResourceObjectives();
    }

    public void AddStone(int amount)
    {
        stone += amount;
        CheckResourceObjectives();
    }

    public void AddFood(int amount)
    {
        food += amount;
        CheckResourceObjectives();
    }

    private void CheckResourceObjectives()
    {
        if (ObjectiveManager.Instance == null)
            return;

        if (Wood >= 40)
        {
            ObjectiveManager.Instance.CompleteObjective("GatherWood");
        }

        if (Stone >= 40)
        {
            ObjectiveManager.Instance.CompleteObjective("GatherStone");
        }

        if (Food >= 30)
        {
            ObjectiveManager.Instance.CompleteObjective("GatherFood");
        }
    }
}