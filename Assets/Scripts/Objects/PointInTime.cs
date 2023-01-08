
using UnityEngine;
public class PointInTime
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 localScale;
    public PointInTime(Vector3 pos, Quaternion rot, Vector3 scale)
    {
        position = pos;
        rotation = rot;
        localScale = scale;
    }
}
