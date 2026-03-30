using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(PuzzleTemplateSO))]
public class PuzzleTemplatePreviewEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);

        PuzzleTemplateSO template = (PuzzleTemplateSO)target;

        if (GUILayout.Button("Spawn Template On Board"))
        {
            PuzzleTemplatePreviewSpawner.Spawn(template);
        }

        if (GUILayout.Button("Clear Board"))
        {
            Piece[] pieces = Object.FindObjectsOfType<Piece>();

            foreach (var piece in pieces)
            {
                Object.DestroyImmediate(piece.gameObject);
            }
        }
    }
}

public static class PuzzleTemplatePreviewSpawner
{
    public static void Spawn(PuzzleTemplateSO template)
    {
        Piece[] pieces = Object.FindObjectsOfType<Piece>();

        foreach (var piece in pieces)
        {
            Object.DestroyImmediate(piece.gameObject);
        }

        BoardController board = Object.FindFirstObjectByType<BoardController>();

        if (board == null)
        {
            Debug.LogError("BoardController not found in scene.");
            return;
        }

        Scene scene = SceneManager.GetActiveScene();
        Debug.Log(scene.name);

        foreach (var p in template.pieces)
        {
            if (p.pieceOptions == null || p.pieceOptions.Count == 0)
                continue;

            PieceDefinitionSO def = p.pieceOptions[0];
            Vector2Int pos = p.positions[Random.Range(0, p.positions.Count)];

            Tile tile = board.GetTile(pos.x, pos.y);
            if (tile == null)
            {
                Debug.LogWarning($"Tile not found at {pos}");
                continue;
            }

            GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(def.prefab.gameObject, scene);
            Piece piece = obj.GetComponent<Piece>();
            piece.Initialize(def, p.isPlayer);

            tile.SetPiece(piece);
        }

        SceneView.RepaintAll();

        Debug.Log("Template preview spawned.");
    }
}