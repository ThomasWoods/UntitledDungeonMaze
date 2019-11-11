using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    //tileSet;
    public int floorWidth;
    public int floorDepth;
    public int roomSize;

    public DungeonBaseController m_DungeonBase;

    int[][][,] roomBlueprints;

    GameObject[][] wallTypes = new GameObject[6][];
    GameObject[] gatheringTiles = new GameObject[4];
    DungeonCard dungeonCard;

    int[,] floorAtlas;
    int[,] floorTileMap;
    int[][,] currentRoomType;
    int[,] chosenRoomBlueprint;

    private int xCoord;
    private int zCoord;
    private int nextXCoord;
    private int nextZCoord;
    private int startXCoord;
    private int endXCoord;
    private int dir;
    private int lastDir;
    private int tileMapWidth;
    private int tileMapDepth;

    // Use this for initialization
    private void Awake()
    {
        m_DungeonBase = GetComponentInParent<DungeonBaseController>();
        roomBlueprints = RoomBlueprints.GetRoomBlueprints();
    }

    public void ResetFloorGenerator()
    {
        floorAtlas = null;
        floorTileMap = null;
        currentRoomType = null;
        chosenRoomBlueprint = null;

        xCoord = 0;
        zCoord = 0;
        nextXCoord = 0;
        nextZCoord = 0;
        startXCoord = 0;
        endXCoord = 0;
        dir = 0;
        lastDir = 0;
        tileMapWidth = 0;
        tileMapDepth = 0;
    }

    public void GenerateDungeon()
    {
        startXCoord = Random.Range(0, floorWidth);
        floorAtlas = new int[floorWidth, floorDepth];
        tileMapWidth = floorWidth * roomSize + 1;
        tileMapDepth = floorDepth * roomSize + 1;
        floorTileMap = new int[tileMapWidth, tileMapDepth];

        Debug.Log("StartX: " + startXCoord);
        Debug.Log("StartRoomID: " + floorAtlas[startXCoord, 0]);

        GenerateDungeonFloorAtlas();
        GenerateFloorTileMap();
        BuildDungeon();
    }

    public void CacheDungeonCard()
    {
        dungeonCard = DungeonManager.instance.dungeonCard;

        if (dungeonCard != null)
        {
            wallTypes[0] = dungeonCard.pillarTiles;
            wallTypes[1] = dungeonCard.endTiles;
            wallTypes[2] = dungeonCard.lineTiles;
            wallTypes[3] = dungeonCard.cornerTiles;
            wallTypes[4] = dungeonCard.tTiles;
            wallTypes[5] = dungeonCard.crossTiles;
        }
        else
            Debug.LogWarning("Tried to build a dungeon without a dungeon card!");
    }

    void GenerateDungeonFloorAtlas()
    {
        bool dungeonIsDone = false;

        xCoord = startXCoord;
        lastDir = 2;

        while (dungeonIsDone == false)
        {
            GenerateDirection();

            if (dir == 2)
            {
                if (zCoord == floorDepth - 1)
                {
                    dungeonIsDone = true;
                    endXCoord = xCoord;
                }

                if (lastDir == 2)
                {
                    floorAtlas[xCoord, zCoord] = 4;
                }
                else
                {
                    floorAtlas[xCoord, zCoord] = 3;
                }
            }
            else
            {
                if (lastDir == 2)
                {
                    floorAtlas[xCoord, zCoord] = 2;
                }
                else
                {
                    // If it is a LR room, flip a coin...
                    int coinToss = Random.Range(0, 2);

                    if (coinToss == 0)
                    {
                        // ...If tails generate a normal LR room
                        floorAtlas[xCoord, zCoord] = 1;
                    }
                    else
                    {
                        //...If heads generate a random non-special room
                        floorAtlas[xCoord, zCoord] = Random.Range(1, 5);
                    }
                }
            }

            if (!dungeonIsDone)
            {
                xCoord = nextXCoord;
                zCoord = nextZCoord;
                lastDir = dir;
            }
        }
    }

    void GenerateDirection()
    {
        dir = Random.Range(0, 3);

        // Force the path inwards if it hits the edge of the floor
        if (dir == 0 && xCoord == 0)
        {
            dir = 1;
        }

        if (dir == 1 && xCoord == floorWidth - 1)
        {
            dir = 0;
        }

        // Force the path upwards if it tries to overwrite itself
        if (dir == 0 && lastDir == 1 || dir == 1 && lastDir == 0)
        {
            dir = 2;
        }

        if (dir == 0)
        {
            //Left
            nextXCoord = xCoord - 1;
            nextZCoord = zCoord;

        }
        else if (dir == 1)
        {
            //Right
            nextXCoord = xCoord + 1;
            nextZCoord = zCoord;
        }
        else
        {
            //Up
            nextXCoord = xCoord;
            nextZCoord = zCoord + 1;
        }
    }

    void GenerateFloorTileMap()
    {
        for (int zz = 0; zz < floorDepth; zz++)
        {
            for (int xx = 0; xx < floorWidth; xx++)
            {
                int currentRoom = floorAtlas[xx, zz];

                currentRoomType = roomBlueprints[currentRoom];
                chosenRoomBlueprint = currentRoomType[Random.Range(0, currentRoomType.Length)];

                // Fill out the tilemap
                for (int zzz = 0; zzz < roomSize; zzz++)
                {
                    for (int xxx = 0; xxx < roomSize; xxx++)
                    {
                        floorTileMap[xx * roomSize + xxx, zz * roomSize + zzz] = chosenRoomBlueprint[xxx, zzz];
                    }
                }

            }
        }

        // Close up sides of dungeon
        for (int zz = 0; zz < tileMapDepth; zz++)
        {
            floorTileMap[0, zz] = 1;
            floorTileMap[tileMapWidth - 1, zz] = 1;
        }

        for (int xx = 0; xx < tileMapWidth; xx++)
        {
            floorTileMap[xx, 0] = 1;
            floorTileMap[xx, tileMapDepth - 1] = 1;
        }

        // Entrance tile
        floorTileMap[startXCoord * roomSize + 7, 1] = 90;

        // Exit tile

        if(DungeonBaseController.instance.floorNumber == dungeonCard.numberOfFloors)
            floorTileMap[endXCoord * roomSize + 7, floorDepth * roomSize - 1] = 22;
        else
            floorTileMap[endXCoord * roomSize + 7, floorDepth * roomSize - 1] = 91;
    }

    void BuildDungeon()
    {
        for (int zz = 0; zz < tileMapDepth; zz++)
        {
            for (int xx = 0; xx < tileMapWidth; xx++)
            {
                Vector3 tilePos = Vector3.zero;
                tilePos.x = xx;
                tilePos.z = zz;

                switch (floorTileMap[xx, zz])
                {
                    case 0:
                        SpawnOpenTile(tilePos);
                        break;

                    case 1:
                        SpawnWall(xx, zz, tilePos);
                        break;

                    case 20:
                        SpawnEnemyTile(tilePos, 0);
                        break;

                    case 21:
                        SpawnEnemyTile(tilePos, 1);
                        break;

                    case 22:
                        SpawnEnemyTile(tilePos, 2);
                        break;

                    case 30:
                        SpawnTrapTile(tilePos);
                        break;

                    case 90:
                        SpawnEntranceTile(tilePos);
                        break;

                    case 91:
                        SpawnExitTile(tilePos);
                        break;
                }
            }
        }
    }

    void SpawnEntranceTile(Vector3 tilePos)
    {
        GameObject instance = Instantiate(dungeonCard.entranceTile, transform.position, Quaternion.identity);
        instance.transform.position = tilePos;
        instance.transform.parent = DungeonManager.instance.tileParentObj.transform;
        m_DungeonBase.entranceTile = instance;
    }

    void SpawnExitTile(Vector3 tilePos)
    {
        GameObject instance = Instantiate(dungeonCard.exitTile, transform.position, Quaternion.identity);
        //instance.GetComponent<ExitTileInteracton>().m_DungeonFloor = gameObject;
        instance.transform.position = tilePos;
        instance.transform.parent = DungeonManager.instance.tileParentObj.transform;
        m_DungeonBase.exitTile = instance;
    }

    void SpawnOpenTile(Vector3 tilePos)
    {
        GameObject instance = Instantiate(dungeonCard.openTiles[Random.Range(0, dungeonCard.openTiles.Length)], transform.position, Quaternion.identity);
        instance.transform.position = tilePos;
        instance.transform.parent = DungeonManager.instance.tileParentObj.transform;
    }
    
    void SpawnWall(int xx, int zz, Vector3 tilePos)
    {
        int tileID = 0;
        int wallType = 0;
        GameObject chosenTile;
        Vector3 tileRot = Vector3.zero;

        tileID += checkTileForWall(xx - 1, zz, 1);
        tileID += checkTileForWall(xx + 1, zz, 2);
        tileID += checkTileForWall(xx, zz - 1, 4);
        tileID += checkTileForWall(xx, zz + 1, 8);

        switch (tileID)
        {
            case 0:
                wallType = 0;
                break;
            case 1:
                wallType = 1;
                tileRot.y = 90;
                break;
            case 2:
                wallType = 1;
                tileRot.y = 270;
                break;
            case 3:
                wallType = 2;
                tileRot.y = 90;
                break;
            case 4:
                wallType = 1;
                break;
            case 5:
                wallType = 3;
                tileRot.y = 180;
                break;
            case 6:
                wallType = 3;
                tileRot.y = 90;
                break;
            case 7:
                wallType = 4;
                tileRot.y = 90;
                break;
            case 8:
                wallType = 1;
                tileRot.y = 180;
                break;
            case 9:
                wallType = 3;
                tileRot.y = 270;
                break;
            case 10:
                wallType = 3;
                break;
            case 11:
                wallType = 4;
                tileRot.y = 270;
                break;
            case 12:
                wallType = 2;
                break;
            case 13:
                wallType = 4;
                tileRot.y = 180;
                break;
            case 14:
                wallType = 4;
                break;
            case 15:
                wallType = 5;
                break;
        }
        
        chosenTile = wallTypes[wallType][Random.Range(0, wallTypes[wallType].Length)];

        GameObject instance = Instantiate(chosenTile, chosenTile.transform.position, Quaternion.identity);

        instance.transform.position = tilePos;
        instance.transform.rotation = Quaternion.Euler(tileRot);
        instance.transform.parent = DungeonManager.instance.tileParentObj.transform;
    }

    void SpawnEnemyTile(Vector3 tilePos, int type)
    {
        GameObject instance = Instantiate(dungeonCard.openTiles[Random.Range(0, dungeonCard.openTiles.Length)], transform.position, Quaternion.identity);
        instance.transform.position = tilePos;
        instance.transform.parent = DungeonManager.instance.tileParentObj.transform;

        
        GameObject enemy = null;

        switch (type)
        {
            case 0:
                enemy = Instantiate(dungeonCard.firstEnemy, instance.transform.position, Quaternion.identity);
                enemy.transform.parent = DungeonManager.instance.enemyParentObj.transform;
                break;

            case 1:
                enemy = Instantiate(dungeonCard.secondEnemy, instance.transform.position, Quaternion.identity);
                enemy.transform.parent = DungeonManager.instance.enemyParentObj.transform;
                break;

            case 2:
                enemy = Instantiate(dungeonCard.bossEnemy, instance.transform.position, Quaternion.identity);
                enemy.transform.parent = DungeonManager.instance.enemyParentObj.transform;
                break;
        }
        

    }

    void SpawnTrapTile(Vector3 tilePos)
    {
        GameObject instance = Instantiate(dungeonCard.trapTiles[Random.Range(0, dungeonCard.trapTiles.Length)], transform.position, Quaternion.identity);
        instance.transform.position = tilePos;
        instance.transform.parent = DungeonManager.instance.tileParentObj.transform;
    }

    int checkTileForWall(int xx, int zz, int dir)
    {
        if (xx >= 0 && xx < tileMapWidth && zz >= 0 && zz < tileMapDepth)
        {
            if (floorTileMap[xx, zz] == 1)
            {
                return dir;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return 0;
        }
    }
}
