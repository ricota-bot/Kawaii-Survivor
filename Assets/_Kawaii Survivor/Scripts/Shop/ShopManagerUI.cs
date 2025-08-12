using System.Collections;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;


public class ShopManagerUI : MonoBehaviour
{
    [Header("Player Stats Elements")]
    [SerializeField] private RectTransform playerStatsPanel;
    [SerializeField] private RectTransform playerStatsClosePanel;

    private Vector2 playerStatsOpenPosition;
    private Vector2 playerStatsClosedPosition;


    [Header("Inventory Elements")]
    [SerializeField] private RectTransform inventoryPanel;
    [SerializeField] private RectTransform inventoryClosedPanel;

    private Vector2 inventoryOpenPosition;
    private Vector2 inventoryClosedPosition;

    [Header("Item Info Elements")]
    [SerializeField] private RectTransform itemInfoSlidePanel;
    private Vector2 itemInfoOpenPosition;
    private Vector2 itemInfoClosedPosition;


    IEnumerator Start()
    {
        yield return null;

        ConfigurePlayerStatsPanel();
        ConfigureInventoryPanel();
        ConfigureItemInfoPanel();
    }

    // PLAYER STATS PANEL
    private void ConfigurePlayerStatsPanel()
    {
        float width = Screen.width / (4 * playerStatsPanel.lossyScale.x);
        playerStatsPanel.offsetMax = playerStatsPanel.offsetMax.With(x: width);

        playerStatsOpenPosition = playerStatsPanel.anchoredPosition;
        playerStatsClosedPosition = playerStatsOpenPosition + Vector2.left * width;

        playerStatsPanel.anchoredPosition = playerStatsClosedPosition;

        HidePlayerStatsPanel();
    }
    [NaughtyAttributes.Button]
    public void ShowPlayerStatsPanel()
    {
        playerStatsPanel.gameObject.SetActive(true);
        playerStatsClosePanel.gameObject.SetActive(true);
        playerStatsClosePanel.GetComponent<Image>().raycastTarget = true;

        LeanTween.cancel(playerStatsPanel);
        LeanTween.move(playerStatsPanel, playerStatsOpenPosition, 0.5f).setEase(LeanTweenType.easeInCubic);

        LeanTween.cancel(playerStatsClosePanel);
        LeanTween.alpha(playerStatsClosePanel, 0.8f, 0.5f).setRecursive(false);

    }

    [NaughtyAttributes.Button]
    public void HidePlayerStatsPanel()
    {
        playerStatsClosePanel.GetComponent<Image>().raycastTarget = false;

        LeanTween.cancel(playerStatsPanel);
        LeanTween.move(playerStatsPanel, playerStatsClosedPosition, 0.5f)
            .setEase(LeanTweenType.easeOutCubic)
            .setOnComplete(() => playerStatsPanel.gameObject.SetActive(false));

        LeanTween.cancel(playerStatsClosePanel);
        LeanTween.alpha(playerStatsClosePanel, 0, 0.5f).setRecursive(false)
            .setOnComplete(() => playerStatsClosePanel.gameObject.SetActive(false));
    }



    // INVENTORY PANEL
    private void ConfigureInventoryPanel()
    {
        float width = Screen.width / (4 * inventoryPanel.lossyScale.x);
        inventoryPanel.offsetMin = inventoryPanel.offsetMin.With(x: -width);

        inventoryOpenPosition = inventoryPanel.anchoredPosition;
        inventoryClosedPosition = inventoryOpenPosition - Vector2.left * width;

        inventoryPanel.anchoredPosition = inventoryClosedPosition;

        HideInventoryPanel(false);
    }

    [NaughtyAttributes.Button]
    public void ShowInventoryPanel()
    {
        inventoryPanel.gameObject.SetActive(true);
        inventoryClosedPanel.gameObject.SetActive(true);
        inventoryClosedPanel.GetComponent<Image>().raycastTarget = true;

        LeanTween.cancel(inventoryPanel);
        LeanTween.move(inventoryPanel, inventoryOpenPosition, 0.5f).setEase(LeanTweenType.easeInCubic);

        LeanTween.cancel(inventoryClosedPanel);
        LeanTween.alpha(inventoryClosedPanel, 0.8f, 0.5f).setRecursive(false);
    }

    [NaughtyAttributes.Button]
    public void HideInventoryPanel(bool hideItemInfoPanel = true)
    {
        inventoryClosedPanel.GetComponent<Image>().raycastTarget = false;

        LeanTween.cancel(inventoryPanel);
        LeanTween.move(inventoryPanel, inventoryClosedPosition, 0.5f)
            .setEase(LeanTweenType.easeOutCubic)
            .setOnComplete(() => inventoryPanel.gameObject.SetActive(false));

        LeanTween.cancel(inventoryClosedPanel);
        LeanTween.alpha(inventoryClosedPanel, 0, 0.5f).setRecursive(false)
            .setOnComplete(() => inventoryClosedPanel.gameObject.SetActive(false));

        if (hideItemInfoPanel)
            HideItemInfoPanel();
    }


    // ITEM INFO PANEL

    private void ConfigureItemInfoPanel()
    {
        float height = Screen.height / (2 * itemInfoSlidePanel.lossyScale.y);

        itemInfoSlidePanel.offsetMax = itemInfoSlidePanel.offsetMax.With(y: height);

        itemInfoOpenPosition = itemInfoSlidePanel.anchoredPosition;
        itemInfoClosedPosition = itemInfoOpenPosition + Vector2.down * height;

        itemInfoSlidePanel.anchoredPosition = itemInfoClosedPosition;

        itemInfoSlidePanel.gameObject.SetActive(false);
    }

    [NaughtyAttributes.Button]
    public void ShowItemInfoPanel()
    {
        itemInfoSlidePanel.gameObject.SetActive(true);

        itemInfoSlidePanel.LeanCancel();
        itemInfoSlidePanel.LeanMove((Vector3)itemInfoOpenPosition, 0.3f).setEase(LeanTweenType.easeOutCubic);
    }

    [NaughtyAttributes.Button]
    public void HideItemInfoPanel()
    {
        itemInfoSlidePanel.LeanCancel();
        itemInfoSlidePanel.LeanMove((Vector3)itemInfoClosedPosition, 0.3f)
            .setEase(LeanTweenType.easeInCubic)
            .setOnComplete(() => itemInfoSlidePanel.gameObject.SetActive(false));
    }


}
