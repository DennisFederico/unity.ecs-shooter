using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace narkdagas.ecs {
    public class EcsManager : MonoBehaviour {
        private static EntityManager _manager;

        public GameObject player;
        public GameObject virusPrefab;
        public int numVirus = 500;
        public GameObject bloodCellPrefab;
        public int numBloodCells = 500;
        public GameObject bulletPrefab;
        private Entity _bulletEntity;
        public GameObject whiteBloodCellPrefab;
        public const int WhiteCellBurstCount = 100;
        public int numBullets = 10;
        private BlobAssetStore _assetStore;

        private void Start() {
            //Setup the entity system and convert the prefab to an entity
            _assetStore = new BlobAssetStore();
            _manager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var gameObjectConversionSettings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, _assetStore);
            
            //Instantiate the entities in the scene (Virus)
            var virusEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(virusPrefab, gameObjectConversionSettings);
            for (int i = 0; i < numVirus; i++) {
                float x = Random.Range(-50, 50);
                float y = Random.Range(-50, 50);
                float z = Random.Range(-50, 50);
                float3 position = new float3(x, y, z);

                var instance = _manager.Instantiate(virusEntity);
                _manager.SetComponentData(instance, new Translation {
                    Value = position
                });

                float speed = Random.Range(1, 5) / 5f;
                _manager.SetComponentData(instance, new FloatData() {
                    LinearSpeed = speed
                });
                
                //Convert the WhiteCell prefab to embed inside the virus
                var whiteCellEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(whiteBloodCellPrefab, gameObjectConversionSettings);
                _manager.SetComponentData(instance, new EntityReferenceData { EntityRef = whiteCellEntity, Seed = (uint) Random.Range(10, 1000)} );
            }

            //Instantiate the entities in the scene (BloodCells)
            var bloodCellEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(bloodCellPrefab, gameObjectConversionSettings);
            for (int i = 0; i < numBloodCells; i++) {
                float x = Random.Range(-50, 50);
                float y = Random.Range(-50, 50);
                float z = Random.Range(-50, 50);
                float3 position = new float3(x, y, z);

                quaternion rotation = Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));
                var instance = _manager.Instantiate(bloodCellEntity);
                _manager.SetComponentData(instance, new Translation { Value = position });
                _manager.SetComponentData(instance, new Rotation { Value = rotation });
                float speed = Random.Range(1, 3) / 10f;
                _manager.SetComponentData(instance, new FloatData() { LinearSpeed = speed });
            }

            _bulletEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(bulletPrefab, gameObjectConversionSettings);
        }

        private void Update() {
            if (Input.GetMouseButtonDown(0)) {
                //SHOOT UP TO NUM BULLETS
                for (int i = 0; i < numBullets; i++) {
                    var bulletInstance = _manager.Instantiate(_bulletEntity);
                    var startPost = player.transform.position + Random.insideUnitSphere * 1.5f;
                    _manager.SetComponentData(bulletInstance, new Translation { Value = startPost });
                    _manager.SetComponentData(bulletInstance, new Rotation { Value = player.transform.rotation });
                }
            }
        }

        private void OnDestroy() {
            _assetStore.Dispose();
        }
    }
}