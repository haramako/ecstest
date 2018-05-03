using UnityEngine;
using UnityEngine.U2D;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using toinfiniityandbeyond.Rendering2D;

public class UseEcsManager : MonoBehaviour
{
    public static UseEcsManager Instance;
    public Camera Camera;
    public Terrain Ground;
    public SpriteAtlas Atlas;


    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        var entityManager = World.Active.GetOrCreateManager<EntityManager>();

        var sprites = new Sprite[2];
        sprites[0] = Atlas.GetSprite("move_0_1");
        sprites[1] = Atlas.GetSprite("move_0_2");

        //Assign loaded sprites to sprite renderers
        var renderers = new[]
        {
            new SpriteInstanceRenderer(sprites[0]),
            new SpriteInstanceRenderer(sprites[1]),
        };
        //Spawn 10,000 entities with random positions/rotations
        var len = 10.0f;
        for (int i = 0; i < 30000; i++)
        {
            var entity = entityManager.CreateEntity(ComponentType.Create<Position>(),
                                                    ComponentType.Create<Heading>(),
                                                    ComponentType.Create<TransformMatrix>(),
                                                    ComponentType.Create<CharacterComponent>());

            var pos = new float3(Random.Range(-len, len), 0, Random.Range(-len, len));
            entityManager.SetComponentData(entity, new Position
            {
                Value = pos,
            });

            entityManager.SetComponentData(entity, new Heading
            {
                Value = new float3(Random.value, 0, Random.value)
            });

            entityManager.SetComponentData(entity, new CharacterComponent
            {
                rand_ = Random.Range(0, 1f),
                dir_ = Random.Range(0, 360f),
            });

            entityManager.AddSharedComponentData(entity, renderers[i % 2]);
        }

    }

    public void Update()
    {
        Vector3 v = Vector3.zero;
        var speed = 3.0f;
        if (Input.GetKey(KeyCode.A))
        {
            v += new Vector3(-speed, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            v += new Vector3(speed, 0, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            v += new Vector3(0, 0, -speed);
        }
        if (Input.GetKey(KeyCode.W))
        {
            v += new Vector3(0, 0, speed);
        }
        Camera.transform.localPosition += v * Time.deltaTime;
    }
}
