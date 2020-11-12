# jackisback

The Twitter Streaming Client App, "Jack Is Back", provides real-time access to public tweets and statistics an end user may want to track. This app will support several clients, (Console, Blazor WebAssembly, and Xamarin). I'm building an application that connects to the Twitter Streaming API and processes incoming tweets to compute various statistics. This as a .NET Core solution.

It consumes the Twitter sample endpoint that provides a random sample of approximately 1% of the full tweet stream. 

This app keep track of the following statistics:

1. Total number of tweets received
2. Average tweets per hour/minute/second
3. Topemojis in tweets
4. Percent of tweets that contains emojis
5. Top hashtags
6. Percent of tweets that contain a url
7. Percent of tweets that contain a photo url (pic.twitter.com or instagram)
8. Top domains of urls in tweets
