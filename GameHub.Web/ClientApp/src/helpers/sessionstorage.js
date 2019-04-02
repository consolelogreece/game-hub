let expirePeriod = 1 * 24 * 60 * 60 * 1000; // days to millisconds

export const setGame = (gameId, playerId) => {
    let data = {
        playerId: playerId,
        timestamp: Date.now().toString()
    }
    localStorage.setItem(gameId, JSON.stringify(data));
}

export const getPlayerId = gameId => {
    var res = JSON.parse(localStorage.getItem(gameId));
    return res == null ? null : res.playerId;
}

export const removeGame = (gameId) => {
    localStorage.removeItem(gameId);
}

export const removeExpiredGames = () => {
    let currentDate = Date.now().toString();

    for (var key in localStorage) {
        let data = JSON.parse(localStorage.getItem(key));

        let expireDate = Date.parse(data.timestamp) + expirePeriod;

        if (currentDate > expireDate) localStorage.removeItem(key);
    }
}