using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

[GenerateAuthoringComponent]
public struct TriggerBuffer : IBufferElementData // Almost like an array for entity.Trigger buffer can store multiple triigers
{
    public Entity entity;
}
