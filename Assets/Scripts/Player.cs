using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    public GameObject bombPrefab;
    private Vector2Int moveDir = new Vector2Int(0,0);
    public float moveTime = .1f;
    public Tilemap tilemap;
    public Tile bombtile;
    private float inverseMovemenTime;
    public int maxBombs = 3;
    private Rigidbody2D rigidbody2D;
    private BoxCollider2D collider2D;
    private SpriteRenderer sr;
    public bool isAlive = true;
    private string bombInput;
    private string horizontalInput;
    private string verticalInput;

    private IEnumerator smoothMovementRef;

    // Use this for initialization
    void Awake()
    {
        collider2D = GetComponent<BoxCollider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        inverseMovemenTime = 1.0f / moveTime;
    }

    void Update()
    {
        int ydir = (int)Input.GetAxisRaw(verticalInput);
        int xdir = (int)Input.GetAxisRaw(horizontalInput);
        if (ydir != 0)
        {
            xdir = 0;
        }

        if (ydir != 0 || xdir != 0)
        {
            tryToMove(xdir, ydir);
        }

        if (Input.GetButton(bombInput))
        {
            tryToPlaceBomb();
        }
    }

    public void initPlayer(int id){
        bombInput = "Bomb_P" + id;
        horizontalInput = "Horizontal_P" + id;
        verticalInput = "Vertical_P" + id;

        if (id == 1){
            sr.color = new Color(1, 1, 0);
        }
        else {
            sr.color = new Color(0, 0, 1);
        }
    }

    public Vector3Int getTileCord()
    {
        return tilemap.WorldToCell(transform.position);
    }

    void tryToMove(int xdir, int ydir)
    {
        if (!isAlive) return;

        Vector3Int originCell = getTileCord();
        Vector3Int endCell = originCell + new Vector3Int(xdir, ydir, 0);
        Tile tile = tilemap.GetTile<Tile>(endCell);

        if (tile) return;
        Vector2 end = tilemap.GetCellCenterWorld(endCell);

        if(moveDir.x != xdir || moveDir.y != ydir){
            moveDir = new Vector2Int(xdir, ydir);
            if (smoothMovementRef != null) StopCoroutine(smoothMovementRef);
            smoothMovementRef = SmoothMovement(end);
            StartCoroutine(smoothMovementRef);
        }
    }

    void tryToPlaceBomb()
    {
        if (!isAlive) return;
        Vector3Int originCell = getTileCord();
        Tile tile = tilemap.GetTile<Tile>(originCell);
        if (tile) return;
        tile = bombtile;
        tile.gameObject = bombPrefab;
        tilemap.SetTile(originCell, tile);
    }

    IEnumerator SmoothMovement(Vector2 end)
    {
        while ((rigidbody2D.position - end).sqrMagnitude > float.Epsilon)
        {
            Vector2 newPosition = Vector2.MoveTowards(rigidbody2D.position, end, inverseMovemenTime * Time.deltaTime);
            rigidbody2D.MovePosition(newPosition);
            yield return null;
        }
        moveDir = Vector2Int.zero;
    }

    public void kill()
    {
        isAlive = false;
        sr.color = new Color(1,0,0);
    }
}
