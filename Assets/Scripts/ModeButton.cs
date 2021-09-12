using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeButton : MonoBehaviour
{
    [SerializeField]
    private GameObject purchaseUI;
    [SerializeField]
    private MonoColorChanger purchaseButton;
    [SerializeField]
    private MonoColorChanger purchaseButtonBack;

    [SerializeField]
    private GameObject rankUI;
    [SerializeField]
    private MonoColorChanger rankButton;
    [SerializeField]
    private MonoColorChanger rankButtonBack;

    [SerializeField]
    private GameObject optionUI;
    [SerializeField]
    private MonoColorChanger optionButton;
    [SerializeField]
    private MonoColorChanger optionButtonBack;

    private void Start()
    {
        PurchaseButtonClicked();
    }

    public void PurchaseButtonClicked()
    {
        purchaseUI.SetActive(true);
        purchaseButton.ToWhiteColor();
        purchaseButtonBack.ToBlackColor();

        rankUI.SetActive(false);
        rankButton.ToBlackColor();
        rankButtonBack.ToWhiteColor();

        optionUI.SetActive(false);
        optionButton.ToBlackColor();
        optionButtonBack.ToWhiteColor();
    }

    public void RankButtonClicked()
    {
        purchaseUI.SetActive(false);
        purchaseButton.ToBlackColor();
        purchaseButtonBack.ToWhiteColor();

        rankUI.SetActive(true);
        rankButton.ToWhiteColor();
        rankButtonBack.ToBlackColor();

        optionUI.SetActive(false);
        optionButton.ToBlackColor();
        optionButtonBack.ToWhiteColor();
    }

    public void OptionButtonClicked()
    {
        purchaseUI.SetActive(false);
        purchaseButton.ToBlackColor();
        purchaseButtonBack.ToWhiteColor();

        rankUI.SetActive(false);
        rankButton.ToBlackColor();
        rankButtonBack.ToWhiteColor();

        optionUI.SetActive(true);
        optionButton.ToWhiteColor();
        optionButtonBack.ToBlackColor();
    }
}
