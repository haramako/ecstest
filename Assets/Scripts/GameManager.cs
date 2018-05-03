using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public Terrain Ground;
    public Camera Camera;

    public GameObject CharacterBase;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        var len = 10.0f;
        for (int i = 0; i < 30000; i++)
        {
            var pos = new Vector3(Random.Range(-len, len), 0, Random.Range(-len, len));
            var c = (GameObject)Instantiate(CharacterBase, pos, Quaternion.identity);
            c.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
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
