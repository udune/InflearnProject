namespace _00_Scripts.Manager
{
    public class PlayerManager
    {
        public int Level;
        public double Exp;
        public double Attack = 10;
        public double Hp = 50;
        public float CriticalPercentage = 20.0f;
        public double CriticalDamage = 140.0d;
    
        public void ExpUp()
        {
            Exp += float.Parse(CSV_Importer.EXP[Level]["Get_EXP"].ToString());
            Attack += Next_ATK();
            Hp += Next_HP();
            if (Exp >= float.Parse(CSV_Importer.EXP[Level]["EXP"].ToString()))
            {
                Level++;
                MainUI.instance.TextCheck();
            }

            foreach (var player in Spawner.players)
            {
                player.SetAttackHp();
            }
        }

        public float EXP_Percentage()
        {
            float exp = float.Parse(CSV_Importer.EXP[Level]["EXP"].ToString());
            double myExp = Exp;
            if (Level >= 1)
            {
                exp -= float.Parse(CSV_Importer.EXP[Level - 1]["EXP"].ToString());
                myExp -= float.Parse(CSV_Importer.EXP[Level - 1]["EXP"].ToString());
            }

            return (float)myExp / exp;
        }

        public float Next_Exp()
        {
            float exp = float.Parse(CSV_Importer.EXP[Level]["EXP"].ToString());
            float myExp = float.Parse(CSV_Importer.EXP[Level]["Get_EXP"].ToString());
            if (Level >= 1)
            {
                exp -= float.Parse(CSV_Importer.EXP[Level - 1]["EXP"].ToString());
            }

            return (myExp / exp) * 100.0f;
        }

        public double Next_ATK()
        {
            return float.Parse(CSV_Importer.EXP[Level]["Get_EXP"].ToString()) * (Level + 1) / 5;
        }

        public double Next_HP()
        {
            return float.Parse(CSV_Importer.EXP[Level]["Get_EXP"].ToString()) * (Level + 1) / 3;
        }

        public double Get_ATK(Rarity rarity)
        {
            return Attack * ((int)rarity + 1);
        }

        public double Get_HP(Rarity rarity)
        {
            return Hp * ((int)rarity + 1);
        }

        public double AverageAttack()
        {
            return Attack + Hp;
        }
    }
}
