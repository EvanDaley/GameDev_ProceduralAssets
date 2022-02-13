using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

public class PlayerColliderScript : MonoBehaviour
{
    private PlayerAttributes playerAttributes;
    private Dictionary<string, string> recentDamagers = new Dictionary<string, string>();

    void Start() {
        playerAttributes = gameObject.GetComponent<PlayerAttributes>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == Constants.WeaponTag) {
            // Debug.Log($"{gameObject.name}");
            PlayerAttributes otherPlayersAttributes = other.transform.parent.gameObject.GetComponent<PlayerAttributes>();
            
            if (!recentDamagers.ContainsKey(otherPlayersAttributes.PlayerId) && playerAttributes.PlayerId != otherPlayersAttributes.PlayerId) {
                float damage = otherPlayersAttributes.PlayerLightAttackDamage;
                if (otherPlayersAttributes.AttackModifier) {
                    damage = otherPlayersAttributes.PlayerHeavyAttackDamage;
                }
                if (playerAttributes.PlayerHealth - damage <= 0) {
                    gameObject.GetComponent<Collider>().enabled = false;
                    gameObject.GetComponent<Renderer>().material.color = Color.red;
                    StartCoroutine(DisableCharacter());
                } else {
                    playerAttributes.PlayerHealth -= damage;
                    Debug.Log($"{gameObject.name}: Took damage ({damage})");
                }
                StartCoroutine(PreventRepeatedCollisions(otherPlayersAttributes.PlayerId, otherPlayersAttributes.AttackStepSpeed));
            }
        } else if (other.gameObject.tag == Constants.PlayerTag) {
            Debug.Log($"{gameObject.name}: Do?");
        } else if (other.gameObject.tag == Constants.ShieldTag) {
            Debug.Log($"{gameObject.name}: Do shield stuff");
        }
    }

    private IEnumerator PreventRepeatedCollisions(string attackerId, float disableTimer) {
        recentDamagers[attackerId] = attackerId;
        yield return new WaitForSeconds(disableTimer);
        recentDamagers.Remove(attackerId);
    }

    private IEnumerator DisableCharacter() {
        yield return new WaitForSeconds(2f);
        PlayerControlScript playerControlScript = gameObject.GetComponent<PlayerControlScript>();
        NpcWarrior npcWarrior = gameObject.GetComponent<NpcWarrior>();

        if (playerControlScript) {
            playerControlScript.enabled = false;
        } else if (npcWarrior) {
            npcWarrior.enabled = false;
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
