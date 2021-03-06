/*Smarbot Profile for fate hunter
* Deck from : http://www.hearthpwn.com/decks/224225-brm-legend-in-1-day-face-hunter
* Contributors : Wirmate
*/

using System.Linq;

namespace SmartBot.Plugins.API
{
	public class bProfile : RemoteProfile
	{
		private int MinionEnemyTauntValue = 5;

		private int HeroEnemyHealthValue = 6;
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

		//HeroPowerCost
		private int HeroPowerGlobalCost = 0;

		//Weapons Attack cost
		private int WeaponAttackGlobalCost = 0;

		//GlobalValueModifier
		private int GlobalValueModifier = 0;
		
		public override float GetBoardValue(Board board)
		{
			float value = 0;

			//Hero friend value
			value += board.HeroFriend.CurrentHealth * HeroFriendHealthValue + board.HeroFriend.CurrentArmor * HeroFriendHealthValue;

			//Hero enemy value
			value -= board.HeroEnemy.CurrentHealth * HeroEnemyHealthValue + board.HeroEnemy.CurrentArmor * HeroEnemyHealthValue;

			value -= board.MinionEnemy.FindAll(x => x.IsTaunt).Count * MinionEnemyTauntValue;
			
			//Friend board
			foreach (Card card in board.MinionFriend)
				value += card.CurrentHealth * MinionFriendHealthValue + card.CurrentAtk * MinionFriendAttackValue;

			//Enemy board
			foreach (Card card in board.MinionEnemy)
				value -= card.CurrentHealth * MinionEnemyHealthValue + card.CurrentAtk * MinionEnemyAttackValue;

			//casting costs
			value -= MinionCastGlobalCost;
			value -= SpellsCastGlobalCost;
			value -= WeaponCastGlobalCost;

			//casting action value
			value += WeaponCastGlobalValue;
			value += SpellsCastGlobalValue;
			value += MinionCastGlobalValue;

			//heropower vost
			value -= HeroPowerGlobalCost;

			//Weapon attack cost
			value -= WeaponAttackGlobalCost;

			if (board.HeroEnemy.CurrentHealth <= 0)
				value += 10000;
			
			if(board.HeroFriend.CurrentHealth <= 0)
				value -= 100000;

			value += GlobalValueModifier;
			
			return value;
		}

		public override void OnCastMinion(Board board, Card minion, Card target)
		{
			switch (minion.Template.Id)
			{
				case Card.Cards.NEW1_019://Knife Juggle
					if (board.Hand.FindAll(x => x.Template.Id == Card.Cards.NEW1_019).Count < 2)
						MinionCastGlobalCost += 10;
					break;
				case Card.Cards.CS2_203://Ironbeak Owl
					if(target != null && !target.IsTaunt)
						MinionCastGlobalCost += 16;
					else if(target != null && target.Template.Id == Card.Cards.EX1_402 && !target.IsSilenced)//Armorsmith
						MinionCastGlobalValue += 5;
					else
						MinionCastGlobalCost += 6;
					break;
				case Card.Cards.CS2_188://Abusive Sergeant
					MinionCastGlobalCost += 5;
					break;
				case Card.Cards.EX1_116://Leeroy Jenkins
					MinionCastGlobalCost += 30;
					break;
				case Card.Cards.FP1_004://Mad Scientist
					if(board.TurnCount <= 2)
						MinionCastGlobalValue += 10;
					break;
			}
		}

		public override void OnCastSpell(Board board, Card spell, Card target)
		{
			switch (spell.Template.Id)
			{
				case Card.Cards.EX1_538://Unleash the Hounds
					SpellsCastGlobalCost += 20;
					break;
				case Card.Cards.BRM_013://Quick Shot
					if(board.Hand.Count > 0 && target.Type == Card.CType.HERO)
						SpellsCastGlobalCost += 20;
					else if(board.Hand.Count > 0 && target.Type == Card.CType.MINION)
						SpellsCastGlobalCost += 14;
					else
						SpellsCastGlobalValue += 5;
					break;
				case Card.Cards.CS2_084://Hunter's Mark
					SpellsCastGlobalCost += 16;
					break;
				case Card.Cards.NEW1_031://Animal Companion
					SpellsCastGlobalValue += 10;
					break;
				case Card.Cards.EX1_539://Kill Command
					SpellsCastGlobalCost += 31;
					break;
				case Card.Cards.GAME_005://The Coin
					SpellsCastGlobalCost += GetCoinValue(board);
					break;
				case Card.Cards.EX1_610://Explosive Trap
				    SpellsCastGlobalCost += 3;
					if(!board.MinionFriend.Any(x => x.IsTaunt))
						SpellsCastGlobalValue += board.MinionEnemy.FindAll(x => x.CurrentHealth <= 2 && !x.IsDivineShield).Count * 2;
					break;
				case Card.Cards.EX1_554://Snake Trap
					if(board.Hand.Any(x => x.Template.Id == Card.Cards.NEW1_019) && board.MinionFriend.Count == 0)
						SpellsCastGlobalValue += 8;
					else if(board.MinionFriend.Any(x => x.Template.Id == Card.Cards.NEW1_019 &&	 !x.IsSilenced))
						SpellsCastGlobalValue += 8;
					else
						SpellsCastGlobalValue += 3;
					break;

			}
		}

		public override void OnCastWeapon(Board board, Card weapon, Card target)
		{
			switch (weapon.Template.Id)
			{
				case Card.Cards.EX1_536://Eaglehorn Bow
					break;

				case Card.Cards.GVG_043://Glaivezooka
					break;
			}
		}

		public override void OnAttack(Board board, Card attacker, Card target)
		{
			bool IsAttackingWithHero = (attacker.Id == board.HeroFriend.Id);
			bool IsAttackingWithWeapon = (board.WeaponFriend != null && attacker.Id == board.WeaponFriend.Id);

			if ((IsAttackingWithHero || IsAttackingWithWeapon) && board.WeaponFriend != null)//If we attack with weapon equipped
			{
				switch (board.WeaponFriend.Template.Id)
				{
					case Card.Cards.EX1_536://Eaglehorn Bow

						if (board.Secret.Count > 0 && board.WeaponFriend.CurrentDurability <= 1 && !board.HasWeaponInHand())
							WeaponAttackGlobalCost += 20;
						else if (board.Hand.Any(x => x.Template.IsSecret)  && board.WeaponFriend.CurrentDurability <= 1 && !board.HasWeaponInHand())
							WeaponAttackGlobalCost += 20;
						else
							WeaponAttackGlobalCost += 8;

						break;
					case Card.Cards.GVG_043://Glaivezooka
						WeaponAttackGlobalCost += 3;
						break;
				}
			}
			
			if(!IsAttackingWithHero && !IsAttackingWithWeapon)
			{
				if(target != null && target.CurrentAtk >= attacker.CurrentHealth && !attacker.IsDivineShield)
					OnMinionDeath(board,attacker);
			}
		}
		public override void OnCastAbility(Board board, Card ability, Card target)
		{
			HeroPowerGlobalCost += 10;
		}

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

			ret.HeroPowerGlobalCost = HeroPowerGlobalCost;
			ret.WeaponAttackGlobalCost = WeaponAttackGlobalCost;
			
			ret.GlobalValueModifier = GlobalValueModifier;

			return ret;
		}

		public int GetCoinValue(Board board)
		{
			int currentMana = board.MaxMana;
			bool HasDropCurrentTurn = (board.GetPlayables(currentMana, currentMana).Count != 0);
			bool HasDropNextTurn = (board.GetPlayables(currentMana + 1, currentMana + 1).Count != 0);

			if (HasDropCurrentTurn)
			{
				if(board.MaxMana == 1 && board.GetPlayables(1, 1).Count == 2)
					return 0;
				return 8;
			}

			if (!HasDropCurrentTurn && HasDropNextTurn)
			{
				bool CanPlayOnCurve = ((board.GetPlayables(currentMana + 1, currentMana + 1).Count > 1) && (board.GetPlayables(currentMana + 2, currentMana + 2).Count >= 1));

				if (CanPlayOnCurve)
				{
					return 0;
				}
				return 8;
			}

			return 5;
		}
		
		public void OnMinionDeath(Board board,Card minion)
		{
			switch (minion.Template.Id)
			{
				case Card.Cards.EX1_029://Leper Gnome
					GlobalValueModifier += HeroEnemyHealthValue*2;
					break;
			}
		}
	}
}