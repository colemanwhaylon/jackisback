using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;

namespace JackIsBack.Console
{
    public class PlayMovieMessage
    {
        public string MovieTitle { get; private set; }
        public int UserId { get; private set; }

        public PlayMovieMessage(string movieTitle, int userId)
        {
            MovieTitle = movieTitle;
            UserId = userId;
        }
    }
}
