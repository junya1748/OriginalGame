using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonstorStateController : MonoBehaviour
{
    //アニメーター
    Animator animator;

    // モンスターのRigidbody2Dコンポーネント
    private Rigidbody2D rb;

    //状態
    private int stateNumber = 0;

    //タイマー
    private float timeCounter = 0.0f;

    //プレイヤー
    private GameObject player;

    //アタックプレファブ
    public GameObject monsterAttackPrefabLeft;
    public GameObject monsterAttackPrefabRight;

    // Start is called before the first frame update
    void Start()
    {
        // １階層下のAnimatorを取得
        animator = this.transform.Find("UnitRoot").gameObject.GetComponent<Animator>();

        // Rigidbody2Dコンポーネントを取得
        rb = GetComponent<Rigidbody2D>();

        //プレイヤーオブジェクトの取得
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //タイム計測
        timeCounter += Time.deltaTime;

        //状態の振り分け
        switch (stateNumber)
        {
            case 0:
                {
                    //最初は右に動く
                    rb.velocity = new Vector2(2.0f, rb.velocity.y);

                    // プレイヤーと自分の X 相対値（絶対）
                    float horizontalDistance = Mathf.Abs(player.transform.position.x - this.transform.position.x);

                    // プレイヤーと自分の Y 軸相対距離（絶対）
                    float verticalDistance = Mathf.Abs(player.transform.position.y - this.transform.position.y);

                    // プレイヤーが近いか？
                    if (horizontalDistance < 2.0f && verticalDistance < 2.0f)
                    {
                        //クリア
                        timeCounter = 0.0f;

                        // 動きを止める
                        rb.velocity = Vector2.zero;

                        //攻撃アニメーション
                        animator.SetTrigger("Attack");

                        // 右向きの攻撃プレファブを生成
                        GameObject attack = Instantiate(monsterAttackPrefabRight);
                        attack.transform.position = this.transform.position + new Vector3(0.3f, 0.5f, 0.0f);

                        //状態を2にする（攻撃）
                        stateNumber = 2;
                    }

                    //１秒経過したか？
                    if (timeCounter > 1.0f)
                    {
                        //クリア
                        timeCounter = 0.0f;

                        //左を向く
                        transform.localScale = new Vector3(-1.5f, 1.5f, 0.0f);

                        //状態を1にする（次）
                        stateNumber = 1;
                    }
                }
                break;

            case 1:
                {
                    //左に動く
                    rb.velocity = new Vector2(-2.0f, rb.velocity.y);

                    // プレイヤーと自分の X 相対値（絶対）
                    float horizontalDistance = Mathf.Abs(player.transform.position.x - this.transform.position.x);

                    // プレイヤーと自分の Y 軸相対距離（絶対値）
                    float verticalDistance = Mathf.Abs(player.transform.position.y - this.transform.position.y);


                    // プレイヤーが近いか？
                    if (horizontalDistance < 2.0f && verticalDistance < 2.0f)
                    {
                        //クリア
                        timeCounter = 0.0f;

                        // 動きを止める
                        rb.velocity = Vector2.zero;

                        //攻撃アニメーション
                        animator.SetTrigger("Attack");

                        // 左向きの攻撃プレファブを生成
                        GameObject attack = Instantiate(monsterAttackPrefabLeft);
                        attack.transform.position = this.transform.position + new Vector3(-0.3f, 0.5f, 0.0f);

                        //状態を2にする（攻撃）
                        stateNumber = 2;
                    }

                    //１秒経過したか？
                    if (timeCounter > 1.0f)
                    {
                        //クリア
                        timeCounter = 0.0f;

                        //右を向く
                        transform.localScale = new Vector3(1.5f, 1.5f, 0.0f);

                        //状態を0にする（ループ）
                        stateNumber = 0;
                    }
                }
                break;

            case 2:
                {   //時間経過（アニメーションが終わり）
                    if (timeCounter > 0.5f)
                    {
                        //クリア
                        timeCounter = 0.0f;

                        //状態を0にする（ループ）
                        //stateNumber = 0;

                        //向きを調べる
                        if (this.transform.localScale.x > 0.0f)
                        {
                            //状態を0にする（右移動ループ）
                            stateNumber = 0;
                        }
                        else
                        {
                            //状態を1にする（左移動ループ）
                            stateNumber = 1;
                        }
                    }
                }
                break;

            default: break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 衝突したオブジェクトが "AttackTag" タグを持つ場合
        if (collision.gameObject.CompareTag("AttackTag"))
        {
            // 死亡アニメーション
            animator.SetTrigger("Death");

            //状態を-1にする（何もしない）
            stateNumber = -1;

            // 動きを止める
            rb.velocity = Vector2.zero;

            // 動きを完全に停止
            //rb.isKinematic = true;

            // オブジェクトを削除（0.5秒後に）
            Destroy(gameObject, 0.5f);
        }
    }
}