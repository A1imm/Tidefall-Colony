using System.Collections;
using UnityEngine;

public class ResourceProducer : MonoBehaviour
{
    public enum ResourceType
    {
        Wood,
        Stone,
        Food
    }

    [Header("Production")]
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private int amountPerTick = 5;
    [SerializeField] private float productionInterval = 3.0f;

    private Coroutine productionCoroutine;

    private void OnEnable()
    {
        productionCoroutine = StartCoroutine(ProductionLoop());
    }

    private void OnDisable()
    {
        if (productionCoroutine != null)
        {
            StopCoroutine(productionCoroutine);
            productionCoroutine = null;
        }
    }

    private IEnumerator ProductionLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(productionInterval);

            ProduceResource();
        }
    }

    private void ProduceResource()
    {
        if (ResourceManager.Instance == null)
        {
            Debug.LogError("ResourceManager not found.");
            return;
        }

        switch (resourceType)
        {
            case ResourceType.Wood:
                ResourceManager.Instance.AddWood(amountPerTick);
                break;

            case ResourceType.Stone:
                ResourceManager.Instance.AddStone(amountPerTick);
                break;

            case ResourceType.Food:
                ResourceManager.Instance.AddFood(amountPerTick);
                break;
        }
    }
}