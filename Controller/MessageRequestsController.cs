    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TUI_Messaging_App.TUI_Messaging_App.Model;
    using TUI_Messaging_App.TUI_Messaging_App.Services;

    namespace TUI_Messaging_App.TUI_Messaging_App.Controller
    {
        internal class MessageRequestsController
        {

            public List<MessageRequestsModel.MessageRequestObject> handleFetchMessageRequests(string username)
            {
                MessageRequestsModel messageRequestsModel = new MessageRequestsModel();


                 // Call the database ONCE and store the result
                  var requests = messageRequestsModel.fetchMessageRequests(username);

                  // Return the list if it has data, otherwise an empty list
                  return requests ?? new List<MessageRequestsModel.MessageRequestObject>();
        }

           public List<MessageRequestsModel.MessageRequestObject> handleFetchApprovedContacts(string username)
        {
            MessageRequestsModel messageRequestsModel = new MessageRequestsModel();

            var requests = messageRequestsModel.fetchAcceptedChatRequests(username);

            return requests ?? new List<MessageRequestsModel.MessageRequestObject>();
        }
        }
}
