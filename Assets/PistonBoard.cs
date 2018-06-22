using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Piston
{
    private GameObject self;
    private int movement;
    private float target;
    private bool impossibleElevation;
    private Rigidbody rb;
    private float speed;
    private float x;
    private float y;


    public Piston(GameObject self, Vector3 position, float speed, float x, float y)
    {
        this.x = x;
        this.y = y;
        this.self = Object.Instantiate(self, position, Quaternion.identity);
        this.rb = this.self.GetComponent<Rigidbody>();
        this.speed = speed;
    }

    public void GoTo(float elevation)
    {
        /*target = elevation;
        if (elevation > self.transform.position.y)
        {
            rb.velocity = Vector3.up * speed;
            movement = 1;
        }
        else
        {
            rb.velocity = Vector3.down * speed;
            movement = -1;
        }
        if (float.IsInfinity(elevation) || float.IsNaN(elevation))
        {
            impossibleElevation = true;
            self.SetActive(false);
        }
        else
        {
            self.SetActive(true);
            //self.transform.position += Vector3.up * elevation;
        }*/
        self.SetActive(true);
        if (float.IsInfinity(elevation) || float.IsNaN(elevation))
        {
            impossibleElevation = true;
            self.SetActive(false);
            return;
        }
        self.transform.position = new Vector3(self.transform.position.x, elevation, self.transform.position.z);
    }

    public void Update()
    {
        /*if (self.transform.position.y > target && movement == 1
            || self.transform.position.y < target && movement == -1)
        {
            rb.velocity = Vector3.zero;
        }*/
        GoTo(PistonBoard.Instance.calc(x, y, Time.time));
    }
}

public class PistonBoard : MonoBehaviour {
    public static PistonBoard Instance;
    public float scale;
    public float xScale;
    public float yScale;
    public float calc(float x, float y, float time)
    {
        x = x * xScale;
        y = y * yScale;
        return Mathf.Sin(10 * (x * x + y * y) + time) * scale;
        //return Mathf.Sin(5 * x + time) * Mathf.Cos(5 * y + time) * scale;
        //return 1f / (x * x + y * y) * scale;
        //return scale * ((1 - Mathf.Sign(-x - .9f + Mathf.Abs(y * 2))) / 3 * (Mathf.Sign(.9f - x) + 1) / 3) * (Mathf.Sign(x + .65f) + 1) / 2 - ((1 - Mathf.Sign(-x - .39f + Mathf.Abs(y * 2))) / 3 * (Mathf.Sign(.9f - x) + 1) / 3) + ((1 - Mathf.Sign(-x - .39f + Mathf.Abs(y * 2))) / 3 * (Mathf.Sign(.6f - x) + 1) / 3) * (Mathf.Sign(x - .35f) + 1) / 2;
    }

    public GameObject pistonPrefab;
    public int width = 15;
    public int height = 15;
    public float spacing = 0.2f;
    public float speed = 5;

    private Piston[,] pistons;

    Vector2 ToCoord(float i, float j)
    {
        return new Vector2(width / 2 - i, height / 2 - j);
    }

	// Use this for initialization
	void Start () {
        pistons = new Piston[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 mathPos = ToCoord(i, j);
                float x = mathPos.x, 
                      y = mathPos.y;
                pistons[i,j] = new Piston(pistonPrefab, pistonPrefab.transform.position + Vector3.forward * (i + spacing * i) + Vector3.right * (j + spacing * j), speed,x,y);
                pistons[i, j].GoTo(calc(x, y, Time.time));
            }
        }
        pistonPrefab.SetActive(false);
        if(Instance == null)
            Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                pistons[i, j].Update();
            }
        }
    }
}
