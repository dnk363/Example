"use strict";

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    //.withAutomaticReconnect()
    .build();

//Disable send button until connection is established
if (document.getElementById("sendButton")) {
    document.getElementById("sendButton").disabled = true;
}

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var date = new Date();
    var time = checkTime(date.getHours()) + ':' + checkTime(date.getMinutes()) + ':' + checkTime(date.getSeconds());

    var mainDiv = document.createElement("div");
    mainDiv.style = "padding:5px; margin:0;";

    var rowDiv = document.createElement("div");
    rowDiv.className = "media-body";

    var name = document.createElement("h5");
    name.className = "col-md-6";
    name.textContent = user;

    var message = document.createElement("div");
    message.className = "col-md-6";
    message.style = "margin:20;";
    message.textContent = msg;

    var dateTime = document.createElement("div");
    dateTime.className = "col-md-3 ml-auto";
    dateTime.textContent = time;

    document.getElementById("messagesList").appendChild(mainDiv);
    mainDiv.appendChild(rowDiv);
    rowDiv.appendChild(name);
    rowDiv.appendChild(message);
    rowDiv.appendChild(dateTime);
    document.getElementById("messageInput").value = "";
    document.getElementById("messagesList").scrollTop = 9999;
});

connection.on("ConnectAll", function (user) {
    if (!document.getElementById(user)) {
        AddUser(user);
    }
});

connection.on("ConnectUser", function (allUsers) {
    for (let allUser of allUsers) {
        if (!document.getElementById(allUser.userName)) {
            AddUser(allUser.userName);
        }
    }
});

connection.on("Disconnect", function (user) {
    document.getElementById(user).remove();
});

if (document.getElementById("sendButton")) {
    connection.start().then(function () {
        document.getElementById("sendButton").disabled = false;
    }).catch(function (err) {
        return console.error(err.toString());
    });
}

if (document.getElementById("sendButton")) {
    document.getElementById("sendButton").addEventListener("click", function (event) {
        var user = document.getElementById("userInput").textContent;
        var message = document.getElementById("messageInput").value;
        connection.invoke("SendMessage", user, message).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    });
}

function checkTime(i) {
    if (i < 10) {
        i = "0" + i;
    }
    return i;
}

function AddUser(newUser) {
    var userNamediv = document.createElement("div");
    userNamediv.id = newUser;

    var userNameText = document.createElement("div");
    userNameText.textContent = newUser;
    userNameText.className = "col-md-1 offset-md-1 text-md-center";
    userNameText.style = "margin-top:5px;";

    var userNameLine = document.createElement("hr");
    userNameLine.className = "offset-md-1 sticky-top";
    userNameLine.style = "align:center;margin-top:5px;";

    userNamediv.appendChild(userNameText);
    userNamediv.appendChild(userNameLine);
    document.getElementById("userList").appendChild(userNamediv);
}
