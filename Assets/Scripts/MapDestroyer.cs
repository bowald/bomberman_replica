using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapDestroyer : MonoBehaviour {

	public Tilemap tilemap;

	public Tile wallTile;
	public Tile destructibleTile;

	public Tile bombTile;
	public GameObject explosionPrefab;

	private GameManager gameManager;

	void Start(){
        gameManager = FindObjectOfType<GameManager>();
	}

	public void Explode(Vector2 worldPos)
	{
		Vector3Int originCell = tilemap.WorldToCell(worldPos);

		ExplodeCell(originCell);
		if (ExplodeCell(originCell + new Vector3Int(1, 0, 0)))
		{
			ExplodeCell(originCell + new Vector3Int(2, 0, 0));
		}
		
		if (ExplodeCell(originCell + new Vector3Int(0, 1, 0)))
		{
			ExplodeCell(originCell + new Vector3Int(0, 2, 0));
		}
		
		if (ExplodeCell(originCell + new Vector3Int(-1, 0, 0)))
		{
			ExplodeCell(originCell + new Vector3Int(-2, 0, 0));
		}
		
		if (ExplodeCell(originCell + new Vector3Int(0, -1, 0)))
		{
			ExplodeCell(originCell + new Vector3Int(0, -2, 0));
		}

	}

	bool ExplodeCell (Vector3Int cell)
	{
		Tile tile = tilemap.GetTile<Tile>(cell);

		if (tile == wallTile)
		{
			return false;
		}

        Vector3 pos = tilemap.GetCellCenterWorld(cell);
        tilemap.SetTile(cell, null);

        if(tile == bombTile){
			Explode(pos);
		}

		var explosion = Instantiate(explosionPrefab, pos, Quaternion.identity);

		foreach (var player in gameManager.getPlayers()){
			if(player.getTileCord() == cell){
				player.kill();
			}
		}
        Destroy(explosion, explosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        return true;
	}

}
