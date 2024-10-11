<script>
    async function loadPlayerInfo(playerName) {
        try {
            const response = await fetch(`https://localhost:7120/api/player/${playerName}`);
    if (response.ok) {
                const data = await response.json();

    data.playerInfo = JSON.parse(data.playerInfo);
    data.friendsList = JSON.parse(data.friendsList);
    data.playerJob = JSON.parse(data.playerJob);
    data.playerClicker = JSON.parse(data.playerClicker);

    displayPlayerInfo(data);
            } else {
        document.getElementById('result').innerText = "Erreur lors de la récupération des données.";
            }
        } catch (error) {
        document.getElementById('result').innerText = "Erreur de requête.";
        }
    }

    document.getElementById('playerForm').addEventListener('submit', async function (event) {
        event.preventDefault();
    const playerName = document.getElementById('playerName').value;
    await loadPlayerInfo(playerName);
    });

    function displayPlayerInfo(data) {
        const resultDiv = document.getElementById('result');
    resultDiv.innerHTML = '';

    const playerInfo = document.createElement('div');
    playerInfo.classList.add('player-info');
    playerInfo.innerHTML = `
    <img src="https://www.example.com/image.png" alt="Image du joueur" class="player-image">
        <h2>Informations du Joueur</h2>
        <div class="info">Nom d'utilisateur: ${data.playerInfo.username}</div>
        <div class="info">Alliance: ${data.playerInfo.alliance}</div>
        <div class="info">Faction: ${data.playerInfo.faction} (${data.playerInfo.factionRank})</div>
        <div class="info">Argent: ${data.playerInfo.money}</div>
        <div class="info">Rang: ${data.playerInfo.rank}</div>
        <div class="info">UUID: ${data.playerInfo.uuid}</div>
        `;
        resultDiv.appendChild(playerInfo);

        // Affichage de la liste des amis
        const friendsList = document.createElement('div');
        friendsList.classList.add('player-info');
        friendsList.innerHTML = `<h2>Liste des Amis</h2>
        <div class="info">Total Amis: ${data.friendsList.totalCount}</div>`;
        resultDiv.appendChild(friendsList);

        // Affichage des métiers
        const playerJob = document.createElement('div');
        playerJob.classList.add('player-info');
        playerJob.innerHTML = `<h2>Métier</h2>`;
        for (const [job, info] of Object.entries(data.playerJob)) {
            playerJob.innerHTML += `<div class="info">${job.charAt(0).toUpperCase() + job.slice(1)}: Niveau ${info.level}, XP ${info.xp}</div>`;
        }
        resultDiv.appendChild(playerJob);

        // Affichage des Clickers
        const playerClicker = document.createElement('div');
        playerClicker.classList.add('player-info');
        playerClicker.innerHTML = `<h2>Clicker</h2>`;
        data.playerClicker.buildings.forEach(building => {
            playerClicker.innerHTML += `<div class="info">${building.name.charAt(0).toUpperCase() + building.name.slice(1)}: Production ${building.production}, Quantité ${building.quantity}</div>`;
        });
        resultDiv.appendChild(playerClicker);
    }
</script>
