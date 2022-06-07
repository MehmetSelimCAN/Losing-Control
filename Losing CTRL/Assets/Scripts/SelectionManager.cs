using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SelectionManager : MonoBehaviour {

    private Tilemap blocks;
    private Tilemap ghostBlocks;
    private LineRenderer lineRenderer;
    private Vector3 firstTilePosition;
    private Vector3 currentTilePosition;
    private Vector3 tempTilePosition;
    private Vector3 direction;
    private Tile nullTile;
    private Tile redNullTile;

    [SerializeField] private Vector2Int levelMaxBoundary;
    [SerializeField] private Vector2Int levelMinBoundary;
    private bool hasSquareDrawn;
    private BoundsInt squareArea;
    private BoundsInt copiedSquareArea;
    private TileBase[] tileArray;
    private Vector3Int[] tilePositionArray;

    private void Awake() {
        blocks = GameObject.Find("blocks").GetComponent<Tilemap>();
        ghostBlocks = GameObject.Find("ghostBlocks").GetComponent<Tilemap>();
        nullTile = Resources.Load<Tile>("Tiles/NullTile");
        redNullTile = Resources.Load<Tile>("Tiles/RedNullTile");

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 4;
    }

    public void Update() {
        if (Input.GetMouseButtonDown(0)) {
            firstTilePosition = GetTileMapCoordinate();
            Arrange1x1SquareBoundaries();
        }

        if (Input.GetMouseButton(0)) {
            currentTilePosition = GetTileMapCoordinate();

            if (tempTilePosition != currentTilePosition) {
                hasSquareDrawn = false;
            }

            if (currentTilePosition != firstTilePosition && !hasSquareDrawn) {
                hasSquareDrawn = true;
                tempTilePosition = currentTilePosition;

                direction = currentTilePosition - firstTilePosition;
                /*lineRenderer.SetPosition(0, firstTilePosition + new Vector3(Mathf.Sign(direction.x) * -0.5f, direction.y + Mathf.Sign(direction.y) * 0.5f));
                lineRenderer.SetPosition(1, firstTilePosition + new Vector3(-1 * Mathf.Sign(direction.x) * 0.5f, -1 * Mathf.Sign(direction.y) * 0.5f));
                lineRenderer.SetPosition(2, firstTilePosition + new Vector3(direction.x + Mathf.Sign(direction.x) * 0.5f, Mathf.Sign(direction.y) * -0.5f));
                lineRenderer.SetPosition(3, currentTilePosition + new Vector3(Mathf.Sign(direction.x) * 0.5f, Mathf.Sign(direction.y) * 0.5f));*/

                #region Arrange Square Boundaries
                squareArea.xMax = (int)Mathf.Max(firstTilePosition.x + Mathf.Sign(direction.x) * -0.5f,
                                              firstTilePosition.x + -1 * Mathf.Sign(direction.x) * 0.5f,
                                              firstTilePosition.x + direction.x + Mathf.Sign(direction.x) * 0.5f,
                                              currentTilePosition.x + Mathf.Sign(direction.x) * 0.5f);

                squareArea.xMin = (int)Mathf.Min(firstTilePosition.x + Mathf.Sign(direction.x) * -0.5f,
                                              firstTilePosition.x + -1 * Mathf.Sign(direction.x) * 0.5f,
                                              firstTilePosition.x + direction.x + Mathf.Sign(direction.x) * 0.5f,
                                              currentTilePosition.x + Mathf.Sign(direction.x) * 0.5f);

                squareArea.yMax = (int)Mathf.Max(firstTilePosition.y + direction.y + Mathf.Sign(direction.y) * 0.5f,
                                              firstTilePosition.y + -1 * Mathf.Sign(direction.y) * 0.5f,
                                              firstTilePosition.y + Mathf.Sign(direction.y) * -0.5f,
                                              currentTilePosition.y + Mathf.Sign(direction.y) * 0.5f);

                squareArea.yMin = (int)Mathf.Min(firstTilePosition.y + direction.y + Mathf.Sign(direction.y) * 0.5f,
                                              firstTilePosition.y + -1 * Mathf.Sign(direction.y) * 0.5f,
                                              firstTilePosition.y + Mathf.Sign(direction.y) * -0.5f,
                                              currentTilePosition.y + Mathf.Sign(direction.y) * 0.5f);

                squareArea.zMin = 0;
                squareArea.zMax = 1;

                squareArea.xMax = Mathf.Min(squareArea.xMax , levelMaxBoundary.x);
                squareArea.yMax = Mathf.Min(squareArea.yMax , levelMaxBoundary.y);
                squareArea.xMin = Mathf.Max(squareArea.xMin , levelMinBoundary.x);
                squareArea.yMin = Mathf.Max(squareArea.yMin , levelMinBoundary.y);
                #endregion

                DrawSquare();
                ArrangeGhostBlocksPositions();

                #region Point calculation
                /*if (direction.x >= 0) {
                   if (direction.y >= 0) {     //direction = +x, +y
                       lineRenderer.SetPosition(0, firstTilePosition + new Vector3(-0.5f, direction.y + 0.5f));
                       lineRenderer.SetPosition(1, firstTilePosition + new Vector3(-0.5f, -0.5f));
                       lineRenderer.SetPosition(2, firstTilePosition + new Vector3(direction.x + 0.5f, -0.5f));
                       lineRenderer.SetPosition(3, currentTilePosition + new Vector3(0.5f, 0.5f));
                   }

                   else {                      //direction = +x, -y
                       lineRenderer.SetPosition(0, firstTilePosition + new Vector3(-0.5f, direction.y - 0.5f));
                       lineRenderer.SetPosition(1, firstTilePosition + new Vector3(-0.5f, 0.5f));
                       lineRenderer.SetPosition(2, firstTilePosition + new Vector3(direction.x + 0.5f, 0.5f));
                       lineRenderer.SetPosition(3, currentTilePosition + new Vector3(0.5f, -0.5f));
                   }
               }
               else {
                   if (direction.y >= 0) {     //direction = -x, +y
                       lineRenderer.SetPosition(0, firstTilePosition + new Vector3(0.5f, direction.y + 0.5f));
                       lineRenderer.SetPosition(1, firstTilePosition + new Vector3(0.5f, -0.5f));
                       lineRenderer.SetPosition(2, firstTilePosition + new Vector3(direction.x - 0.5f, -0.5f));
                       lineRenderer.SetPosition(3, currentTilePosition + new Vector3(-0.5f, 0.5f));
                   }

                   else {                      //direction = -x, -y
                       lineRenderer.SetPosition(0, firstTilePosition + new Vector3(0.5f, direction.y - 0.5f));
                       lineRenderer.SetPosition(1, firstTilePosition + new Vector3(0.5f, 0.5f));
                       lineRenderer.SetPosition(2, firstTilePosition + new Vector3(direction.x - 0.5f, 0.5f));
                       lineRenderer.SetPosition(3, currentTilePosition + new Vector3(-0.5f, -0.5f));
                   }
               }*/
                #endregion
            }
            else if (currentTilePosition == firstTilePosition && !hasSquareDrawn) {  //If mouse position comes back to first position
                tempTilePosition = currentTilePosition;
                hasSquareDrawn = true;
                Arrange1x1SquareBoundaries();
            }
        }

        if (Input.GetKey(KeyCode.LeftControl)) {
            if (tileArray != null) {

                if (tileArray.Length > 0 && squareArea.size.x > 0 && squareArea.size.y > 0) {
                    ghostBlocks.SetTiles(tilePositionArray, tileArray);
                }

                if (Input.GetKeyDown(KeyCode.C)) {
                    if (UIManager.remainingCTRLCount > 0) {
                        UIManager.UseCTRL();

                        copiedSquareArea = squareArea;
                        tileArray = blocks.GetTilesBlock(squareArea);

                        tilePositionArray = new Vector3Int[tileArray.Length];

                        for (int xOffset = 0, yOffset = 0, i = 0; i < tileArray.Length; i++) {
                            tilePositionArray[i] = new Vector3Int(squareArea.xMin + xOffset, squareArea.yMin + yOffset, 0);

                            xOffset++;
                            if (squareArea.xMin + xOffset > squareArea.xMax - 1) {
                                xOffset = 0;
                                yOffset++;
                            }
                        }
                    }
                    else {
                        UIManager.OutOfControl();
                    }
                }

                if (Input.GetKeyDown(KeyCode.V)) {
                    if (UIManager.remainingCTRLCount > 0) {
                        if (tileArray.Length > 0 && squareArea.size.x > 0 && squareArea.size.y > 0 && ghostBlocks.color == Color.yellow) {
                            UIManager.UseCTRL();
                            tileArray = blocks.GetTilesBlock(copiedSquareArea);
                            blocks.SetTiles(tilePositionArray, tileArray);
                        }
                    }

                    else {
                        UIManager.OutOfControl();
                    }
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftControl)) {
            ghostBlocks.ClearAllTiles();
        }
    }

    private void Arrange1x1SquareBoundaries() {
        squareArea.xMax = Mathf.Min((int)(firstTilePosition.x + 0.5f), levelMaxBoundary.x);
        squareArea.yMax = Mathf.Min((int)(firstTilePosition.y + 0.5f), levelMaxBoundary.y);
        squareArea.xMin = Mathf.Max((int)(firstTilePosition.x - 0.5f), levelMinBoundary.x);
        squareArea.yMin = Mathf.Max((int)(firstTilePosition.y - 0.5f), levelMinBoundary.y);
        squareArea.zMax = 1;
        squareArea.zMin = 0;

        DrawSquare();
    }

    private void DrawSquare() {
        Vector3 pointA = new Vector3(squareArea.xMax, squareArea.yMax);
        Vector3 pointB = new Vector3(squareArea.xMin, squareArea.yMax);
        Vector3 pointC = new Vector3(squareArea.xMin, squareArea.yMin);
        Vector3 pointD = new Vector3(squareArea.xMax, squareArea.yMin);

        lineRenderer.SetPosition(0, pointA);
        lineRenderer.SetPosition(1, pointB);
        lineRenderer.SetPosition(2, pointC);
        lineRenderer.SetPosition(3, pointD);

        if (pointA.Equals(pointB) || pointA.Equals(pointC) || pointA.Equals(pointD) || pointB.Equals(pointC) || pointB.Equals(pointD) || pointC.Equals(pointD)) {
            lineRenderer.SetPosition(0, Vector3.zero);
            lineRenderer.SetPosition(1, Vector3.zero);
            lineRenderer.SetPosition(2, Vector3.zero);
            lineRenderer.SetPosition(3, Vector3.zero);
        }

        ArrangeGhostBlocksPositions();
    }

    private void ArrangeGhostBlocksPositions() {
        ghostBlocks.ClearAllTiles();
        ghostBlocks.color = Color.yellow;
        tileArray = blocks.GetTilesBlock(copiedSquareArea);

        if (tileArray.Length > 0) {
            tilePositionArray = new Vector3Int[tileArray.Length];
            for (int xOffset = 0, yOffset = 0, i = 0; i < tileArray.Length; i++) {
                tilePositionArray[i] = new Vector3Int(squareArea.xMin + xOffset, squareArea.yMax - copiedSquareArea.size.y + yOffset, 0);

                xOffset++;
                if (xOffset > copiedSquareArea.size.x - 1) {
                    xOffset = 0;
                    yOffset++;
                }
            }

            for (int i = 0; i < tilePositionArray.Length; i++) {
                if (tilePositionArray[i].x >= levelMaxBoundary.x || tilePositionArray[i].x < levelMinBoundary.x ||
                    tilePositionArray[i].y >= levelMaxBoundary.y || tilePositionArray[i].y < levelMinBoundary.y) {
                    ghostBlocks.color = Color.red;
                    break;
                }
            }

            for (int i = 0; i < tilePositionArray.Length; i++) {
                if (blocks.GetTile(tilePositionArray[i]) != nullTile && tileArray[i] == nullTile) {
                    tileArray[i] = redNullTile;
                }
            }
        }
    }

    private Vector3 GetTileMapCoordinate() {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 coordinate = blocks.GetCellCenterWorld(blocks.WorldToCell(mouseWorldPos));
        return coordinate;
    }
}
