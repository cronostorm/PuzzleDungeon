using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/*
 * AStar pathfinding class
 */
public static class AStar {

#region Structs
  /*
   * Node struct for AStar,
   * Contains appropriate fields for implementing AStar
   * G: The known minimum distance to the start node
   * H: The distance to the end node determined by heuristic guess
   * Parent: the parent node
   */ 
  struct node {
    private int _h;
   
    public int G;
    public int H {get {return _h;}}
    public int Parent;
     
    public node(int g, int h, int parent) {
      G = g;
      _h = h;
      Parent = parent;
    }
  }

  /*
   * Pair struct for AStar
   * Just contains the F value and index of a node for easy sorting
   * idx: the index of the corresponding node
   * F: G + H value of the corresponding node
   */
  struct pair { 
    public int F;
    int _idx;
    public int idx {get {return _idx;}}
    

    public pair(int f, int i) {
      F = f;
      _idx = i;
    }

    public override bool Equals(object ob) {
      if (ob is pair) {
        return ((pair) ob).F == F;
      }
      return false;
    }

    public override int GetHashCode() {
      int hash = 23;
      hash = hash * 31 + F;
      hash = hash * 31 + idx;
      return idx;
    }
  }

#endregion
#region Public Static Methods

  /*
   * AStar pathfinding from start to end.
   * map:   The array of tiles that make up the dungeon
   * start: The start index represented as a Vector2
   * end:   The end index represented as a Vector2
   * width: The width of the dungeon
   */
  public static List<int> Pathfind(Tile[] map, Vector2 start, Vector2 end, int width) {
    return Pathfind(map, (int) (start.y * width + start.x),
       (int) (end.y * width + end.x), width);
  }

  public static List<int> Pathfind(Tile[] map, int start, int end, int width) {
    /* initialize datastructs
     * open:    list of open nodes, potential to be visited
     * closed:  set of closed nodes, done visiting
     * nodes:   Dictionary of all nodes for easy reference
     */
    List<pair> open = new List<pair>();
    Dictionary<int, node> nodes = new Dictionary<int, node>();
    HashSet<int> closed = new HashSet<int>();

    // create and add the starting node to the open list
    node first = new node (0, Manhattan(start, end, width), start);
    nodes.Add(start, first);
    open.Add(new pair(first.G + first.H, start));

    // Loop until we have reached the end, or we have run out of nodes
    int current = start;
    while (current != end && open.Count > 0) {
      // Check the node with the lowest F-Value
      current = open.First().idx;
      node currentNode = nodes[current];
      open.RemoveAt(0);
      closed.Add(current);

      // Check all the current node's neighbors
      foreach (int n in neighbors(current, map, width)) {
        int neighbor = current + n;
        if (!closed.Contains(neighbor)) {
          // calculate neighbor's G value
          int G = currentNode.G + 1;
          
          // If the neighbor is not in open, add it to open
          int openidx = open.FindIndex(e => e.idx == neighbor);
          if (openidx < 0) {
            int H = Manhattan(current, end, width);
            nodes[neighbor] = new node(G, H, current);
            open.Add(new pair(G + H, neighbor));
          }
          // Otherwise, check if the path from the new parent is shorter
          else {
            node neighborNode = nodes[neighbor];
            if (G < neighborNode.G) {
              // Update the neighbor node since hte new path is shorter
              neighborNode.G = G;
              neighborNode.Parent = current;
              open[openidx] = new pair(neighborNode.G + neighborNode.H, neighbor);
              open.Sort();
            }
          }
        }
      }
    }
    
    // Check if we reached the end
    List<int> path = new List<int>();
    if (current == end) {
      // Start from the end node and construct the path to the start
      int next = end;
      while (next != start) {
        path.Add(next);
        next = nodes[next].Parent;
      }
    }

    return path;
  }

#endregion
#region Private Methods

  /*
   * Function to calculate neighbors
   * Modify tile.IsPassable() to change A* behavior
   */
  private static HashSet<int> neighbors(int idx, Tile[] map, int width) {
    int x = idx % width;
    int y = idx / width;
    int height = map.Length / width;
    HashSet<int> neighbors = new HashSet<int>() {-width, -1, +1, +width};
    if (x == width - 1)   neighbors.Remove(+1);
    if (x == 0)           neighbors.Remove(-1);
    if (y == height - 1)  neighbors.Remove(+width);
    if (y == 0)           neighbors.Remove(-width);

    HashSet<int> output = new HashSet<int>();
    foreach (int n in neighbors) {
      if (idx + n > 0 && idx + n < map.Length 
          && map[idx + n].IsPassable()) {
        output.Add(n);
      }
    }
    return output;
  }

  /*
   * Manhattan distance function
   */
  private static int Manhattan(int start, int end, int width) {
    int x0 = start % width;
    int y0 = start / width;
    int x1 = end % width;
    int y1 = end / width;
    return (Math.Abs(x1 - x0) + Math.Abs(y1 - y0));
  }

#endregion
}
