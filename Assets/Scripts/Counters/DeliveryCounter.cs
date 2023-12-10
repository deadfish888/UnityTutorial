using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public static DeliveryCounter Instance {  get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            // Player carry KitchenObject
            if(player.GetKitchenObject().TryGetPlate(out var plateKitchenObject))
            {
                DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
                // Player carry a plate!
                player.GetKitchenObject().DestroySelf();
            }
        }
    }
}
