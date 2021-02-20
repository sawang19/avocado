using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Maze : MonoBehaviour {
	public static int[,] mazeMap;
	int mazeWidth = 10;
	int mazeHeight = 10;
	string level = "HARD";


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

		Vector3 position;

		// Draw 4 corners
		position = new Vector3 (0, 0 + offsetY, 0f);
		Instantiate(wall_ul, position, Quaternion.identity);

		position = new Vector3 (0, -mazeMapY + 1 + offsetY, 0f);
		Instantiate(wall_bl, position, Quaternion.identity);

		position = new Vector3 (mazeMapX - 1, 0 + offsetY, 0f);
		Instantiate(wall_ur, position, Quaternion.identity);

		position = new Vector3 (mazeMapX - 1, -mazeMapY + 1 + offsetY, 0f);
		Instantiate(wall_br, position, Quaternion.identity);

		// Draw 2 horizontal borders
		for (int i = 1; i < mazeMapX - 1; i++) {
			position = new Vector3 (i, 0 + offsetY, 0f);
			if (mazeMapTrf[i, 1] == 1) {
				Instantiate(wall_rbl, position, Quaternion.identity);
			}
			else {
				Instantiate(wall_h, position, Quaternion.identity);
			}

			position = new Vector3 (i, -mazeMapY + 1 + offsetY, 0f);
			if (mazeMapTrf[i, mazeMapY - 2] == 1) {
				Instantiate(wall_lur, position, Quaternion.identity);
			}
			else {
				Instantiate(wall_h, position, Quaternion.identity);
			}
		}

		// Draw 2 vertical borders
		for (int j = 1; j < mazeMapY - 1; j++) {
			position = new Vector3 (0, -j + offsetY, 0f);
			if (mazeMapTrf[1, j] == 1) {
				Instantiate(wall_urb, position, Quaternion.identity);
			}
			else {
				Instantiate(wall_v, position, Quaternion.identity);
			}

			position = new Vector3 (mazeMapX - 1, -j + offsetY, 0f);
			if (mazeMapTrf[mazeMapX - 2, j] == 1) {
				Instantiate(wall_blu, position, Quaternion.identity);
			}
			else {
				Instantiate(wall_v, position, Quaternion.identity);
			}
		}

		// Draw inner
		for (int i = 1; i < mazeMapX - 1; i++) {
			for (int j = 1; j < mazeMapY - 1; j++) {
				if (mazeMapTrf[i, j] == 1) {
					position = new Vector3 (i, -j + offsetY, 0f);

					if (getTileType(mazeMapTrf, i, j).Equals("h")) {
						Instantiate(wall_h, position, Quaternion.identity);
					}
					if (getTileType(mazeMapTrf, i, j).Equals("v")) {
						Instantiate(wall_v, position, Quaternion.identity);
					}
					if (getTileType(mazeMapTrf, i, j).Equals("ul")) {
						Instantiate(wall_ul, position, Quaternion.identity);
					}
					if (getTileType(mazeMapTrf, i, j).Equals("ur")) {
						Instantiate(wall_ur, position, Quaternion.identity);
					}
					if (getTileType(mazeMapTrf, i, j).Equals("bl")) {
						Instantiate(wall_bl, position, Quaternion.identity);
					}
					if (getTileType(mazeMapTrf, i, j).Equals("br")) {
						Instantiate(wall_br, position, Quaternion.identity);
					}
					if (getTileType(mazeMapTrf, i, j).Equals("lur")) {
						Instantiate(wall_lur, position, Quaternion.identity);
					}
					if (getTileType(mazeMapTrf, i, j).Equals("urb")) {
						Instantiate(wall_urb, position, Quaternion.identity);
					}
					if (getTileType(mazeMapTrf, i, j).Equals("rbl")) {
						Instantiate(wall_rbl, position, Quaternion.identity);
					}
					if (getTileType(mazeMapTrf, i, j).Equals("blu")) {
						Instantiate(wall_blu, position, Quaternion.identity);
					}
					if (getTileType(mazeMapTrf, i, j).Equals("urbl")) {
						Instantiate(wall_urbl, position, Quaternion.identity);
					}
					if (getTileType(mazeMapTrf, i, j).Equals("u")) {
						Instantiate(wall_u, position, Quaternion.identity);
					}
					if (getTileType(mazeMapTrf, i, j).Equals("r")) {
						Instantiate(wall_r, position, Quaternion.identity);
					}
					if (getTileType(mazeMapTrf, i, j).Equals("b")) {
						Instantiate(wall_b, position, Quaternion.identity);
					}
					if (getTileType(mazeMapTrf, i, j).Equals("l")) {
						Instantiate(wall_l, position, Quaternion.identity);
					}
					if (getTileType(mazeMapTrf, i, j).Equals("o")) {
						Instantiate(wall_o, position, Quaternion.identity);
					}
				}
			}
		}

		putItems(mazeMapTrf, key, 2, 5);
		putItems(mazeMapTrf, coin, 3, 10);
		putItems(mazeMapTrf, boot, 4, 5);
	}

	// randomly put [total] [obj]s on the map, and mark the position with [mark]
	void putItems(int[,] mazeMapTrf, GameObject obj, int mark, int total)
	{
		int mazeMapX = mazeMapTrf.GetLength(0);//2 * mazeWidth + 1;
		int mazeMapY = mazeMapTrf.GetLength(1);//2 * mazeHeight + 1;
		int offsetY = mazeMapY - 1;

		int items = 0;
		while (items < 5)
		{
			int i = Random.Range(1, mazeMapX - 1);
			int j = Random.Range(1, mazeMapY - 1);
			if (mazeMapTrf[i, j] == 0)
			{
				mazeMapTrf[i, j] = mark;
				mazeMap[i, -j + mazeMapY - 1] = mark;
				Vector3 position = new Vector3(i, -j + offsetY, 0f);
				Instantiate(obj, position, Quaternion.identity);
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
