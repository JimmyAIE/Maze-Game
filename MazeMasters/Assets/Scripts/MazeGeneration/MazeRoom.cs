using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRoom
{
    public Vector3 position;
    public Vector3 size;
    public List<Vector3> doorPositions = new List<Vector3>();
    public MazeRoom(Vector3 _position, Vector3 _size)
    {
        position = _position;
        size = _size;
    }
}