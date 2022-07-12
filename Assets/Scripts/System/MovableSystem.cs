using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

public class MovableSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref PhysicsVelocity physVel, in Moveble mov) =>
        {
            var step = mov.directions * mov.speed;
            physVel.Linear = step;
        }).Schedule();
    }
}