var stations;

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
    stations = JSON.parse(this.responseText);
    console.log(stations);
}

function getDistanceFrom2GpsCoordinates(lat1, lon1, lat2, lon2) {
    // Radius of the earth in km
    var earthRadius = 6371;
    var dLat = deg2rad(lat2 - lat1);
    var dLon = deg2rad(lon2 - lon1);
    var a =
        Math.sin(dLat / 2) * Math.sin(dLat / 2) +
        Math.cos(deg2rad(lat1)) * Math.cos(deg2rad(lat2)) *
        Math.sin(dLon / 2) * Math.sin(dLon / 2);
    var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
    var d = earthRadius * c; // Distance in km
    return d;
}

function deg2rad(deg) {
    return deg * (Math.PI / 180)
}

function getClosestStation() {
    var lat = document.getElementById("latitude").value;
    var lon = document.getElementById("longitude").value;
    var distance = null;
    var name;
    stations.forEach(station => {
        var newDistance = getDistanceFrom2GpsCoordinates(lat, lon, station.position.lat, station.position.lng);
        if (!distance || distance > newDistance) {
            distance = newDistance;
            name = station.name;
        }
    });

    console.log(name);
}