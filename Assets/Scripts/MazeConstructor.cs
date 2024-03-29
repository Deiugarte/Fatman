﻿using UnityEngine;
using UnityEngine.AI;
public class MazeConstructor : MonoBehaviour {
  //1
  public bool showDebug;
  private MazeDataGenerator dataGenerator;
  private MazeMeshGenerator meshGenerator;
  public NavMeshSurface agent;

  [SerializeField] private Material mazeMat1;
  [SerializeField] private Material mazeMat2;
  [SerializeField] private Material startMat;
  [SerializeField] private Material treasureMat;

  public float hallWidth {
    get;
    private set;
  }
  public float hallHeight {
    get;
    private set;
  }

  public int startRow {
    get;
    private set;
  }
  public int startCol {
    get;
    private set;
  }

  public int goalRow {
    get;
    private set;
  }
  public int goalCol {
    get;
    private set;
  }
  //Enemies
  public int secondEnemyRow {
    get;
    private set;
  }
  public int secondEnemyCol {
    get;
    private set;
  }

  public int lastEnemyRow {
    get;
    private set;
  }
  public int lastEnemyCol {
    get;
    private set;
  }
  // power up
  public int powerUpRow {
    get;
    private set;
  }
  public int powerUpCol {
    get;
    private set;
  }

  //2
  public int[, ] data {
    get;
    private set;
  }

  //3
  void Awake () {
    // default to walls surrounding a single empty cell
    data = new int[, ] { { 1, 1, 1 }, { 1, 0, 1 }, { 1, 1, 1 }
    };
    dataGenerator = new MazeDataGenerator ();
    meshGenerator = new MazeMeshGenerator ();
   
  }

  public void GenerateNewMaze (int sizeRows, int sizeCols,
    TriggerEventHandler startCallback = null, TriggerEventHandler goalCallback = null) {
    if (sizeRows % 2 == 0 && sizeCols % 2 == 0) {
      Debug.LogError ("Odd numbers work better for dungeon size.");
    }

    DisposeOldMaze ();

    data = dataGenerator.FromDimensions (sizeRows, sizeCols);

    FindStartPosition ();
    FindGoalPosition ();
    FindSecondEnemyPosition ();
    FindLastEnemyPosition ();
    FindPowerUpPosition ();

    // store values used to generate this mesh
    hallWidth = meshGenerator.width;
    hallHeight = meshGenerator.height;

    DisplayMaze ();

    PlaceStartTrigger (startCallback);
    PlaceGoalTrigger (goalCallback);
  }
  void OnGUI () {
    //1
    if (!showDebug) {
      return;
    }

    //2
    int[, ] maze = data;
    int rMax = maze.GetUpperBound (0);
    int cMax = maze.GetUpperBound (1);

    string msg = "";

    //3
    for (int i = rMax; i >= 0; i--) {
      for (int j = 0; j <= cMax; j++) {
        if (maze[i, j] == 0) {
          msg += "....";
        } else {
          msg += "==";
        }
      }
      msg += "\n";
    }

    //4
    GUI.Label (new Rect (20, 20, 500, 500), msg);
  }

  private void DisplayMaze () {
    GameObject go = new GameObject ();
    go.transform.position = Vector3.zero;
    go.name = "Procedural Maze";
    go.tag = "Generated";

    MeshFilter mf = go.AddComponent<MeshFilter> ();
    mf.mesh = meshGenerator.FromData (data);

    MeshCollider mc = go.AddComponent<MeshCollider> ();
    mc.sharedMesh = mf.mesh;

    MeshRenderer mr = go.AddComponent<MeshRenderer> ();
    mr.materials = new Material[2] { mazeMat1, mazeMat2 };

    agent.BuildNavMesh();
  }

  public void DisposeOldMaze () {
    GameObject[] objects = GameObject.FindGameObjectsWithTag ("Generated");
    foreach (GameObject go in objects) {
      Destroy (go);
    }
  }

  private void FindStartPosition () {
    int[, ] maze = data;
    int rMax = maze.GetUpperBound (0);
    int cMax = maze.GetUpperBound (1);

    for (int i = 0; i <= rMax; i++) {
      for (int j = 0; j <= cMax; j++) {
        if (maze[i, j] == 0) {
          startRow = i;
          startCol = j;
          return;
        }
      }
    }
  }

  private void FindGoalPosition () {
    int[, ] maze = data;
    int rMax = maze.GetUpperBound (0);
    int cMax = maze.GetUpperBound (1);

    // loop top to bottom, right to left
    for (int i = rMax; i >= 0; i--) {
      for (int j = cMax; j >= 0; j--) {
        if (maze[i, j] == 0) {
          goalRow = i;
          goalCol = j;
          return;
        }
      }
    }
  }

  //Enemies
  private void FindSecondEnemyPosition () {
    int[, ] maze = data;
    int rMax = maze.GetUpperBound (0);
    int cMax = maze.GetUpperBound (1);

    // loop top to bottom, right to left
    for (int i = rMax - 8; i >= 0; i--) {
      for (int j = cMax - 8; j >= 0; j--) {
        if (maze[i, j] == 0) {
          secondEnemyRow = i;
          secondEnemyCol = j;
          return;
        }
      }
    }
  }

  private void FindLastEnemyPosition () {
    int[, ] maze = data;
    int rMax = maze.GetUpperBound (0);
    int cMax = maze.GetUpperBound (1);

    // loop top to bottom, right to left
    for (int i = rMax - 3; i >= 0; i--) {
      for (int j = cMax - 3; j >= 0; j--) {
        if (maze[i, j] == 0) {
          lastEnemyRow = i;
          lastEnemyCol = j;
          return;
        }
      }
    }
  }

  private void FindPowerUpPosition () {
    int[, ] maze = data;
    int rMax = maze.GetUpperBound (0);
    int cMax = maze.GetUpperBound (1);

    // loop top to bottom, right to left
    for (int i = rMax - 6; i >= 0; i--) {
      for (int j = cMax - 6; j >= 0; j--) {
        if (maze[i, j] == 0) {
          powerUpRow = i;
          powerUpCol = j;
          return;
        }
      }
    }
  }

  private void PlaceStartTrigger (TriggerEventHandler callback) {
    GameObject go = GameObject.CreatePrimitive (PrimitiveType.Cube);
    go.transform.position = new Vector3 (startCol * hallWidth, .5f, startRow * hallWidth);
    go.name = "Start Trigger";
    go.tag = "Generated";

    go.GetComponent<BoxCollider> ().isTrigger = true;
    go.GetComponent<MeshRenderer> ().sharedMaterial = startMat;

    TriggerEventRouter tc = go.AddComponent<TriggerEventRouter> ();
    tc.callback = callback;
  }

  private void PlaceGoalTrigger (TriggerEventHandler callback) {
    GameObject go = GameObject.CreatePrimitive (PrimitiveType.Cube);
    go.transform.position = new Vector3 (1 + goalCol * hallWidth, .5f, 1 + goalRow * hallWidth);
    go.name = "Treasure";
    go.tag = "Generated";

    go.GetComponent<BoxCollider> ().isTrigger = true;
    go.GetComponent<MeshRenderer> ().sharedMaterial = treasureMat;

    TriggerEventRouter tc = go.AddComponent<TriggerEventRouter> ();
    tc.callback = callback;
  }

}