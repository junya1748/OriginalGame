using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//namespace Cainos.PixelArtTopDown_Basic
//{
    public class TopDownCharacterController : MonoBehaviour
    {
        public float speed;

        private Animator animator;

        public GameObject attackPrefabLeft;
        public GameObject attackPrefabRight;

        // プレイヤーの初期HPを設定
        public int playerHP;

        //敵の数
        public int enemyCounter;

        //ゲーム終了時に表示するテキスト
        private GameObject stateText;

        // 通常BGMのための AudioSource
        private AudioSource bgmAudioSource;

        // クリアBGMのための AudioSource
        private AudioSource clearBgmAudioSource;

        // 攻撃サウンドのための AudioSource
        private AudioSource audioSource;

        // 通常BGMの AudioClip
        public AudioClip normalBGM;

        // クリアBGMの AudioClip
        public AudioClip clearBGM;

        // 攻撃サウンドの AudioClip
        public AudioClip attackSound;

        // ダメージを受けた時のサウンド
        public AudioClip damageSound;

        private void Start()
        {
            //この方法では取得できない
            //animator = GetComponent<Animator>();

            //１階層下のAnimatorを取得
            animator = this.transform.Find("UnitRoot").gameObject.GetComponent<Animator>();

            // 通常BGM用の AudioSource コンポーネントを取得または追加
            bgmAudioSource = gameObject.AddComponent<AudioSource>();
            bgmAudioSource.loop = true;
            bgmAudioSource.clip = normalBGM;

            // クリアBGM用の AudioSource を追加
            clearBgmAudioSource = gameObject.AddComponent<AudioSource>();
            clearBgmAudioSource.loop = true;


            // AudioSource コンポーネントを取得
            audioSource = GetComponent<AudioSource>();

            //シーン中のstateTextオブジェクトを取得
            this.stateText = GameObject.Find("GameResultText");

            // 通常のBGMを再生開始
            bgmAudioSource.Play();
        }


        private void Update()
        {
            //敵が0になった？
            if(enemyCounter <= 0)
            {
                //Debug.Log("クリアー！");

                //stateTextにGAME CLEARを表示
                this.stateText.GetComponent<Text>().text = "CLEAR!!";

                // 通常BGMが再生中なら停止して、クリアBGMを再生
                if (bgmAudioSource.isPlaying)
                {
                    bgmAudioSource.Stop();  // 通常BGMを停止
                }

                if (!clearBgmAudioSource.isPlaying && clearBGM != null)
                {
                    clearBgmAudioSource.clip = clearBGM;
                    clearBgmAudioSource.Play();  // クリアBGMを再生
                }

                return;  // ゲームクリア後は操作を無効にするため、処理をここで終了
            }
            
            //プレイヤーが生きている
            if (playerHP > 0)
            {

                Vector2 dir = Vector2.zero;

                if (Input.GetKey(KeyCode.A))
                {
                    dir.x = -1;
                    //animator.SetInteger("Direction", 3);

                    this.transform.localScale = new Vector2(1.5f, 1.5f);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    dir.x = 1;
                    //animator.SetInteger("Direction", 2);

                    this.transform.localScale = new Vector2(-1.5f, 1.5f);
                }

                if (Input.GetKey(KeyCode.W))
                {
                    dir.y = 1;
                    //animator.SetInteger("Direction", 1);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    dir.y = -1;
                    //animator.SetInteger("Direction", 0);
                }

                //ベクトルを正規化する（0または1）
                dir.Normalize();

                //animator.SetBool("IsMoving", dir.magnitude > 0);

                //Parametar名が違うので改造
                animator.SetBool("Run", dir.magnitude > 0);

                //移動させる
                GetComponent<Rigidbody2D>().velocity = speed * dir;

                //スペースキーで攻撃
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    //攻撃
                    animator.SetTrigger("Attack");

                    // 攻撃音を再生
                    if (audioSource != null && attackSound != null)
                    {
                        audioSource.PlayOneShot(attackSound);
                    }
                   
                    //向きを調べる
                    if (this.transform.localScale.x > 0.0f)
                    {
                        //左向き
                        GameObject attack = Instantiate(attackPrefabLeft);
                        //座標のコピー（このスクリプトの位置）
                        attack.transform.position = this.transform.position + new Vector3(-0.3f, 0.5f, 0.0f);
                    }
                    else
                    {
                        //右向き
                        GameObject attack = Instantiate(attackPrefabRight);
                        //座標のコピー（このスクリプトの位置）
                        attack.transform.position = this.transform.position + new Vector3(0.3f, 0.5f, 0.0f);
                    }
                }
            }
        }
        // 敵と衝突した時の処理
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // 敵にぶつかったとき
            if (collision.gameObject.CompareTag("EnemyTag"))
            {
                playerHP -= 1;  // HPを1減らす

                // HPが0になったらプレイヤーが死亡
                if (playerHP <= 0)
                {
                    //死亡アニメーションの再生
                    animator.SetTrigger("Death");

                    //【追加】走りっぱなしになる不具合があるので強制停止
                    animator.SetBool("Run", false);

                    // プレイヤーオブジェクトを廃棄
                    //Destroy(gameObject, 1.0f);

                    //stateTextにGAME OVERを表示
                    this.stateText.GetComponent<Text>().text = "GAME OVER";
            }
                else
                {
                    //【追加】ダメージアニメーションの再生
                    animator.SetTrigger("Damage");
                }
            }
        }

        //敵の攻撃を受けた時の処理
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // 衝突したオブジェクトが "MonsterAttackTag" タグを持つ場合
            if (collision.gameObject.CompareTag("MonsterAttackTag"))
            {
                playerHP -= 1;  // HPを1減らす

                // 攻撃を受けた時の効果音を再生
                if (audioSource != null && damageSound != null)
                {
                    audioSource.PlayOneShot(damageSound);
                }

                // HPが0になったらプレイヤーが死亡
                if (playerHP <= 0)
                {
                    //死亡アニメーションの再生
                    animator.SetTrigger("Death");

                    //【追加】走りっぱなしになる不具合があるので強制停止
                    animator.SetBool("Run", false);

                    // プレイヤーオブジェクトを廃棄
                    //Destroy(gameObject, 1.0f);

                    //ゲームオーバーの表示
                    //Debug.Log("GAME OVER");

                    //stateTextにGAME OVERを表示
                    this.stateText.GetComponent<Text>().text = "GAME OVER";
            }
                else
                {
                    //【追加】ダメージアニメーションの再生
                    animator.SetTrigger("Damage");
                }
            }
        }
    }
//}