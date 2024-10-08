﻿using GTANetworkAPI;
using Server.AccountInfo;
using System.Threading;

namespace Server.Vehicles.RentBikes
{
    public class RentBikeCommand : Script
    {
        [Command("rentbike")]
        public void RentBike(Player player)
        {
            if(RentBikesDictionary.GetRentBike(player.Vehicle).Client != null)
            {
                return;
            }

            if(AccountHandlerDictionary.GetAccount(player).IsPlayerRentingBike == true)
            {
                player.SendChatMessage("~r~[Ошибка]~w~:Вы уже арендуете велосипед!");
                return;
            }

            if(AccountHandlerDictionary.GetAccount(player).Money < 50)
            {
                player.SendChatMessage("~r~[Ошибка]~w~:У вас недостаточно денег!");
                return;
            }

            RentBikesDictionary.GetRentBike(player.Vehicle).Client = player;
            MoneyTransaction.TakeMoney(AccountHandlerDictionary.GetAccount(player), 50);
            AccountHandlerDictionary.GetAccount(player).IsPlayerRentingBike = true;

            player.SetData<Vehicle>("player_renting_bike", player.Vehicle);

            player.SendChatMessage("~g~[Велопарк]~w~:~w~Вы успешно арендовали велосипед");
            NAPI.ClientEvent.TriggerClientEvent(player, "SERVER:CLIENT::PLAYER_VEHICLE_UNFREEZE");
        }
    }
}
