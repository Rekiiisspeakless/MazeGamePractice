using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour {

    public int sizeX, sizeZ;
    public GameObject wallPrefab;
	public GameObject treasurePrefab;
	public GameObject enemyPrefab;
	public GameObject applePrefab;
	public GameObject potionPrefab;
	public GameObject[] apples;
	public GameObject[] potions;
    private static float cellSize = 1f;
    private static float wallThick = 0.1f;


    public MazeCell[,] cells;
	public bool[,] mark;
	public Enemy[] enemies;
	public bool playerHit = false;
	public int itemNum = 5;
	public int appleNum = 0;
	public int potionNum = 0;

	// Use this for initialization
	void Start () {
        Initiate();
		MazeGenerator generator = new MazeGenerator(cells);
		enemies = generator.CreateMaze();
		CreateEnemy ();
		CreateItem ();
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < enemies[0].enemyNum; ++i){
			if(enemies[i].enemyObject != null && enemies[i].enemyObject.GetComponent<Animator>().GetBool("isAttack")){
				playerHit = true;
				return;
			}
		}
		playerHit = false;
	}

	void CreateEnemy(){
		Debug.Log ("enemies length = " + enemies[0].enemyNum);
		for (int i = 0; i < enemies[0].enemyNum - 1; ++i) {
			enemies [i].enemyObject = Instantiate (enemyPrefab, enemies[i].enemyVect, Quaternion.identity) as GameObject;
			mark [(int)(enemies [i].enemyVect.x / cellSize), (int)(enemies [i].enemyVect.z / cellSize)] = true;
			//north: 1, west: 2, south: 3, east: 4
			/*switch (enemies [i].dir) {
				case 1:
					Quaternion rot1 = Quaternion.AngleAxis (270f, Vector3.up);
					enemies [i].enemyObject.GetComponent<Rigidbody> ().MoveRotation (rot1);
					break;
				case 2:
					Quaternion rot2 = Quaternion.AngleAxis (360f, Vector3.up);
					enemies [i].enemyObject.GetComponent<Rigidbody> ().MoveRotation (rot2);
					break;
				case 3:
					Quaternion rot3 = Quaternion.AngleAxis (180f, Vector3.up);
					enemies [i].enemyObject.GetComponent<Rigidbody> ().MoveRotation (rot3);
					break;
				case 4:
					Quaternion rot4 = Quaternion.AngleAxis (90f, Vector3.up);
					enemies [i].enemyObject.GetComponent<Rigidbody> ().MoveRotation (rot4);
					break;
				default:

					break;
			}*/
		}
	}
	private void CreateItem(){
		while (itemNum != 0) {
			Random.seed = System.Guid.NewGuid().GetHashCode();
			int itemX = Random.Range(0, sizeX);
			int itemZ = Random.Range (0, sizeZ);
			if (!mark [itemX, itemZ]) {
				// 0 = apple, 1 = potion
				int kind = Random.Range (0, 2);
				if (kind == 1) {
					//potions [potionNum] = new GameObject ();
					potions[potionNum] = Instantiate(potionPrefab, new Vector3(itemX * cellSize, 0.3f, itemZ * cellSize), Quaternion.identity) as GameObject;
					potionNum++;
					itemNum--;
					potionPrefab.name = "potion " + potionNum;
				} else {
					//apples [appleNum] = new GameObject ();
					apples[appleNum] = Instantiate(applePrefab, new Vector3(itemX * cellSize, 0.3f, itemZ * cellSize), Quaternion.identity) as GameObject;
					appleNum++;
					itemNum--;
					applePrefab.name = "apple " + appleNum;
				}
			}
		}
	}
    private void Initiate()
    {
        cells = new MazeCell[sizeX, sizeZ];
		mark = new bool[sizeX, sizeZ];
		potions = new GameObject[itemNum];
		apples = new GameObject[itemNum];
        for(int i = 0; i < sizeX; i++)
        {
            for(int j = 0; j < sizeZ; ++j)
            {
                cells[i, j] = new MazeCell();
				mark [i, j] = new bool ();
				mark [i, j] = false;

                // floor
                cells[i, j].floor = Instantiate(wallPrefab, new Vector3(i * cellSize, -wallThick/2, j * cellSize),
                    Quaternion.identity) as GameObject;
                
                cells[i, j].floor.name = "floor: (" + i + ", " + j + ")";
                cells[i, j].floor.transform.Rotate(Vector3.right, 90f);

                //south wall
                cells[i, j].south = Instantiate(wallPrefab, new Vector3(i * cellSize + cellSize / 2, cellSize/2 - wallThick, j * cellSize),
                    Quaternion.identity) as GameObject;
                cells[i, j].south.name = "south: (" + i + ", " + j + ")";
                cells[i, j].south.transform.Rotate(Vector3.up, 90f);

                //east wall
                cells[i, j].east = Instantiate(wallPrefab, new Vector3(i * cellSize, cellSize / 2 - wallThick, j * cellSize + cellSize / 2),
                        Quaternion.identity) as GameObject;
                cells[i, j].east.name = "east: (" + i + ", " + j + ")";

                if (i == 0)
                {
                    //north wall
                    cells[i, j].north = Instantiate(wallPrefab, new Vector3(i * cellSize - cellSize / 2 , cellSize/2 - wallThick, j * cellSize),
                        Quaternion.identity) as GameObject;
                    cells[i, j].north.name = "north: (" + i + ", " + j + ")";
                    cells[i, j].north.transform.Rotate(Vector3.up, 90f);

                }
                if(j == 0)
                {
                    //west wall
                    cells[i, j].west = Instantiate(wallPrefab, new Vector3(i * cellSize, cellSize/2 - wallThick, j * cellSize - cellSize / 2),
                        Quaternion.identity) as GameObject;
                    cells[i, j].west.name = "west: (" + i + ", " + j + ")";
                }
				//TEST
				/*if(i == 0 && j == 1){
					applePrefab = Instantiate(applePrefab, new Vector3(i * cellSize, 0.2f, j * cellSize), Quaternion.identity) as GameObject;
					applePrefab.name = "apple";
				}
				if(i == 1 && j == 0){
					potionPrefab = Instantiate(potionPrefab, new Vector3(i * cellSize, 0.2f, j * cellSize), Quaternion.identity) as GameObject;
					potionPrefab.name = "potion";
				}*/
				if (i == sizeX - 1 && j == sizeZ - 1) {
					treasurePrefab = Instantiate(treasurePrefab, new Vector3(i * cellSize, 0f, j * cellSize), Quaternion.identity) as GameObject;
					treasurePrefab.name = "treature";
					treasurePrefab.transform.RotateAround (treasurePrefab.transform.position, Vector3.up, 90f);
				}
            }
        }
		mark [0, 0] = true;
		mark [sizeX - 1, sizeZ - 1] = true;
    }
}
