using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartBotUI;
using SmartBotUI.Settings;

namespace SmartBotUI.Mulligan
{
	[Serializable]
    public class bMulliganProfile : MulliganProfile
    {
		public bMulliganProfile() : base()
        {
            
        }
		
		public override List<Card> HandleMulligan(List<Card> Choices, CClass opponentClass, CClass ownClass)
        {
			List<Card> CardsToKeep = new List<Card>();
			List<string> WhiteList = new List<string>();
			List<string> BlackList = new List<string>();
			int MaxManaCost = 1;

			bool HasCoin = Choices.Any(x => x.Name == "GAME_005");
			
			bool HasOneDrop = Choices.Any(x => x.Cost == 1);
			bool HasTwoDrop = Choices.Any(x => x.Cost == 2);
			
			WhiteList.Add("EX1_029");//Leper Gnome
			WhiteList.Add("FP1_004");//Mad Scientist
			
			WhiteList.Add("CS2_188");//Abusive Sergeant
			
			if(HasOneDrop)
				WhiteList.Add("NEW1_031");//Animal Companion

			BlackList.Add("CS2_084");//Hunter's Mark
			
			foreach(Card c in Choices)
			{
				if(BlackList.Contains(c.Name))
					continue;
				
				if(WhiteList.Contains(c.Name))
				{
					CardsToKeep.Add(c);
					continue;
				}
				
				if(MaxManaCost > c.Cost)
					CardsToKeep.Add(c);
			}
			
            return CardsToKeep;
        }
    }
}
