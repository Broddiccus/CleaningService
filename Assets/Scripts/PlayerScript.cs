using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//handles all player movements
public class PlayerScript : MonoBehaviour
{
    //SINGLETON VARIABLES
    public static PlayerScript Instance;
    //GAMEPLAY VARIABLES
    public float speed = 1.0f;
    public float dashMax = 5.0f;
    public float dashMoveSpeed = 3.0f;
    public float minimumDist = 0.0f;
    public bool dashing = false;
    public Animator animationCon;
    private Vector3 startLoc;
    private float dashSpeed = 0.0f;
    private float useSpeed;
    
    //======================================================================
    //SINGLETON INITIALIZATION
    void SingletonInit()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }
    private void Start()
    {
        GameManager.Instance.onGameRestart += Reset;
        SingletonInit();
        useSpeed = speed;
        startLoc = transform.position;
    }
    //======================================================================
    //GAMEPLAY
    private void Update()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.Gameplay)
        {
            if (dashing)
            {
                transform.position += transform.right * dashSpeed * Time.deltaTime;
                if (dashSpeed > 0)
                    dashSpeed -= Time.deltaTime * 2;
                if (dashSpeed <= 0)
                {
                    
                    dashSpeed = 0.0f;
                    dashing = false;
                    GameManager.Instance.comboGain = true;
                    useSpeed = speed;
                    GameManager.Instance.comboMulitplier /= 2;
                }
            }
            if (transform.position.x > minimumDist)
                transform.position += transform.right * -1.0f * Time.deltaTime;
        }
    }
    public void Walk(Vector3 pos)
    {
        float tempS = 0.0f;
        if (transform.position.z + 0.1f < pos.z)
            tempS = useSpeed;
        if (transform.position.z - 0.1f > pos.z)
            tempS = -useSpeed;
        transform.position += transform.forward * tempS * Time.deltaTime;
    }
    public void Dash()
    {
        if (!dashing)
        {
            AudioManager.Instance.DashSFX();
            dashing = true;
            DashAnim();
            dashSpeed = dashMax;
            useSpeed = dashMoveSpeed;
            GameManager.Instance.comboMulitplier *= 2;
            GameManager.Instance.comboGain = true;
        }
    }
    public void Reset()
    {
        transform.position = startLoc;
        useSpeed = speed;

    }
    public void ChangeColliderScale(float inc)
    {
        BoxCollider temp = gameObject.GetComponent<BoxCollider>();
        temp.size = new Vector3(temp.size.x + inc, temp.size.y, temp.size.z + inc);
    }
    //======================================================================
    //ANIMATION FUNCTIONS
    public void DashResetAnim()
    {
        animationCon.SetBool("Dash", false);
        animationCon.SetBool("Run", true);

    }
    public void DamageAnim()
    {
        animationCon.SetBool("Dash", false);
        animationCon.SetBool("Run", false);
        animationCon.SetBool("Damage",true);
    }
    public void DamageResetAnim()
    {
        animationCon.SetBool("Dash", true);
        animationCon.SetBool("Run", true);
        animationCon.SetBool("Damage", false);
    }
    public void DashAnim()
    {
        animationCon.SetBool("Dash", true);
        animationCon.SetBool("Run", false);
    }
    //======================================================================
}
