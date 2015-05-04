namespace SmartBot.Plugins.API
{
    public class bProfile : RemoteProfile
    {
        private int HeroEnemyHealthValue = 2;
        private int HeroFriendHealthValue = 2;
        
        private int MinionEnemyAttackValue = 2;
        private int MinionEnemyHealthValue = 2;
        private int MinionFriendAttackValue = 2;
        private int MinionFriendHealthValue = 2;

        //Spells cast cost
        private int SpellsCastGlobalCost = 0;
        //Spells cast value
        private int SpellsCastGlobalValue = 0;

        //Weapons cast cost
        private int WeaponCastGlobalCost = 0;
        //Weapons cast value
        private int WeaponCastGlobalValue = 0;

        //Minions cast cost
        private int MinionCastGlobalCost = 0;
        //Minions cast value
        private int MinionCastGlobalValue = 0;


        public override float GetBoardValue(Board board)
        {
            float value = 0;

            //Hero friend value
            value += board.HeroFriend.CurrentHealth*HeroFriendHealthValue;

            //Hero enemy value
            value -= board.HeroEnemy.CurrentHealth*HeroEnemyHealthValue;

            //Friend board
            foreach (Card card in board.MinionFriend)
                value += card.CurrentHealth*MinionFriendHealthValue + card.CurrentAtk*MinionFriendAttackValue;

            //Enemy board
            foreach (Card card in board.MinionEnemy)
                value -= card.CurrentHealth*MinionEnemyHealthValue + card.CurrentAtk*MinionEnemyAttackValue;

            //casting costs
            value -= MinionCastGlobalCost;
            value -= SpellsCastGlobalCost;
            value -= WeaponCastGlobalCost;

            //casting action value
            value += WeaponCastGlobalValue;
            value += SpellsCastGlobalValue;
            value += MinionCastGlobalValue;

            return value;
        }

        public override void OnCastMinion(Board board, Card minion, Card target)
        {
            switch (minion.Template.Id)
            {
                    /*case Card.Cards.CRED_01:
                    MinionCastGlobalCost += 10;
                    if(goodconditions)
                       MinionCastGlobalValue += 30;
                    break;*/
            }
        }

        public override void OnCastSpell(Board board, Card spell, Card target)
        {
             switch (spell.Template.Id)
            {
                    /*case Card.Cards.CRED_01:
                    SpellsCastGlobalCost += 50;
                    if(goodconditions)
                       SpellsCastGlobalValue += 30;
                    break;*/
            }
        }

        public override void OnCastWeapon(Board board, Card weapon, Card target)
        {
            switch (weapon.Template.Id)
            {
                    /*case Card.Cards.CRED_01:
                    WeaponCastGlobalCost += 50;
                    if(goodconditions)
                       WeaponCastGlobalValue += 30;
                    break;*/
            }
        }

        public override void OnAttack(Board board, Card attacker, Card target) {}
        public override void OnCastAbility(Board board, Card ability, Card target) {}

        public override RemoteProfile DeepClone()
        {
            bProfile ret = new bProfile();
            ret.HeroEnemyHealthValue = HeroEnemyHealthValue;
            ret.HeroFriendHealthValue = HeroFriendHealthValue;
            ret.MinionEnemyAttackValue = MinionEnemyAttackValue;
            ret.MinionEnemyHealthValue = MinionEnemyHealthValue;
            ret.MinionFriendAttackValue = MinionFriendAttackValue;
            ret.MinionFriendHealthValue = MinionFriendHealthValue;

            ret.SpellsCastGlobalCost = SpellsCastGlobalCost;
            ret.SpellsCastGlobalValue = SpellsCastGlobalValue;
            ret.WeaponCastGlobalCost = WeaponCastGlobalCost;
            ret.WeaponCastGlobalValue = WeaponCastGlobalValue;
            ret.MinionCastGlobalCost = MinionCastGlobalCost;
            ret.MinionCastGlobalValue = MinionCastGlobalValue;
            return ret;
        }
    }
}