using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class FollowSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var dt = Time.DeltaTime;


        Entities.WithAll<Translation,Rotation>().ForEach(( Entity e,in Follow follow) =>
        {
            if (HasComponent<Translation>(follow.target) && HasComponent<Rotation>(follow.target))
            {
                var currentPos = GetComponent<Translation>(e).Value;
                var currentRot = GetComponent<Translation>(e).Value;


                var tragetPos = GetComponent<Translation>(follow.target).Value;
                var tragetRot = GetComponent<Rotation>(follow.target).Value;

                tragetPos += math.mul(tragetRot, tragetPos) * -follow.distance;
                tragetPos += follow.offSet;


                tragetPos = math.lerp(currentPos, tragetPos, dt * follow.speedMove);

                SetComponent(e, new Translation() { Value = tragetPos });
                SetComponent(e, new Rotation() { Value = tragetRot });


            }

        }).Schedule();// getting it into the job
    }
}
