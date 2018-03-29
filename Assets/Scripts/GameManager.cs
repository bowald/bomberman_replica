using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour {

	private bool gameIsRunning = false;
	public Tilemap tilemap;
    public List<Player> players;

	public Player playerprefab;
	// Use this for initialization
	void Start () {
        gameIsRunning = true;
		
		spawnPlayer(1);
        spawnPlayer(2);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("RestartGame")){
            foreach (var player in players)
            {
				Destroy(player.gameObject);
            }
			players.Clear();
            spawnPlayer(1);
            spawnPlayer(2);
		}
	}

	bool gameIsOver(){
		bool isOver = false;
		foreach (var player in players)
		{
            isOver = player.isAlive || isOver;
		}
		return isOver;
	}

	void spawnPlayer(int id){
        var cell = randomFreeTile();
		Player temp  = Instantiate(playerprefab, tilemap.GetCellCenterWorld(cell), Quaternion.identity);
		temp.tilemap = tilemap;
		temp.name = "Player_" + id;
		temp.initPlayer(id);
		players.Add(temp);
	}

	Vector3Int randomFreeTile() {
		for(int i = 0; i < 1000; i++){
			var cell = randomTile();
            if(!tilemap.HasTile(cell)) {
                return cell;
			} 
		}
		return new Vector3Int(0,0,0);
	}

	Vector3Int randomTile(){
        int x = Random.Range(-4, 4);
        int y = Random.Range(-4, 4);
        return new Vector3Int(x,y,0);
    }

	public List<Player> getPlayers(){
		return players;
	}
}
