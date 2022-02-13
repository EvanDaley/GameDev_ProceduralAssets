using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionAnimator : MonoBehaviour
{
    public Color MatrixCellStandbyColor = Color.grey;
    public Color MatrixCellActiveColor = Color.black;
    public Color MatrixCellActivePathColor = Color.yellow;
    
    public bool ShowGrid = false;
    private bool isGridShowing = false;

    public GameObject ActiveMatrixCellPrefab;

    private int width = 3;
    private int height = 3;
    private int depth = 3;
    private GameObject[,,] visualGridCells;
    private Vector3?[] actionPath;
    public Vector3?[] ActionPath
    {
        get { return actionPath; }
        set { if (!isAttacking) { actionPath = value; StartCoroutine(AttackAnimation()); } }
    }
    private int currentActionPathIndex = 0;
    private bool isAttacking = false;
    private bool canAttack = true;
    public bool CanAttack
    {
        get { return canAttack; }
        set { canAttack = value; }
    }
    private float scale = 1f;



    private PlayerAttributes playerAttributes;

    // Start is called before the first frame update
    void Start()
    {
        playerAttributes = gameObject.GetComponent<PlayerAttributes>();
        visualGridCells = new GameObject[width, height, depth];
        CreateActionMatrix();
        // ActionDefinitinos.Action action = ActionDefinitions.ActionMap[ActionDefinitions.ActionId.LeftAttack][ActionDefinitions.ActionType.LightAttack];
        // ShowActionPath();
        // StartCoroutine(AttackAnimation());
    }

    private void CreateActionMatrix() {
        for (int z = 0; z < depth; z++) {
            for (int x = 0; x < width; x++) {
                for(int y = 0; y < height; y++) {
                    GameObject cell = GetNewCell(MatrixCellStandbyColor);
                    // cell.name = $"Player Matrix ({x-1},{y-1},{z-1})";
                    SetVisualGridCell(cell, x, y, z);
                }
            }
        }

        Destroy(visualGridCells[1,1,1]);
    }

    private void DestroyActionMatrix() {
        for (int z = 0; z < depth; z++) {
            for (int x = 0; x < width; x++) {
                for(int y = 0; y < height; y++) {
                    Destroy(visualGridCells[x, y, z]);
                    visualGridCells[x, y, z] = null;
                }
            }
        }
    }

    private GameObject GetNewCell(Color color, bool isActive = false) {
        GameObject cell;
        if (!isActive) {
            cell = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cell.GetComponent<BoxCollider>().enabled = false;
            cell.GetComponent<Renderer>().enabled = ShowGrid;

        } else {
            cell = Instantiate(ActiveMatrixCellPrefab);
            cell.tag = Constants.WeaponTag;
        }
        Renderer cellRenderer = cell.GetComponent<Renderer>();
        cellRenderer.material.color = color;

        return cell;
    }

    // private GameObject GetPlayerCell(Color color) {
    //     GameObject cell = GetNewCell(color);
    //     cell.tag = Constants.PlayerTag;
    //     BoxCollider collider = cell.GetComponent<BoxCollider>();
    //     collider.enabled = true;
    //     Rigidbody rb = cell.AddComponent<Rigidbody>();
    //     rb.useGravity = false;
    //     rb.mass = 0f;
    //     rb.angularDrag = 0f;

    //     return cell;
    // }

    private void SetVisualGridCell(GameObject cell, int x_pos, int y_pos, int z_pos) {
        cell.transform.SetParent(transform);
        cell.transform.localPosition = new Vector3(x_pos-scale, y_pos-scale, z_pos-scale);
        cell.transform.localScale = new Vector3(scale*.95f, scale*.95f, scale*.95f);
        cell.transform.rotation = transform.rotation;
        visualGridCells[x_pos, y_pos, z_pos] = cell;
    }

    private void ReplaceVisualGridCell(GameObject cell, int x_pos, int y_pos, int z_pos) {
        Destroy(visualGridCells[x_pos, y_pos, z_pos]);
        SetVisualGridCell(cell, x_pos, y_pos, z_pos);
    }

    private void ShowActionPath() {
        for (int i = 0; i < actionPath.Length; i++) {
            Vector3? actionCell = actionPath[i];
            if (actionCell != null) {
                Vector3 ac = (Vector3)actionCell;
                GameObject pathCell = GetNewCell(MatrixCellActivePathColor);
                ReplaceVisualGridCell(pathCell, (int)ac.x+1, (int)ac.y+1, (int)ac.z+1);
            }
        }
    }

    private IEnumerator AttackAnimation() {
        isAttacking = true;
        if (ShowGrid && actionPath != null) {
            ShowActionPath();
        }
        for (currentActionPathIndex = 0; currentActionPathIndex <= actionPath.Length && CanAttack; currentActionPathIndex++) {
            if (currentActionPathIndex < actionPath.Length) {
                Vector3? currentActionCell = actionPath[currentActionPathIndex];
                if (currentActionCell != null) {
                    Vector3 ac = (Vector3)currentActionCell;
                    GameObject newCell  = GetNewCell(MatrixCellActiveColor, true);
                    ReplaceVisualGridCell(newCell, (int)ac.x+1, (int)ac.y+1, (int)ac.z+1);
                }
            }
        
            if (currentActionPathIndex - 1 >= 0) {
                Vector3? prevActionCell = actionPath[currentActionPathIndex-1];
                if (prevActionCell != null) {
                    Vector3 ac = (Vector3)prevActionCell;
                    GameObject pathCell = GetNewCell(MatrixCellStandbyColor);
                    ReplaceVisualGridCell(pathCell, (int)ac.x+1, (int)ac.y+1, (int)ac.z+1);
                }
            }

            if (currentActionPathIndex + 1 <= actionPath.Length) {
                yield return new WaitForSeconds(playerAttributes.AttackStepSpeed);
            }

        }
        PlayerControlScript playerControlScript = gameObject.GetComponent<PlayerControlScript>();
        if (playerControlScript) {
            playerControlScript.ActionId = null;
        }
        isAttacking = false;
        CanAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if ((!isGridShowing && ShowGrid) || (isGridShowing && !ShowGrid)) {
            DestroyActionMatrix();
            CreateActionMatrix();
            isGridShowing = ShowGrid;
        }
    }
}
