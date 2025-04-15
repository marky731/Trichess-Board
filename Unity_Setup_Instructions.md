# Unity Setup Instructions for Trichess Board

To make the chess pieces appear when you press play in Unity, follow these setup instructions:

## 1. Set Up the BoardPositions GameObject

1. In the Unity Hierarchy, create a new empty GameObject and name it "BoardPositions"
2. Add the `BoardPositions` component to this GameObject:
   - Select the GameObject
   - In the Inspector, click "Add Component"
   - Type "BoardPositions" and select it

## 2. Set Up the PieceView GameObject

1. In the Unity Hierarchy, create a new empty GameObject and name it "PieceView"
2. Add the `PieceView` component to this GameObject:
   - Select the GameObject
   - In the Inspector, click "Add Component"
   - Type "PieceView" and select it

3. Assign the chess piece prefabs to the PieceView component:
   - You need to create or import prefabs for each chess piece type (King, Queen, Rook, Bishop, Knight, Pawn)
   - Each prefab should have a SpriteRenderer component
   - Drag each prefab to the corresponding field in the PieceView component:
     - King Prefab
     - Queen Prefab
     - Rook Prefab
     - Bishop Prefab
     - Knight Prefab
     - Pawn Prefab

## 3. Set Up the GameManager GameObject

1. In the Unity Hierarchy, create a new empty GameObject and name it "GameManager"
2. Add the `GameManager` component to this GameObject:
   - Select the GameObject
   - In the Inspector, click "Add Component"
   - Type "GameManager" and select it

3. Assign the references in the GameManager component:
   - Drag the BoardPositions GameObject to the "Board" field
   - Drag the PieceView GameObject to the "Piece View" field

## 4. Creating Simple Chess Piece Prefabs

I've created a helpful editor tool to automatically generate simple chess piece prefabs:

1. In Unity, go to the menu: **Tools > Create Chess Piece Prefabs**
2. A window will open with color options for each piece type
3. Click the "Create All Prefabs" button
4. The prefabs will be created in the Assets/Prefabs folder
5. Assign these prefabs to the PieceView component in your scene

Alternatively, you can create them manually:

1. For each piece type (King, Queen, Rook, Bishop, Knight, Pawn):
   - Create a new empty GameObject (e.g., "KingPrefab")
   - Add a SpriteRenderer component
   - Set the Sprite to a simple shape (square, circle) or import chess piece sprites
   - Make it a prefab by dragging it from the Hierarchy to the Project window
   - Delete the GameObject from the scene (keep the prefab)

## 5. Set Up the TriChess GameObject (Optional)

The TriChess component is now simplified and doesn't require any prefabs to be assigned. It just logs when the Board is found successfully.

1. In the Unity Hierarchy, create a new empty GameObject and name it "TriChess"
2. Add the `TriChess` component to this GameObject:
   - Select the GameObject
   - In the Inspector, click "Add Component"
   - Type "TriChess" and select it

## 6. Testing

1. Make sure all the GameObjects are set up correctly:
   - BoardPositions GameObject with BoardPositions component
   - PieceView GameObject with PieceView component and all prefabs assigned
   - GameManager GameObject with GameManager component and references assigned
   - (Optional) TriChess GameObject with TriChess component
2. Press Play in Unity

If everything is set up correctly, you should see the chess pieces appear on the board when you press Play.

## Troubleshooting

If the pieces still don't appear:

1. Check the Console for error messages
2. Verify that all references are properly assigned in the Inspector
3. Make sure the BoardPositions GameObject is active in the scene
4. Ensure the PieceView component has all prefabs assigned
5. Check that the GameManager component has both the Board and PieceView references assigned
