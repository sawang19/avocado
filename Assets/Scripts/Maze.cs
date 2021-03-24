using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;


public class Maze : MonoBehaviour {
	const int WALL_V = 0, WALL_H = 1, WALL_UL = 2, WALL_UR = 3, WALL_BL = 4, WALL_BR = 5, WALL_LUR = 6, WALL_URB = 7, WALL_RBL = 8, WALL_BLU = 9, WALL_URBL = 10, WALL_U = 11, WALL_R = 12, WALL_B = 13, WALL_L = 14, WALL_O = 15;
	const int MOLE = 2, NPC = 3, GOLEM = 4, DOG = 5, GHOST = 6;
	const int POT = 10, KEY = 11, COIN = 12, BOOT = 13, HP_POTION = 14, RANDOM_POTION = 15, TRAP = 16;

	public GameObject[] steelWalls = new GameObject[16];
	public GameObject[] brownWalls = new GameObject[16];
	public GameObject[] weedWalls = new GameObject[16];

	public GameObject maze_bk;
	public GameObject brownDoorV; // 20210313
	public GameObject brownDoorH; // 20210313

	public static int[,] mazeMap;
	int mazeWidth = 10;
	int mazeHeight = 10;
	string level = "HARD";

	public GameObject grid;
	public GameObject wall_v;
	public GameObject wall_h;

	public GameObject wall_ul;
	public GameObject wall_ur;
	public GameObject wall_bl;
	public GameObject wall_br;

	public GameObject wall_lur;
	public GameObject wall_urb;
	public GameObject wall_rbl;
	public GameObject wall_blu;

	public GameObject wall_urbl;

	public GameObject wall_u;
	public GameObject wall_r;
	public GameObject wall_b;
	public GameObject wall_l;
	public GameObject wall_o;

	public GameObject maze_brown_bk1; // 20210322
	public GameObject maze_brown_bk2; // 20210322
	public GameObject maze_brown_bk3; // 20210322
	public GameObject maze_brown_bk4; // 20210322

	public GameObject maze_fire_bottom; // 20210322
	public GameObject maze_fire_anim; // 20210322

	public GameObject mole;
	public GameObject npc;
	public GameObject golem;
	public GameObject dog;
	public GameObject ghost;

	public GameObject pot;
	public GameObject key;
	public GameObject coin;
	public GameObject boot;
	public GameObject hpPotion;
	public GameObject randomPotion;
	public GameObject trap;
	public GameObject sword;

	public int keyNum = 0; // 5
	public int coinNum = 0; // 10
	public int bootNum = 0; // 8 
	public int hpPotionNum = 0; // 10
	public int randomPotionNum = 0; // 10 
	public int swordNum = 0;

	[SerializeField]
	NavMeshSurface2d[] navMeshSurfaces;

	// Start is called before the first frame update
	void Start() {
		DrawMaze(mazeWidth, mazeHeight, level);

		// Test empty space
		if (false) {
			for (int x = 0; x < mazeMap.GetLength(0); x++) {
				for (int y = 0; y < mazeMap.GetLength(1); y++) {
					if (mazeMap[x, y] == 0) {
						Vector3 position = new Vector3 (x, y, 0f);
						Instantiate(wall_urbl, position, Quaternion.identity);
					}
				}
			}
		}
		//wall_b.transform.SetParent(grid.transform);
		//AstarPath.active.Scan();
	}

    // Update is called once per frame
	void Update() {

	}

	void DrawMaze(int mazeWidth, int mazeHeight, string level) {
		MazeGenerator maze = new MazeGenerator(mazeWidth, mazeHeight, level);
		maze.generate();
        
        mazeMap = maze.mazeGrid;

		

		int[,] mazeMapTrf = mazeTransfer(mazeMap);
		int mazeMapX = mazeMapTrf.GetLength(0);//2 * mazeWidth + 1;
		int mazeMapY = mazeMapTrf.GetLength(1);//2 * mazeHeight + 1;

		int offsetY = mazeMapY - 1;

		//Vector3 position;

		// Load wall sprites
		loadSprites();
		GameObject[] renderWalls = steelWalls;


		wall_v.layer = 8;
        wall_h.layer = 8;
        wall_ul.layer = 8;
        wall_ur.layer = 8;
        wall_bl.layer = 8;
        wall_br.layer = 8;
        wall_lur.layer = 8;
        wall_urb.layer = 8;
        wall_rbl.layer = 8;
        wall_blu.layer = 8;
        wall_urbl.layer = 8;
        wall_u.layer = 8;
        wall_r.layer = 8;
        wall_b.layer = 8;
        wall_l.layer = 8;
        wall_o.layer = 8;


        // Draw background
        Vector3 position;
		for (int i = 0; i < mazeMapX; i++)
		{ // 20210322
			for (int j = 0; j < mazeMapY; j++)
			{
				position = new Vector3(i, j, -0.1f);
				if (i % 2 == 0)
				{
					if (j % 2 == 0)
					{
						Instantiate(maze_brown_bk1, position, Quaternion.identity);
					}
					else
					{
						Instantiate(maze_brown_bk2, position, Quaternion.identity);
					}
				}
				else
				{
					if (j % 2 != 0)
					{
						Instantiate(maze_brown_bk1, position, Quaternion.identity);
					}
					else
					{
						Instantiate(maze_brown_bk2, position, Quaternion.identity);
					}
				}
			}
		}

		// Draw 4 corners
		position = new Vector3 (0, 0 + offsetY, 0f);
		GameObject UL_corner = Instantiate(wall_ul, position, Quaternion.identity);
		UL_corner.transform.SetParent(grid.transform);

		position = new Vector3 (0, -mazeMapY + 1 + offsetY, 0f);
		GameObject BL_corner = Instantiate(wall_bl, position, Quaternion.identity);
		BL_corner.transform.SetParent(grid.transform);

		position = new Vector3 (mazeMapX - 1, 0 + offsetY, 0f);
		GameObject UR_corner = Instantiate(wall_ur, position, Quaternion.identity);
		UR_corner.transform.SetParent(grid.transform);

		position = new Vector3 (mazeMapX - 1, -mazeMapY + 1 + offsetY, 0f);
		GameObject BR_corner = Instantiate(wall_br, position, Quaternion.identity);
		BR_corner.transform.SetParent(grid.transform);

		// Draw 2 horizontal borders
		for (int i = 1; i < mazeMapX - 1; i++) {
			position = new Vector3 (i, 0 + offsetY, 0f);
			if (mazeMapTrf[i, 1] == 1) {
				GameObject RBL_hborder = Instantiate(wall_rbl, position, Quaternion.identity);
				RBL_hborder.transform.SetParent(grid.transform);
			}
			else if (mazeMapTrf[i, 0] == 1 && mazeMapTrf[i - 1, 0] == 1 && mazeMapTrf[i + 1, 0] == 1)
			{
				GameObject H_hborder = Instantiate(wall_h, position, Quaternion.identity);
				H_hborder.transform.SetParent(grid.transform);
			}

			position = new Vector3 (i, -mazeMapY + 1 + offsetY, 0f);
			if (mazeMapTrf[i, mazeMapY - 2] == 1) {
				GameObject LUR_hborder = Instantiate(wall_lur, position, Quaternion.identity);
				LUR_hborder.transform.SetParent(grid.transform);
			}
			else if (mazeMapTrf[i, mazeMapY - 1] == 1 && mazeMapTrf[i - 1, mazeMapY - 1] == 1 && mazeMapTrf[i + 1, mazeMapY - 1] == 1)	
			{
				GameObject H_hborder = Instantiate(wall_h, position, Quaternion.identity);
				H_hborder.transform.SetParent(grid.transform);
			}
		}

		// Draw 2 vertical borders
		for (int j = 1; j < mazeMapY - 1; j++) {
			position = new Vector3 (0, -j + offsetY, 0f);
			if (mazeMapTrf[1, j] == 1) {
				GameObject URB_vborder = Instantiate(wall_urb, position, Quaternion.identity);
				URB_vborder.transform.SetParent(grid.transform);
			}
			else if (mazeMapTrf[0, j] == 1 && mazeMapTrf[0, j - 1] == 1 && mazeMapTrf[0, j + 1] == 1)
			{
				GameObject V_vborder = Instantiate(wall_v, position, Quaternion.identity);
				V_vborder.transform.SetParent(grid.transform);
			}

			position = new Vector3 (mazeMapX - 1, -j + offsetY, 0f);
			if (mazeMapTrf[mazeMapX - 2, j] == 1) {
				GameObject BLU_vborder = Instantiate(wall_blu, position, Quaternion.identity);
				BLU_vborder.transform.SetParent(grid.transform);
			}
			else if (mazeMapTrf[mazeMapX - 1, j] == 1 && mazeMapTrf[mazeMapX - 1, j - 1] == 1 && mazeMapTrf[mazeMapX - 1, j + 1] == 1)
			{
				GameObject V_vborder = Instantiate(wall_v, position, Quaternion.identity);
				V_vborder.transform.SetParent(grid.transform);
			}
		}

		// Draw inner
		for (int i = 1; i < mazeMapX - 1; i++) {
			for (int j = 1; j < mazeMapY - 1; j++) {
				if (mazeMapTrf[i, j] == 1) {
					position = new Vector3 (i, -j + offsetY, 0f);

					if (getTileType(mazeMapTrf, i, j).Equals("h")) {
						GameObject H_inner = Instantiate(wall_h, position, Quaternion.identity);
						H_inner.transform.SetParent(grid.transform);
					}
					if (getTileType(mazeMapTrf, i, j).Equals("v")) {
						GameObject V_inner = Instantiate(wall_v, position, Quaternion.identity);
						V_inner.transform.SetParent(grid.transform);
					}
					if (getTileType(mazeMapTrf, i, j).Equals("ul")) {
						GameObject UL_inner = Instantiate(wall_ul, position, Quaternion.identity);
						UL_inner.transform.SetParent(grid.transform);
					}
					if (getTileType(mazeMapTrf, i, j).Equals("ur")) {
						GameObject UR_inner = Instantiate(wall_ur, position, Quaternion.identity);
						UR_inner.transform.SetParent(grid.transform);
					}
					if (getTileType(mazeMapTrf, i, j).Equals("bl")) {
						GameObject BL_inner = Instantiate(wall_bl, position, Quaternion.identity);
						BL_inner.transform.SetParent(grid.transform);
					}
					if (getTileType(mazeMapTrf, i, j).Equals("br")) {
						GameObject BR_inner = Instantiate(wall_br, position, Quaternion.identity);
						BR_inner.transform.SetParent(grid.transform);
					}
					if (getTileType(mazeMapTrf, i, j).Equals("lur")) {
						GameObject LUR_inner = Instantiate(wall_lur, position, Quaternion.identity);
						LUR_inner.transform.SetParent(grid.transform);
					}
					if (getTileType(mazeMapTrf, i, j).Equals("urb")) {
						GameObject URB_inner = Instantiate(wall_urb, position, Quaternion.identity);
						URB_inner.transform.SetParent(grid.transform);
					}
					if (getTileType(mazeMapTrf, i, j).Equals("rbl")) {
						GameObject RBL_inner = Instantiate(wall_rbl, position, Quaternion.identity);
						RBL_inner.transform.SetParent(grid.transform);
					}
					if (getTileType(mazeMapTrf, i, j).Equals("blu")) {
						GameObject BLU_inner = Instantiate(wall_blu, position, Quaternion.identity);
						BLU_inner.transform.SetParent(grid.transform);
					}
					if (getTileType(mazeMapTrf, i, j).Equals("urbl")) {
						GameObject URBL_inner = Instantiate(wall_urbl, position, Quaternion.identity);
						URBL_inner.transform.SetParent(grid.transform);
					}
					if (getTileType(mazeMapTrf, i, j).Equals("u")) {
						GameObject U_inner = Instantiate(wall_u, position, Quaternion.identity);
						U_inner.transform.SetParent(grid.transform);
					}
					if (getTileType(mazeMapTrf, i, j).Equals("r")) {
						GameObject R_inner = Instantiate(wall_r, position, Quaternion.identity);
						R_inner.transform.SetParent(grid.transform);
					}
					if (getTileType(mazeMapTrf, i, j).Equals("b")) {
						GameObject B_inner = Instantiate(wall_b, position, Quaternion.identity);
						B_inner.transform.SetParent(grid.transform);
					}
					if (getTileType(mazeMapTrf, i, j).Equals("l")) {
						GameObject L_inner = Instantiate(wall_l, position, Quaternion.identity);
						L_inner.transform.SetParent(grid.transform);
					}
                    
					if (getTileType(mazeMapTrf, i, j).Equals("o"))
					{
						position = new Vector3(i, -j + offsetY + 0.5f, 0f);
						GameObject fire_bottom = Instantiate(maze_fire_bottom, position, Quaternion.identity); // 20210322
						fire_bottom.transform.SetParent(grid.transform);
                        position = new Vector3(i, -j + offsetY + 0.15f, 0f);
                        GameObject fire_anim = Instantiate(maze_fire_anim, position, Quaternion.identity); // 20210322
                        fire_anim.transform.SetParent(grid.transform);
                    }
				}
			}
		}

		for (int i = 0; i < navMeshSurfaces.Length; i++)
		{
			navMeshSurfaces[i].BuildNavMesh();
		}

		// Draw door
		drawDoors(mazeMapTrf, mazeMapX, mazeMapY, renderWalls, offsetY);

        //putItems(mazeMapTrf, key, 2, 5);
        //putItems(mazeMapTrf, coin, 3, 10);
        //putItems(mazeMapTrf, boot, 4, 5);
        //putItems(mazeMapTrf, pot, 5, 1);
        //putItems(mazeMapTrf, mole, 6, 2);
        //putItems(mazeMapTrf, npc, 7, 2);
        //putItems(mazeMapTrf, hpPotion, 8, 5);
        //putItems(mazeMapTrf, slowPotion, 9, 5);
        //putItems(mazeMapTrf, trap, 10, 5);
        //putItems(mazeMapTrf, golem, 11, 2);
        //putItems(mazeMapTrf, dog, 12, 5);
        //putItems(mazeMapTrf, ghost, 13, 2);
        //putItems(mazeMapTrf, door, 14, 5);
        //ItemWorld.SpawnItemWorld(new Vector3(10, 10), new Item { itemType = Item.ItemType.boots, amount = 1 });
        //ItemWorld.SpawnItemWorld(new Vector3(10, 11), new Item { itemType = Item.ItemType.coins, amount = 1 });
        //ItemWorld.SpawnItemWorld(new Vector3(10, 12), new Item { itemType = Item.ItemType.keys, amount = 1 });


        //putItems(mazeMapTrf, mole, MOLE, 2);
        putItems(mazeMapTrf, npc, NPC, 2);
		putItems(mazeMapTrf, golem, GOLEM, 2);
        //putItems(mazeMapTrf, dog, DOG, 5);
        //putItems(mazeMapTrf, ghost, GHOST, 1);

        putItems(mazeMapTrf, pot, POT, 23);
		putItems(mazeMapTrf, trap, TRAP, 5);



	}

	// randomly put [total] [obj]s on the map, and mark the position with [mark]
	void putItems(int[,] mazeMapTrf, GameObject obj, int mark, int total)
	{
		int mazeMapX = mazeMapTrf.GetLength(0);//2 * mazeWidth + 1;
		int mazeMapY = mazeMapTrf.GetLength(1);//2 * mazeHeight + 1;
		int offsetY = mazeMapY - 1;

		int[,] directions = new int[4, 2] { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };

		int items = 0;
		while (items < total)
		{
			int i = Random.Range(1, mazeMapX - 1);
			int j = Random.Range(1, mazeMapY - 1);
			bool hasNeighborWall = false;
			for (int x = 0; x < directions.GetLength(0); x++)
			{
				int i1 = i + directions[x, 0];
				int j1 = j + directions[x, 1];
				if (1 <= i1 && i1 < mazeMapX - 1 && 1 <= j1 && j1 < mazeMapY - 1 && mazeMapTrf[i1, j1] == 1)
				{
					hasNeighborWall = true;
				}
			}

			if (mazeMapTrf[i, j] == 0 && hasNeighborWall)
			{
				mazeMapTrf[i, j] = mark;
				mazeMap[i, -j + mazeMapY - 1] = mark;
				Vector3 position = new Vector3(i, -j + offsetY, 0f);
				if(mark == MOLE || mark == GOLEM || mark == DOG || mark == GHOST)
                {
					//GameObject OBJ = Instantiate(obj, position, Quaternion.identity);
					//OBJ.transform.SetParent(grid.transform);
					GameObject OBJ = Instantiate(obj, position, Quaternion.identity);
				}

				else if (mark == POT)
				{
					GameObject OBJ = Instantiate(obj, position, Quaternion.identity);
                    OBJ.transform.SetParent(grid.transform);
                    if (keyNum > 0)
					{
						OBJ.GetComponent<pot>().item = key;
						keyNum--;
					}
					else if (coinNum > 0)
					{
						OBJ.GetComponent<pot>().item = coin;
						coinNum--;
					}
					else if (bootNum > 0)
					{
						OBJ.GetComponent<pot>().item = boot;
						bootNum--;
					}
					else if (hpPotionNum > 0)
					{
						OBJ.GetComponent<pot>().item = hpPotion;
						hpPotionNum--;
					}
					else if (randomPotionNum > 0)
					{
						OBJ.GetComponent<pot>().item = randomPotion;
						randomPotionNum--;
					}
					else if (swordNum > 0)
					{
						OBJ.GetComponent<pot>().item = sword;
						swordNum--;

					}
				}

				//else if(mark == 2)
    //            {
    //                //Vector3 position3 = new Vector3(position.x, position.y, 0);
    //                //GameObject OBJ = Instantiate(obj, position, Quaternion.identity);
    //                ItemWorld.SpawnItemWorld(position, new Item { itemType = Item.ItemType.keys, amount = 1 });

    //            } else if(mark == 3)
    //            {
				//	ItemWorld.SpawnItemWorld(position, new Item { itemType = Item.ItemType.coins, amount = 1 });
				//} else if(mark == 4)
    //            {
				//	ItemWorld.SpawnItemWorld(position, new Item { itemType = Item.ItemType.boots, amount = 1 });
				else
                {
                    GameObject OBJ = Instantiate(obj, position, Quaternion.identity);
                    OBJ.transform.SetParent(grid.transform);
                }

				items++;
			}
		}
	}

	int[,] mazeTransfer(int[,] mazeOld) {
		int mazeMapX = mazeOld.GetLength(0);
		int mazeMapY = mazeOld.GetLength(1);

		int[,] mazeNew = new int[mazeMapX, mazeMapY];

		for (int i = 0; i < mazeMapX; i++) {
			for (int j = 0; j < mazeMapY; j++) {
				mazeNew[i, mazeMapY - j - 1] = mazeOld[i, j];
			}
		}

		return mazeNew;
	}

	string getTileType(int[,] localMaze, int i, int j) {
		if (localMaze[i, j - 1] == 0 && localMaze[i + 1, j] == 1 && localMaze[i, j + 1] == 0 && localMaze[i - 1, j] == 1) {
			return "h";
		}

		if (localMaze[i, j - 1] == 1 && localMaze[i + 1, j] == 0 && localMaze[i, j + 1] == 1 && localMaze[i - 1, j] == 0) {
			return "v";
		}

		if (localMaze[i, j - 1] == 0 && localMaze[i + 1, j] == 1 && localMaze[i, j + 1] == 1 && localMaze[i - 1, j] == 0) {
			return "ul";
		}

		if (localMaze[i, j - 1] == 0 && localMaze[i + 1, j] == 0 && localMaze[i, j + 1] == 1 && localMaze[i - 1, j] == 1) {
			return "ur";
		}

		if (localMaze[i, j - 1] == 1 && localMaze[i + 1, j] == 1 && localMaze[i, j + 1] == 0 && localMaze[i - 1, j] == 0) {
			return "bl";
		}

		if (localMaze[i, j - 1] == 1 && localMaze[i + 1, j] == 0 && localMaze[i, j + 1] == 0 && localMaze[i - 1, j] == 1) {
			return "br";
		}

		if (localMaze[i, j - 1] == 1 && localMaze[i + 1, j] == 1 && localMaze[i, j + 1] == 0 && localMaze[i - 1, j] == 1) {
			return "lur";
		}

		if (localMaze[i, j - 1] == 1 && localMaze[i + 1, j] == 1 && localMaze[i, j + 1] == 1 && localMaze[i - 1, j] == 0) {
			return "urb";
		}

		if (localMaze[i, j - 1] == 0 && localMaze[i + 1, j] == 1 && localMaze[i, j + 1] == 1 && localMaze[i - 1, j] == 1) {
			return "rbl";
		}

		if (localMaze[i, j - 1] == 1 && localMaze[i + 1, j] == 0 && localMaze[i, j + 1] == 1 && localMaze[i - 1, j] == 1) {
			return "blu";
		}

		if (localMaze[i, j - 1] == 1 && localMaze[i + 1, j] == 1 && localMaze[i, j + 1] == 1 && localMaze[i - 1, j] == 1) {
			return "urbl";
		}

		if (localMaze[i, j - 1] == 0 && localMaze[i + 1, j] == 0 && localMaze[i, j + 1] == 1 && localMaze[i - 1, j] == 0) {
			return "u";
		}

		if (localMaze[i, j - 1] == 0 && localMaze[i + 1, j] == 0 && localMaze[i, j + 1] == 0 && localMaze[i - 1, j] == 1) {
			return "r";
		}

		if (localMaze[i, j - 1] == 1 && localMaze[i + 1, j] == 0 && localMaze[i, j + 1] == 0 && localMaze[i - 1, j] == 0) {
			return "b";
		}

		if (localMaze[i, j - 1] == 0 && localMaze[i + 1, j] == 1 && localMaze[i, j + 1] == 0 && localMaze[i - 1, j] == 0) {
			return "l";
		}

		return "o";
	}


	void loadSprites()
	{
		// Steel
		steelWalls[WALL_V] = Resources.Load("Walls/Steel/steel_V") as GameObject;
		steelWalls[WALL_H] = Resources.Load("Walls/Steel/steel_H") as GameObject;

		steelWalls[WALL_UL] = Resources.Load("Walls/Steel/steel_UL") as GameObject;
		steelWalls[WALL_UR] = Resources.Load("Walls/Steel/steel_UR") as GameObject;
		steelWalls[WALL_BL] = Resources.Load("Walls/Steel/steel_BL") as GameObject;
		steelWalls[WALL_BR] = Resources.Load("Walls/Steel/steel_BR") as GameObject;

		steelWalls[WALL_LUR] = Resources.Load("Walls/Steel/steel_LUR") as GameObject;
		steelWalls[WALL_URB] = Resources.Load("Walls/Steel/steel_URB") as GameObject;
		steelWalls[WALL_RBL] = Resources.Load("Walls/Steel/steel_RBL") as GameObject;
		steelWalls[WALL_BLU] = Resources.Load("Walls/Steel/steel_BLU") as GameObject;

		steelWalls[WALL_URBL] = Resources.Load("Walls/Steel/steel_URBL") as GameObject;

		steelWalls[WALL_U] = Resources.Load("Walls/Steel/steel_U") as GameObject;
		steelWalls[WALL_R] = Resources.Load("Walls/Steel/steel_R") as GameObject;
		steelWalls[WALL_B] = Resources.Load("Walls/Steel/steel_B") as GameObject;
		steelWalls[WALL_L] = Resources.Load("Walls/Steel/steel_L") as GameObject;

		steelWalls[WALL_O] = Resources.Load("Walls/Steel/steel_O") as GameObject;

		// Brown
		brownWalls[WALL_V] = Resources.Load("Walls/Brown/brown_V") as GameObject;
		brownWalls[WALL_H] = Resources.Load("Walls/Brown/brown_H") as GameObject;

		brownWalls[WALL_UL] = Resources.Load("Walls/Brown/brown_UL") as GameObject;
		brownWalls[WALL_UR] = Resources.Load("Walls/Brown/brown_UR") as GameObject;
		brownWalls[WALL_BL] = Resources.Load("Walls/Brown/brown_BL") as GameObject;
		brownWalls[WALL_BR] = Resources.Load("Walls/Brown/brown_BR") as GameObject;

		brownWalls[WALL_LUR] = Resources.Load("Walls/Brown/brown_LUR") as GameObject;
		brownWalls[WALL_URB] = Resources.Load("Walls/Brown/brown_URB") as GameObject;
		brownWalls[WALL_RBL] = Resources.Load("Walls/Brown/brown_RBL") as GameObject;
		brownWalls[WALL_BLU] = Resources.Load("Walls/Brown/brown_BLU") as GameObject;

		brownWalls[WALL_URBL] = Resources.Load("Walls/Brown/brown_URBL") as GameObject;

		brownWalls[WALL_U] = Resources.Load("Walls/Brown/brown_U") as GameObject;
		brownWalls[WALL_R] = Resources.Load("Walls/Brown/brown_R") as GameObject;
		brownWalls[WALL_B] = Resources.Load("Walls/Brown/brown_B") as GameObject;
		brownWalls[WALL_L] = Resources.Load("Walls/Brown/brown_L") as GameObject;

		brownWalls[WALL_O] = Resources.Load("Walls/Brown/brown_O") as GameObject;

		brownDoorV = Resources.Load("Walls/Brown/brown_door_V") as GameObject; // 20210313
		brownDoorH = Resources.Load("Walls/Brown/brown_door_H") as GameObject; // 20210313

		maze_brown_bk1 = Resources.Load("Walls/Brown/brown_bk1") as GameObject; // 20210322
		maze_brown_bk2 = Resources.Load("Walls/Brown/brown_bk2") as GameObject; // 20210322
		maze_brown_bk3 = Resources.Load("Walls/Brown/brown_bk3") as GameObject; // 20210322
		maze_brown_bk4 = Resources.Load("Walls/Brown/brown_bk4") as GameObject; // 20210322

		// Weed
		weedWalls[WALL_V] = Resources.Load("Walls/Weed/weed_V") as GameObject;
		weedWalls[WALL_H] = Resources.Load("Walls/Weed/weed_H") as GameObject;

		weedWalls[WALL_UL] = Resources.Load("Walls/Weed/weed_UL") as GameObject;
		weedWalls[WALL_UR] = Resources.Load("Walls/Weed/weed_UR") as GameObject;
		weedWalls[WALL_BL] = Resources.Load("Walls/Weed/weed_BL") as GameObject;
		weedWalls[WALL_BR] = Resources.Load("Walls/Weed/weed_BR") as GameObject;

		weedWalls[WALL_LUR] = Resources.Load("Walls/Weed/weed_LUR") as GameObject;
		weedWalls[WALL_URB] = Resources.Load("Walls/Weed/weed_URB") as GameObject;
		weedWalls[WALL_RBL] = Resources.Load("Walls/Weed/weed_RBL") as GameObject;
		weedWalls[WALL_BLU] = Resources.Load("Walls/Weed/weed_BLU") as GameObject;

		weedWalls[WALL_URBL] = Resources.Load("Walls/Weed/weed_URBL") as GameObject;

		weedWalls[WALL_U] = Resources.Load("Walls/Weed/weed_U") as GameObject;
		weedWalls[WALL_R] = Resources.Load("Walls/Weed/weed_R") as GameObject;
		weedWalls[WALL_B] = Resources.Load("Walls/Weed/weed_B") as GameObject;
		weedWalls[WALL_L] = Resources.Load("Walls/Weed/weed_L") as GameObject;

		weedWalls[WALL_O] = Resources.Load("Walls/Weed/weed_O") as GameObject;

		maze_fire_bottom = Resources.Load("Walls/Common/fire_bottom") as GameObject; // 20210322
		maze_fire_anim = Resources.Load("Walls/Common/fire_anim") as GameObject; // 20210322

		wall_v = brownWalls[WALL_V];
		wall_h = brownWalls[WALL_H];

		wall_ul = brownWalls[WALL_UL];
		wall_ur = brownWalls[WALL_UR];
		wall_bl = brownWalls[WALL_BL];
		wall_br = brownWalls[WALL_BR];

		wall_lur = brownWalls[WALL_LUR];
		wall_urb = brownWalls[WALL_URB];
		wall_rbl = brownWalls[WALL_RBL];
		wall_blu = brownWalls[WALL_BLU];

		wall_urbl = brownWalls[WALL_URBL];

		wall_u = brownWalls[WALL_U];
		wall_r = brownWalls[WALL_R];
		wall_b = brownWalls[WALL_B];
		wall_l = brownWalls[WALL_L];
		wall_o = brownWalls[WALL_O];
	}

	void drawDoors(int[,] mazeMapTrf, int mazeMapX, int mazeMapY, GameObject[] renderWalls, int offsetY)
	{
		Vector3 position;

		for (int i = 1; i < mazeMapX - 1; i++)
		{
			position = new Vector3(i, 0 + offsetY, 0f);
			if (mazeMapTrf[i, 0] == 2021)
			{
				GameObject doorH = Instantiate(brownDoorH, position, Quaternion.identity);
				doorH.transform.SetParent(grid.transform);
			}
			else if (mazeMapTrf[i + 1, 0] == 2021)
			{
				GameObject wallR = Instantiate(wall_r, position, Quaternion.identity);
				wallR.transform.SetParent(grid.transform);
			}
			else if (mazeMapTrf[i - 1, 0] == 2021)
			{
				GameObject wallL = Instantiate(wall_l, position, Quaternion.identity);
				wallL.transform.SetParent(grid.transform);
			}

			position = new Vector3(i, -mazeMapY + 1 + offsetY, 0f);
			if (mazeMapTrf[i, mazeMapY - 1] == 2021)
			{
				GameObject doorH = Instantiate(brownDoorH, position, Quaternion.identity);
				doorH.transform.SetParent(grid.transform);
			}
			else if (mazeMapTrf[i + 1, mazeMapY - 1] == 2021)
			{
				GameObject wallR = Instantiate(wall_r, position, Quaternion.identity);
				wallR.transform.SetParent(grid.transform);
			}
			else if (mazeMapTrf[i - 1, mazeMapY - 1] == 2021)
			{
				GameObject wallL = Instantiate(wall_l, position, Quaternion.identity);
				wallL.transform.SetParent(grid.transform);
			}
		}

		for (int j = 1; j < mazeMapY - 1; j++)
		{
			position = new Vector3(0, -j + offsetY, 0f);
			if (mazeMapTrf[0, j] == 2021)
			{
				if (mazeMapTrf[0, j + 1] == 2021)
				{
					position = new Vector3(0, -j + offsetY + 0.2f, 0f);
				}
				GameObject doorV = Instantiate(brownDoorV, position, Quaternion.identity);
				doorV.transform.SetParent(grid.transform);
			}
			else if (mazeMapTrf[0, j + 1] == 2021)
			{
				GameObject wallB = Instantiate(wall_b, position, Quaternion.identity);
				wallB.transform.SetParent(grid.transform);
			}
			else if (mazeMapTrf[0, j - 1] == 2021)
			{
				GameObject wallU = Instantiate(wall_u, position, Quaternion.identity);
				wallU.transform.SetParent(grid.transform);
			}

			position = new Vector3(mazeMapX - 1, -j + offsetY, 0f);
			if (mazeMapTrf[mazeMapX - 1, j] == 2021)
			{
				if (mazeMapTrf[mazeMapX - 1, j + 1] == 2021)
				{
					position = new Vector3(mazeMapX - 1, -j + offsetY + 0.2f, 0f);
				}
				GameObject doorV = Instantiate(brownDoorV, position, Quaternion.identity);
				doorV.transform.SetParent(grid.transform);
			}
			else if (mazeMapTrf[mazeMapX - 1, j + 1] == 2021)
			{
				GameObject wallB = Instantiate(wall_b, position, Quaternion.identity);
				wallB.transform.SetParent(grid.transform);
			}
			else if (mazeMapTrf[mazeMapX - 1, j - 1] == 2021)
			{
				GameObject wallU = Instantiate(wall_u, position, Quaternion.identity);
				wallU.transform.SetParent(grid.transform);
			}
		}
	}
}
