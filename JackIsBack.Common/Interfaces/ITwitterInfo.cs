﻿namespace JackIsBack.Common
{
    public interface ITwitterInfo
    {
        TwitterSecrets Secrets { get; set; }
        TwitterApiLinks ApiLinks { get; set; }
    }
}