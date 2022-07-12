using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[DisallowMultipleComponent]
public class CameraAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public AudioListener audioListener;
    public Camera cam;
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new CameraTag() { });
        
        conversionSystem.AddHybridComponent(audioListener);// converting audio listener to ecs
        //converting camera comp from mono to ecs world,so it can render the game,otherwise camera will show a black screen
        conversionSystem.AddHybridComponent(cam);


    }
}
