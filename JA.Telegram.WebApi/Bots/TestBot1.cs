﻿using Telegram.Bot;
using Telegram.BotBuilder.Models;

namespace JA.Telegram.WebApi.Bots
{
    public class TestBot1 : BotBase
    {
        public TestBot1(string username, ITelegramBotClient client) : base(username, client)
        {
        }

        public TestBot1(string username, string token) : base(username, token)
        {
        }
    }
}
