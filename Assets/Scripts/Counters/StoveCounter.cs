using System;
using System.Collections;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }

    [SerializeField] private FryingRecipeSO[] fryingObjectSOArray;
    [SerializeField] private BurningRecipeSO[] burningObjectSOArray;

    private State state;
    private FryingRecipeSO fryingObjectSO;
    private BurningRecipeSO burningObjectSO;
    private float fryingTimer;
    private float burningTimer;

    private void Start()
    {
        state = State.Idle;
        //StartCoroutine(HandleFryTimer());
    }

    private IEnumerator HandleFryTimer()
    {
        yield return new WaitForSeconds(1f);
    }

    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                break;
            case State.Frying:
                fryingTimer += Time.deltaTime;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = fryingTimer / fryingObjectSO.fryingTimerMax
                });
                if (fryingTimer >= fryingObjectSO.fryingTimerMax)
                {
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(fryingObjectSO.output, this);
                    burningTimer = 0f;
                    state = State.Fried;
                    burningObjectSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetkitchenObjectSO());
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = State.Fried
                    });
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = 0f
                    });
                }
                break;
            case State.Fried:
                burningTimer += Time.deltaTime;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = burningTimer / burningObjectSO.burningTimerMax
                });
                if (burningTimer >= burningObjectSO.burningTimerMax)
                {
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(burningObjectSO.output, this);
                    state = State.Burned;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = State.Burned
                    });

                }
                break;
            case State.Burned:
                break;
        }
    }

    public override void Interact(Player player)
    {
        // Counter not have KitchenObject
        if (!HasKitchenObject())
        {
            // Player carry KitchenObject that is cutable
            if (player.HasKitchenObject() && HasRecipeWithInput(player.GetKitchenObject().GetkitchenObjectSO()))
            {
                player.GetKitchenObject().setKitchenObjectParent(this);

                fryingObjectSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetkitchenObjectSO());
                fryingTimer = 0f;
                state = State.Frying;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = State.Frying
                });
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
            return;
        }
        // Counter have KitchenObject
        // Player carry KitchenObject
        if (player.HasKitchenObject())
        {
            // Player carry Plate
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetkitchenObjectSO()))
                {
                    GetKitchenObject().DestroySelf();
                }
            }
        }
        else
        {
            GetKitchenObject().setKitchenObjectParent(player);
        }
        state = State.Idle;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
        {
            state = state
        });
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = 1f
        });

    }

    private KitchenObjectSO GetFryingObjectSOFromInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }
        return null;
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO FryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return (FryingRecipeSO != null);
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (var cuttingRecipeSO in fryingObjectSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningObjectSOArray)
        {
            if (burningRecipeSO.input == inputKitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }
}
