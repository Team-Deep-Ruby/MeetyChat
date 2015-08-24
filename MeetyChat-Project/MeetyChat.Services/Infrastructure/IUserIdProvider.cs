using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MeetyChat.Services.Infrastructure
{
    public interface IUserIdProvider
    {
        string GetUserId();
    }
}