using System.Collections.Generic;
using System;
using System.Linq;
using Random = System.Random;

public class MazeGenerator {
	public int nx;
	public int ny;
	public int ix;
	public int iy;
	public Cell[,] maze_map;
	public int[,] mazeGrid;
	private int level;

	private static Random random;
	private static object syncObj = new object();
	private static void InitRandomNumber(int seed) {
		random = new Random(seed);
	}

	private static int GenerateRandomNumber(int min, int max) {
		lock(syncObj) {
			if (random == null) random = new Random();
			return random.Next(min, max);
		}
	}

	public MazeGenerator(int nx, int ny, string level) {
		this.nx = nx;
		this.ny = ny;
		this.ix = 0;
		this.iy = 0;

		if (level.Equals("EASY")) this.level = (int)(nx * ny * 0.9);
		if (level.Equals("MEDIUM")) this.level = (int)(nx * ny * 0.7);
		if (level.Equals("HARD")) this.level = (int)(nx * ny * 0.3);

		maze_map = new Cell [nx, ny];

		for (int i = 0; i < nx; i++) {
			for (int j = 0; j < ny; j++) {
				maze_map[i, j] = new Cell(i, j);
			}
		}
	}

	public Cell cell_at(int x, int y) {
		return maze_map[x, y];
	}

	public Dictionary<string, Cell> find_valid_neighbours(Cell cell) {
		Dictionary<string, int[]> delta = new Dictionary<string, int[]>(){{"W", new int[] {-1, 0}}, {"E", new int[] {1, 0}}, {"S", new int[] {0, 1}}, {"N", new int[] {0, -1}}};

		int x2, y2;
		Dictionary<string, Cell> neighbours = new Dictionary<string, Cell>();
		foreach(var item in delta){
			x2 = cell.x + item.Value[0];
			y2 = cell.y + item.Value[1];
			if ((0 <= x2 && x2 < this.nx) && (0 <= y2 && y2 < this.ny)) {
				Cell neighbour = cell_at(x2, y2);
				if (neighbour.has_all_walls()) {
					neighbours.Add(item.Key, neighbour);
				}
			}
		}
		return neighbours;
	}

	public void generate() {
		int n = this.nx * this.ny;

		Stack<Cell> cell_stack = new Stack<Cell>();

		Cell current_cell = cell_at(this.ix, this.iy);

		int nv = 1;

		while (nv < n) {
			Dictionary<string, Cell> neighbours = find_valid_neighbours(current_cell);

			if (neighbours.Count == 0) {
				if(cell_stack.Count() != 0) current_cell = cell_stack.Pop();
				continue;
			}

			var randomEntry = neighbours.ElementAt(GenerateRandomNumber(0, neighbours.Count));

			current_cell.knock_down_wall(randomEntry.Value, randomEntry.Key);
			cell_stack.Push(current_cell);
			current_cell = randomEntry.Value;
			nv += 1;
		}


		mazeGrid = new int[2 * nx + 1, 2 * ny + 1];

		for (int i = 0; i < 2 * nx + 1; i++) {
			mazeGrid[i, 0] = 1;
		}

		for (int i = 0; i < 2 * ny + 1; i++) {
			mazeGrid[0, i] = 1;
		}

		for (int y = 0; y < this.ny; y++) {
			for (int x = 0; x < this.nx; x++) {
				if (maze_map[x, y].walls["E"]) {
					mazeGrid[2 * x + 1, 2*y + 1] = 0;
					mazeGrid[2 * x + 2, 2*y + 1] = 1;
				}
				else {
					mazeGrid[2 * x + 1, 2*y + 1] = 0;
					mazeGrid[2 * x + 2, 2*y + 1] = 0;
				}
			}

			for (int x = 0; x < this.nx; x++) {
				if (maze_map[x, y].walls["S"]) {
					mazeGrid[2 * x + 1, 2*y + 2] = 1;
					mazeGrid[2 * x + 2, 2*y + 2] = 1;
				}
				else {
					mazeGrid[2 * x + 1,2*y + 2] = 0;
					mazeGrid[2 * x + 2, 2*y + 2] = 1;
				}
			}
		}

		int cnt = 0;
		while (cnt < level) {
			int row = GenerateRandomNumber(1, 2 * nx);
			int col = GenerateRandomNumber(1, 2 * ny);

			if (mazeGrid[row - 1, col] == 0 && mazeGrid[row + 1, col] == 0 && mazeGrid[row, col - 1] != 0 && mazeGrid[row, col + 1] != 0) {
				mazeGrid[row, col] = 0;
				cnt += 1;
			}

			if (mazeGrid[row, col - 1] == 0 && mazeGrid[row, col + 1] == 0 && mazeGrid[row - 1, col] != 0 && mazeGrid[row + 1, col] != 0) {
				mazeGrid[row, col] = 0;
				cnt += 1;
			}
		}

		for (int row = 1; row < 2 * nx; row++) {
			for (int col = 1; col < 2 * ny; col++) {
				if (mazeGrid[row, col] == 1 && mazeGrid[row, col + 1] == 0 && mazeGrid[row + 1, col] == 0 && mazeGrid[row - 1, col] == 0 && mazeGrid[row, col - 1] == 1) {
					mazeGrid[row, col] = 0;
				}
			}
		}

		mazeGrid = expandArray(mazeGrid);
	}

	private int[,] expandArray(int[,] arr) {
        int[,] newArr = new int[arr.GetLength(0) * 2 - 1, arr.GetLength(1) * 2 - 1];
            
        for (int i = 0; i < arr.GetLength(0); i++) {
            for (int j = 0; j < arr.GetLength(1); j++) {
                newArr[2 * i, 2 * j] = arr[i, j];
            }
        }
        //printArray(newArr);
        
        for (int i = 0; i < newArr.GetLength(0) - 2; i++) {
            for (int j = 0; j < newArr.GetLength(1) - 2; j++) {
                if (newArr[i, j] == 1) {
                    if (newArr[i + 2, j] == 1) {
                        newArr[i + 1, j] = 2;
                    }
                    if (newArr[i, j + 2] == 1) {
                        newArr[i, j + 1] = 2;
                    }
                }
            }
        }
        
        for (int i = 0; i < newArr.GetLength(1); i++) {
            newArr[newArr.GetLength(0) - 1, i] = 1;
        }
        
        for (int i = 0; i < newArr.GetLength(0); i++) {
            newArr[i, newArr.GetLength(1) - 1] = 1;
        }
        
        for (int i = 0; i < newArr.GetLength(0); i++) {
            for (int j = 0; j < newArr.GetLength(1); j++) {
                if (newArr[i, j] == 2) {
                    newArr[i, j] = 1;
                }
            }
        }
        
        return newArr;
    }
        

	public void printMaze() {
		Console.WriteLine("Maze binary format:");
		for (int k = 0; k < 2 * nx + 1; k++) {
			for (int l = 0; l < 2 * ny + 1; l++) {
				Console.Write(mazeGrid[k, l]);
				Console.Write(" ");
			}
			Console.WriteLine();
		}

		Console.WriteLine();
		Console.WriteLine("Maze symbol format:");
		for (int k = 0; k < 2 * nx + 1; k++) {
			for (int l = 0; l < 2 * ny + 1; l++) {
				if (mazeGrid[k, l] == 1) {
					Console.Write("+"); 
				}
				else {
					Console.Write(" ");
				}
				Console.Write(" ");
			}
			Console.WriteLine();
		}
	}
}
