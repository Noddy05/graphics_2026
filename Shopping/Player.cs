using Graphics2026.Model.Game.BuildTools;
using Graphics2026.Shopping.Tools;
using OpenTK.Mathematics;

namespace Graphics2026.Shopping
{
    internal static class Player
    {
        public readonly static Builder builder;
        private static long balance;

        static Player()
        {
            builder = new Builder();
            builder.AddTool(new GroundTool());
            builder.AddTool(new WallTool());
        }

        public static long GetBalance() => balance; 
        public static bool CanAfford(int cost) => balance >= cost;

        public static void SetBalance(long newBalance) => balance = newBalance;

        public static void ChangeBalance(int difference)
        {
            balance += difference;
        }

        public static void ChangeBalance(int difference, Vector3 location)
        {
            ChangeBalance(difference);
            if (difference >= 0)
            {
                Console.WriteLine("+" + difference);
            }
            else
            {
                Console.WriteLine("-" + (-difference));
            }
        }
    }
}
