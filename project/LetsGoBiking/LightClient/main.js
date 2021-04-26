const WITHBIKE = 200;
const WITHOUTBIKE = 201;
const NOSTATION = 202;
const BADADDRESS = 203;

var itineraire;

var divResult = document.getElementById("result");

var map;

function requestGet(url, callback) {
    var request = new XMLHttpRequest();
    request.open("GET", url, true);
    request.setRequestHeader("Accept", "application/json");
    request.onload = callback;
    request.send();
}



function calculItinerary() {
    divResult.innerHTML = "";
    divResult.classList.remove("map");
    divResult.classList.add("lds-dual-ring");

    var laVille = document.getElementById("laVille").value;
    var depart = document.getElementById("depart").value;
    var arrive = document.getElementById("arrive").value;
    var url = "http://localhost:8733/Design_Time_Addresses/Routing/Service/rest/GetDirectionREST?depart=" + depart + "&arrive=" + arrive + "&laVille=" + laVille;

    requestGet(url, manageItinerary);
}


function manageItinerary() {
    var response = JSON.parse(this.responseText);
    console.log(response.result.route);
    itineraire = response.result;
    divResult.classList.remove("lds-dual-ring");
    if (response.code == BADADDRESS) {
        divResult.innerHTML = "MAUVAISE ADRESSE (départ ou arrivée ?)";
    } else if (response.code == NOSTATION) {
        divResult.innerHTML = "PAS DE STATATION DANS CETTE VILLE <br> Voulez-vous mettre l'itinéraire sans vélo?<br>";
        divResult.innerHTML += "<button onclick=\"displayItinerary()\"> oui </button>"
    } else if (response.code == WITHOUTBIKE) {
        divResult.innerHTML = "LE CHEMIN LE PROPOSE N'UTILISE PAS DE VELO <br> Voulez-vous mettre l'itinéraire sans vélo?<br>";
        divResult.innerHTML += "<button onclick=\"displayItinerary()\"> oui </button>"
    } else {
        displayItinerary();
    }
}

function displayItinerary() {
	var temps = [];
	temps.push(Math.floor(itineraire.duration /3600));
	temps.push(Math.floor(itineraire.duration /60) - (temps[0] * 60));
	divResult.innerHTML = "Temps: ";
	if(temps[0] > 0) {
		divResult.innerHTML += temps[0] + " heure ";
	}
		divResult.innerHTML += temps [1] + " min ";

    divResult.innerHTML += "et distance: " + itineraire.distance /1000 + " km";
    divResult.classList.add("map");
    createMap();
}

function createMap() {
    var url = "http://localhost:8733/Design_Time_Addresses/Routing/Service/rest/GetPositionCityREST?laVille=" + document.getElementById("laVille").value;
    requestGet(url, createMap_callback);
}

function createMap_callback() {
    var response = JSON.parse(this.responseText);
    map = new ol.Map({
        target: 'result', // <-- This is the id of the div in which the map will be built.
        layers: [
            new ol.layer.Tile({
                source: new ol.source.OSM()
            })
        ],

        view: new ol.View({
            center: ol.proj.fromLonLat([response.longitude, response.latitude]), // <-- Those are the GPS coordinates to center the map to.
            zoom: 12 // You can adjust the default zoom.
        })

    });

    // Create an array containing the GPS positions you want to draw
    for (var i = 0; i < itineraire.positions.length - 2; i++) {
        var coords = [
            [itineraire.positions[i].longitude, itineraire.positions[i].latitude],
            [itineraire.positions[i + 1].longitude, itineraire.positions[i + 1].latitude]
        ];
        var lineString = new ol.geom.LineString(coords);

        // Transform to EPSG:3857
        lineString.transform('EPSG:4326', 'EPSG:3857');

        // Create the feature
        var feature = new ol.Feature({
            geometry: lineString,
            name: 'Line'
        });

        // Configure the style of the line
        var lineStyle = new ol.style.Style({
            stroke: new ol.style.Stroke({
                color: '#000',
                width: 10
            })
        });

        var source = new ol.source.Vector({
            features: [feature]
        });

        var vector = new ol.layer.Vector({
            source: source,
            style: [lineStyle]
        });

        map.addLayer(vector);
    }
}