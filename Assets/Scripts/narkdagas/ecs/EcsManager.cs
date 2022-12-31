using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace narkdagas.ecs {
    public class EcsManager : MonoBehaviour {
        public static EntityManager Manager;

        //EntityCommandBuffer ??
        public GameObject player;
        public GameObject virusPrefab;
        public int numVirus = 500;
        public GameObject bloodCellPrefab;
        public int numBloodCells = 500;
        public GameObject bulletPrefab;
        private Entity bulletEntity;
        public int numBullets = 10;
        private BlobAssetStore _assetStore;

        private void Start() {
            //Setup the entity system and convert the prefab to an entity
            _assetStore = new BlobAssetStore();
            Manager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var gameObjectConversionSettings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, _assetStore);

            //Instantiate the entities in the scene (Virus)
            var virusEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(virusPrefab, gameObjectConversionSettings);
            for (int i = 0; i < numVirus; i++) {
                float x = Random.Range(-50, 50);
                float y = Random.Range(-50, 50);
                float z = Random.Range(-50, 50);
                float3 position = new float3(x, y, z);

                var instance = Manager.Instantiate(virusEntity);
                Manager.SetComponentData(instance, new Translation {
                    Value = position
                });

                float rspeed = Random.Range(1, 5) / 5f;
                Manager.SetComponentData(instance, new FloatData() {
                    Speed = rspeed
                });
            }

            //Instantiate the entities in the scene (BloodCells)
            var bloodCellEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(bloodCellPrefab, gameObjectConversionSettings);
            for (int i = 0; i < numBloodCells; i++) {
                float x = Random.Range(-50, 50);
                float y = Random.Range(-50, 50);
                float z = Random.Range(-50, 50);
                float3 position = new float3(x, y, z);

                var instance = Manager.Instantiate(bloodCellEntity);
                Manager.SetComponentData(instance, new Translation {
                    Value = position
                });

                float rspeed = Random.Range(1, 3) / 10f;
                Manager.SetComponentData(instance, new FloatData() {
                    Speed = rspeed
                });
            }

            bulletEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(bulletPrefab, gameObjectConversionSettings);
        }

        private void Update() {
            if (Input.GetMouseButtonDown(0)) {
                //SHOOT UP TO NUM BULLETS
                for (int i = 0; i < numBullets; i++) {
                    var bulletInstance = Manager.Instantiate(bulletEntity);
                    var startPost = player.transform.position + Random.insideUnitSphere * 1.5f;
                    Manager.SetComponentData(bulletInstance, new Translation { Value = startPost });
                    Manager.SetComponentData(bulletInstance, new Rotation { Value = player.transform.rotation });
                }
            }
        }

        private void OnDestroy() {
            _assetStore.Dispose();
        }
    }
}