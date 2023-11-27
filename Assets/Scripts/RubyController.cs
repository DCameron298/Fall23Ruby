using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;
    
    public int maxHealth = 5;
    public float timeInvincible = 2.0f;

    public AudioClip throwSound;
    public AudioClip hitSound;
    AudioSource audioSource;

    public int score;
    public TextMeshProUGUI scoreText;
    public GameObject loseTextObject;
    public GameObject winTextObject;
    public GameObject leveltwoTextObject;
    
    public TextMeshProUGUI cogText;

    public int level;
    
    public int health { get { return currentHealth; }}
     public int currentHealth;
    
    bool isInvincible;
    float invincibleTimer;
    
    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
    Vector2 lookDirection = new Vector2(1,0);
    Animator animator;
    
    private bool playerLose;
    private bool playerWin;
    private bool leveltwo;

    public GameObject projectilePrefab;
    public GameObject hitEffect;
    public GameObject healthEffect;

    public AudioClip JambiSpeak;
    public AudioClip NoCogs;
    public AudioClip Victory;
    public AudioClip Defeat;
    public int cog;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
         audioSource = GetComponent<AudioSource>();
         cog = 4;
         score = 0;
         level = 1;

         playerLose = false;
         playerWin = false;
         leveltwo = false;

        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);
        leveltwoTextObject.SetActive(false);
        
    
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        Vector2 move = new Vector2(horizontal, vertical);
        if (Input.GetKeyDown(KeyCode.X))
{
    RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
   if (hit.collider != null)
{
    NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
    if (character != null)
    {
        character.DisplayDialog();
        PlaySound(JambiSpeak);
    }  
}
}

if (score == 5)
{
    if(Input.GetKeyDown(KeyCode.R))
    {
        SceneManager.LoadScene("T3");
    }

    
}

if (cog == 0)
{
    PlaySound(NoCogs);
}

        if(Input.GetKeyDown(KeyCode.C) && (cog >= 1))
{
   Launch();
   PlaySound(throwSound);

   cog -= 1;
    cogText.text = "Cogs: " + cog.ToString();
}

if (currentHealth == 0)
{
speed = 0;
playerLose = true;
loseTextObject.SetActive(true);
PlaySound(Defeat);
}

 if(Input.GetKey(KeyCode.R))
            {
                if(currentHealth == 0)
                {
                    Application.LoadLevel(Application.loadedLevel);
                }
            }

            if ((score == 4) && (Input.GetKeyDown(KeyCode.X)))
            {
                            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
                if (hit.collider != null)
                {
                    NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                    if (character != null)
                    {
                        SceneManager.LoadScene("Level2");
                    }
                }
            }
if (score == 4)
{
    leveltwo = true;
    leveltwoTextObject.SetActive(true);
    level = 2;
    cog = 4;
}
        
if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
{
    lookDirection.Set(move.x, move.y);
    lookDirection.Normalize();
}
        
animator.SetFloat("Look X", lookDirection.x);
animator.SetFloat("Look Y", lookDirection.y);
animator.SetFloat("Speed", move.magnitude);
        
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
   
   if ((score == 4))
   {
playerWin = true;
winTextObject.SetActive(true);
speed = 0;
PlaySound(Victory);
   }

   if((level == 2) && (currentHealth == 0) && (Input.GetKey(KeyCode.R)))
            {
                SceneManager.LoadScene("T3");
                loseTextObject.SetActive(true);
            }
   
   
   
   
    }
    
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            animator.SetTrigger("Hit");
            if (isInvincible)
                return;
            
            isInvincible = true;
            invincibleTimer = timeInvincible;
           GameObject hitEffectObject = Instantiate(hitEffect, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
            PlaySound(hitSound);
        }

        if (amount > 0)
        {
            GameObject healthEffectObject = Instantiate(healthEffect, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        }
        
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log(currentHealth + "/" + maxHealth);
          
        
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    void Launch()
 {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300f);

        animator.SetTrigger("Launch");
    }
     public void PlaySound(AudioClip throwSound)
    {
        audioSource.PlayOneShot(throwSound);
    }

   public void ChangeScore(int amount)
    {
        score += 1;
        scoreText.text = "Robots Fixed: " + score.ToString();

       
        
       

    }

    public void ChangeCog()
    {
        cog += 4;
        cogText.text = "Cogs: " + cog.ToString();

    }

    public void ChangeSpeed()
{
speed +=4;

}    
    
}