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
    document.getElementById("dataItem").addEventListener("click", function () {
        fetch('/home/data/').then(
            function (response) {
                if (response.status !== 200) {
                    return;
                }

                response.text().then(function (data) {
                    document.getElementById('body').innerHTML = data;
                });
            }
        );
    });
}