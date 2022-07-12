using UnityEngine;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
public class CollisionSystem : SystemBase
{

    private struct CollisionSystemJob : ICollisionEventsJob
    {
     
        //Allows us to get a specific type from the entity
        public BufferFromEntity<CollisionBuffer> collisions;
        //Think of buffer like an array for entities
        public void Execute(CollisionEvent collisionEvent) // like monobehavior on collision enter
        {
            if (collisions.HasComponent(collisionEvent.EntityA))
                collisions[collisionEvent.EntityA].Add(new CollisionBuffer() { entity = collisionEvent.EntityB }); // Way of knowing when the collision has happened
            if (collisions.HasComponent(collisionEvent.EntityB))
                collisions[collisionEvent.EntityB].Add(new CollisionBuffer() { entity = collisionEvent.EntityA });
        }
    }


    private struct TriggerSystemJob : ITriggerEventsJob // same as Monobehavoir OnTriggerEnter
    {
        public BufferFromEntity<TriggerBuffer> triggers;
        public void Execute(TriggerEvent triggerEvent)
        {
            if (triggers.HasComponent(triggerEvent.EntityA))
                triggers[triggerEvent.EntityA].Add(new TriggerBuffer() { entity = triggerEvent.EntityB }); // Way of knowing when the collision has happened
            if (triggers.HasComponent(triggerEvent.EntityB))
                triggers[triggerEvent.EntityB].Add(new TriggerBuffer() { entity = triggerEvent.EntityA });
        }
    }
    protected override void OnUpdate()
    {
        var pw = World.GetOrCreateSystem<BuildPhysicsWorld>().PhysicsWorld;
        var sim = World.GetOrCreateSystem<StepPhysicsWorld>().Simulation;

        Entities.ForEach((DynamicBuffer<CollisionBuffer> collisions) =>
        {
            collisions.Clear();


        }).Run();// running it on the main thread

        var colJobHandle = new CollisionSystemJob()
        {
            collisions = GetBufferFromEntity<CollisionBuffer>() // populating collisions as it was empty

        }.Schedule(sim, ref pw, this.Dependency);

        colJobHandle.Complete();


        ////////////////////////////////////////////////////////////////////////////////////////////////////

        Entities.ForEach((DynamicBuffer<TriggerBuffer> triggers) =>
        {
            triggers.Clear();


        }).Run();// running it on the main thread

        var trigJobHandle = new TriggerSystemJob()
        {
            triggers = GetBufferFromEntity<TriggerBuffer>() // populating triggrs as it was empty

        }.Schedule(sim, ref pw, this.Dependency);

        trigJobHandle.Complete();
    }
}
    



