"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var date = new Date();
    var time = checkTime(date.getHours()) + ':' + checkTime(date.getMinutes()) + ':' + checkTime(date.getSeconds());

    var mainDiv = document.createElement("div");
    mainDiv.className = "message-candidate center-block";

    var rowDiv = document.createElement("div");
    rowDiv.className = "row";

    var subDiv = document.createElement("div");
    subDiv.className = "col-xs-8 col-md-6";

    var name = document.createElement("h5");
    name.className = "message-name";
    name.textContent = user;

    var dateTime = document.createElement("div");
    dateTime.className = "col-xs-4 col-md-6 text-right message-date";
    dateTime.textContent = time;

    var message = document.createElement("div");
    message.className = "row message-text";
    message.textContent = msg;

    document.getElementById("messagesList").appendChild(mainDiv);
    mainDiv.appendChild(rowDiv);
    mainDiv.appendChild(message);
    rowDiv.appendChild(subDiv);
    rowDiv.appendChild(dateTime);
    subDiv.appendChild(name);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").textContent;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

function checkTime(i) {
    if (i < 10) {
        i = "0" + i;
    }
    return i;
}