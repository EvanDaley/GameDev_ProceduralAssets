using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionDefinitions {
    public enum ActionId {
        UpLeftAttack, UpAttack, UpRightAttack,
        LeftAttack, CenterAttack, RightAttack,
        DownLeftAttack, DownAttack, DownRightAttack
    };
    public enum ActionType {LightAttack, HeavyAttack}

// Down Left    -1,-1 = 0,0 
// Left         -1,0  = 0,1
// Up Left      -1,1  = 0,2
// Down         0,-1  = 1,0
// Center       0,0   = 1,1
// Up           0,1   = 1,2
// Down Right   1,-1  = 2,0
// Right        1,0   = 2,1
// Up Right     1,1   = 2,2

    public static ActionId[,] AttackVectorToActionIdMapping = new ActionId[,] {
        { ActionId.DownLeftAttack, ActionId.LeftAttack, ActionId.UpLeftAttack },
        { ActionId.DownAttack, ActionId.CenterAttack, ActionId.UpAttack },
        { ActionId.DownRightAttack, ActionId.RightAttack, ActionId.UpRightAttack }
    };

    public static Dictionary<ActionId, ActionId> ActionIdParryMapping = new Dictionary<ActionId, ActionId>()
    {
        { ActionId.DownLeftAttack, ActionId.DownRightAttack },
        { ActionId.LeftAttack, ActionId.RightAttack },
        { ActionId.UpLeftAttack, ActionId.UpRightAttack },
        { ActionId.DownAttack, ActionId.DownAttack },
        { ActionId.CenterAttack, ActionId.CenterAttack },
        { ActionId.UpAttack, ActionId.UpAttack },
        { ActionId.DownRightAttack, ActionId.DownLeftAttack },
        { ActionId.RightAttack, ActionId.LeftAttack },
        { ActionId.UpRightAttack, ActionId.UpLeftAttack },
    };

    public class Action {
        public Vector3?[] actionPath;
        public ActionType actionType;

        public Action(Vector3?[] actionPath, ActionType actionType) {
            this.actionPath = actionPath;
            this.actionType = actionType;
        }
    }

    #region LeftAction
    public static Vector3?[] LightDownLeftActionPath = {
        new Vector3(-1,-1,1),
        new Vector3(0,0,1),
        new Vector3(1,1,1),
    };
    public static Action LightDownLeftAction;
    public static Vector3?[] HeavyDownLeftActionPath = {
        new Vector3(-1,0,-1),
        new Vector3(-1,-1,0),
        new Vector3(-1,-1,1),
        new Vector3(0,0,1),
        new Vector3(1,1,1),
    };
    public static Action HeavyDownLeftAction;
    public static Vector3?[] LightLeftActionPath = {
        new Vector3(-1,0,1),
        new Vector3(0,0,1),
        new Vector3(1,0,1),
    };
    public static Action LightLeftAction;
    public static Vector3?[] HeavyLeftActionPath = {
        new Vector3(-1,0,-1),
        new Vector3(-1,0,0),
        new Vector3(-1,0,1),
        new Vector3(0,0,1),
        new Vector3(1,0,1),
    };
    public static Action HeavyLeftAction;
    public static Vector3?[] LightUpLeftActionPath = {
        new Vector3(-1,1,1),
        new Vector3(0,0,1),
        new Vector3(1,-1,1),
    };
    public static Action LightUpLeftAction;
    public static Vector3?[] HeavyUpLeftActionPath = {
        new Vector3(0,1,-1),
        new Vector3(-1,1,0),
        new Vector3(-1,1,1),
        new Vector3(0,0,1),
        new Vector3(1,-1,1),
    };
    public static Action HeavyUpLeftAction;
    #endregion

    #region CenterAction
    public static Vector3?[] LightDownActionPath = {
        new Vector3(0,-1,1),
        new Vector3(0,0,1),
        new Vector3(0,1,1),
    };
    public static Action LightDownAction;
    public static Vector3?[] HeavyDownActionPath = {
        new Vector3(0,-1,-1),
        new Vector3(0,-1,0),
        new Vector3(0,-1,1),
        new Vector3(0,0,1),
        new Vector3(0,1,1),
    };
    public static Action HeavyDownAction;
    public static Vector3?[] LightCenterActionPath = {
        null,
        new Vector3(0,0,1),
    };
    public static Action LightCenterAction;
    public static Vector3?[] HeavyCenterActionPath = {
        null,
        null,
        new Vector3(0,0,1),
    };
    public static Action HeavyCenterAction;
    public static Vector3?[] LightUpActionPath = {
        new Vector3(0,1,1),
        new Vector3(0,0,1),
        new Vector3(0,-1,1),
    };
    public static Action LightUpAction;
    public static Vector3?[] HeavyUpActionPath = {
        new Vector3(0,1,-1),
        new Vector3(0,1,0),
        new Vector3(0,1,1),
        new Vector3(0,0,1),
        new Vector3(0,-1,1),
    };
    public static Action HeavyUpAction;
    #endregion

    #region RightAction
    public static Vector3?[] LightDownRightActionPath = {
        new Vector3(1,-1,1),
        new Vector3(0,0,1),
        new Vector3(-1,1,1),
    };
    public static Action LightDownRightAction;
    public static Vector3?[] HeavyDownRightActionPath = {
        new Vector3(1,0,-1),
        new Vector3(1,-1,0),
        new Vector3(1,-1,1),
        new Vector3(0,0,1),
        new Vector3(-1,1,1),
    };
    public static Action HeavyDownRightAction;
    public static Vector3?[] LightRightActionPath = {
        new Vector3(1,0,1),
        new Vector3(0,0,1),
        new Vector3(-1,0,1),
    };
    public static Action LightRightAction;
    public static Vector3?[] HeavyRightActionPath = {
        new Vector3(1,0,-1),
        new Vector3(1,0,0),
        new Vector3(1,0,1),
        new Vector3(0,0,1),
        new Vector3(-1,0,1),
    };
    public static Action HeavyRightAction;

    public static Vector3?[] LightUpRightActionPath = {
        new Vector3(1,1,1),
        new Vector3(0,0,1),
        new Vector3(-1,-1,1),
    };
    public static Action LightUpRightAction;
    public static Vector3?[] HeavyUpRightActionPath = {
        new Vector3(0,1,-1),
        new Vector3(1,1,0),
        new Vector3(1,1,1),
        new Vector3(0,0,1),
        new Vector3(-1,-1,1),
    };
    public static Action HeavyUpRightAction;
    #endregion
    
    public static Dictionary<ActionId, Dictionary<ActionType, Action>> ActionMap = new Dictionary<ActionId, Dictionary<ActionType, Action>>()
    {
        {
            ActionId.DownLeftAttack, new Dictionary<ActionType, Action>()
            { 
                { ActionType.LightAttack, new Action(LightDownLeftActionPath, ActionType.LightAttack) },
                { ActionType.HeavyAttack, new Action(HeavyDownLeftActionPath, ActionType.HeavyAttack) },
            }
        },
        {
            ActionId.LeftAttack, new Dictionary<ActionType, Action>()
            { 
                { ActionType.LightAttack, new Action(LightLeftActionPath, ActionType.LightAttack) },
                { ActionType.HeavyAttack, new Action(HeavyLeftActionPath, ActionType.HeavyAttack) },
            }
        },
        {
            ActionId.UpLeftAttack, new Dictionary<ActionType, Action>()
            { 
                { ActionType.LightAttack, new Action(LightUpLeftActionPath, ActionType.LightAttack) },
                { ActionType.HeavyAttack, new Action(HeavyUpLeftActionPath, ActionType.HeavyAttack) },
            }
        },
        {
            ActionId.DownAttack, new Dictionary<ActionType, Action>()
            { 
                { ActionType.LightAttack, new Action(LightDownActionPath, ActionType.LightAttack) },
                { ActionType.HeavyAttack, new Action(HeavyDownActionPath, ActionType.HeavyAttack) },
            }
        },
        {
            ActionId.CenterAttack, new Dictionary<ActionType, Action>()
            { 
                { ActionType.LightAttack, new Action(LightCenterActionPath, ActionType.LightAttack) },
                { ActionType.HeavyAttack, new Action(HeavyCenterActionPath, ActionType.HeavyAttack) },
            }
        },
        {
            ActionId.UpAttack, new Dictionary<ActionType, Action>()
            { 
                { ActionType.LightAttack, new Action(LightUpActionPath, ActionType.LightAttack) },
                { ActionType.HeavyAttack, new Action(HeavyUpActionPath, ActionType.HeavyAttack) },
            }
        },
        {
            ActionId.DownRightAttack, new Dictionary<ActionType, Action>()
            { 
                { ActionType.LightAttack, new Action(LightDownRightActionPath, ActionType.LightAttack) },
                { ActionType.HeavyAttack, new Action(HeavyDownRightActionPath, ActionType.HeavyAttack) },
            }
        },
        {
            ActionId.RightAttack, new Dictionary<ActionType, Action>()
            { 
                { ActionType.LightAttack, new Action(LightRightActionPath, ActionType.LightAttack) },
                { ActionType.HeavyAttack, new Action(HeavyRightActionPath, ActionType.HeavyAttack) },
            }
        },
        {
            ActionId.UpRightAttack, new Dictionary<ActionType, Action>()
            { 
                { ActionType.LightAttack, new Action(LightUpRightActionPath, ActionType.LightAttack) },
                { ActionType.HeavyAttack, new Action(HeavyUpRightActionPath, ActionType.HeavyAttack) },
            }
        },
    };
}
