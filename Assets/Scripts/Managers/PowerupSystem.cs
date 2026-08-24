using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PowerupSystem : MonoBehaviour
{

    private SanityMeter sanityMeter;
    [SerializeField] List<Powerup> powerupPool;

    [SerializeField] float maxHPRestoreAmount;
    [SerializeField] float maxSanityRestoreAmount;

    private List<Powerup> currentOffers = new List<Powerup>();
    private List<Powerup> collectedPowerups = new List<Powerup>();

    private PlayerHealth playerHealth;
    private PlayerController playerController;
    private WeaponSystem weaponSystem;


    void Start()
    {
        sanityMeter = FindObjectOfType<SanityMeter>();
        GameManager.OnGameStateChanged += HandleStateChange;

        playerHealth = FindObjectOfType<PlayerHealth>();
        playerController = FindObjectOfType<PlayerController>();
        weaponSystem = FindObjectOfType<WeaponSystem>();
    }

    void OnDestroy()
    {
        GameManager.OnGameStateChanged -= HandleStateChange;
    }

    void HandleStateChange(GameManager.GameState newState)
    {
        switch (newState)
        {
            case GameManager.GameState.PowerUpSelection:
                GenerateOffers();
                break;
        }
    }

    void GenerateOffers()
    {
        currentOffers.Clear();
        float sanityRatio = sanityMeter.CurrentSanity / sanityMeter.MaxSanity;
        float inverseRatio = 1 - sanityRatio;

        float commonWeight = 100f * sanityRatio;
        float uncommonWeight = 50f * (1 + inverseRatio);
        float epicWeight = 20f * (1 + inverseRatio * 3);
        float legendaryWeight = 5f * (1 + inverseRatio * 8);

        Powerup offer1 = GetWeightedRandomPowerup(commonWeight, uncommonWeight, epicWeight, legendaryWeight);
        currentOffers.Add(offer1);

        Powerup offer2 = GetWeightedRandomPowerup(commonWeight, uncommonWeight, epicWeight, legendaryWeight);
        while (offer2.type == offer1.type)
        {
            offer2 = GetWeightedRandomPowerup(commonWeight, uncommonWeight, epicWeight, legendaryWeight);
        }
        currentOffers.Add(offer2);

        Powerup hpRestore = new Powerup();
        hpRestore.type = PowerupType.MaxHP;
        hpRestore.displayName = "Restore HP";
        hpRestore.amount = maxHPRestoreAmount * inverseRatio;
        currentOffers.Add(hpRestore);

        Powerup sanityRestore = new Powerup();
        sanityRestore.type = PowerupType.MaxSanity;
        sanityRestore.displayName = "Restore Sanity";
        sanityRestore.amount = maxSanityRestoreAmount * inverseRatio;
        currentOffers.Add(sanityRestore);
    }

    Powerup GetWeightedRandomPowerup(float commonWeight, float uncommonWeight, float epicWeight, float legendaryWeight)
    {
        float totalWeight = commonWeight + uncommonWeight + epicWeight + legendaryWeight;

        float roll = Random.Range(0, totalWeight);

        Rarity selectedRarity;

        if (roll < commonWeight)
            selectedRarity = Rarity.Common;
        else if (roll < commonWeight + uncommonWeight)
            selectedRarity = Rarity.Uncommon;
        else if (roll < commonWeight + uncommonWeight + epicWeight)
            selectedRarity = Rarity.Epic;
        else
            selectedRarity = Rarity.Legendary;

        List<Powerup> filteredPool = powerupPool.FindAll(p => p.rarity == selectedRarity);

        if (filteredPool.Count > 0)
            return filteredPool[Random.Range(0, filteredPool.Count)];

        return powerupPool[Random.Range(0, powerupPool.Count)];

    }

    public void ApplyPowerup(Powerup powerup)
    {
        collectedPowerups.Add(powerup);

        switch (powerup.type)
        {
            case PowerupType.MaxHP:
                playerHealth.HealHP(powerup.amount);
                break;
            case PowerupType.MaxSanity:
                sanityMeter.RestoreSanity(powerup.amount);
                break;
            case PowerupType.Damage:
                weaponSystem.IncreaseDamage(powerup.amount);
                break;
            case PowerupType.FireRate:
                weaponSystem.IncreaseFireRate(powerup.amount);
                break;
            case PowerupType.MoveSpeed:
                playerController.IncreaseMoveSpeed(powerup.amount);
                break;
            case PowerupType.JumpHeight:
                playerController.IncreaseJumpforce(powerup.amount);
                break;
            case PowerupType.AmmoCapacity:
                weaponSystem.IncreaseAmmoCapacity((int)powerup.amount);
                break;
            case PowerupType.MaxJumps:
                playerController.IncreaseMaxJumps((int)powerup.amount);
                break;
        }

        GameManager.Instance.ChangeState(GameManager.GameState.Corridor);
    }

    public enum PowerupType
    {
        Damage,
        FireRate,
        AmmoCapacity,
        MaxHP,
        MaxSanity,
        MoveSpeed,
        JumpHeight,
        MaxJumps
    }

    public enum Rarity
    {
        Common,
        Uncommon,
        Epic,
        Legendary
    }


    [System.Serializable]
    public class Powerup
    {
        public PowerupType type;
        public float amount;
        public Rarity rarity;
        public string displayName;





    }

}
