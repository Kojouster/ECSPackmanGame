using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class PlayerSystem : SystemBase
{
    protected override void OnUpdate()
    {
        // unity old input system for simplicity
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        var dt = Time.DeltaTime;

        Entities.
            WithAll<Player>()   // running on every entitty that has a player script to it
            .ForEach((ref Moveble mov) =>
        {
            mov.directions = new float3(x, 0, y);
        }).Schedule();

        //Accessing the command buffer
        var ecb = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>().CreateCommandBuffer();
        Entities
            .WithAll<Player>()
            .ForEach((Entity e, ref Health hp, ref PowerCherry pill, ref Damage dmg) =>
            {
                // When cherry is picked damage will be 100 so enemy dies straight away
                dmg.value = 100;

                pill.pillTimer -= dt; // when cherry is picked time starts to decrement by time.deltaTime
                hp.invinsibleTimer = pill.pillTimer;

                //When pill timer is lees or equal to 0 than powerCherry effects will be removed    
                if (pill.pillTimer <= 0)
                {
                    ecb.RemoveComponent<PowerCherry>(e);
                    dmg.value = 0;
                }

            }).WithStructuralChanges().Run();// running on the main thread
    }
}
