using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour{

    private MazeCell[,] cells;
    private int sizeX, sizeZ;

    private int curX, curZ;
    private bool complete;
	private float cellSize = 1f;
	private int enemyNum = 0;

	//public GameObject enemyPrefab;
	private Enemy[] enemies;

	public MazeGenerator(MazeCell[,] cells)
    {
        this.cells = cells;
		//enemyPrefab = Resources.Load ("Dragon") as GameObject;
        sizeX = cells.GetLength(0);
        sizeZ = cells.GetLength(1);
        curX = 0;
        curZ = 0;
        complete = false;
    }

	public Enemy[] CreateMaze()
    {
		enemies = new Enemy[sizeX * sizeZ];
		if (enemies == null) {
			Debug.Log ("Enemies is null!");
		} else {
			Debug.Log ("Enemies is not null!");
		}
		KillAndHunt();
		return enemies;
    }

    void KillAndHunt()
    {
        cells[curX, curZ].SetVisit(true);
        while (!complete)
        {
            Kill();
            Hunt();
        }
    }

    void Kill()
    {
        Debug.Log("kill start");
		int enemyDir = 0;
        while (!IsDeadend(curX, curZ))
        {
            Random.seed = System.Guid.NewGuid().GetHashCode();
			int dir = Random.Range(1, 5);

            //north: 1, west: 2, south: 3, east: 4
            Debug.Log("dir = " + dir);
            Debug.Log("( " + curX + ", " + curZ + " )");
            switch (dir)
            {
                case 1:
                    if(IsAvailable(curX-1, curZ))
                    {
                        DestroyWall(cells[curX-1, curZ].south);
                        curX--;
						enemyDir = 1;
                    }
                    break;
                case 2:
                    if(IsAvailable(curX, curZ - 1))
                    {
                        DestroyWall(cells[curX, curZ-1].east);
                        curZ--;
						enemyDir = 2;
                    }
                    break;
                case 3:
                    if(IsAvailable(curX+1, curZ))
                    {
                        DestroyWall(cells[curX, curZ].south);
						curX++;
						enemyDir = 3;
                    }
                    break;
                case 4:
                    if(IsAvailable(curX, curZ + 1))
                    {
                        DestroyWall(cells[curX, curZ].east);
                        curZ++;
						enemyDir = 4;
                    }
                    break;

                default:

                    break;
            }
            cells[curX, curZ].SetVisit(true); 
        }
		float enemyX = curX * cellSize;
		float enemyY = 0.1f;
		float enemyZ = curZ * cellSize;
		enemies [enemyNum] = new Enemy();
		//enemies [enemyNum].dir = enemyDir;
		enemies [enemyNum].enemyVect = new Vector3 (enemyX, enemyY, enemyZ);
		//Debug.Log ("enemyNum = " + enemyNum + "( " + enemyX + ", " + enemyY + ", " + enemyZ + " )");

		//enemies[enemyNum].enemyObject = Instantiate(enemyPrefab, new Vector3(enemyX, enemyY, enemyZ), Quaternion.identity) as GameObject;
		//enemies [enemyNum].enemyObject.transform.Rotate (Vector3.left, 90f);
		enemyNum++;
		enemies [0].enemyNum = enemyNum;
        Debug.Log("Kill end at : (" + curX + ", " + curZ + " )");
    }

    void Hunt()
    {
        Debug.Log("Hunt start!");
        for(int i = 0; i < sizeX; ++ i)
        {
            for(int j = 0; j < sizeZ; ++j)
            {
                if (!cells[i, j].GetVisit() && AdjacentCellVisited(i, j))
                {
                    complete = false;
                    curX = i;
                    curZ = j;
                    DestroyAdjacentWall(i, j);
                    cells[i, j].SetVisit(true);
                    Debug.Log("Hunt -> " + "( " + i + ", " + j + " )");
                    return;
                }
            }
        }
        complete = true;
    }

    bool IsDeadend(int x, int z)
    {
        bool deadend = true;
        if (x-1 > 0 && !cells[x-1, z].GetVisit() && deadend)
        {
            deadend = false;
        }
        if (z-1 > 0 && !cells[x, z-1].GetVisit() && deadend)
        {
            deadend = false;
        }
        if(x+1 < sizeX && !cells[x+1, z].GetVisit() && deadend)
        {
            deadend = false;
        }
        if(z+1 < sizeZ && !cells[x, z + 1].GetVisit() && deadend)
        {
            deadend = false;
        }
        Debug.Log("deadend = " + deadend);
        //bool t = cells[x + 1, z].GetVisit();
        //Debug.Log("t = " + t);
        return deadend;
    }

    bool IsAvailable(int x, int z)
    {
        if(x >= 0 && z >= 0 && x < sizeX && z < sizeZ && !cells[x, z].GetVisit())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool AdjacentCellVisited(int x, int z)
    {
        bool visited = false;
        if (x-1 >= 0 && cells[x-1, z].GetVisit() && !visited)
        {
            visited = true;
        }
        if(z-1 >= 0 && cells[x, z - 1].GetVisit() && !visited)
        {
            visited = true;
        }
        if(x+1 < sizeX-1 && cells[x+1, z].GetVisit() && !visited)
        {
            visited = true;
        }
        if(z+1 < sizeZ-1 && cells[x, z + 1].GetVisit() && !visited)
        {
            visited = true;
        }
        return visited;
    }

    void DestroyAdjacentWall(int x, int z)
    {
        bool destroyed = false;
        while (!destroyed)
        {
            Random.seed = System.Guid.NewGuid().GetHashCode();
            int dir = Random.Range(1, 5);
            switch (dir)
            {
                //north: 1, west: 2, south: 3, east: 4
                case 1:
                    if(x > 0 && cells[x-1, z].GetVisit())
                    {
                        DestroyWall(cells[x-1, z].south);
                        destroyed = true;
                    }
                    break;
                case 2:
                    if(z > 0 && cells[x, z - 1].GetVisit())
                    {
                        DestroyWall(cells[x, z-1].east);
                        destroyed = true;
                    }
                    break;
                case 3:
                    if(x < sizeX - 2 && cells[x+1, z].GetVisit())
                    {
                        DestroyWall(cells[x, z].south);
                        destroyed = true;
                    }
                    break;
                case 4:
                    if(z < sizeZ - 2 && cells[x, z + 1].GetVisit())
                    {
                        DestroyWall(cells[x, z].east);
                        destroyed = true;
                    }
                    break;
                default:

                    break;

            }

        }

    }

    void DestroyWall(GameObject wall)
    {
        if(wall != null)
        {
            GameObject.Destroy(wall);
        }else
        {
            Debug.Log("Wall is null!");
        }
    }
}
