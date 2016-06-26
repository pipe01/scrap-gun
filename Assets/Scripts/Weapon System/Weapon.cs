using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public abstract class Weapon
{

    public abstract int GetBulletDamage();

    public abstract string GetWeaponName();

    public abstract int GetMaximumAmmo();

    public abstract float GetFireRate();

    public abstract float GetBulletMaxRange();

    public abstract float GetReloadTime();
}
