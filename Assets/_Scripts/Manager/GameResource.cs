public static class GameResource 
{
    private static int goldAmount;

    public static void AddGoldAmount(int amount)
    {
        goldAmount += amount;
    }

    public static bool TrySpendGold(int amount)
    {
        if (goldAmount <= amount)
        {
            goldAmount -= amount;
            EventHandler.WhenGoldAmountChanged();
            return true;
        }
        else return false;
    }

    public static int GetGoldAmount() 
    { 
        return goldAmount; 
    }
}
