using System.Collections.Generic;

public class Boxer : GoodGuy
{
    public Boxer()
    {
        stats = new Stats("Boxer");
        description = "He's tough, he can withstand a lot of damage, but maybe his fists are not too accurate.";
        fighter_number = 2;
    }
}
