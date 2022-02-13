// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class WeaponColliderScript : MonoBehaviour
{
    private PlayerAttributes playerAttributes;
    void Start() {
        playerAttributes = transform.parent.gameObject.GetComponent<PlayerAttributes>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == Constants.WeaponTag) {
            Debug.Log($"{transform.parent.gameObject.name}: Do parry stuff");
        } else if (other.gameObject.tag == Constants.PlayerTag) {
            PlayerAttributes otherPlayersAttributes = other.gameObject.GetComponent<PlayerAttributes>();
            float damage = playerAttributes.PlayerLightAttackDamage;
            if (playerAttributes.AttackModifier) {
                damage = playerAttributes.PlayerHeavyAttackDamage;
            }
            Debug.Log($"{transform.parent.gameObject.name}: Did damage ({damage}). Remaining health ({otherPlayersAttributes.PlayerHealth})");
        } else if (other.gameObject.tag == Constants.ShieldTag) {
            Debug.Log($"{transform.parent.gameObject.name}: Do shield");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
