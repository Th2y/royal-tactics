using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public static class PuzzleTemplateEditor
{
    const string SAVE_FOLDER = "Assets/ScriptableObjects/PuzzleTemplates";

    static KingState selectedState = KingState.Checkmate;
    static Difficulty selectedDifficulty = Difficulty.Easy;

    [MenuItem("Tools/Royal Tactics/Save Puzzle Template From Board")]
    public static void SaveTemplate()
    {
        PuzzleTemplateWindow.ShowWindow();
    }

    static void CreateTemplate(KingState state, Difficulty difficulty, string extraName)
    {
        Piece[] pieces = Object.FindObjectsOfType<Piece>();

        if (pieces.Length == 0)
        {
            Debug.LogWarning("No pieces found on board.");
            return;
        }

        if (!Directory.Exists(SAVE_FOLDER))
            Directory.CreateDirectory(SAVE_FOLDER);

        PuzzleTemplateSO template =
            ScriptableObject.CreateInstance<PuzzleTemplateSO>();

        template.state = state;
        template.pieces = new List<PuzzlePiece>();

        foreach (var piece in pieces)
        {
            PuzzlePiece p = new PuzzlePiece();

            p.pieceOptions = new List<PieceDefinitionSO>
            {
                piece.Definition
            };

            p.isPlayer = piece.IsFromPlayer;
            p.constraint = PositionConstraint.Fixed;
            p.position = piece.CurrentTile.Position;
            p.spawnChance = 100;

            template.pieces.Add(p);
        }

        string stateName = state.ToString().ToLower();
        string diffName = difficulty.ToString().ToLower();

        string name = $"template_{stateName}_{diffName}";

        if (!string.IsNullOrEmpty(extraName))
            name += "_" + extraName.ToLower();

        string path = $"{SAVE_FOLDER}/{name}.asset";

        AssetDatabase.CreateAsset(template, path);
        AssetDatabase.SaveAssets();

        Debug.Log($"Puzzle template saved at: {path}");
    }

    enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    class PuzzleTemplateWindow : EditorWindow
    {
        KingState state = KingState.Checkmate;
        Difficulty difficulty = Difficulty.Easy;
        string extraName = "";

        public static void ShowWindow()
        {
            GetWindow<PuzzleTemplateWindow>("Save Puzzle Template");
        }

        void OnGUI()
        {
            GUILayout.Label("Puzzle Settings", EditorStyles.boldLabel);

            state = (KingState)EditorGUILayout.EnumPopup("Puzzle Type", state);
            difficulty = (Difficulty)EditorGUILayout.EnumPopup("Difficulty", difficulty);

            extraName = EditorGUILayout.TextField("Extra Name (optional)", extraName);

            GUILayout.Space(10);

            if (GUILayout.Button("Save Template"))
            {
                CreateTemplate(state, difficulty, extraName);
                Close();
            }
        }
    }
}