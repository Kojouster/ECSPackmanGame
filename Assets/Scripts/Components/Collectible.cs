using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent] // can now be applied to objects in the scene
public struct Collectible : IComponentData
{
    public int points;



}
