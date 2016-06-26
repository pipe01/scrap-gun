using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class WeaponLogic : MonoBehaviour
{

    public Weapon currentWeapon = new AK47();

    private GameObject weaponObject;
    
    public string reloadAnimationName;
    public string shootAnimationName;
    public string weaponName;
    public bool enableInput = true;
    public bool shootFromCamera = true;

    private int currentAmmo = 0;
    private AudioSource[] audioSources;
    private float lastShotTime = 0f;
    private float tmpReloadTime;
    //private Task reloadTask;
    private bool reloading = false;
    private SpriteRenderer weaponRenderer;

    void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform item = transform.GetChild(i);
            if (item.name == "weaponParent" && item.childCount == 1)
            {
                weaponObject = item.gameObject;
                weaponRenderer = weaponObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
                break;
            }
            else if (item.name == "weaponParent" && item.childCount == 0)
            {
                enabled = false;
            }
        }

        if (currentWeapon != null)
        {
            currentAmmo = currentWeapon.GetMaximumAmmo();
        }
    }

    void Update()
    {
        if (currentWeapon != null)
        {
            UpdateInput();
        }
    }

    void UpdateInput()
    {

        if (!PauseMenu.Shown)
        {
            //Aim player at mouse
            //which direction is up
            Vector3 upAxis = new Vector3(0, 0, -1);
            Vector3 mouseScreenPosition = Input.mousePosition;

            //set mouses z to your targets
            mouseScreenPosition.z = weaponObject.transform.position.z;
            Vector3 mouseWorldSpace = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            weaponObject.transform.LookAt(mouseWorldSpace, upAxis);

            //zero out all rotations except the axis I want
            float z = -weaponObject.transform.eulerAngles.z - 90;
            weaponObject.transform.eulerAngles = new Vector3(0, 0, z);

            //if the weapon passed -270 (up) or -450 degrees (down), flip
            if (z < -270)
            {
                weaponRenderer.flipY = false;
            }
            else if (z > -450)
            {
                weaponRenderer.flipY = true;
            }


            //fire button
            bool fire = Input.GetButton("Fire1");

            //reload button
            bool reload = Input.GetButtonDown("Reload");

            //if the input is enabled, the fire button is pressed and we aren't reloading
            if (enableInput && fire && !reloading)
            {
                //fire a bullet
                fireBullet(mouseWorldSpace);
            }

            //if the reload button is pressed and we aren't reloading
            if (reload && !reloading)
            {
                //reload
                Reload();
            }
        }
    }

    public bool fireBullet(Vector2 target)
    {
        //if the current time minus the time we last shot is more or equal to the fire rate
        if (Time.time - lastShotTime >= currentWeapon.GetFireRate())
        {
            //set last shot time to the current time
            lastShotTime = Time.time;

            //if we have any remaining ammo
            if (currentAmmo > 0)
            {
                //unload one bullet
                currentAmmo--;

                //if we are out of ammo after unloading a bullet, invoke the event Player.WeaponOoA
                if (currentAmmo == 0)
                {
                    EventManager.InvokeEvent("Player.WeaponOoA", this);
                }
                //but still shoot because this is happening before actually firing
            }
            else {
                //if we don't have ammo left, don't continue
                return false;
            }

            Vector2 origin = weaponObject.transform.position;
            Vector2 end = target;

            RaycastHit2D hit = Physics2D.Raycast(origin, end, currentWeapon.GetBulletMaxRange());
            //print(target);

            //drawMyLine(origin, hit.point, Color.yellow, 0.025f, 0.05f);
            drawMyLine(origin, end, Color.yellow, 1f, 0.05f);
            
            /*RaycastHit2D hit = new RaycastHit2D();
            Vector3 origin = head.transform.position;
            Vector3 end;

            hit = Physics2D.Raycast(origin, target, weaponma);

            if (ishit)
            {
                //print ("HIT");
                end = hit.point;
                EventManager.InvokeEvent("World.BulletHit", hit.point, hit.transform.gameObject, bulletDamage, this);
            }
            else {
                //return false;
                //print ("NOTHIT");
                end = transform.forward * bulletRange;
            }*/

            //audioSources[0].Play();

            //we succesfully shot
            return true;
        }

        //we can't fire yet because of the fire rate
        return false;
    }
    public void fireBullet()
    {
        //fire a bullet forwards
        fireBullet(transform.forward);
    }

    void drawMyLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f, float width = 0.1f)
    {

        StartCoroutine(drawLine(start, end, color, duration, width));

    }

    public void Reload(int bullets, bool instant = false)
    {
        //if it's instant, don't wait
        if (instant)
        {
            reloading = false;

            //add the specified bullets to the ammo, if there are more bullets than there are
            //missing, set to the weapon's maximum ammo
            currentAmmo = Mathf.Clamp(currentAmmo + bullets, 0, currentWeapon.GetMaximumAmmo());
        }
        else //not instant, so invoke the waiting method and then reload
        {
            reloading = true;
            //audioSources[1].Play();
            ReloadWait(bullets);
        }
    }
    public void Reload(bool instant = false)
    {
        //by default, reload to the weapon's maximum ammo
        Reload(currentWeapon.GetMaximumAmmo(), instant);
    }

    Vector3 LerpByDistance(Vector3 A, Vector3 B, float x)
    {
        Vector3 P = x * Vector3.Normalize(B - A) + A;
        return P;
    }

    void ReloadWait(int bullets)
    {
        //start the waiting coroutine and then reload
        StartCoroutine(ReloadWait(currentWeapon.GetReloadTime(), bullets));
    }
    IEnumerator ReloadWait(float waitTime, int bullets)
    {
        //wait for the reload time
        yield return new WaitForSeconds(waitTime);

        //instantly reload
        Reload(bullets, true);

        //not reloading anymore
        reloading = false;
    }

    IEnumerator drawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f, float width = 0.1f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Additive"));
        lr.SetColors(color, color);
        lr.SetWidth(width, width);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        yield return new WaitForSeconds(duration);
        GameObject.Destroy(myLine);
    }

}