# An EpicCheckers Bot
Checkers bot [on GitHub](https://github.com/sleepyparadox/EpicCheckersBot) for [Epic Checkers](http://epiccheckers.appspot.com/)



### How to use:

 * Browse to [epic checkers](http://epiccheckers.appspot.com/)
 * Open console
 * Grab a copy of [BootLoader.js](https://raw.githubusercontent.com/sleepyparadox/EpicCheckersBot/master/Source/EpicCheckersBot/BootLoader.js) 
  * Configure [BootLoader.js](https://raw.githubusercontent.com/sleepyparadox/EpicCheckersBot/master/Source/EpicCheckersBot/BootLoader.js) (optional)
  * Paste into browser console
  * Expect response ```Started bootloader as Red```

### Configure:

###### Teams
```javascript
BootLoader.PlayAsBlue = true;
BootLoader.PlayAsRed = true;
```
###### Speed
```javascript
BootLoader.TimeBetweenStepsMs = 100;
```

### Building your own server:
   * Everytime the bot needs to make a move it will POST to a target url and move as per the response
###### POST Request url:
 *  Change the BootLoader's target url to your server 
 ```javascript
 BootLoader.Url = "http://ec2-34-205-139-1.compute-1.amazonaws.com/"
 ```
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
  * Respond with MoveUnit's four int arguments
 ```javascript
 BootLoader.MoveUnit(fromRow, fromCol, toRow, toCol)
 ``` 
 
 ### Server Headers:
   * You may need to enable ```Access-Control-Allow-Origin``` so that browsers can contact your sever
   * C# example of this exists in [CheckersListener.cs](https://github.com/sleepyparadox/EpicCheckersBot/blob/master/Source/EpicCheckersBot/Listener/CheckersListener.cs#L60)