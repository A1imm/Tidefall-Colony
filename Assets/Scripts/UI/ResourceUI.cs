using TMPro;
using UnityEngine;

public class ResourceUI : MonoBehaviour
{
    [SerializeField] private ResourceManager resourceManager;

    [SerializeField] private TMP_Text woodValueText;
    [SerializeField] private TMP_Text stoneValueText;
    [SerializeField] private TMP_Text foodValueText;

    private void OnEnable()
    {
        if (resourceManager != null)
        {
            resourceManager.OnResourcesChanged += RefreshUI;
        }

        RefreshUI();
    }

    private void OnDisable()
    {
        if (resourceManager != null)
        {
            resourceManager.OnResourcesChanged -= RefreshUI;
        }
    }

    private void RefreshUI()
    {
        if (resourceManager == null)
            return;

        woodValueText.text =
            $"{resourceManager.Wood} / {resourceManager.ResourceCapacity}";

        stoneValueText.text =
            $"{resourceManager.Stone} / {resourceManager.ResourceCapacity}";

        foodValueText.text =
            $"{resourceManager.Food} / {resourceManager.ResourceCapacity}";
    }
}