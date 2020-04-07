using System.Collections.Generic;

public class Paladin : GoodGuy
{
    public Paladin()
    {
        stats = new Stats("Paladin");
        description = "He sweared to protect and serve his kingdom, and he's willing to give his life if the King asks him to.";
        fighter_number = 0;
    }
}
