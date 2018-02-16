<img src="https://raw.githubusercontent.com/sleepyparadox/EpicCheckersBot/master/Img/bot_vs_bot.jpg">


# An EpicCheckers Bot
Checkers bot [on GitHub](https://github.com/sleepyparadox/EpicCheckersBot) for [Epic Checkers](http://epiccheckers.appspot.com/) created by the incredible Plus Pingya


### How to use:

 * Browse to [epic checkers](http://epiccheckers.appspot.com/)
 * Open console
 * Grab a copy of [BootLoader.js](https://raw.githubusercontent.com/sleepyparadox/EpicCheckersBot/master/Source/EpicCheckersBot/BootLoader.js) 
  * Configure [BootLoader.js](https://raw.githubusercontent.com/sleepyparadox/EpicCheckersBot/master/Source/EpicCheckersBot/BootLoader.js) (optional)
  * Paste into browser console
  * Expect response ```Started bootloader as Red```

### Configure:

###### Choose your 
 *  You can edit which server bots will request moves from using these variables
```javascript
BootLoader.BlueUrl = "http://ec2-34-205-139-1.compute-1.amazonaws.com/";
BootLoader.RedUrl = null; // null is human player
```
###### Turn Speed
```javascript
BootLoader.TimeBetweenStepsMs = 100;
```

### Building your own server:
   * Everytime the bot needs to make a move it will POST to a target url and move as per the response
###### POST Request body format:
 *  Sends game state
 ```json
{  
   "Turn":"Blue",
   "Round":1,
   "Board":[  
      [ "Red", "Red", "Red", "Red", "Red", "Red", "Red", "Red" ],
      [ null, null, null, null, null, null, null, null ],
      [ null, null, null, null, null, null, null, null ],
      [ null, null, null, null, null, null, null, null ],
      [ null, null, null, null, null, null, null, null ],
      [ null, null, null, null, null, null, null, null ],
      [ null, null, null, null, null, null, null, null ],
      [ "Blue", "Blue", "Blue", "Blue", "Blue", "Blue", "Blue", "Blue" ]
   ]
}
 ```

###### Server Response format:
```javascript
 [ 0, 0, 2, 0 ]
 ```
  * Respond with from position and to position
  * In same order as the EpicCheckers MoveUnit() arguments
 ```javascript
 BootLoader.MoveUnit(fromRow, fromCol, toRow, toCol)
 ``` 
 
 ### Connecting to Local Host - Server Headers:
   * You may need to enable ```Access-Control-Allow-Origin``` so that browsers can contact your sever
   * C# example of this exists in [CheckersListener.cs](https://github.com/sleepyparadox/EpicCheckersBot/blob/master/Source/EpicCheckersBot/Listener/CheckersListener.cs#L60)
   
   
Special thanks to Plus Pingya for creating Epic Checkers