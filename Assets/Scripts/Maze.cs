using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;


public class Maze : MonoBehaviour {
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

    public GameObject key; // 2
	public GameObject coin; // 3
	public GameObject boot; // 4
	public GameObject pot;

	[SerializeField]
	public GameObject mole;

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
		//wall_v.layer = 8;
		//wall_h.layer = 8;
		//wall_ul.layer = 8;
		//wall_ur.layer = 8;
		//wall_bl.layer = 8;
		//wall_br.layer = 8;
		//wall_lur.layer = 8;
		//wall_urb.layer = 8;
		//wall_rbl.layer = 8;
		//wall_blu.layer = 8;
		//wall_urbl.layer = 8;
		//wall_u.layer = 8;
		//wall_r.layer = 8;
		//wall_b.layer = 8;
		//wall_l.layer = 8;
		//wall_o.layer = 8;
		mazeMap = maze.mazeGrid;

		

		int[,] mazeMapTrf = mazeTransfer(mazeMap);
		int mazeMapX = mazeMapTrf.GetLength(0);//2 * mazeWidth + 1;
		int mazeMapY = mazeMapTrf.GetLength(1);//2 * mazeHeight + 1;

		int offsetY = mazeMapY - 1;

		Vector3 position;

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
			else {
				GameObject H_hborder = Instantiate(wall_h, position, Quaternion.identity);
				H_hborder.transform.SetParent(grid.transform);
			}

			position = new Vector3 (i, -mazeMapY + 1 + offsetY, 0f);
			if (mazeMapTrf[i, mazeMapY - 2] == 1) {
				GameObject LUR_hborder = Instantiate(wall_lur, position, Quaternion.identity);
				LUR_hborder.transform.SetParent(grid.transform);
			}
			else {
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
			else {
				GameObject V_vborder = Instantiate(wall_v, position, Quaternion.identity);
				V_vborder.transform.SetParent(grid.transform);
			}

			position = new Vector3 (mazeMapX - 1, -j + offsetY, 0f);
			if (mazeMapTrf[mazeMapX - 2, j] == 1) {
				GameObject BLU_vborder = Instantiate(wall_blu, position, Quaternion.identity);
				BLU_vborder.transform.SetParent(grid.transform);
			}
			else {
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
                        GameObject O_inner = Instantiate(wall_o, position, Quaternion.identity);
                        O_inner.transform.SetParent(grid.transform);
                    }
                }
			}
		}

		putItems(mazeMapTrf, key, 2, 5);
		putItems(mazeMapTrf, coin, 3, 10);
		putItems(mazeMapTrf, boot, 4, 5);
		putItems(mazeMapTrf, pot, 5, 5);
		putItems(mazeMapTrf, mole, 6, 10);


		for (int i = 0; i < navMeshSurfaces.Length; i++)
		{
			navMeshSurfaces[i].BuildNavMesh();
		}
		
	}

	// randomly put [total] [obj]s on the map, and mark the position with [mark]
	void putItems(int[,] mazeMapTrf, GameObject obj, int mark, int total)
	{
		int mazeMapX = mazeMapTrf.GetLength(0);//2 * mazeWidth + 1;
		int mazeMapY = mazeMapTrf.GetLength(1);//2 * mazeHeight + 1;
		int offsetY = mazeMapY - 1;

		Debug.Log("Item: " + mark);

		int items = 0;
		while (items < total)
		{
			int i = Random.Range(1, mazeMapX - 1);
			int j = Random.Range(1, mazeMapY - 1);
			if (mazeMapTrf[i, j] == 0)
			{
				mazeMapTrf[i, j] = mark;
				mazeMap[i, -j + mazeMapY - 1] = mark;
				Vector3 position = new Vector3(i, -j + offsetY, 0f);
				if(mark != 6)
                {
					GameObject OBJ = Instantiate(obj, position, Quaternion.identity);
					OBJ.transform.SetParent(grid.transform);
				}else
                {
                    //Vector3 position3 = new Vector3(position.x, position.y, 0);
                    GameObject OBJ = Instantiate(obj, position, Quaternion.identity);
					
                    //OBJ.transform.SetParent(Canvas);
                    //OBJ.transform.rotation = new Quaternion(0, 0, 0, 0);
                    //GameObject prefab = PrefabUtility.GetPrefabParent(obj);
                    //GameObject OBJ = (GameObject)PrefabUtility.InstantiatePrefab(obj);
                    //OBJ.transform.position = position;
                    //OBJ.transform.rotation = Quaternion.Euler(0, 0, 0);
                    //OBJ.SetActive(true);
                    //Selection.activeGameObject = OBJ;
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
}
