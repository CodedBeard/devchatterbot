using DevChatter.Bot.Core.BotModules.DuelingModule;
using DevChatter.Bot.Core.Systems.Chat;
using FluentAssertions;
using Moq;
using UnitTests.Fakes;
using Xunit;

namespace UnitTests.Core.BotModules.DuelingModule.DuelingSystemTests
{
    public class GetChallengesShould
    {
        [Fact]
        public void ReturnNull_GivenNoDuelsYet()
        {
            var duelingSystem = new DuelingSystem(new Mock<IChatClient>().Object, new FakeActionSystem());

            Duel challenges = duelingSystem.GetChallenges("Brendan", "Crimson");

            challenges.Should().BeNull();
        }

        [Fact]
        public void ReturnDuel_WhenAlreadyAnOpponent()
        {
            var initialChallenger = "Crimson";
            var acceptingOpponent = "Brendan";

            var duelingSystem = new DuelingSystem(new Mock<IChatClient>().Object, new FakeActionSystem());

            duelingSystem.RequestDuel(initialChallenger, acceptingOpponent);

            Duel challenges = duelingSystem.GetChallenges(acceptingOpponent, initialChallenger);

            challenges.Should().NotBeNull();
        }

        [Fact]
        public void ReturnNull_WhenTryingToAcceptOwnChallenge()
        {
            var initialChallenger = "Crimson";
            var acceptingOpponent = "Brendan";

            var duelingSystem = new DuelingSystem(new Mock<IChatClient>().Object, new FakeActionSystem());

            duelingSystem.RequestDuel(initialChallenger, acceptingOpponent);

            Duel challenges = duelingSystem.GetChallenges(initialChallenger, acceptingOpponent);

            challenges.Should().BeNull();
        }
    }
}
