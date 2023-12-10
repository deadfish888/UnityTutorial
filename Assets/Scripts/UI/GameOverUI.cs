using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipesDeliveredText;

    private void Start()
    {
        KitchenChaosManager.Instance.OnStateChanged += KCManager_OnStateChanged;
        Hide();
    }

    private void KCManager_OnStateChanged()
    {
        if (KitchenChaosManager.Instance.IsGameOver())
        {
            recipesDeliveredText.text = DeliveryManager.Instance.SuccessfulRecipesAmount.ToString();
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
