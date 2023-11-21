using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //Counter has nothing
            if (player.HasKitchenObject())
            {
                // Player carry KitchenObject
                player.GetKitchenObject().setKitchenObjectParent(this);
            }
            else
            {
                // Player carry nothing
            }
        }
        else
        {
            //Counter has kitchenObject
            if (player.HasKitchenObject())
            {
                // Player carry KitchenObject
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // Player carry plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetkitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
                else
                {
                    // Player not carry plate but sth else
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        // Counter has plate
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetkitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
            else
            {
                // Player carry nothing
                GetKitchenObject().setKitchenObjectParent(player);
            }
        }

    }

}
