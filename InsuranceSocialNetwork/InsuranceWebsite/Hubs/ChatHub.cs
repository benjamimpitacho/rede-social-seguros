using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using InsuranceSocialNetworkBusiness;

namespace InsuranceWebsite.Hubs
{
    public class ChatHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }

        //public void Send(string profileId, string message)
        //{
        //    // Call the addNewMessageToPage method to update clients.
        //    Clients.All.addNewMessageToPage(profileId, message);
        //}

        public void Send(string userId, string message, string chatId, string firstName, string lastName)
        {
            InsuranceBusiness.BusinessLayer.SaveMessage(userId, chatId, message);
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(string.Format("{0} {1}", firstName, lastName), message, chatId);
            //Clients.Client(chatId).sendPrivateMessage(name, name, message, name);
            //Clients.Caller.sendPrivateMessage(chatId, name, message, chatId);
        }
    }
}