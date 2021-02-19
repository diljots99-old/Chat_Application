"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
//document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (conversationId,userID, message) {
    //var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var currentConversationId = document.getElementById("currentConversationId").value;
    console.log(message);
    console.log(connection.connectionId);
    console.log(userID);

    var messageObj = JSON.parse(message);
    console.log(messageObj)
    var currentUserID = document.getElementById("currentUserId").value;
    var position = "left";
    if (currentUserID == userID) {
        position = "right";
    }
    if (currentConversationId == conversationId) {
        //var encodedMsg = userID + " says " + msg;

        var li = document.createElement("li");
        //li.textContent = encodedMsg;
        li.className = `${position} clearfix`;
        li.innerHTML = ` <span class="chat-img pull-${position}">
                                                <img src="https://bootdey.com/img/Content/user_1.jpg" alt="User Avatar">
                                            </span>
<div class="chat-body clearfix">
                               

                            <div class="header">
                                <strong class="primary-font">${ messageObj.senderName}</strong>
                                <small class="pull-right text-muted"><i class="fa fa-clock-o"></i> ${messageObj.messageSent}</small>
                            </div>
                            <p>
                                ${messageObj.messageContent}

                            </p>
                        </div>`;
        

        document.getElementById("chat").appendChild(li);
    }


   
});

connection.start().then(function () {
    //document.getElementById("sendButton").disabled = false;
    console.log("Connection established "+connection.connectionId);
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var userID = document.getElementById("currentUserId").value;
    var conversationId = document.getElementById("currentConversationId").value;
    var message = document.getElementById("messageContent").value;
   

    connection.invoke("SendMessage", conversationId, userID,message).catch(function (err) {
        return console.error(err.toString());
    });
    document.getElementById("messageContent").value = "";
    event.preventDefault();
});