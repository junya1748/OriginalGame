using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackContorollerRight : MonoBehaviour
{
    Rigidbody2D rd; //Rigidbodyオブジェクト
    float attspeed = 6.0f;  //オブジェクト移動スピード

    void Start()
    {
        rd = GetComponent<Rigidbody2D>();   //Rigidbodyコンポーネント取得

        //ｎ秒後に破棄　※残ってしまうので
        Destroy(gameObject, 0.2f);
    }

    void Update()
    {
        rd.velocity = new Vector2(attspeed, 0); //スピードをつけて攻撃オブジェクトを移動
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Enemy以外
        if (other.gameObject.tag != "EnemyTag")
        {
            Destroy(gameObject);    //攻撃オブジェクトの破棄
        }
    }
}