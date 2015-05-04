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
            WhiteList.Add("CS2_146");//Southsea Deckhand, ID: CS2_146
            WhiteList.Add("EX1_010");//Worgen Infiltrator, ID: EX1_010
            WhiteList.Add("CS2_188");//Abusive Sergeant
			WhiteList.Add("FP1_004");//Mad Scientist
			WhiteList.Add("GVG_043");//Glaivezooka			 			
			
			if (HasOneDrop)
				WhiteList.Add("NEW1_031");//Animal Companion

            if (opponentClass == CClass.WARRIOR)
                WhiteList.Add("CS2_203");//Ironbeak Owl                

            if (HasCoin)
            {
                WhiteList.Add("FP1_002"); //Haunted Creeper
                WhiteList.Add("NEW1_019");//Knife Juggler
            }

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
