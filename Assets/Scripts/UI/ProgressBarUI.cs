using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;

    private IHasProgress hasProgress;

    private void Start()
    {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if (hasProgress == null )
        {
            Debug.LogError("no progress");
        }
        hasProgress.OnProgressChanged += CuttingCounter_OnProgressChanged;
        barImage.fillAmount = 0f;
        gameObject.SetActive(false);
    }

    private void CuttingCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        barImage.fillAmount = e.progressNormalized;
        gameObject.SetActive(e.progressNormalized >= 0f && e.progressNormalized < 1f);
    }
}
