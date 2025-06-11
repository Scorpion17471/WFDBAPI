This is an API I am creating for personal development growth. It is a work in progress and will be updated frequently.
It is a .NET 8 Web API that uses EFCore to connect to an Azure SQL Server database.
It uses data from the game **Warframe** and its *Prime Parts/Relic* system to create reward tables of warframe parts from relics.

Warframe Wiki Examples:
	-Warframe: https://wiki.warframe.com/w/Mesa/Prime
 	-Relic: https://wiki.warframe.com/w/Axi_M3
  	-Reward: https://wiki.warframe.com/w/Axi_M3 (Same page as relics, specifically the drop table with components and rewards displayed)

DB Tables:

	- Relic: 
 		-ID int IDENTITY PK
   		-Vaulted bit NOT NULL
   		-Name varchar 20 NOT NULL
	- Warframe:
 		-ID int IDENTITY PK
   		-Name varchar 25 UNIQUE NOT NULL
	- Reward:)
   		-ID int IDENTITY PK
   		-PartType varchar 15 NOT NULL (CHECK = 'Main', 'Neuroptics', 'Chassis', 'Systems')
   		-Rarity varchar 10 NOT NULL (CHECK = 'Common', 'Uncommon', 'Rare', 'UNKNOWN')
   		-RelicID int FK (Relic.ID)
   		-WarframeID int FK (Warframe.ID)

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
