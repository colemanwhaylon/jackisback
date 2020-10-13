# jackisback

The Twitter Streaming API, "Jack Is Back", provides real-time access to public tweets. In this assignment you will build an application that connects to the Streaming API and processes incoming tweets to compute various statistics. We'd like to see this as a .NET Core or .NET Framework project (Visual Studio Community Edition or VS Code), but otherwise feel free to use any libraries you want to accomplish this task.

The sample endpoint provides a random sample of approximately 1% of the full tweet stream. Your app should consume this sample stream and keep track of the following:

Total number of tweets received
Average tweets per hour/minute/second
Topemojis in tweets
Percent of tweets that contains emojis
Top hashtags
Percent of tweets that contain a url
Percent of tweets that contain a photo url (pic.twitter.com or instagram)
Top domains of urls in tweets
