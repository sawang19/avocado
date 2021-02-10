using System.Collections.Generic;

public class Cell {
	static Dictionary<string, string> wall_pairs = new Dictionary<string, string>(){{"N", "S"}, {"S", "N"}, {"E", "W"}, {"W", "E"}};
	public int x;
	public int y;
	public Dictionary<string, bool> walls;

	public Cell(int x, int y) {
		this.x = x;
		this.y = y;
		walls = new Dictionary<string, bool>(){{"N", true}, {"S", true}, {"E", true}, {"W", true}};
	}

	public bool has_all_walls() {
		foreach(var item in walls.Values) {
			if (item == false) return false;
		}
		return true;
	}

	public void knock_down_wall(Cell other, string wall) {
		walls[wall] = false;
		other.walls[Cell.wall_pairs[wall]] = false;
	}
}
