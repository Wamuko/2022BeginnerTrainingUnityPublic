using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 _direction;
    private float Speed = 20.0f;
    public float lifetime = 5f; // 弾丸の生存時間（秒）
    float timeAlive;

    // Start is called before the first frame update
    void Start()
    {
        timeAlive = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // 経過時間を更新
        timeAlive += Time.deltaTime;

        // 生存時間を超えたら弾丸を破棄
        if (timeAlive > lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        transform.position += _direction * Speed * Time.fixedDeltaTime;
    }

    public void SetDirection(Vector3 vec)
    {
        _direction = vec;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Stage"))
        {
            Destroy(this.gameObject);
        }
    }
}
