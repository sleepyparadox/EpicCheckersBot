
// goto http://epiccheckers.appspot.com/
// and copy paste this into the console
// AI will PlayAsRed by default, set config below

(function()
{
	var boardGame = g_sceneManager.scene.boardGame;
	var gameData = boardGame.gameData;
	
	if(typeof BootLoader === "undefined")
	{
	    BootLoader = {};
	}
		
	BootLoader.OldTurn = null;
	BootLoader.CurrentSession = null;
	BootLoader.PlayAsBlue = false;
	BootLoader.PlayAsRed = true;
	//BootLoader.Url = "http://localhost/";
	BootLoader.Url = "http://ec2-34-205-139-1.compute-1.amazonaws.com/";
	BootLoader.TimeBetweenStepsMs = 100;

	BootLoader.Run = function(s)
	{
		if(BootLoader == null || BootLoader.CurrentSession != s)
			return;
		
		BootLoader.Step();
		
		setTimeout(function () { BootLoader.Run(s); }, BootLoader.TimeBetweenStepsMs);
	};
	
	BootLoader.Step = function()
	{
	    if(gameData.turn != BootLoader.OldTurn && gameData.state === "playing")
	    {
	        if ((gameData.turn === true && BootLoader.PlayAsRed)
                || (gameData.turn === false && BootLoader.PlayAsBlue))
	        {
	            BootLoader.TurnStarted();
	        }
	        else
	        {
	            console.log("Waiting for player");
	        }
	        BootLoader.OldTurn = gameData.turn;
	    }	
	};
	
	BootLoader.TurnStarted = function()
	{
	    // Blue, false, 0, 
	    // Red, true, 1, 

	    var requestBody =
        {
            Turn: (gameData.turn ? "Red" : "Blue"),
            Round: gameData.round,
            Board: []
        };

	    for (var r = 0; r < gameData.map.length; ++r)
	    {
	        requestBody.Board.push([]);
	        for (var c = 0; c < gameData.map[r].length; ++c)
	        {
	            var cell = gameData.map[r][c];
	            if (cell >= 10 && cell <= 19)
	                requestBody.Board[r][c] = "Blue";
	            else if (cell >= 20 && cell <= 29)
	                requestBody.Board[r][c] = "Red";
	            else
	                requestBody.Board[r][c] = null;
	        }
	    }

	    var requestJson = JSON.stringify(requestBody);

	    console.log("Requesting move");
	    console.log(requestJson);

	    var request = new XMLHttpRequest();
	    request.open('POST', BootLoader.Url, true);
	    request.onreadystatechange = function () {
	        if (request.readyState == 4)
	        {
	            console.log("Recieving move");
	            console.log(request.responseText);

	            var nextMove = JSON.parse(request.responseText);
	            BootLoader.MoveUnit(nextMove[0], nextMove[1], nextMove[2], nextMove[3]);
	        }
	    };
	    request.setRequestHeader("Content-type", "jsonp");
	    request.send(requestJson);
	};

	BootLoader.MoveUnit = function(fromRow, fromCol, toRow, toCol)
	{
	    console.log("Recieved move from [" + fromRow + ", " + fromCol + "] to [" + toRow + "," + toCol + "]");

	    var pickedUnit = gameData.map[fromRow][fromCol];

	    gameData.map[toRow][toCol] = pickedUnit;
	    gameData.map[fromRow][fromCol] = 0;

	    gameData.pick.r = toRow;
	    gameData.pick.c = toCol;

		
	    var tileSize = boardGame.tileSprite.size
		
	    var d = toRow * tileSize.width / 2 - toCol * tileSize.width / 2;
	    var f = toRow * tileSize.height / 2 + toCol * tileSize.height / 2;
		
	    var unitArray = pickedUnit >= 20 ? boardGame.unit2 : boardGame.unit1;
	    var unitIndex = pickedUnit >= 20 ? pickedUnit - 20 : pickedUnit - 10;
		
	    unitArray[unitIndex].setTargetPosition(boardGame.position.x - d, boardGame.position.y / 2 + f);
		
	    gameData.state = "moveUnit";
	};
	
    // CurrentSession garuantees old instances of BootLoader.js will be killed without refreshing page
	var currentSession = Date();
	BootLoader.CurrentSession = currentSession;
	BootLoader.Run(currentSession);

	var playingAs = [];
	if (BootLoader.PlayAsBlue)
	    playingAs.push("Blue");
	if (BootLoader.PlayAsRed)
	    playingAs.push("Red");

	return ("Started bootloader as " + playingAs);
})();