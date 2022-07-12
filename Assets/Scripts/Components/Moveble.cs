using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent] // can now be applied to objects in the scene
public struct Moveble : IComponentData
{

    public float speed;
    public float3 directions; // exists in unity.mathematics think of it like vector3 


}
