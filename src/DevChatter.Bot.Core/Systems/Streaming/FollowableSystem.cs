using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Systems.Streaming
{
    public class FollowableSystem : IFollowableSystem
    {
        private const int TOKENS_FOR_FOLLOWING = 100;

        private readonly IChatClient _chatClient;
        private readonly IFollowerService _followerService;
        private readonly ICurrencyGenerator _currencyGenerator;

        public FollowableSystem(IChatClient chatClient, IFollowerService followerService,
            ICurrencyGenerator currencyGenerator)
        {
            _chatClient = chatClient;
            _followerService = followerService;
            _currencyGenerator = currencyGenerator;
        }

        public void HandleFollowerNotifications()
        {
            _followerService.OnNewFollower += FollowerServiceOnOnNewFollower;
        }

        private void FollowerServiceOnOnNewFollower(object sender, NewFollowersEventArgs eventArgs)
        {
            foreach (string followerName in eventArgs.FollowerNames)
            {
                _currencyGenerator.AddCurrencyTo(followerName, TOKENS_FOR_FOLLOWING);
                _chatClient.SendMessage(
                    $"Welcome, {followerName}! Thank you for following! {TOKENS_FOR_FOLLOWING} coins to have some fun. Everyone, say \"hello\"!");
            }
        }

        public void StopHandlingNotifications()
        {
            _followerService.OnNewFollower -= FollowerServiceOnOnNewFollower;
        }
    }
}
