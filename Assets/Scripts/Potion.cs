using UnityEngine;

public class Potion : Item
{
    private int healthRestored;
    private float duration;

    // getters
    public int getHealthRestored()
    {
        return healthRestored;
    }

    public float getDuration()
    {
        return duration;
    }

    // Méthode spéficique
    public override void UseItem(PlayerCharacter player)
    {
        // Logique d'utilisation d'une potion
        //player.RestoreHealth(healthRestored);
    }
}
