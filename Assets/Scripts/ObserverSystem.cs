using Unity.VisualScripting;
using UnityEngine.VFX;

//functions are auto absract when in interface
public interface IWeaponHandler
{
    void PickUp();
}

public abstract class Enemy
{
    string name;
    int health;
    int atkDmg;

    public Enemy(string name, int health, int atkDmg)
    {
        this.name = name;
        this.health = health;
        this.atkDmg = atkDmg;
    }
    public abstract void Attack();
}
//classes, is-a      interface has-a
public class Goblin : Enemy,IWeaponHandler
{
    public Goblin(string name, int health, int atkDmg):base(name,health,atkDmg)
    {
       
    }

    public override void Attack()
    {
        
    }

    public void PickUp()
    {
        
    }
}
public class GameController
{
    void SpawnEnemy()
    {
        Enemy goblin = new Goblin("goblin",100,10);

    }
}