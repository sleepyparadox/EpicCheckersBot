
// goto http://epiccheckers.appspot.com/
// and copy paste this into the console

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
	BootLoader.IsRed = true;
	BootLoader.Url = "http://localhost/";

	BootLoader.Run = function(s)
	{
		if(BootLoader == null || BootLoader.CurrentSession != s)
			return;
		
		BootLoader.Step();
		
		setTimeout(function(){ BootLoader.Run(s);}, 1000);
	};
	
	BootLoader.Step = function()
	{
	    if(gameData.turn != BootLoader.OldTurn)
	    {
	        if(gameData.turn == BootLoader.IsRed)
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

	    console.log("Requesting move");
	    console.log(requestBody);

	    var requestJson = JSON.stringify(requestBody);
	    var requestBase64 = btoa(requestJson);

	    var script = document.createElement('script');
	    script.src = BootLoader.Url + "?base64=" + requestBase64;

	    document.querySelector('head').appendChild(script);
	};

	BootLoader.MoveUnit = function(fromRow, fromCol, toRow, toCol)
	{
	    console.log("Recieved move from [" + fromRow + ", " + fromCol + "] to [" + toRow + "," + toCol + "]");

	    var pickedUnit = gameData.map[fromRow][fromCol];
		
	    gameData.map[toRow][toCol] = pickedUnit;
	    gameData.map[fromRow][fromCol] = 0;
		
		
	    var tileSize = boardGame.tileSprite.size
		
	    var d = toRow * tileSize.width / 2 - toCol * tileSize.width / 2;
	    var f = toRow * tileSize.height / 2 + toCol * tileSize.height / 2;
		
	    var unitArray = pickedUnit >= 20 ? boardGame.unit2 : boardGame.unit1;
	    var unitIndex = pickedUnit >= 20 ? pickedUnit - 20 : pickedUnit - 10;
		
	    unitArray[unitIndex].setTargetPosition(boardGame.position.x + d, boardGame.position.y / 2 + f);
		
	    gameData.state = "moveUnit";
	};
	
	var currentSession = Date();
	BootLoader.CurrentSession = currentSession;
	BootLoader.Run(currentSession);

	return "Started bootloader as player " + (BootLoader.IsRed ? "Red" : "Blue");
})();