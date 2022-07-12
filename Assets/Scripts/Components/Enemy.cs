using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent] // can now be applied to objects in the scene
public struct Enemy : IComponentData
{
    public float3 lastCell;

}
