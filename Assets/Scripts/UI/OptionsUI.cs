using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }

    [SerializeField] private Button musicButton;
    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button interactAltButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Transform pressToRebindKeyTransform;
    private Action onCloseAction;

    private void Awake()
    {
        Instance = this;
        musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        soundEffectsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        closeButton.onClick.AddListener(() =>
        {
            Hide();
            onCloseAction();
        });
        moveUpButton.onClick.AddListener(() =>
        {
            RebingBinding(GameInput.Binding.Move_Up);
        });
        moveDownButton.onClick.AddListener(() => { RebingBinding(GameInput.Binding.Move_Down); });
        moveLeftButton.onClick.AddListener(() => { RebingBinding(GameInput.Binding.Move_Left); });
        moveRightButton.onClick.AddListener(() => { RebingBinding (GameInput.Binding.Move_Right);});
        interactButton.onClick.AddListener(() => { RebingBinding(GameInput.Binding.Interact); });
        pauseButton.onClick.AddListener(() => { RebingBinding(GameInput.Binding.Pause); });
        interactAltButton.onClick.AddListener(() => { RebingBinding(GameInput.Binding.InteractAlternate); });
    }

    private void Start()
    {
        KitchenChaosManager.Instance.OnTogglePauseGame += KCManager_OnStateChanged;
        UpdateVisual();
        Hide();
    }

    private void KCManager_OnStateChanged()
    {
        if(!KitchenChaosManager.Instance.IsPausedGame)
        {
            Hide();
        }
    }

    private void UpdateVisual()
    {
        soundEffectsButton.GetComponentInChildren<TextMeshProUGUI>().text = "Sound Effects: " + Mathf.Round(SoundManager.Instance.Volume * 10);
        musicButton.GetComponentInChildren<TextMeshProUGUI>().text = "Music: " + Mathf.Round(MusicManager.Instance.Volume * 10);
        moveUpButton.GetComponentInChildren<TextMeshProUGUI>().text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        moveDownButton.GetComponentInChildren<TextMeshProUGUI>().text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        moveLeftButton.GetComponentInChildren<TextMeshProUGUI>().text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        moveRightButton.GetComponentInChildren<TextMeshProUGUI>().text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        interactButton.GetComponentInChildren<TextMeshProUGUI>().text = GameInput.Instance.GetBindingText (GameInput.Binding.Interact);
        interactAltButton.GetComponentInChildren<TextMeshProUGUI>().text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
        pauseButton.GetComponentInChildren<TextMeshProUGUI>().text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
    }

    public void Show(Action onCloseAction)
    {
        this.onCloseAction = onCloseAction;
        gameObject.SetActive(true);
        soundEffectsButton.Select();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ShowPressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(true);
    }

    private void HidePressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(false);
    }

    private void RebingBinding(GameInput.Binding binding)
    {
        ShowPressToRebindKey();
        GameInput.Instance.RebindBinding(binding, () =>
        {
            HidePressToRebindKey();
            UpdateVisual();
        });
    }
}
