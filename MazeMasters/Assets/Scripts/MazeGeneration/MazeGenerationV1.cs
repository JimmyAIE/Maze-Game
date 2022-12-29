using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerationV1 : MonoBehaviour
{

    public List<MazeRoom> rooms = new List<MazeRoom>();
    public Dictionary<Vector2, CellType> cells = new Dictionary<Vector2, CellType>();
    public Vector2 cellSize;

    public void Awake()
    {
        rooms.Add(new MazeRoom(Vector3.zero, new Vector3(5, 3, 5)));
        rooms[0].doorPositions.Add(new Vector3(0, 0.5f, 0.5f));
    }
    public void OnDrawGizmos()
    {
        foreach (MazeRoom room in rooms)
        {
            DrawRoom(room);
        }
    }
    public void DrawRoom(MazeRoom room)
    {
        Gizmos.color = new Color(255, 255, 0, 0.5f);
        Gizmos.DrawCube(room.position, room.size);
        foreach (Vector3 door in room.doorPositions)
        {
            Vector3 doorPosition = (room.position - room.size / 2) + Vector3.Scale(room.size, door);
            Gizmos.color = new Color(255, 0, 0, 0.5f);
            Gizmos.DrawCube(doorPosition, Vector3.one);
        }
    }

    public void GenerateMaze()
    {

    }
}
