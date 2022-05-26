using BirdsiteLive.Common.Settings;
using BirdsiteLive.Twitter.Extractors;
using BirdsiteLive.Twitter.Tools;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BirdsiteLive.Twitter.Tests.Extractors
{
    [TestClass]
    public class TweetExtractorTests
    {
        string ConsumerKey = "(key)";
        string ConsumerSecret = "(secret)";

        [TestMethod]
        public void TwitterTweetsService_Test()
        {
            var initializer = new TwitterAuthenticationInitializer(new TwitterSettings
            {
                ConsumerKey = this.ConsumerKey,
                ConsumerSecret = this.ConsumerSecret,
            }, new Mock<ILogger<TwitterAuthenticationInitializer>>().Object);

            var stat = new Mock<Statistics.Domain.ITwitterStatisticsHandler>();

            var twitterService = new TwitterTweetsService(
                initializer,
                new TweetExtractor(),
                stat.Object,
                new TwitterUserService(initializer, stat.Object, new Mock<ILogger<TwitterUserService>>().Object),
                new Mock<ILogger<TwitterTweetsService>>().Object
            );

            var tweets = twitterService.GetTimeline("rui_kanimiso", 200, 1524402699693072385);
            foreach (var tweet in tweets)
            {
                System.Diagnostics.Debug.WriteLine(tweet.Id);
            }

            tweets = twitterService.GetTimeline("satohina1223", 200, 1525049728290811905);
            foreach (var tweet in tweets)
            {
                System.Diagnostics.Debug.WriteLine(tweet.Id);
            }

            tweets = twitterService.GetTimeline("tomori_kusunoki", 200, 1524698068070723591);
            foreach (var tweet in tweets)
            {
                System.Diagnostics.Debug.WriteLine(tweet.Id);
            }
        }

        [TestMethod]
        public void TweetExtractor_Test()
        {
            Tweetinvi.Auth.SetApplicationOnlyCredentials(
                this.ConsumerKey,
                this.ConsumerSecret,
                true
            );
            Tweetinvi.ExceptionHandler.SwallowWebExceptions = false;
            Tweetinvi.TweetinviConfig.CurrentThreadSettings.TweetMode = Tweetinvi.TweetMode.Extended;

            var tweets = Tweetinvi.Timeline.GetUserTimeline(
                910065949159460864,
                new Tweetinvi.Parameters.UserTimelineParameters
                {
                    SinceId = 1524402699693072385,
                    MaximumNumberOfTweetsToRetrieve = 200
                }
            );

            var extractor = new TweetExtractor();
            foreach (var tweet in tweets)
            {
                var extractedTweet = extractor.Extract(tweet);
                System.Diagnostics.Debug.WriteLine(extractedTweet.Id);
            }
        }
    }
}
