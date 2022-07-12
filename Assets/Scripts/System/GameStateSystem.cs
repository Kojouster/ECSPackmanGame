using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.Physics;

[AlwaysUpdateSystem]
public class GameStateSystem : SystemBase
{
    protected override void OnUpdate()
    {
        //Way of creating pallet query
        GetEntityQuery(typeof(Pellet));
        //creating a query for different entitites
        var pelletQuery = GetEntityQuery(ComponentType.ReadOnly<Pellet>());
        var playerQuery = GetEntityQuery(ComponentType.ReadOnly<Player>());
        var enemyQuery = GetEntityQuery(ComponentType.ReadOnly<Enemy>());
        var spawnerQuery = GetEntityQuery(typeof(Spawner));

        var pelleteCount = pelletQuery.CalculateEntityCount();

        if (pelletQuery.CalculateEntityCount() >= 0)  // checking the number of entities if its less than or equal to 0 than player wins
        {

            GameManager.Instance.SetPelletCount(pelleteCount);
            // when the game is won all physicsVelocity components will be removed from entities,it was made so all the objects stop moving when the game is won
            if (pelleteCount == 0) 
            {
                Entities
                        .WithAll<PhysicsVelocity>()
                        .ForEach((Entity e) =>
                        {
                            
                            EntityManager.RemoveComponent<PhysicsVelocity>(e);

                        }).WithStructuralChanges().Run();
            


            }
        
        }


        Entities
            .WithAll<Player, PhysicsVelocity>()
            .ForEach((Entity e, in Kill kill) =>
            {
                EntityManager.RemoveComponent<PhysicsVelocity>(e);
                EntityManager.RemoveComponent<Moveble>(e); // removing moveble component so player stops moving when he is dead
                GameManager.Instance.LoseLife();

                if (GameManager.Instance.lives < 0)
                {

                    //running this logic when our lives are at 0
                    var spawnerArray = spawnerQuery.ToEntityArray(Allocator.TempJob);

                    foreach (Entity spawnerEntity in spawnerArray)
                    {
                        EntityManager.DestroyEntity(spawnerEntity);
                    }
                    spawnerArray.Dispose();
                }
                var enemyArray = enemyQuery.ToEntityArray(Allocator.TempJob);

                foreach (Entity enemyEntity in enemyArray)
                {
                    EntityManager.AddComponentData(enemyEntity, kill);
                    EntityManager.RemoveComponent<PhysicsVelocity>(enemyEntity);
                    EntityManager.RemoveComponent<Moveble>(enemyEntity); // removing moveble component so player stops moving when he is dead
                    EntityManager.RemoveComponent<OnKill>(enemyEntity);
                }
                enemyArray.Dispose();

            }).WithStructuralChanges().Run();
    }
}
