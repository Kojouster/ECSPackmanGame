using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class CollectibleSystem : SystemBase
{
    protected override void OnUpdate()
    {
        // used for struct changes by creating command buffer
        var ecb = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>().CreateCommandBuffer();
        Entities
              .WithAll<Player>()
              .ForEach((Entity playerEntity,DynamicBuffer<TriggerBuffer> tBuffer) =>
              {
                  // iterating through each buffer
                  for (int i = 0; i < tBuffer.Length; i++)
                  {
                      var e = tBuffer[i].entity;
                      // checking if it has collectible component but yet has no kill component, so it wont cause errors in the console
                      if (HasComponent<Collectible>(e) && !HasComponent<Kill>(e))
                      {
                          ecb.AddComponent(e, new Kill() { Timer = 0 });
                          GameManager.Instance.AddScore(GetComponent<Collectible>(e).points);
                      }

                      if (HasComponent<PowerCherry>(e) && !HasComponent<Kill>(e))
                      {
                          ecb.AddComponent(playerEntity, GetComponent<PowerCherry>(e));
                          ecb.AddComponent(e, new Kill() { Timer = 0 });
                      }

                  }



              }).WithStructuralChanges().Run();


    }
}
