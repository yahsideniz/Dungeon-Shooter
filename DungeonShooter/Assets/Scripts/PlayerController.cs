using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    //Hareket i�in
    public float moveSpeed;
    private Vector2 moveInput;
    public Rigidbody2D RB;

    //Aim
    public Transform gunArm;

    //Animasyon
    public Animator anim;

    //Gecici Yenilmezlik
    public SpriteRenderer bodySR;

    //Dash
    private float activeMoveSpeed;
    public float dashSpeed = 8f, dashLength = .5f, dashCooldown = 1f, dashInvincibility = .5f;
    [HideInInspector] // Bundan sonraki deger ne olursa olsun inspecterda g�stermez
    public float dashCounter;
    private float dashCoolCounter;


    //Level bitiminde haraketin durmasi
    [HideInInspector]
    public bool canMove = true;

    //Silah degisme
    public List<Gun> availableGuns = new List<Gun>();
    [HideInInspector]
    public int currentGun;



    private void Awake()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        //Dash
        activeMoveSpeed = moveSpeed;

        //UI Silah
        UIController.instance.currentGun.sprite = availableGuns[currentGun].gunUI;
        UIController.instance.gunText.text = availableGuns[currentGun].weaponName;
    }

    void Update()
    {
        //Level bitiminde haraket etmemesi icin canMove sonradan ekledim
        //Sonrada pause kismini ekledim
        if(canMove && !LevelManager.instance.isPaused)
        {


                // Hareket kodlar�
                moveInput.x = Input.GetAxisRaw("Horizontal"); // Horizontal paketini atad�k  a-d tu�lar�na
                moveInput.y = Input.GetAxisRaw("Vertical"); // w-s tu�lar�n� aktif ettik
                moveInput.Normalize();

                RB.velocity = moveInput * activeMoveSpeed; // time.deltaTime kullanmad�k velocity burada kendisi hesapl�yor onu zaten.


                //Aim
                Vector3 mousePos = Input.mousePosition;
                Vector3 screenPoint = CameraController.instance.mainCamera.WorldToScreenPoint(transform.localPosition); // karakterin sahnedeki yeri




                //Y�n degistirme, mause ne taraftaysa karakterin kafas� o tarafa bak�yo
                if(mousePos.x < screenPoint.x) // mouse karakterin solundaysa
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f); // Scale'i -1 yaparsak silah tam tersi y�ne d�necek. Sagdan sola yani
                    gunArm.localScale = new Vector3(-1f, -1f, 1f); // Silah koluda kafayla ayn� tarafta olcak
                }
                else
                {
                    transform.localScale = Vector3.one; 
                    gunArm.localScale = Vector3.one;  
                }



                //Rotate gun arm
                Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y); //Matematik islemi yap�yoruz mause ile silah�n(karakterin) aras�ndaki mesafe buluyoruz
                float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg; //Radius to degree
                gunArm.rotation = Quaternion.Euler(0, 0, angle); //Z degeri degismeli


                //Silah degistirme
                if(Input.GetKeyDown(KeyCode.Tab))
                {
                    if (availableGuns.Count > 0)
                    {
                        currentGun++;
                        if(currentGun >= availableGuns.Count)
                        {
                             currentGun = 0;
                        }
                         SwitchGun();
                    }
                    else
                    {
                        Debug.LogError("Player has no guns!!!");
                    }
                }




                //Dash
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    if(dashCoolCounter <= 0 && dashCounter <=0)
                    {
                        activeMoveSpeed = dashSpeed;
                        dashCounter = dashLength;

                        //Animasyonu
                        anim.SetTrigger("dash");

                        //Dash atarken hasar almama
                        PlayerHealthController.instance.MakeInvincible(dashInvincibility);

                        //Dash muzik
                        AudioManager.instance.PlaySFX(8);

                    }

                }

                if(dashCounter > 0)
                {
                    dashCounter -= Time.deltaTime;
                    if(dashCounter <= 0)
                    {
                        activeMoveSpeed = moveSpeed;
                        dashCoolCounter = dashCooldown;
                    }
                }

                if(dashCoolCounter > 0)
                {
                    dashCoolCounter -= Time.deltaTime;
                }






                //Animasyon
                if(moveInput != Vector2.zero) // 0,0 i�te x ve y de�erleri
                {
                    anim.SetBool("isMoving", true);
                }
                else
                {
                    anim.SetBool("isMoving", false);

                }

        } else
        {
            RB.velocity = Vector2.zero; // fiziksel islemleri s�f�rlad�k
            anim.SetBool("isMoving", false);
        }



    }


    //Silah degistirme
    public void SwitchGun()
    {
        foreach (Gun theGun in availableGuns)
        {
            theGun.gameObject.SetActive(false);
        }

        availableGuns[currentGun].gameObject.SetActive(true);

        //UI Silah
        UIController.instance.currentGun.sprite = availableGuns[currentGun].gunUI;
        UIController.instance.gunText.text = availableGuns[currentGun].weaponName;
    }
}
