using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Start is called before the first frame update
    CharacterScriptableObject characterData;
    
    public float currentHealth;
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentRecovery;
    [HideInInspector]
    public float currentMight;
    [HideInInspector]
    public float currentProjectileSpeed;
    [HideInInspector]
    public float currentPickupRange;
    //vu khi khoi diem
    public List<GameObject> spawnedWeapons;

    [Header("I-frames")]//khung hinh bat hoai
    public float inviciblityDuration;
    float invicibilityTimer;
    bool isInvicible;


    [Header("Experience/Level")]
    public int experience = 0;//kinh nghiem
    public int level = 1;//cap do
    public int experienceCap;// gioi han thanh kinh nghiem
    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;//level bat dau
        public int endLevel;//level ket thuc
        public int experienceCapIncrease;
    }
    public List<LevelRange> levelRanges;

    private void Awake()
    {
        characterData = CharacterSelector.GetData();
        CharacterSelector.instance.DestroySingleton();
        //gan' bien
        currentHealth = characterData.MaxHealth;
        currentMoveSpeed = characterData.MoveSpeed;
        currentMight = characterData.Might;
        currentProjectileSpeed = characterData.ProjectileSpeed;
        currentRecovery = characterData.Recovery;
        currentPickupRange = characterData.PickupRange;
        //spawn vu khi
        SpawnWeapon(characterData.StartingWeapon);
    }

    private void Start()
    {
        experienceCap = levelRanges[0].experienceCapIncrease;// chinh cho gioi han level de len cap khong = 0
        
    }
    private void Update()
    {
        if(invicibilityTimer>0)//neu thoi gian bat hoai la lon hon 0 thi tru no xuong tu tu cho den khi = 0
        {
            invicibilityTimer-=Time.deltaTime;

        }
        else if(isInvicible)
        {
            isInvicible = false;//reset trang thai bat hoai
        }

        Recover();
    }
    public void IncreaseExperience(int amount)
    {
        experience += amount;//kinh nghiem duoc + bang so luong nhan dc
        LevelUpChecker();// kiem tra level 
    }
    public void LevelUpChecker()// trong khoang tu startLevel den endLevel thi level cap se tang len
    {
        if(experience>=experienceCap)//neu kn hien tai lon hon gioi han kn
        {
            level++;// thi len cap
            experience-=experienceCap;// so kinh nghiem bay gio se bang so kinh nghiem bi du khi len cp
            int experienceCapIncrease = 0;
            foreach (LevelRange range in levelRanges)
            {
                if(level >= range.startLevel && level <= range.endLevel) // tu level bat dau den ket thuc
                {
                    experienceCapIncrease = range.experienceCapIncrease;//tang gioi han kinh nghiem
                    break;
                }
            }
            experienceCap+=experienceCapIncrease;

        }
    }
    public void TakeDamage(float dmg)
    {
        if(!isInvicible)//trong khung hinh bat hoai thi khong nhan sat thuong
        { 
            currentHealth -= dmg;//tru mau = damage khi goi method take dmg(nhan sat thuong)
            invicibilityTimer = inviciblityDuration;//timer = duration
            isInvicible = true;//bat hoai = true
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Kill();
            }
        }

    }
    public void Kill()
    {
        Debug.Log("player died");
    }

    public void RestoreHealth(float amount)
    {
        if(currentHealth<characterData.MaxHealth)// chi hoi mau neu nguoi choi co mau hein tai nho hon mau toi da
        {
            currentHealth+=amount;//hoi mau bang so amount
            if(currentHealth>characterData.MaxHealth)//chac chan rang mau nguoi choi khong vuot mau toi da
            {
                currentHealth = characterData.MaxHealth;
            }
        }
    }
    public void Recover()
    {
        if(currentHealth<characterData.MaxHealth)
        {
            currentHealth+=currentRecovery*Time.deltaTime;
            if (currentHealth > characterData.MaxHealth)//chac chan rang mau nguoi choi khong vuot mau toi da
            {
                currentHealth = characterData.MaxHealth;
            }
        }
    }

    public void SpawnWeapon(GameObject weapon)//spawn ra vu khi khoi dau
    {
        

        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);//spawn vu khi
        spawnedWeapon.transform.SetParent(transform);// chinh cho vu khi la con cua nguoi choi
        spawnedWeapons.Add(spawnedWeapon);// them vao danh sach vu khi da spawn
    }
   
}
