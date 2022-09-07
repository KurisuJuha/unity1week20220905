using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCursor : MonoBehaviour
{
    public Vector2 direction;
    public GameObject Player;

    public static Vector2Int chunkPos;
    public static Vector2Int tilePos;
    public static Vector2 Pos;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            direction.x = Input.GetAxisRaw("Horizontal");
            direction.y = 0;
        }
        if (Input.GetAxisRaw("Vertical") != 0)
        {
            direction.y = Input.GetAxisRaw("Vertical");
            direction.x = 0;
        }

        transform.position = new Vector2(Mathf.Floor(Player.transform.position.x), Mathf.Floor(Player.transform.position.y)) + direction + new Vector2(0.5f, 0.5f);

        chunkPos = Utility.GetChunkPos(transform.position);
        tilePos = Utility.GetTilePos(transform.position);
        Pos = transform.position;
    }
}
