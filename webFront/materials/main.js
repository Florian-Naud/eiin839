function requestGet(url, callback) {
    var request = new XMLHttpRequest();
    request.open("GET", url, true);
    request.setRequestHeader("Accept", "application/json");
    request.onload = callback;
    request.send();
}

function retrieveAllContracts() {
    var key = document.getElementById("key").value;
    var url = "https://api.jcdecaux.com/vls/v3/contracts?apiKey=" + key;

    requestGet(url, contractsRetrieved);
}

function contractsRetrieved() {
    var response = JSON.parse(this.responseText);
    var list = document.getElementById("list-of-contracts");
    response.forEach(contract => {
        var option = document.createElement("option");
        option.value = contract.name;
        list.appendChild(option);
    });

}

function retrieveContractStations() {
    var key = document.getElementById("key").value;
    var contract = document.getElementById("contracts").value;
    var url = "https://api.jcdecaux.com/vls/v1/stations?contract=" + contract + "&apiKey=" + key;

    requestGet(url, stationsRetrieved);
}

function stationsRetrieved() {
    var response = JSON.parse(this.responseText);
    console.log(response);
}