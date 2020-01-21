function dataItem(message) {
    fetch('/home/data/').then(
        function (response) {
            if (response.status !== 200) {
                return;
            }

            response.text().then(function (data) {
                document.getElementById('body').innerHTML = data;
                if (message != null) {
                    //document.getElementById('messageData').innerHTML = message;
                    var name = document.createElement("div");
                    name.className = "alert alert-primary";
                    name.textContent = message;
                    document.getElementById("messageData").appendChild(name);
                }
            });
        }
    );
}

//Create short url
if (document.getElementById("short")) {
    document.getElementById("short").addEventListener("click", function () {
        var longurl = '?id=' + document.getElementById('longurl').value;
        fetch('/home/shorturl/' + longurl).then(
            function (response) {
                if (response.status !== 200) {
                    return
                }

                response.json().then(function (result) {
                    document.getElementById('result').innerHTML = result;
                });
            }
        );
    });
}

//Show Data
if (document.getElementById("dataItem")) {
    document.getElementById("dataItem").addEventListener("click", function () { dataItem() });
}

//Show Message
if (document.getElementById("message")) {
    if (document.getElementById("message").textContent.trim(" ").length == 0) {
        document.getElementById("message").style.display = "none";
    }
    else {
        document.getElementById("message").style.display = "block";
    }
}

//Delete data
document.addEventListener('click', function (e) {
    if (e.target.name == "delete") {
        fetch('/home/delete/' + e.target.id).then(
            function (response) {
                if (response.status !== 200) {
                    return
                }

                response.json().then(function (result) {
                    dataItem(result);
                });
            }
        );
    }
});