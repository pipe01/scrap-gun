using System;

public class AK47 : Weapon
{
    public override int GetBulletDamage()
    {
        return 35;
    }

    public override float GetBulletMaxRange()
    {
        return 1000.0f;
    }

    public override float GetFireRate()
    {
        return 0.15f;
    }

    public override int GetMaximumAmmo()
    {
        return 32;
    }

    public override float GetReloadTime()
    {
        return 2.0f;
    }

    public override string GetWeaponName()
    {
        return "AK-47";
    }
}
 