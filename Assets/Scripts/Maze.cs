using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Maze : MonoBehaviour
{
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


	public static int[,] mazeMap;
	int mazeWidth = 10;
	int mazeHeight = 10;
	string level = "EASY";

    // Start is called before the first frame update
	void Start()
	{
		DrawMaze(mazeWidth, mazeHeight, level);
	}

    // Update is called once per frame
	void Update()
	{
	}

	string getTileType(int i, int j) {
		if (mazeMap[i, j - 1] == 0 && mazeMap[i + 1, j] == 1 && mazeMap[i, j + 1] == 0 && mazeMap[i - 1, j] == 1) {
			return "h";
		}

		if (mazeMap[i, j - 1] == 1 && mazeMap[i + 1, j] == 0 && mazeMap[i, j + 1] == 1 && mazeMap[i - 1, j] == 0) {
			return "v";
		}

		if (mazeMap[i, j - 1] == 0 && mazeMap[i + 1, j] == 1 && mazeMap[i, j + 1] == 1 && mazeMap[i - 1, j] == 0) {
			return "ul";
		}

		if (mazeMap[i, j - 1] == 0 && mazeMap[i + 1, j] == 0 && mazeMap[i, j + 1] == 1 && mazeMap[i - 1, j] == 1) {
			return "ur";
		}

		if (mazeMap[i, j - 1] == 1 && mazeMap[i + 1, j] == 1 && mazeMap[i, j + 1] == 0 && mazeMap[i - 1, j] == 0) {
			return "bl";
		}

		if (mazeMap[i, j - 1] == 1 && mazeMap[i + 1, j] == 0 && mazeMap[i, j + 1] == 0 && mazeMap[i - 1, j] == 1) {
			return "br";
		}

		if (mazeMap[i, j - 1] == 1 && mazeMap[i + 1, j] == 1 && mazeMap[i, j + 1] == 0 && mazeMap[i - 1, j] == 1) {
			return "lur";
		}

		if (mazeMap[i, j - 1] == 1 && mazeMap[i + 1, j] == 1 && mazeMap[i, j + 1] == 1 && mazeMap[i - 1, j] == 0) {
			return "urb";
		}

		if (mazeMap[i, j - 1] == 0 && mazeMap[i + 1, j] == 1 && mazeMap[i, j + 1] == 1 && mazeMap[i - 1, j] == 1) {
			return "rbl";
		}

		if (mazeMap[i, j - 1] == 1 && mazeMap[i + 1, j] == 0 && mazeMap[i, j + 1] == 1 && mazeMap[i - 1, j] == 1) {
			return "blu";
		}

		if (mazeMap[i, j - 1] == 1 && mazeMap[i + 1, j] == 1 && mazeMap[i, j + 1] == 1 && mazeMap[i - 1, j] == 1) {
			return "urbl";
		}

		if (mazeMap[i, j - 1] == 0 && mazeMap[i + 1, j] == 0 && mazeMap[i, j + 1] == 1 && mazeMap[i - 1, j] == 0) {
			return "u";
		}

		if (mazeMap[i, j - 1] == 0 && mazeMap[i + 1, j] == 0 && mazeMap[i, j + 1] == 0 && mazeMap[i - 1, j] == 1) {
			return "r";
		}

		if (mazeMap[i, j - 1] == 1 && mazeMap[i + 1, j] == 0 && mazeMap[i, j + 1] == 0 && mazeMap[i - 1, j] == 0) {
			return "b";
		}

		if (mazeMap[i, j - 1] == 0 && mazeMap[i + 1, j] == 1 && mazeMap[i, j + 1] == 0 && mazeMap[i - 1, j] == 0) {
			return "l";
		}

		return "";
	}

	void DrawMaze(int mazeWidth, int mazeHeight, string level) {
		MazeGenerator maze = new MazeGenerator(mazeWidth, mazeHeight, level);
		maze.generate();
		mazeMap = maze.mazeGrid;

		int mazeMapWidth = 2 * mazeWidth + 1;
		int mazeMapHeight = 2 * mazeHeight + 1;

		Vector3 position;

		position = new Vector3 (-mazeWidth, mazeHeight, 0f);
		Instantiate(wall_ul, position, Quaternion.identity);

		position = new Vector3 (-mazeWidth, -mazeHeight, 0f);
		Instantiate(wall_bl, position, Quaternion.identity);

		position = new Vector3 (mazeWidth, mazeHeight, 0f);
		Instantiate(wall_ur, position, Quaternion.identity);

		position = new Vector3 (mazeWidth, -mazeHeight, 0f);
		Instantiate(wall_br, position, Quaternion.identity);


		for (int i = 1; i < mazeMapWidth - 1; i++) {
			if (mazeMap[i, 1] == 1) {
				position = new Vector3 (i - mazeWidth, mazeHeight, 0f);
				Instantiate(wall_rbl, position, Quaternion.identity);
			}
			else {
				position = new Vector3 (i - mazeWidth, mazeHeight, 0f);
				Instantiate(wall_h, position, Quaternion.identity);
			}

			if (mazeMap[i, mazeMapHeight - 2] == 1) {
				position = new Vector3 (i - mazeWidth, -mazeHeight, 0f);
				Instantiate(wall_lur, position, Quaternion.identity);
			}
			else {
				position = new Vector3 (i - mazeWidth, -mazeHeight, 0f);
				Instantiate(wall_h, position, Quaternion.identity);
			}
		}

		for (int j = 1; j < mazeMapHeight - 1; j++) {
			if (mazeMap[1, j] == 1) {
				position = new Vector3 (-mazeWidth, mazeHeight - j, 0f);
				Instantiate(wall_urb, position, Quaternion.identity);
			}
			else {
				position = new Vector3 (-mazeWidth, mazeHeight - j, 0f);
				Instantiate(wall_v, position, Quaternion.identity);
			}

			if (mazeMap[mazeMapWidth - 2, j] == 1) {
				position = new Vector3 (mazeWidth, mazeHeight - j, 0f);
				Instantiate(wall_blu, position, Quaternion.identity);
			}
			else {
				position = new Vector3 (mazeWidth, mazeHeight - j, 0f);
				Instantiate(wall_v, position, Quaternion.identity);
			}
		}

		for (int i = 1; i < mazeMapWidth - 1; i++) {
			for (int j = 1; j < mazeMapHeight - 1; j++) {
				if (mazeMap[i, j] == 1) {
					position = new Vector3 (i - mazeWidth, mazeHeight - j, 0f);

					if (getTileType(i, j).Equals("h")) {
						Instantiate(wall_h, position, Quaternion.identity);
					}
					if (getTileType(i, j).Equals("v")) {
						Instantiate(wall_v, position, Quaternion.identity);
					}
					if (getTileType(i, j).Equals("ul")) {
						Instantiate(wall_ul, position, Quaternion.identity);
					}
					if (getTileType(i, j).Equals("ur")) {
						Instantiate(wall_ur, position, Quaternion.identity);
					}
					if (getTileType(i, j).Equals("bl")) {
						Instantiate(wall_bl, position, Quaternion.identity);
					}
					if (getTileType(i, j).Equals("br")) {
						Instantiate(wall_br, position, Quaternion.identity);
					}
					
					if (getTileType(i, j).Equals("lur")) {
						Instantiate(wall_lur, position, Quaternion.identity);
					}

					if (getTileType(i, j).Equals("urb")) {
						Instantiate(wall_urb, position, Quaternion.identity);
					}

					if (getTileType(i, j).Equals("rbl")) {
						Instantiate(wall_rbl, position, Quaternion.identity);
					}

					if (getTileType(i, j).Equals("blu")) {
						Instantiate(wall_blu, position, Quaternion.identity);
					}

					if (getTileType(i, j).Equals("urbl")) {
						Instantiate(wall_urbl, position, Quaternion.identity);
					}

					if (getTileType(i, j).Equals("u")) {
						Instantiate(wall_u, position, Quaternion.identity);
					}

					if (getTileType(i, j).Equals("r")) {
						Instantiate(wall_r, position, Quaternion.identity);
					}

					if (getTileType(i, j).Equals("b")) {
						Instantiate(wall_b, position, Quaternion.identity);
					}

					if (getTileType(i, j).Equals("l")) {
						Instantiate(wall_l, position, Quaternion.identity);
					}
				}
			}
		}

		/*
		for (int i = 0; i < mazeMapWidth - 1; i++) {
			position = new Vector3 (i - mazeMapWidth, 0, 0f);
			Instantiate(wall_h, position, Quaternion.identity);
		}

		for (int j = 0; j < mazeMapHeight - 1; j++) {
			position = new Vector3 (0, j - mazeMapHeight, 0f);
			Instantiate(wall_v, position, Quaternion.identity);
		}
		*/
		/*
		for (int i = 0; i < mazeMapWidth; i++) {
			for (int j = 0; j < mazeMapHeight; j++) {
				if (mazeMap[i, j] == 1) {
					position = new Vector3 (i - mazeWidth, j - mazeHeight, 0f);
					//Instantiate(wall_v, position, Quaternion.identity);

					if (i != 0 && i != (mazeMapWidth - 1) && mazeMap[i - 1, j] == 1 && mazeMap[i + 1, j] == 1) {
						Instantiate(wall_h, position, Quaternion.identity);
					}

					if (j != 0 && j != (mazeMapHeight - 1) && mazeMap[i, j - 1] == 1 && mazeMap[i, j + 1] == 1) {
						Instantiate(wall_v, position, Quaternion.identity);
					}

					if (i < (mazeMapWidth - 1) && j < (mazeMapHeight - 1) && mazeMap[i + 1, j] == 1 && mazeMap[i, j + 1] == 1) {
						Instantiate(wall_ul, position, Quaternion.identity);
					}
				}
			}
		}
		*/
		/*
		//Vector3 position;
		for (int i = 0; i < mazeMapWidth; i++) {
			for (int j = 0; j < mazeMapHeight; j++) {
				if (mazeMap[i, j] == 1) {
					position = new Vector3 (i - mazeWidth, j - mazeHeight, 0f);
					Instantiate(wall_v, position, Quaternion.identity);
					if ((i + 1) < mazeMapWidth && mazeMap[i + 1, j] != 0) {
						position = new Vector3 ((float)i - mazeWidth + 0.5f, j - mazeHeight, 0f);
						Instantiate(wall_v, position, Quaternion.identity);
					}

					if ((j + 1) < mazeMapHeight && mazeMap[i, j + 1] != 0) {
						position = new Vector3 (i - mazeWidth, (float)j - mazeHeight + 0.5f, 0f);
						Instantiate(wall_v, position, Quaternion.identity);
					}
				}
			}
		}
		*/
	}
}
