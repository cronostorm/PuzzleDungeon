using UnityEngine;
using System.Collections;


public class GemGen : MonoBehaviour
{
 
    public GameObject GemCube;
    /*
  * Squares are 1 unit wide on a gameSize (10 standard) unit square. 
  * Board goes from -gamesize/ 2 to + gamesize/ 2. 
  * Grid is col by row (or x by z) (shown in y axis).
  * Grid starts in bottom left corner and goes up and to the right.
  * Increasing x moves you along the columns (right)
  * Increasing z moves you up the rows (up).
      * Index is x + z * gameSize --> where z increases downwards column 
  * and x increases accross row. 
  */ 
    public Vector3 origin = new Vector3 (0, 0, 100); // To adjust for camera viewing
    public int gameSize = 10; // Size of board
    // Array of gems generated.
    public GameObject[] gemBoard = new GameObject[100];
    // Colors of gems
    public Color[] gemColors = {Color.green, Color.magenta, Color.red,
     Color.blue, Color.yellow};
    private float t;  // For time
    
    // For Gem movement
    Transform trans;
    Vector2 oldMousePos;   // For moving gems.
    private Camera cam;
    public Character player;
    
 
    // Initalization
    // Should generate gameSize ^ 2 gems. 
    void Start ()
    {
        t = Time.time;
        // Generate board.
        for (int row = 0; row < gameSize; row++) {
            for (int col = 0; col < gameSize; col++) {
                generateRandomGem (row, col);
            }
        }
        cam = Camera.main;
        
    }
    
    
    // Update is called once per frame
    void Update ()
    {
            
        // Check if mouse button down and gets gem that you're clicking on.
        if (Input.GetMouseButtonDown (0)) {
            oldMousePos = Input.mousePosition;
            Ray r = cam.ScreenPointToRay (oldMousePos);
            RaycastHit hit = new RaycastHit ();
            bool foundObj = Physics.Raycast (r, out hit);
            if (foundObj) {
                trans = hit.transform;
                
            }
        }
        
        // If mouse button up then we've swapped gems.
        // Swap the two gems.
        if (Input.GetMouseButtonUp (0)) {
            Vector2 newMousePos = Input.mousePosition;
            float xdiff = newMousePos.x - oldMousePos.x;
            float ydiff = newMousePos.y - oldMousePos.y;
            
            if (trans != null) {
                Vector3 p = trans.localPosition;
                int z = (int)(p.x - .5f + gameSize / 2);
                int x = (int)(p.z - .5f + gameSize / 2);
                
                if (Mathf.Abs (xdiff) > Mathf.Abs (ydiff)) {
                    if (xdiff > 0) {
                        if (z + 1 < gameSize)
                            swapGemPosition (z + x * gameSize, (z + 1) + x * gameSize);
                    }
                    if (xdiff <= 0) {
                        if (z - 1 >= 0)
                            swapGemPosition (z + x * gameSize, (z - 1) + x * gameSize);
                    }
                } else {
                    if (ydiff > 0) {
                        if (x + 1 < gameSize)
                            swapGemPosition (z + x * gameSize, z + (x + 1) * gameSize);
                    }
                    if (ydiff <= 0) {
                        if (x - 1 >= 0)
                            swapGemPosition (z + x * gameSize, z + (x - 1) * gameSize);
                    }
                }
            }
        }
    
        // After I've maybe moved something, move.
        postMove ();
        
        
    }
 
    
    
    // Function to be called post move to update the board.
    void postMove ()
    {
        bool matches_found = false;
        if (Time.time - t > 0.5f) {
            
            for (int col = 0; col < gameSize; col++) {
                bool r_found = checkForMatches (col, true);  // in row
                bool c_found = checkForMatches (col, false); // in column
                matches_found = r_found || c_found || matches_found;
            }
            if (matches_found) {
                shiftBoardDown ();
            }
            t = Time.time;
        }
     
    }
 
    // Generates a randomly colored gem given a position on the board.
    bool generateRandomGem (int x, int z)
    {
        GameObject gem = Instantiate (GemCube, 
                 new Vector3 (z - gameSize / 2 + .5f, 0, 
                             x - gameSize / 2 + .5f) + origin, 
                 Quaternion.identity) as GameObject;
        gem.transform.parent = this.gameObject.transform;
             
        gem.renderer.material.color = 
                 gemColors [Random.Range (0, gemColors.Length)];
        gemBoard [z + x * gameSize] = gem;
        return true;
    }

    // Returns true if gem is hidden
    bool gemIsHidden (GameObject gem)
    {
        return !gem.renderer.enabled;
    }
 
    /*
     * Swaps the position of two gems. 
     * This is done by setting gem1 position to gem2 and vice versa.
     * Then swapping those two gems on the board so the position is reflected
     * in the board as well as physically.
     */
    void swapGemPosition (int idx1, int idx2)
    {
        
        GameObject temp1 = gemBoard [idx1];
        GameObject temp2 = gemBoard [idx2];
        Vector3 pos1 = temp1.transform.position;
        temp1.transform.position = temp2.transform.position;
        temp2.transform.position = pos1;
        gemBoard [idx1] = temp2;
        gemBoard [idx2] = temp1;
    }
 
    /*
  * Fills in gems for a given column starting from rowStart
  * and going up until gameSize. Destroys old gems in those spots.
  */
    void fillInGems (int column, int rowStart)
    {
        for (int i = rowStart; i < gameSize; i++) { 
            GameObject gem = gemBoard [column + i * gameSize];
            generateRandomGem (i, column);
            Destroy (gem);
        }
    }
    
    /**
     * Based on color, update corresponding stat when gem row is cleared.
     */
    void updateStat (Color gemColor)
    {
        
        if (gemColor == (Color.red)) {
            player.IncrementStat (Stat.Health);
        }
        else if (gemColor.Equals(Color.green)) {
            player.IncrementStat (Stat.Armor);
        }
        else if (gemColor.Equals(Color.blue)) {
            player.IncrementStat (Stat.Magic);
        }
        else if (gemColor.Equals(Color.yellow)) {
            player.IncrementStat (Stat.Moves);
        }
        else if (gemColor.Equals(Color.magenta)) {
            player.IncrementStat (Stat.Attack);
        }
       
    }
    
    // If row is true, then we're checking a row
    //  --> index = arrayIndex + i * gameSize
    // Else --> index = i + arrayIndex * gameSize
    bool checkForMatches (int arrayIndex, bool row)
    {
         
        // Start to clear from
        bool match_found = false;
        Color prevColor = Color.white; 
        int start_clear = -1;
        int clearCount = 1;  // How many gems in a array with matching color.
        
        // Check through array
        // If we want to disable a gem, we simply disable the renderer. 
        for (int c = 0; c < gameSize; c++) {
            GameObject gem;
             
            if (row) {
                gem = gemBoard [arrayIndex + c * gameSize];
            } else {
                gem = gemBoard [c + arrayIndex * gameSize];
            }
             
            Color currColor = gem.renderer.material.color;
        
            // White is default non-used color for gems. 
            if (prevColor == Color.white) {
                prevColor = currColor;
                start_clear = c;
                    
            } else if (prevColor == currColor) {
                // If prevColor already seen, it might be a row.
                clearCount++;
            } else {
                
                // Check if we need to clear. Only clear if 3+ colored gems in a row.
                if (clearCount >= 3) {
                    updateStat (prevColor);
                    for (int i = start_clear; i < start_clear + clearCount; i++) {
                        if (row) {
                            gemBoard [arrayIndex + i * gameSize].renderer.enabled = false;
                        } else {
                            gemBoard [i + arrayIndex * gameSize].renderer.enabled = false;
                        }
                        match_found = true;
                    }
                }
                // Reset counts. 
                prevColor = currColor;
                clearCount = 1;
                start_clear = c;
            }
        }
        
        // Check if we need to clear. Only clear if 3+ colored gems in a row.
        if (clearCount >= 3) {
            GameObject gem;
            if (row) {
                gem = gemBoard [arrayIndex + start_clear * gameSize];
            } else {
                gem = gemBoard [start_clear + arrayIndex * gameSize];
            }
            updateStat(gem.renderer.material.color);
            
            for (int i = start_clear; i < start_clear + clearCount; i++) {
                if (row) {
                    gemBoard [arrayIndex + i * gameSize].renderer.enabled = 
                     false;
                } else {
                    gemBoard [i + arrayIndex * gameSize].renderer.enabled = 
                     false;
                }
                match_found = true;
            }
        }
        return match_found;
    }
 
    // Go up a column and shift gem down one by one. 
    void shiftBoardDown ()
    {
        for (int c = 0; c < gameSize; c++) {
            int currGemIndex = 1;
            int fillFrom = -1;
            // Find lowest gem I need to replace
            for (int r = 0; r < gameSize; r++) {
                currGemIndex = r;
                GameObject gem = gemBoard [c + r * gameSize];
                // If gem not hidden, go to next row up.
                if (gemIsHidden (gem)) {
                    while (currGemIndex < gameSize) {
                        GameObject foundGem =
                            gemBoard [c + currGemIndex * gameSize];
                        if (gemIsHidden (foundGem))
                            currGemIndex++;
                        else {
                            swapGemPosition (c + r * gameSize,
                                c + currGemIndex * gameSize);
                            break;
                        }
                    }
                    fillFrom = r;
                    
                }
                if (currGemIndex >= gameSize) { 
                    break;
                }
            }
            if (fillFrom != -1) {
                fillInGems (c, fillFrom);
            }
        }
            
    }
}
