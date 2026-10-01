using UnityEngine;

public class StorageCapacityUpgrade : MonoBehaviour
{
    [SerializeField] private int upgradedCapacity = 50;

    private void OnEnable()
    {
        if (ResourceManager.Instance == null)
        {
            Debug.LogError("ResourceManager not found.");
            return;
        }

        ResourceManager.Instance.UpgradeStorageCapacity(
            upgradedCapacity
        );
    }
}