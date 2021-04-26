# Lancer le projet:
	On peut lancer les .bat dans n'importe quel ordre, tant que les clients ne demandent rien aux web services il n'y aura aucun problème.
	Lancer *LaunchWebService.bat* pour lancer les 2 web services: proxy et routing
	Lancer *HeavyClient.bat* pour lancer le client lourd.
	Ouvrir *index.html* dans un navigateur pour lancer le client leger.
	
# Client leger:
	Application pour les clients de LetsGoBiking.
	Permet de chercher un itinéraire dans UNE ville.
	Propose l'itinérairele plus rapide.
	Si l'itinéraire le plus rapide n'utilise pas de vélo, alors il y a un avertissement mais on peut quand même afficher l'itinéraire.
	
# Client lourd:
	Application pour l'entreprise de LetsGoBiking.
	Permet de recupérer les stats.
	Stats disponible: le nombre de fois qu'un itinéraire passe par une station.
	
# Command REST
	Recevoir toutes les stations JCDecaux d'une ville.
	http://localhost:8733/Design_Time_Addresses/Proxy/Service/GetJCDecauxItemsByCity?name=marseille
	
	Recevoir la position GPS d'une ville.
	http://localhost:8733/Design_Time_Addresses/Routing/Service/GetPositionCityREST?laVille=marseille
	
	Recevoir l'itinéraire entre 2 adresses. Reçoit aussi un code pour signaler le status de l'itinéraire:
	200: chemin en passant par les stations.
	201: chemin sans stations entre (à pied).
	202: pas de station disponible + chemin a pied.
	203: mauvaise adresses ou nom de ville.
	http://localhost:8733/Design_Time_Addresses/Routing/Service/GetDirectionREST?depart=Boulevard%20Ledru%20Rollin&arrive=2-10%20Rue%20du%20Coteau&laVille=marseille
	
	Recevoir les statistiques sur le nombre de fois qu'un itinéraire passe par une station
	http://localhost:8733/Design_Time_Addresses/Routing/Service/GetHistoriqueStationsREST
	
	
	