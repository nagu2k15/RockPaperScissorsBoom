﻿using FluentAssertions;
using RockPaperScissor.Core.Game;
using RockPaperScissor.Core.Game.Bots;
using RockPaperScissor.Core.Game.Results;
using RockPaperScissor.Core.Model;
using UnitTests.DataBuilders;
using UnitTests.Fakes;
using Xunit;

namespace UnitTests.Core.Game.RoundRunnerTests
{
    public class RunRoundShould
    {
        private readonly RockOnlyBot _rockOnlyBot 
            = new RockOnlyBot {Competitor = new Competitor()};
        private readonly PaperOnlyBot _paperOnlyBot 
            = new PaperOnlyBot { Competitor = new Competitor() };
        private readonly WaterOnlyBot _waterOnlyBot 
            = new WaterOnlyBot { Competitor = new Competitor() };
        private readonly DynamiteOnlyBot _dynamiteOnlyBot 
            = new DynamiteOnlyBot { Competitor = new Competitor() };
        private readonly ScissorsOnlyBot _scissorsOnlyBot 
            = new ScissorsOnlyBot { Competitor = new Competitor() };
        private readonly RoundRunner _roundRunner = new RoundRunner();
        private readonly RoundResultBuilder _builder = new RoundResultBuilder();

        [Fact]
        public void ReturnResultsOfRound_GivenSimpleWinCase()
        {

            RoundResult roundResult = _roundRunner.RunRound(_rockOnlyBot, _scissorsOnlyBot, _builder.WithDefaults().Build());

            roundResult.Winner.Should().Be(_rockOnlyBot.Competitor);
            roundResult.Player1.Should().Be(_rockOnlyBot.Competitor);
            roundResult.Player2.Should().Be(_scissorsOnlyBot.Competitor);
            roundResult.Player1Played.Should().Be(Decision.Rock);
            roundResult.Player2Played.Should().Be(Decision.Scissors);
        }

        [Fact]
        public void ReturnNoWinner_GivenSameChoice()
        {
            RoundResult roundResult = _roundRunner.RunRound(_rockOnlyBot, _rockOnlyBot, _builder.WithDefaults().Build());

            roundResult.Winner.Should().BeNull();
        }

        [Fact]
        public void ReturnCorrectWinner_GivenSimpleWins()
        {
            RoundResult rockWin = _roundRunner.RunRound(_rockOnlyBot, _scissorsOnlyBot, _builder.WithDefaults().Build());
            rockWin.Winner.Should().Be(_rockOnlyBot.Competitor);

            RoundResult paperWin = _roundRunner.RunRound(_paperOnlyBot, _rockOnlyBot, _builder.WithDefaults().Build());
            paperWin.Winner.Should().Be(_paperOnlyBot.Competitor);

            RoundResult scissorsWin = _roundRunner.RunRound(_scissorsOnlyBot, _paperOnlyBot, _builder.WithDefaults().Build());
            scissorsWin.Winner.Should().Be(_scissorsOnlyBot.Competitor);

            RoundResult dynamiteWin = _roundRunner.RunRound(_dynamiteOnlyBot, _rockOnlyBot, _builder.WithDefaults().Build());
            dynamiteWin.Winner.Should().Be(_dynamiteOnlyBot.Competitor);

            RoundResult waterWin = _roundRunner.RunRound(_waterOnlyBot, _dynamiteOnlyBot, _builder.WithDefaults().Build());
            waterWin.Winner.Should().Be(_waterOnlyBot.Competitor);
        }

        [Fact]
        public void IncrementDyanmite_GivenOneDynamiteUsage()
        {
            int previousUsage = _dynamiteOnlyBot.DynamiteUsed;
            _roundRunner.RunRound(_dynamiteOnlyBot, _rockOnlyBot, _builder.WithDefaults().Build());
            _dynamiteOnlyBot.DynamiteUsed.Should().Be(previousUsage + 1);
        }

        [Fact]
        public void IncrementDyanmite_GivenTwoDynamiteUsage()
        {
            int previousUsage = _dynamiteOnlyBot.DynamiteUsed;
            _roundRunner.RunRound(_dynamiteOnlyBot, _dynamiteOnlyBot, _builder.WithDefaults().Build());
            _dynamiteOnlyBot.DynamiteUsed.Should().Be(previousUsage + 2);
        }

        [Fact]
        public void IncrementDyanmite_EvenWhenInvalid()
        {
            int previousUsage = _dynamiteOnlyBot.DynamiteUsed;
            var fakeBot = new FakeBot(Decision.Dynamite, 100);
            _roundRunner.RunRound(_dynamiteOnlyBot, fakeBot, _builder.WithDefaults().Build());
            _dynamiteOnlyBot.DynamiteUsed.Should().Be(previousUsage + 1);
            fakeBot.DynamiteUsed.Should().Be(101);
        }
    }
}