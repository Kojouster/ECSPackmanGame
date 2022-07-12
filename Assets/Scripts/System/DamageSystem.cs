using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

public class DamageSystem : SystemBase
{
    protected override void OnUpdate()
    {

        var dt = Time.DeltaTime;
        Entities.ForEach((DynamicBuffer<CollisionBuffer> col, ref Health hp) =>
        {

            for (int i = 0; i < col.Length; i++) // we want to check that if the collision has damage component
            {
                if (hp.invinsibleTimer <= 0 && HasComponent<Damage>(col[i].entity)) // checking if entity has damage component in the array
                {
                    //Getting damage component
                    hp.value -= GetComponent<Damage>(col[i].entity).value;
                    hp.invinsibleTimer = 1; // getting value to 1 so the player has some time to react


                }
            
            }

        }).Schedule();

        Entities
            .WithNone<Kill>() // this logic will run with entities that have health component to them but dont have a kill component on them which is our player
            .ForEach((Entity e ,ref Health hp) =>
        {
            hp.invinsibleTimer -= dt;
            if (hp.value < 0) // if health is less than 0 we are setting a kill timer
                EntityManager.AddComponentData(e, new Kill() { Timer = hp.killTimer });// kill timer is always 1 so every frame it will be always set to 1

        }).WithStructuralChanges().Run(); // hop it on the main thread and use whthstructuralChanges so we don't have any console errors


        var ecbSys = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        var ecb = ecbSys.CreateCommandBuffer(); 


        Entities.ForEach((Entity e, ref Kill kill, in Translation trns, in Rotation rot) =>
        {

           

            kill.Timer -= dt; // Than timer is being decremented by delta time 
            if (kill.Timer <= 0) // if timer is less than 0 request onkill and destroys the entity 
            {
                ecb.DestroyEntity(e);
                if (HasComponent<OnKill>(e))
                {
                    var onKill = GetComponent<OnKill>(e);
                    AudioManager.inst.PlayMusicSfx(onKill.sfxName.ToString());
                    GameManager.Instance.AddScore(onKill.pointValue);

                    if (EntityManager.Exists(onKill.spawnPrefab))
                    {

                        var spawnedEntity = ecb.Instantiate(onKill.spawnPrefab);
                        ecb.AddComponent(spawnedEntity, trns);
                        ecb.AddComponent(spawnedEntity, rot);
                    }
                }
            }
                
            
            



        }).WithStructuralChanges().Run();

      
    }
}
