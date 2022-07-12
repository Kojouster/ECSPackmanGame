using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


public class SpawnSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref Spawner spawner, in Translation trans, in Rotation rot) =>
        {
            if (!EntityManager.Exists(spawner.spawnObject))
            {

               spawner.spawnObject = EntityManager.Instantiate(spawner.spawnPrefab);
                // Setting up the position and rotation of spawning object it will be the same as spawner pos
                EntityManager.SetComponentData(spawner.spawnObject, trans);
                EntityManager.SetComponentData(spawner.spawnObject, rot);

            }

        }).WithStructuralChanges().Run();


    }
}
