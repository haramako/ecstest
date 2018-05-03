using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Character : MonoBehaviour
{
    public SpriteAtlas Atlas;
    public SpriteRenderer Sprite;
    float dir_;
    float rand_;

    private void Start()
    {
        dir_ = Random.Range(0, 360);
        rand_ = Random.Range(0, 1f);
    }

    static string[] sprs = new string[]
    {
        "move_0_1",
        "move_0_2",
    };

    static List<Sprite> sprCache_;

    Sprite GetSprite(int anim)
    {
        if (sprCache_ == null)
        {
            sprCache_ = new List<Sprite>();
            for (int i = 0; i < 2; i++)
            {
                sprCache_.Add(Atlas.GetSprite(sprs[i]));
            }
        }
        return sprCache_[anim];
    }

    void Update()
    {
        var g = UseGameObject.Instance.Ground;
        var pos = this.transform.localPosition;
        var h = g.SampleHeight(pos);
        pos.y = h + 0.5f;
        this.transform.localPosition = pos;

        var anim = Mathf.FloorToInt((Time.time + rand_) * 2) % 2;
        var sprite = GetSprite(anim);
        Sprite.sprite = sprite;

        var v = new Vector3(0, 0, 1);
        v = Quaternion.Euler(0, dir_, 0) * v;
        transform.localPosition += v * Time.deltaTime;

        var r = Mathf.PerlinNoise(rand_, Time.time * 0.1f);
        dir_ += r * Time.deltaTime * 80f;
    }
}
