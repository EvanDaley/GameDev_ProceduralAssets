using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcWarrior : MonoBehaviour
{
    public float reactiveSearchRadius = 1f;
    public float proactiveSearchRadius = 10f;
    public float defaultSearchDelay = 10f;
    public float searchDelayMin = 10f;
    public float searchDelayMax = 20f;
    public float defenseCapability = .5f;
    public float attackCapability = .5f;
    public float attackDelay = 1f;
    private bool canAttack = true;

    private bool isSearching = false;
    private GameObject targetEnemy = null;
    private GameObject targetAlly = null;
    private float? targetLockTime;

    private Vector3 vectorToTarget;
    public Vector3 VectorToTarget {
        get { return vectorToTarget; }
        set { vectorToTarget = value; }
    }
    
    private PlayerAttributes npcAttributes;
    private ActionAnimator npcAnimator;

    void Start()
    {
        npcAttributes = gameObject.GetComponent<PlayerAttributes>();
        npcAnimator = gameObject.GetComponent<ActionAnimator>();
    }

    private GameObject[] FindLocalEnemies(float radius)
    {
        // Consider layers for performance. Could also retrieve a list of "Player" GameObjects for longer radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        List<GameObject> enemies = new List<GameObject>();
        for (int i = 0; i < hitColliders.Length; i++) {
            GameObject potentialEnemy = hitColliders[i].gameObject;
            PlayerAttributes potentialEnemyAttributes = potentialEnemy.GetComponent<PlayerAttributes>();
            if (potentialEnemy.tag == Constants.PlayerTag && potentialEnemyAttributes.FactionId != npcAttributes.FactionId) {
                enemies.Add(potentialEnemy);
            }
        }

        return enemies.ToArray();
    }

    private GameObject[] FindLocalAllies(float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        List<GameObject> allies = new List<GameObject>();
        for (int i = 0; i < hitColliders.Length; i++) {
            GameObject potentialEnemy = hitColliders[i].gameObject;
            PlayerAttributes potentialEnemyAttributes = potentialEnemy.GetComponent<PlayerAttributes>();
            if (potentialEnemy.tag == Constants.PlayerTag && potentialEnemyAttributes.FactionId == npcAttributes.FactionId) {
                allies.Add(potentialEnemy);
            }
        }

        return allies.ToArray();
    }

    private void SearchForEnemies() {
        isSearching = true;
        GameObject[] enemies = FindLocalEnemies(reactiveSearchRadius);
        if (enemies.Length == 0) {
            enemies = FindLocalEnemies(proactiveSearchRadius);
        }
        
        if (enemies.Length > 0) {
            int chosenEnemyIndex = (int)((Random.value * enemies.Length) % enemies.Length);
            targetEnemy = enemies[chosenEnemyIndex];
            targetLockTime = Mathf.Max(Random.value * searchDelayMax, searchDelayMin);
        }
        isSearching = false;
        StartCoroutine(TargetEnemyReset());
    }

    private void SearchForAllies() {
        isSearching = true;
        GameObject[] allies = FindLocalAllies(proactiveSearchRadius);
        
        if (allies.Length > 0) {
            for (int i = 0; i < allies.Length; i++) {
                GameObject potentialMover = allies[i];
                PlayerControlScript playerControlScript = potentialMover.GetComponent<PlayerControlScript>();
                NpcWarrior npcWarrior = potentialMover.GetComponent<NpcWarrior>();
                if (playerControlScript && playerControlScript.MovementDirection != null) {
                    targetAlly = potentialMover;
                } else if (npcWarrior && npcWarrior.VectorToTarget != null) {
                    targetAlly = potentialMover;
                }
            }
            targetLockTime = Mathf.Max(Random.value * searchDelayMax, searchDelayMin);
        }
        isSearching = false;
        StartCoroutine(TargetEnemyReset());
    }

    private IEnumerator TargetEnemyReset() {
        if (targetLockTime != null && targetLockTime > 0f) {
            yield return new WaitForSeconds((float)targetLockTime);
        } else {
            yield return new WaitForSeconds(defaultSearchDelay);
        }
        targetEnemy = null;
    }

    private IEnumerator TargetAllyReset() {
        if (targetLockTime != null && targetLockTime > 0f) {
            yield return new WaitForSeconds((float)targetLockTime);
        } else {
            yield return new WaitForSeconds(defaultSearchDelay);
        }
        targetAlly = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSearching) {
            if (targetEnemy == null) {
                SearchForEnemies();
            } else if (targetAlly == null) {
                // If no ally, try and move with any local allies
                SearchForAllies();
            }
        }
        
        if (targetEnemy != null) {
            transform.LookAt(targetEnemy.transform);

            if (Vector3.Distance(transform.position, targetEnemy.transform.position) <= reactiveSearchRadius) {
                PlayerControlScript playerControl = targetEnemy.GetComponent<PlayerControlScript>();
                // Parry (block eventually)
                if (playerControl && playerControl.ActionId != null && Random.value <= defenseCapability) {
                    ActionDefinitions.ActionId actionId = ActionDefinitions.ActionIdParryMapping[(ActionDefinitions.ActionId)playerControl.ActionId];
                    // TODO: Only a chance that it chooses the right type
                    ActionDefinitions.ActionType actionType = ActionDefinitions.ActionType.LightAttack;
                    if (playerControl.AttackModifier) {
                        actionType = ActionDefinitions.ActionType.HeavyAttack;
                    }
                    ActionDefinitions.Action action = ActionDefinitions.ActionMap[actionId][actionType];
                    // NOTE: Trigger animation for this action
                    npcAnimator.ActionPath = action.actionPath;
                } else if (canAttack && Random.value <= attackCapability) {
                    // TODO: Randomly choose attack
                    // int chosenAttackIndex = Random.Range(0, ActionDefinitions.ActionIdParryMapping.Keys.Count - 1);

                    ActionDefinitions.ActionId actionId = ActionDefinitions.ActionIdParryMapping[ActionDefinitions.ActionId.DownLeftAttack];
                    ActionDefinitions.ActionType actionType = ActionDefinitions.ActionType.HeavyAttack;
                    ActionDefinitions.Action action = ActionDefinitions.ActionMap[actionId][actionType];
                    // NOTE: Trigger animation for this action
                    npcAnimator.ActionPath = action.actionPath;
                    canAttack = false;
                    StartCoroutine(AttackTimerReset());
                }
            } else {
                Vector3 targetVector;
                if (transform.position.z < targetEnemy.transform.position.z) {
                    targetVector = (targetEnemy.transform.position - transform.position).normalized;
                } else {
                    targetVector = (transform.position - targetEnemy.transform.position).normalized;
                }
                targetVector.y = 0;
                VectorToTarget = targetVector;
                transform.Translate(targetVector * (npcAttributes.MovementSpeed * Time.deltaTime));
            }
        } else if (targetAlly != null) {
            PlayerControlScript allyPlayerControlScript = targetAlly.GetComponent<PlayerControlScript>();
            NpcWarrior allyWarrior = targetAlly.GetComponent<NpcWarrior>();

            Vector3? movementVector = null;

            if (allyPlayerControlScript && allyPlayerControlScript.MovementDirection != null) {
                movementVector = allyPlayerControlScript.MovementDirection;
            } else if (allyWarrior && allyWarrior.VectorToTarget != null) {
                movementVector = allyWarrior.VectorToTarget;
            }
            
            if (movementVector != null) {
                transform.Translate((Vector3)movementVector * (npcAttributes.MovementSpeed * Time.deltaTime));
            }
        }
    }

    
    private IEnumerator AttackTimerReset() {
        yield return new WaitForSeconds((float)attackDelay);
        canAttack = true;
    }
}
