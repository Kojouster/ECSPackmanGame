using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct Follow : IComponentData
{
    public Entity target;
    public float distance;
    public float speedMove;
    public float speedRot;
    public float3 offSet;
    public bool frezzeXpos, frezzeYpos, frezzeZpos,freezeRot;
}
