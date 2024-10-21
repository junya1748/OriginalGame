using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateController : MonoBehaviour
{
    //??????
    Animator animator;

    // ??????Rigidbody2D???????
    private Rigidbody2D rb;

    //??
    private int stateNumber = 0;

    //????
    private float timeCounter = 0.0f;

    //?????
    private GameObject player;

    //?????????
    public GameObject monsterAttackPrefabLeft;
    public GameObject monsterAttackPrefabRight;

    // Start is called before the first frame update
    void Start()
    {
        // ?????Animator???
        animator = this.transform.Find("UnitRoot").gameObject.GetComponent<Animator>();

        // Rigidbody2D??????????
        rb = GetComponent<Rigidbody2D>();

        //??????????????
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // ?????????????HP?????
        TopDownCharacterController playerController = player.GetComponent<TopDownCharacterController>();

        // ????????????????????????????
        if (playerController != null && playerController.playerHP <= 0)
        {
            // ???????????????????????
            rb.velocity = Vector2.zero;
            return;  // ???Update?????
        }

        //?????
        timeCounter += Time.deltaTime;

        //???????
        switch (stateNumber)
        {
            case 0:
                {
                    //???????
                    rb.velocity = new Vector2(2.0f, rb.velocity.y);

                    // ????????? X ???????
                    float horizontalDistance = Mathf.Abs(player.transform.position.x - this.transform.position.x);

                    // ????????? Y ?????????
                    float verticalDistance = Mathf.Abs(player.transform.position.y - this.transform.position.y);

                    // ??????????
                    if (horizontalDistance < 2.0f && verticalDistance < 2.0f)
                    {
                        //???
                        timeCounter = 0.0f;

                        // ??????
                        rb.velocity = Vector2.zero;

                        //?????????
                        animator.SetTrigger("Attack");

                        // ??????????????
                        GameObject attack = Instantiate(monsterAttackPrefabRight);
                        attack.transform.position = this.transform.position + new Vector3(0.3f, 0.5f, 0.0f);

                        //???2???????
                        stateNumber = 2;
                    }

                    //????????
                    if (timeCounter > 1.0f)
                    {
                        //???
                        timeCounter = 0.0f;

                        //????
                        transform.localScale = new Vector3(-1.5f, 1.5f, 0.0f);

                        //???1??????
                        stateNumber = 1;
                    }
                }
                break;

            case 1:
                {
                    //????
                    rb.velocity = new Vector2(-2.0f, rb.velocity.y);

                    // ????????? X ???????
                    float horizontalDistance = Mathf.Abs(player.transform.position.x - this.transform.position.x);

                    // ????????? Y ??????????
                    float verticalDistance = Mathf.Abs(player.transform.position.y - this.transform.position.y);


                    // ??????????
                    if (horizontalDistance < 2.0f && verticalDistance < 2.0f)
                    {
                        //???
                        timeCounter = 0.0f;

                        // ??????
                        rb.velocity = Vector2.zero;

                        //?????????
                        animator.SetTrigger("Attack");

                        // ??????????????
                        GameObject attack = Instantiate(monsterAttackPrefabLeft);
                        attack.transform.position = this.transform.position + new Vector3(-0.3f, 0.5f, 0.0f);

                        //???2???????
                        stateNumber = 2;
                    }

                    //????????
                    if (timeCounter > 1.0f)
                    {
                        //???
                        timeCounter = 0.0f;

                        //????
                        transform.localScale = new Vector3(1.5f, 1.5f, 0.0f);

                        //???0????????
                        stateNumber = 0;
                    }
                }
                break;

            case 2:
                {   //?????????????????
                    if (timeCounter > 0.5f)
                    {
                        //???
                        timeCounter = 0.0f;

                        //???0????????
                        //stateNumber = 0;

                        //??????
                        if (this.transform.localScale.x > 0.0f)
                        {
                            //???0???????????
                            stateNumber = 0;
                        }
                        else
                        {
                            //???1???????????
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
        // ??????????? "AttackTag" ???????
        if (collision.gameObject.CompareTag("AttackTag"))
        {
            // ?????????
            animator.SetTrigger("Death");

            //???-1??????????
            stateNumber = -1;

            // ??????
            rb.velocity = Vector2.zero;

            // ????????
            //rb.isKinematic = true;

            //??????????????????????
            player.GetComponent<TopDownCharacterController>().enemyCounter--;
           
            // ??????????0.5????
            Destroy(gameObject, 0.5f);
        }
    }
}