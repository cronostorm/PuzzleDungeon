using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dungeon {
#region Public Variables

  public int minRooms; 
  public int maxRooms;
  public int minRoomSize; 
  public int maxRoomSize;
  public int maxExtraTunnels;

#endregion
#region Private Variables

  private int[] map;
  private int _width;
  private int _height;
  private List<Tile> tiles = 
    new List<Tile>() {Tile.Stone, Tile.Wall, Tile.Floor, Tile.Water, Tile.Lava};
  private List<Rect> rooms;
  
#endregion
#region Constructors

	public Dungeon() {
	  map = new int[_width * _height];
	}
	
	public Dungeon (int w, int h) {
	  _width = w;
    _height = h;
	  map = new int[_width * _height];
	}

#endregion
#region Private Methods
  // Carve a room of a type of tile from the walls
  private void CarveRoom(Rect room) { 
    int type = Random.Range(2, 5);
    for (float x = room.xMin; x <= room.xMax; x++) {
      for (float y = room.yMin; y <= room.yMax; y++) {
        map[(int) (x + y * _width)] = type;
      }
    }
  }

  // Create a border of walls around a room
  private void CarveWalls(Rect room) {
    int x0 = (int) room.xMin;
    int y0 = (int) room.yMin;
    int xf = (int) room.xMax;
    int yf = (int) room.yMax;
    for (int x = x0; x <= xf; x++) {
      map [x + (y0)* _width] = 1;
      map [x + (yf)* _width] = 1;
    }
    for (int y = y0; y <= yf; y++) {
      map [(x0) + y * _width] = 1;
      map [(xf) + y * _width] = 1;
    }
  }

  // Carve out a door from a wall
  // sides: 0 = right, 1 = bot, 2 = left, 3 = top
  // dimensions: x0, y0, xf, yf
  private void CarveDoor(int side, int num, Rect room) {
    int[] dimensions = new int[] 
      {(int) room.xMin, (int) room.yMin, (int) room.xMax, (int) room.yMax};
    int dir = 1 - (side) % 2;
    for (int i = 0; i < num; i++) {
      int doorSize = Random.Range(1, 5);
      int start = Random.Range(dimensions[dir] + 1, dimensions[dir + 2] - doorSize);
      for (int j = start; j < start + doorSize; j++) {
        int idx;
        if (dir == 1) {
          idx = dimensions[(side + 2) % 4]  + j * _width;
        }
        else {
          idx = j + dimensions[(side + 2) % 4] * _width;
        }
        map[idx] = 0;
      }
    }
  }

  // checks if the room is overlapping any other rooms
  private bool Overlapping(Rect room) {
    foreach (Rect other in rooms) { 
      if ((room.xMin < other.xMax && room.xMax > other.xMin &&
            room.yMin < other.yMax && room.yMax > other.yMin)) {
        return true;
      }
    }
    return false;
  }

#endregion
#region Accessor Methods

  public int width {get {return _width;}}
  public int height {get {return _height;}}
  
  public void SetStats (int _minRooms, int _maxRooms, 
      int _minRoomSize, int _maxRoomSize, int _maxExtraTunnels) {
    minRooms = _minRooms;
    maxRooms = _maxRooms;
    minRoomSize = _minRoomSize;
    maxRoomSize = _maxRoomSize;
    maxExtraTunnels = _maxExtraTunnels;
  }

  // Construct the dungeon out of rectangles
  public void Init() {
    int numRooms = Random.Range(minRooms, maxRooms + 1);
    rooms = new List<Rect>();
    for (int i = 0; i < numRooms; i++) {
      Rect room = new Rect(0, 0, 0, 0);
      int roomWidth = 0;
      int roomHeight = 0;
      int tries = 0;

      // randomly try to place rooms inside the dungeon
      do {
        roomWidth = Random.Range(minRoomSize + 1, maxRoomSize + 1);
        roomHeight = Random.Range(minRoomSize + 1, maxRoomSize + 1);
        int x0 = Random.Range(0, _width - 1 - roomWidth);
        int y0 = Random.Range(0, _width - 1 - roomHeight);
        room = new Rect(x0, y0, roomWidth, roomHeight);
        tries ++;
      } while (Overlapping(room) && tries < 500);
      // If we fail too much, the dungeon is too packed, give up
      if (tries >= 500) { 
        break;
      }
    
      rooms.Add(room);
      // Actually carve out the room and give it walls and openings
      CarveRoom(room);
      CarveWalls(room);
      for (int j = 0; j < 4; j++) {
        int numDoors = Random.Range(1, Mathf.Min(roomWidth/4, 1) + 1);
        CarveDoor(j, numDoors, room);
      }
    }
    
    // Construct paths from centers of one room to the next in the list
    List<int> path = new List<int>();
    for (int i = 0; i < rooms.Count - 1; i++) {
      int xc1 = (int) rooms[i].center.x;
      int yc1 = (int) rooms[i].center.y;
      int xc2 = (int) rooms[i + 1].center.x;
      int yc2 = (int) rooms[i + 1].center.y;


      path = AStar.Pathfind(this, xc1 + yc1 * _width, xc2 + yc2 * _width, 0);
      foreach (int k in path) {
        if (map[k] == 0 || map[k] == 1) {
          map[k] = 2;
        }
      }
    }

    // Construct a random amount of additional tunnels between random rooms
    int extraTunnels = Random.Range(0, maxExtraTunnels);
    for (int i = 0; i < extraTunnels; i ++) {
      int room1 = Random.Range(0, rooms.Count - 1);
      int room2 = Random.Range(0, rooms.Count - 1);
      int xc1 = (int) rooms[room1].center.x;
      int yc1 = (int) rooms[room1].center.y;
      int xc2 = (int) rooms[room2].center.x;
      int yc2 = (int) rooms[room2].center.y;
      path = AStar.Pathfind(this, xc1 + yc1 * _width, xc2 + yc2 * _width, 0);
      foreach (int k in path) {
        if (map[k] == 0 || map[k] == 1) {
          map[k] = 2;
        }
      }
    }
  }

  public Vector2 GetStart() {
    return new Vector2((int) rooms[0].center.x, (int) rooms[0].center.y);
  }

  public Tile GetTile (int idx) {
    return tiles[map[idx]];
  }

  public Tile GetTile (int i, int j) {
    return tiles[map[i + j * _width]];
  }

  public Tile GetTile (Vector2 idx) {
    return GetTile((int) idx.x, (int) idx.y);
  }

  public Vector2 GetIdx (Vector3 pos) {
    return new Vector2((int) (pos.x + _width/2), (int) (pos.z + _height/2));
  }

  public Vector2 GetVec2 (int idx) {
    return new Vector2(idx % width, idx / width);
  }

  public Vector2 GetCenter() {
    return new Vector2((int) _width/2, (int) _height/2);
  }

  public Vector3 GetTilePos(Vector2 idx) {
    return new Vector3(idx.x, 0, idx.y);
  }

#endregion
#region Public Methods

  public bool CanMove (Vector2 idx) {
    int i = (int) idx.x;
    int j = (int) idx.y;
    if (i < 0 || i >= _width || j < 0 || j >= _height) {
      return false;
    }
    return GetTile(i,j).IsPassable(1);
  }

#endregion
}
