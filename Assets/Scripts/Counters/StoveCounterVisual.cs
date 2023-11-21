using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private GameObject StoveOn;
    [SerializeField] private GameObject Particles;

    private void Awake()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool showVisual = (e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried);
        StoveOn.SetActive(showVisual);
        Particles.SetActive(showVisual);
    }
}
