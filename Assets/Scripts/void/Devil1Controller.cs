using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devil1Controller : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        //この方法では取得できない
        //animator = GetComponent<Animator>();

        //１階層下のAnimatorを取得
        animator = this.transform.Find("UnitRoot").gameObject.GetComponent<Animator>();
    }

    private void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 衝突したオブジェクトが "AttackTag" タグを持つ場合
        if (collision.gameObject.CompareTag("AttackTag"))
        {
           //死亡
           animator.SetTrigger("Death");

           // もし敵を倒したあと敵のオブジェクトを削除
           Destroy(gameObject, 0.5f);
        }
    }
}