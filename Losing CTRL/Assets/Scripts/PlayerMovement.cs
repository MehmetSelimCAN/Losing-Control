using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour {

    private Tilemap blocks;
    private Tilemap lavaFloor;
    private Tile stoneTile;
    private Tile lavaTile;
    public List<Vector3Int> lavaLocations;

    private void Awake() {
        blocks = GameObject.Find("blocks").GetComponent<Tilemap>();
        lavaFloor = GameObject.Find("Lava").GetComponent<Tilemap>();
        stoneTile = Resources.Load<Tile>("Tiles/StoneTile");
        lavaTile = Resources.Load<Tile>("Tiles/LavaTile");
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            Move(Vector3.left);
        }

        else if (Input.GetKeyDown(KeyCode.D)) {
            Move(Vector3.right);
        }

        else if (Input.GetKeyDown(KeyCode.W)) {
            Move(Vector3.up);
        }

        else if (Input.GetKeyDown(KeyCode.S)) {
            Move(Vector3.down);
        }
    }


    private void Move(Vector3 direction) {
        if (blocks.GetTile(Vector3Int.FloorToInt(transform.position + direction)) == stoneTile &&
            blocks.GetTile(Vector3Int.FloorToInt(transform.position)) == stoneTile) {
            transform.position += direction;
            LavaSpread();
        }
    }

    private void LavaSpread() {
        lavaLocations = new List<Vector3Int>();

        foreach (var pos in lavaFloor.cellBounds.allPositionsWithin) {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            if (lavaFloor.HasTile(localPlace)) {
                lavaLocations.Add(localPlace);
            }
        }

        for (int i = 0; i < lavaLocations.Count; i++) {
            if (blocks.GetTile(lavaLocations[i] + Vector3Int.right) == stoneTile) {
                lavaFloor.SetTile(lavaLocations[i] + Vector3Int.right, lavaTile);
            }
            if (blocks.GetTile(lavaLocations[i] + Vector3Int.left) == stoneTile) {
                lavaFloor.SetTile(lavaLocations[i] + Vector3Int.left, lavaTile);
            }
            if (blocks.GetTile(lavaLocations[i] + Vector3Int.up) == stoneTile) {
                lavaFloor.SetTile(lavaLocations[i] + Vector3Int.up, lavaTile);
            }
            if (blocks.GetTile(lavaLocations[i] + Vector3Int.down) == stoneTile) {
                lavaFloor.SetTile(lavaLocations[i] + Vector3Int.down, lavaTile);
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Finish") {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (collision.tag == "Lava") {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
