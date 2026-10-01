using System;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }
    public event Action OnResourcesChanged;

    [Header("Starting Resources")]
    [SerializeField] private int wood = 20;
    [SerializeField] private int stone = 0;
    [SerializeField] private int food = 0;

    [Header("Storage")]
    [SerializeField] private int resourceCapacity = 20;

    public int Wood => wood;
    public int Stone => stone;
    public int Food => food;

    public int ResourceCapacity => resourceCapacity;

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

        NotifyResourcesChanged();

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
        wood = Mathf.Min(wood + amount, resourceCapacity);
        NotifyResourcesChanged();
        CheckResourceObjectives();
    }

    public void AddStone(int amount)
    {
        stone = Mathf.Min(stone + amount, resourceCapacity);
        NotifyResourcesChanged();
        CheckResourceObjectives();
    }

    public void AddFood(int amount)
    {
        food = Mathf.Min(food + amount, resourceCapacity);
        NotifyResourcesChanged();
        CheckResourceObjectives();
    }

    private void NotifyResourcesChanged()
    {
        OnResourcesChanged?.Invoke();
    }
    private void CheckResourceObjectives()
    {
        if (ObjectiveManager.Instance == null)
            return;

        if (Wood >= 20)
        {
            ObjectiveManager.Instance.CompleteObjective("GatherWoodForWarehouse");
        }
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

    public void UpgradeStorageCapacity(int newCapacity)
    {
        if (newCapacity <= resourceCapacity)
            return;

        resourceCapacity = newCapacity;

        NotifyResourcesChanged();

        Debug.Log($"Resource capacity upgraded to {resourceCapacity}.");
    }
}