"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (conversationId,user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("chat").appendChild(li);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
    console.log("Connection established "+connection.connectionId);
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var userID = document.getElementById("currentUserId").value;
    var conversationId = document.getElementById("currentconversationId").value;
    var message = document.getElementById("messageContent").value;
    connection.invoke("SendMessage", conversationId, userID,message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});