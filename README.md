This is an API I am creating for personal development growth. It is a work in progress and will be updated frequently.
It is a .NET 8 Web API that uses EFCore to connect to an Azure SQL Server database.
It uses data from the game **Warframe** and its *Prime Parts/Relic* system to create reward tables of warframe parts from relics.

DB Tables:

	- Relic: (ID, Name)
	- Warframe: (ID, Name)
	- Reward: (ID, PartType, Rarity, RelicID (*References Relic.ID*), WarframeID (*References Warframe.ID*))

API Endpoints:
	
	- Relic Table:
		- GET /api/Relic: Returns All Relics
		- GET /api/Relic/{name}: Returns Relic by Name (e.g. 'Lith X10', 'Axi M3', 'Meso R2', 'Neo G4', etc.)
		- POST /api/Relic/Create: Creates and Stores Relic (from Relic object in request body)
		- PATCH /api/Relic/Update/{relicName}: Updates Vaulted Status (new status in request body)
		- DELETE /api/Relic/Remove: Removes Relic by Name (passed in body)
	
	- Warframe Table:
		- GET /api/Warframe: Returns All Warframes
		- GET /api/Warframe/{name}: Returns Warframe by Name
		- POST /api/Warframe/Create: Creates and Stores Warframe (from Warframe object in request body)
		- PUT /api/Warframe/{WarframeName}: Updates Warframe Name (new name in request body)
		- DELETE /api/Warframe/Remove: Removes Warframe by Name (passed in body)

	- Reward Table
		- GET /api/Reward: Returns All Rewards
		- GET /api/Reward/{relicName}+{warframeName}: Returns All Rewards for a specific Warframe and Relic combination
		- GET /api/Reward/Relic/{relicName}: Returns All Rewards Filtered by Relic Name
		- GET /api/Reward/Warframe/{warframeName}: Returns All Rewards Filtered by Warframe Name
		- POST /api/Reward/Create/{warframeName}+{relicName}: Creates and Stores Reward (from Reward object in request body + query params)
		- DELETE /api/Reward/Delete/{warframeName}+{relicName}+{partType}: Removes Reward by WF name + PartType + relicName