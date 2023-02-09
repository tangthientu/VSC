using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeaponBehaviour : MonoBehaviour
{
    // Start is called before the first frame update

    protected Vector3 direction; //huong cua projectile
    public float destroyAfterSeconds;//xoa sau vai giay
   protected virtual void Start()
    {
        Destroy(gameObject,destroyAfterSeconds);//pha huy projectile sau vai giay
    }

   public void directionChecker(Vector3 dir)
    {
        direction = dir;
        float dirx = direction.x;
        float diry = direction.y;

        Vector3 scale = transform.localScale;
        Vector3 rotation = transform.rotation.eulerAngles;

        if(dirx<0 && diry==0) //neu huong x nho hon 0 va y =0 thi scale x va y = -1 de projectile co the lat nguoc lai (day la phia ben trai)
        {
            scale.x = scale.y * -1;
            scale.y = scale.y * -1;
        }
        else if (dirx==0 && diry<0)//down
        {
            scale.y = scale.y * -1;
        }
        else if (dirx == 0 && diry > 0)//up
        {
            scale.x = scale.x * -1;
        }
        else if (dirx > 0 && diry > 0)//right up
        {
            rotation.z = 0f;
        }
        else if (dirx > 0 && diry < 0)//right down
        {
            rotation.z = -90f;
        }
        else if (dirx < 0 && diry > 0)//left up
        {
            scale.x = scale.y * -1;
            scale.y = scale.y * -1;
            rotation.z = -90f;
        }
        else if (dirx < 0 && diry < 0)//left down
        {
            scale.x = scale.y * -1;
            scale.y = scale.y * -1;
            rotation.z = 0f;
        }
        transform.localScale = scale;
        transform.rotation = Quaternion.Euler(rotation); 

    }
}
