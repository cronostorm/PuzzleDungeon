using UnityEngine;
using System.Collections;

public class GemGen : MonoBehaviour {
	
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
    public Vector3 origin = new Vector3(0,0,100); // To adjust for camera viewing
	public int gameSize = 10; // Size of board
	// Colors of gems
	public Color[] gemColors = {Color.red, Color.green, Color.blue, 
		Color.yellow, Color.magenta};
	// Array of gems generated.
	public GameObject[] gemBoard = new GameObject[100];
	// int numUpdate = 0;
 	private float t;  // For time
	
	// Initalization
    // Should generate gameSize ^ 2 gems. 
	void Start () {
        t = Time.time;
        Debug.Log("Calling start");
		for (int col = 0; col < gameSize; col++) {
			for (int row = 0; row < gameSize; row++) {
				generateRandomGem (col, row);
			}
		}	
	}
	
	// Update is called once per frame
	void Update () {
		postMove();
	}
	
    // Function to be called post move to update the board.
	void postMove() {
		
        if (Time.time - t > 2.0f) {
			for (int col = 0; col < gameSize; col++) {
				checkForMatches(col, true);  // in row
				checkForMatches(col, false); // in column
			}
			shiftBoardDown();
			// numUpdate++;
            t = Time.time;
		}
		
	}
	
	// If row is true, then we're checking a row
	//  --> index = arrayIndex + i * gameSize
	// Else --> index = i + arrayIndex * gameSize
	void checkForMatches(int arrayIndex, bool row) {
		
		// Start to clear from
		Color prevColor = Color.white; 
		int start_clear = -1;
		int clearCount = 1;  // How many gems in a array with matching color.
        
		// Check through array
		// If we want to disable a gem, we simply disable the renderer. 
		for (int c = 0; c < gameSize; c++) {
			GameObject gem;
			
			if (row) {
				gem = gemBoard[arrayIndex + c * gameSize];
			} else {
				gem = gemBoard[c + arrayIndex * gameSize];
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
				prevColor = currColor;
				// Check if we need to clear. Only clear if 3+ colored gems in a row.
				if (clearCount >= 3) {
					for (int i = start_clear; i < start_clear + clearCount; i++) {
						if (row) {
							gemBoard[arrayIndex + i * gameSize].renderer.enabled = false;
						} else {
							gemBoard[i + arrayIndex * gameSize].renderer.enabled = false;
						}
					}
				}
				// Reset counts. 
				clearCount = 1;
				start_clear = c;
			}
		}
        
		// Check if we need to clear. Only clear if 3+ colored gems in a row.
		if (clearCount >= 3) {
			for (int i = start_clear; i < start_clear + clearCount; i++) {
				if (row) {
					gemBoard[arrayIndex + i * gameSize].renderer.enabled = 
						false;
				} else {
					gemBoard[i + arrayIndex * gameSize].renderer.enabled = 
						false;
				}
			}
		}
	
	}
	
    // Generates a randomly colored gem given a position on the board.
	bool generateRandomGem(int x, int z) {
		// Debug.Log ("Making new gem at " + x + " " + z);
		GameObject gem = Instantiate(GemCube, 
					new Vector3(x - gameSize/2 + .5f, 0, 
								z - gameSize/2 + .5f) + origin, 
					Quaternion.identity) as GameObject;
				gem.transform.parent = this.gameObject.transform;
				
				gem.renderer.material.color = 
					gemColors[Random.Range(0, gemColors.Length)];
				gemBoard[x + z * gameSize] = gem;
		return true;
	}

    // Returns true if gem is hidden
	bool gemIsHidden(GameObject gem) {
		return !gem.renderer.enabled;
	}
	
    /*
     * Swaps the position of two gems. 
     * This is done by setting gem1 position to gem2 and vice versa.
     * Then swapping those two gems on the board so the position is reflected
     * in the board as well as physically.
     */
	void swapGemPosition(int idx1, int idx2) {
		GameObject temp1 = gemBoard[idx1];
		GameObject temp2 = gemBoard[idx2];
		Vector3 pos1 = temp1.transform.position;
		temp1.transform.position = temp2.transform.position;
		temp2.transform.position = pos1;
		gemBoard[idx1] = temp2;
		gemBoard[idx2] = temp1;
	}
	
	/*
	 * Fills in gems for a given column starting from rowStart
	 * and going up until gameSize. Destroys old gems in those spots.
	 */
	void fillInGems(int column, int rowStart){
		for(int i = rowStart; i < gameSize; i++) { 
			Destroy(gemBoard[column + i * gameSize]);
			generateRandomGem (column, i);
		}
	}
	
	// Go up a column and shift gem down one by one. 
	void shiftBoardDown() {
		for (int c = 0; c < gameSize; c++) {
			int bottomGemIndex = 0;
			int currGemIndex = 1;
            
			for (int r = 0; r < gameSize; r++) {
				GameObject gem = gemBoard[c + r * gameSize];
				// If gem is hidden, find next visible gem to move down.
				if (gemIsHidden(gem)) {
                    // While I can still find gems to shift down.
					while (currGemIndex <= gameSize - 1) {
						GameObject findGem = 
							gemBoard[c + currGemIndex * gameSize];
                        // If already hidden, check gem above that.
						if (gemIsHidden(findGem)) {
							currGemIndex++;	
						} else {
                            // Else, swap gem positions.
							swapGemPosition(c + bottomGemIndex * gameSize, 
								c + currGemIndex * gameSize);
							bottomGemIndex++;
							break;
						}
					}
                    // No more gems to check in column. They've all been
                    // shifted down.
					if (currGemIndex == gameSize - 1){
						break;
					}
					
				} else {
					currGemIndex++;
				}
				// Else move up.
			}
			if (bottomGemIndex <= gameSize - 1) {
                // TODO(brooklyn) Bug is likely here that's causing board to reset.
				fillInGems(c, bottomGemIndex);
				// Fill in from curr to bot
			}
			// Check top gem. Make new ones if necessary. 
		}
	}
}
