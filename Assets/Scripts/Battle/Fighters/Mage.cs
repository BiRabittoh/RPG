using System.Collections.Generic;

public class Mage : GoodGuy
{
    public Mage()
    {
        stats = new Stats("Mage");
        description = "She's a skilled healer, a bit weak against physical attacks. Remember to keep her healthy or she might die on you when you least expect it.";
        fighter_number = 3;
    }
}
