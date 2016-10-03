﻿using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using VerifyBot.Service;
using VerifyBot.Services;

namespace VerifyBot
{
    public class Program
    {
        private ConfigurationService configService;

        private DiscordSocketClient client;

        private static void Main(string[] args) => new Program().Run().GetAwaiter().GetResult();

        public async Task Run()
        {
            try
            {
                this.CheckIfDatabaseExists();

                this.configService = new ConfigurationService();
                this.client = new DiscordSocketClient();

                var config = this.configService.GetConfiguration();

                var verify = new WorldVerificationService(this.client, config);
                var reverify = new ReverifyService(this.client, config);

                await client.LoginAsync(TokenType.Bot, Helper.SecretsReader.GetSecret("discord_token"));                
                await client.ConnectAsync();

                var me = await this.client.GetCurrentUserAsync();

                client.MessageReceived += async (message) =>
                {
                    if (message.Author.IsBot)
                    {
                        return;
                    }
                    
                    await verify.Process(message);                    
                };

                while (true)
                {
                    var line = Console.ReadLine();

                    if (line.Equals("reverify"))
                    {
                        Console.WriteLine("Reverifying...");
                        await reverify.Process(null);
                    }

                    if (line.Equals("quit"))
                    {
                        this.client.Dispose();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Aplication crashing. Reason: {ex}");
            }
        }

        private void CheckIfDatabaseExists()
        {
            var path = System.IO.Path.Combine(AppContext.BaseDirectory, "Users.db");

            if (!System.IO.File.Exists(path))
            {
                Console.WriteLine("Database does not exist. Run the following command: dotnet ef database update");
                throw new Exception("No Database");
            }
        }
    }
}