using UnityEngine;
using System.Collections;

public class LivingThing : MonoBehaviour {

    public string name;

    private int _healthPoints = 100;
    public int healthPoints
    {
        get
        {
            return _healthPoints;
        }
    }
    public int maxHealth = 100;
    public int healthPercentage { get { return (_healthPoints / maxHealth) * 100; } }

    public bool invincible = false;

    public delegate void DeathDelegate();
    public event DeathDelegate Death;
    public void Kill()
    {
        if (Death != null)
            Death();
    }

    void BulletHitHandle(object[] args)
    {
        if ((GameObject)args[1] == gameObject)
        {
            Damage((int)args[2]);
        }
    }

    public void Damage(int points)
    {
        if (!invincible && points > 0)
        {
            _healthPoints -= points;
            if (_healthPoints <= 0)
            {
                _healthPoints = 0;
                Kill();
            }
        }
    }

    public void DamageFast(int points)
    {
        _healthPoints -= points;
    }

    public void Heal(int points)
    {
        _healthPoints = Mathf.Clamp(healthPoints + points, 0, maxHealth);
    }

    public void HealFast(int points)
    {
        _healthPoints += points;
    }
}
