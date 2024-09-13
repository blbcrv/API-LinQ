const API_BASE_URL = 'https://localhost:7038/api';

function loadPokemons() {
    axios.get(`${API_BASE_URL}/pokemon`)
        .then(response => {
            const pokemons = response.data;
            let html = '<h2>Pokémon</h2>';
            pokemons.forEach(pokemon => {
                console.log(pokemon)
                html += `<div>
                            <p>${pokemon.name} <button onclick="deletePokemon(${pokemon.id})">Supprimer</button></p>
                            <img src="${pokemon.image}" alt="Image de ${pokemon.name}" style="width:100px; height:auto;">
                         </div>`;
            });
            document.getElementById('pokemons').innerHTML = html;
        })
        .catch(error => console.error('Erreur de chargement des Pokémon', error));
}


function loadTrainers() {
    axios.get(`${API_BASE_URL}/trainer`)
        .then(response => {
            const trainers = response.data;
            let html = '<h2>Dresseurs</h2>';
            trainers.forEach(trainer => {
                html += `<p>${trainer.name} - <button onclick="deleteTrainer(${trainer.id})">Supprimer</button></p>`;
            });
            document.getElementById('trainers').innerHTML = html;
        })
        .catch(error => console.error('Erreur de chargement des Dresseurs', error));
}

window.onload = function() {
    loadPokemons();
    loadTrainers();
};

function deletePokemon(id) {
    axios.delete(`${API_BASE_URL}/pokemon/${id}`)
        .then(() => loadPokemons())
        .catch(error => console.error('Erreur de suppression du Pokémon', error));
}

function deleteTrainer(id) {
    axios.delete(`${API_BASE_URL}/trainer/${id}`)
        .then(() => loadTrainers())
        .catch(error => console.error('Erreur de suppression du Dresseur', error));
}

document.getElementById('addPokemon').addEventListener('click', function() {
    document.getElementById('pokemonForm').style.display = 'block';
});

document.getElementById('addTrainer').addEventListener('click', function() {
    document.getElementById('trainerForm').style.display = 'block';
});


function addPokemon() {
    const name = document.getElementById('pokemonName').value;
    const type = document.getElementById('pokemonType').value;
    const image = document.getElementById('pokemonImg').value;
    const attacks = document.getElementById('pokemonAttacks').value.split(',').map(attack => attack.trim()); // Transformer la chaîne en tableau

    axios.post(`${API_BASE_URL}/pokemon`, { name, type, attack: attacks, image })
        .then(() => {
            document.getElementById('pokemonForm').style.display = 'none'; // Fermer le formulaire
            loadPokemons();
        })
        .catch(error => console.error('Erreur lors de l’ajout du Pokémon', error));
}


function addTrainer() {
    const name = document.getElementById('trainerName').value;
    const age = document.getElementById('trainerAge').value;
    const region = document.getElementById('trainerRegion').value;

    axios.post(`${API_BASE_URL}/trainer`, { name, age, region })
        .then(() => {
            document.getElementById('trainerForm').style.display = 'none'; // Fermer le formulaire
            loadTrainers();
        })
        .catch(error => console.error('Erreur lors de l’ajout du Dresseur', error));
}

document.getElementById('loginBtn').addEventListener('click', function() {
    document.getElementById('loginForm').style.display = 'block';
});

function login() {
    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;

    axios.post(`${API_BASE_URL}/login`, { username, password })
        .then(response => {
            const token = response.data.token;
            alert("Token JWT : " + token);
            document.getElementById('loginForm').style.display = 'none';
        })
        .catch(error => {
            console.error('Erreur lors de la connexion', error);
            alert('Échec de la connexion');
        });
}
