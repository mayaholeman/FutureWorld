using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TargetData 
{
    public int level;
    public float health;

    public float[] position;

    public TargetData(Target target, Vector3 positionVec) {
        health = target.Health;
        level = target.Level;
        position = new float[3];
        position[0] = positionVec.x;
        position[1] = positionVec.y;
        position[2] = positionVec.z;

    }
}
