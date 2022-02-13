using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlScript : MonoBehaviour, Controls.IPlayerActions
{
    private Vector3 movementDirection;
    public Vector3 MovementDirection {
        get { return movementDirection; }
        set { movementDirection = value; }
    }
    private Vector3? lookDirection;
    public Vector3? LookDirection {
        get { return lookDirection; }
        set {
            lookDirection = value;
        }
    }
    private bool attackModifier;
    public bool AttackModifier {
        get { return attackModifier; }
        set {
            attackModifier = value;
            if (playerAttributes) {
                playerAttributes.AttackModifier = value;
            }
        }
    }
    private bool lookModifier;
    public bool LookModifier {
        get { return lookModifier; }
        set { lookModifier = value; }
    }
    private Vector3? attack;
    public Vector3? Attack {
        get { return attack; }
        set { attack = value; }
    }

    private ActionDefinitions.ActionId? actionId;
    public ActionDefinitions.ActionId? ActionId {
        get { return actionId; }
        set { actionId = value; }
    }

    private bool isAttackDelay = false;
    private Vector3 stagedAttack;
    private float attackDelay = .05f;
    private PlayerAttributes playerAttributes;
    private ActionAnimator actionAnimator;
    private Transform playerCamera;

    void Start() {
        playerAttributes = gameObject.GetComponent<PlayerAttributes>();
        actionAnimator = gameObject.GetComponent<ActionAnimator>();
        playerCamera = transform.Find("Camera");
    }

    public void OnMovement(InputAction.CallbackContext context) {
        movementDirection = context.ReadValue<Vector3>();
    }

    public void OnAttack(InputAction.CallbackContext context) {
        if (!LookModifier) {
            LookDirection = null;
            stagedAttack = context.ReadValue<Vector3>();
            if (!isAttackDelay) {
                StartCoroutine(AttackInputDelay());
            }
        } else {
            LookDirection = context.ReadValue<Vector3>();
            Debug.Log($"Look Direction: {LookDirection}");
        }
    }

    public void OnAttackModifier(InputAction.CallbackContext context) {
        AttackModifier = context.ReadValueAsButton();
    }

    public void OnLookModifier(InputAction.CallbackContext context) {
        LookModifier = context.ReadValueAsButton();
    }

    private IEnumerator AttackInputDelay() {
        isAttackDelay = true;
        Vector3? firstStagedAttack = stagedAttack;
        yield return new WaitForSeconds(attackDelay);
        // NOTE: We use the value *after* the delay as this allows time for multi-key press resolution
        Vector3? secondStagedAttack = stagedAttack;

        // Ignore zero Vector unless it is the key down event
        //      Vector will always zero after keys are released, we do not want this to result in a center attack
        //      But we do still want to capture the center attack
        if (secondStagedAttack.Equals(new Vector3(0, 0, 0)) && Attack != null) {
            secondStagedAttack = null;
        } else {
            Debug.Log($"Allowed it: first -> {firstStagedAttack}. second -> {secondStagedAttack}. prev -> {Attack}");
        }

        Attack = secondStagedAttack;

        if (Attack != null) {
            Vector3 att = (Vector3)Attack;
            ActionId = ActionDefinitions.AttackVectorToActionIdMapping[(int)att.x+1, (int)att.y+1];
            ActionDefinitions.ActionType actionType = ActionDefinitions.ActionType.LightAttack;
            if (AttackModifier) {
                actionType = ActionDefinitions.ActionType.HeavyAttack;
            }
            ActionDefinitions.Action action = ActionDefinitions.ActionMap[(ActionDefinitions.ActionId)ActionId][actionType];
            // NOTE: Trigger animation for this action
            actionAnimator.ActionPath = action.actionPath;
            // Attack = null;
            // ActionId = null;
        }
        isAttackDelay = false;
    }

    void Update()
    {
        #region Movement
        transform.Translate(MovementDirection * (playerAttributes.MovementSpeed * Time.deltaTime));
        if (LookDirection != null) {
            Vector3 lookDir = (Vector3)LookDirection;
            transform.Rotate(new Vector3(0, lookDir.x, 0));
            playerCamera.Rotate(new Vector3(lookDir.y, 0, 0));
        }
        #endregion
    }
}