using System.Collections.Generic;

public class Archer : GoodGuy
{
    public Archer()
    {
        stats = new Stats("Archer");
        description = "She's a trained markswoman who can snipe anybody from 1km across, she's also trained for basic healing in case of emergency.";
        fighter_number = 1;
    }
}
