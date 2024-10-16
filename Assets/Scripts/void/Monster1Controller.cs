using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster1Controller : MonoBehaviour
{
    Animator animator;

    // モンスターの移動速度
    public float moveSpeed = 1.0f;
    // 現在の移動方向
    private float moveDirection = 1.0f;
    // モンスターのRigidbody2Dコンポーネント
    private Rigidbody2D rb;

    // 方向転換するとキャラが小さくなるのを防ぐためモンスターの初期スケールを保存
    private Vector3 initialScale;

    // モンスターが死亡したかどうかを管理
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        // １階層下のAnimatorを取得
        animator = this.transform.Find("UnitRoot").gameObject.GetComponent<Animator>();

        // Rigidbody2Dコンポーネントを取得
        rb = GetComponent<Rigidbody2D>();

        // モンスターの初期スケールを取得して保存
        initialScale = transform.localScale;

        // 1秒後に初回呼び出し、その後ランダムな間隔で方向変更
        InvokeRepeating("ChangeDirection", 1f, Random.Range(2f, 4f));
    }

    private void Update()
    {
        if (!isDead) // モンスターが死亡していない場合のみ移動
        {
            // モンスターを左右に移動
            rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
        }
    }

    // ランダムな方向にモンスターを移動させる
    private void ChangeDirection()
    {
        // モンスターが死亡していない場合のみ方向変更
        if (!isDead)
        {
            // 左右ランダムに方向を決定
            moveDirection = Random.Range(0, 2) == 0 ? -1.0f : 1.0f;

            // モンスターの向きを変更
            transform.localScale = new Vector3(initialScale.x * moveDirection, initialScale.y, initialScale.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 衝突したオブジェクトが "AttackTag" タグを持つ場合
        if (collision.gameObject.CompareTag("AttackTag"))
        {
            // 死亡アニメーション
            animator.SetTrigger("Death");

            // 動きを止める
            isDead = true;
            rb.velocity = Vector2.zero;

            // 動きを完全に停止
            rb.isKinematic = true;

            // オブジェクトを削除（0.5秒後に）
            Destroy(gameObject, 0.5f);
        }
    }
}