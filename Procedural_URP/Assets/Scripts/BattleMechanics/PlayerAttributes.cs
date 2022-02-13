using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour {

    [Tooltip("The ID of this player.")]
    public string PlayerId;

    [Tooltip("The Squad of this player.")]
    public string SquadId;

    [Tooltip("The Faction of this player.")]
    public string FactionId;
    
    [Tooltip("The rate at which the player walks.")]
    public float MovementSpeed = 5f;
    
    [Tooltip("The rate at which the player looks.")]
    public float LookSpeed = 2.5f;

    [Tooltip("The maximum health this player can have.")]
    public float PlayerMaxHealth = 50f;
    [Tooltip("The damage of a Light Attack.")]
    public float PlayerLightAttackDamage = 5f;
    [Tooltip("The time (in seconds) between each point in a Light Attack path.")]
    public float PlayerLightAttackSpeed = .05f;
    [Tooltip("The damage of a Heavy Attack.")]
    public float PlayerHeavyAttackDamage = 10f;
    [Tooltip("The time (in seconds) between each point in a Heavy Attack path.")]
    public float PlayerHeavyAttackSpeed = .10f;

    public float AttackStepSpeed {
        get {
            if (attackModifier) {
                return PlayerHeavyAttackSpeed;
            }
            return PlayerLightAttackSpeed;
        }
    }
    private bool attackModifier;
    public bool AttackModifier
    {
        get { return attackModifier; }
        set { attackModifier = value; }
    }

    private float playerHealth;
    public float PlayerHealth
    {
        get { return playerHealth; }
        set { playerHealth = Mathf.Min(value, PlayerMaxHealth); }
    }

    void Start() {
        PlayerHealth = PlayerMaxHealth;
    }
}