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

        public void Send(string userId, string message, string chatId, string firstName, string lastName, bool isImage = false, bool isFile = false)
        {
            // Call the addNewMessageToPage method to update clients.
            //Clients.All.addNewMessageToPage(string.Format("{0} {1}", firstName, lastName), message, chatId);
            //Clients.Caller.addNewMessageToPage(string.Format("{0} {1}", firstName, lastName), message, chatId);
            if(isImage)
            {
                Clients.Client(Context.ConnectionId).addNewImageToPage(string.Format("{0} {1}", firstName, lastName), message, chatId);
            }
            else if (isFile)
            {
                Clients.Client(Context.ConnectionId).addNewFileToPage(string.Format("{0} {1}", firstName, lastName), message, chatId);
            }
            else
            {
                Clients.Client(Context.ConnectionId).addNewMessageToPage(string.Format("{0} {1}", firstName, lastName), message, chatId);
            }
            //Clients.Client(chatId).sendPrivateMessage(firstName, message);
            //Clients.Caller.sendPrivateMessage(chatId, name, message, chatId);
            InsuranceBusiness.BusinessLayer.SaveMessage(userId, chatId, message, isImage, isFile);

            InsuranceBusiness.BusinessLayer.CreateNotificationForChat(chatId, userId, InsuranceSocialNetworkCore.Enums.NotificationTypeEnum.NEW_MESSAGE_RECEIVED);
        }
    }
}