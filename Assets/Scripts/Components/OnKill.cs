using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent] // can now be applied to objects in the scene
public struct OnKill : IComponentData
{
    public FixedString64 sfxName;
    public Entity spawnPrefab;
    public int pointValue;
}
