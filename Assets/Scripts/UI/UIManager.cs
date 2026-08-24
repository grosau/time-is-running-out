using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject hudPanel;
    [SerializeField] GameObject PowerupPanel;
    [SerializeField] GameObject gameOverPanel;

    [SerializeField] TMP_Text hpText;
    [SerializeField] Slider sanityBar;
    [SerializeField] TMP_Text bulletCounterText;

    private SanityMeter sanityMeter;

    void Start()
    {
        sanityMeter = FindObjectOfType<SanityMeter>();

        GameManager.OnGameStateChanged += HandleStateChange;

        PlayerHealth._onHealthChange += HandleHealthChanged;

        SanityMeter._onSanityChanged += HandleSanityChanged;

        WeaponSystem.OnAmmoChanged += HandleAmmoChanged;
    }

    void OnDestroy()
    {
        GameManager.OnGameStateChanged -= HandleStateChange;

        PlayerHealth._onHealthChange -= HandleHealthChanged;

        SanityMeter._onSanityChanged -= HandleSanityChanged;

        WeaponSystem.OnAmmoChanged -= HandleAmmoChanged;
    }

    void HandleStateChange(GameManager.GameState newState)
    {
        Debug.Log("UIManager recived State " + newState);
        switch (newState)
        {
            case GameManager.GameState.MainMenu:
                mainMenuPanel.SetActive(true);
                hudPanel.SetActive(false);
                PowerupPanel.SetActive(false);
                gameOverPanel.SetActive(false);
                break;
            case GameManager.GameState.Corridor:
                mainMenuPanel.SetActive(false);
                hudPanel.SetActive(true);
                PowerupPanel.SetActive(false);
                gameOverPanel.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;
            case GameManager.GameState.Arena:
                mainMenuPanel.SetActive(false);
                hudPanel.SetActive(true);
                PowerupPanel.SetActive(false);
                gameOverPanel.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;
            case GameManager.GameState.PowerUpSelection:
                mainMenuPanel.SetActive(false);
                hudPanel.SetActive(true);
                PowerupPanel.SetActive(true);
                gameOverPanel.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case GameManager.GameState.GameOver:
                mainMenuPanel.SetActive(false);
                hudPanel.SetActive(false);
                PowerupPanel.SetActive(false);
                gameOverPanel.SetActive(true);
                break;
        }
    }

    void HandleHealthChanged(float currentHealth)
    {
        hpText.text = currentHealth.ToString();
    }

    void HandleSanityChanged(float currentSanity)
    {
        sanityBar.value = currentSanity / sanityMeter.MaxSanity;
    }

    void HandleAmmoChanged(int currentAmmo)
    {
        bulletCounterText.text = currentAmmo.ToString();
    }
}
