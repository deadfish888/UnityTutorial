using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenChaosManager : MonoBehaviour
{
    public static KitchenChaosManager Instance { get; private set; }
    public float CountdownToStartTimer { get => countdownToStartTimer; }
    public bool IsPausedGame { get => isPausedGame; }

    public event Action OnStateChanged;
    public event Action OnTogglePauseGame;
    public enum State
    {
        WaitingToStart,
        CountdownToStart,
        Playing,
        Over
    }

    private State state;
    private float waitingToStartTimer = 1f;
    private float countdownToStartTimer = 3f;
    private float playingTimer;
    private float playingTimerMax = 10f;
    private bool isPausedGame;

    private void Awake()
    {
        Instance = this;
        state = State.WaitingToStart;
        isPausedGame = false;
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    public void TogglePauseGame()
    {
        isPausedGame = !isPausedGame;
        if (isPausedGame)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
        OnTogglePauseGame?.Invoke();
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                waitingToStartTimer -= Time.deltaTime;
                if (waitingToStartTimer < 0f)
                {
                    state = State.CountdownToStart;
                    OnStateChanged?.Invoke();
                }
                break;
            case State.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer < 0f)
                {
                    state = State.Playing;
                    playingTimer = playingTimerMax;
                    OnStateChanged?.Invoke();
                }
                break;
            case State.Playing:
                playingTimer -= Time.deltaTime;
                if (playingTimer < 0f)
                {
                    state = State.Over;
                    OnStateChanged?.Invoke();
                }
                break;
            case State.Over:
                break;
        }
    }

    public bool IsPlaying()
    {
        return state == State.Playing;
    }

    public bool IsCountdownToStartActive()
    {
        return state == State.CountdownToStart;
    }

    internal bool IsGameOver()
    {
        return state == State.Over;
    }

    internal float GetPlayingTimerNormalized()
    {
        return 1 - playingTimer / playingTimerMax;
    }
}
