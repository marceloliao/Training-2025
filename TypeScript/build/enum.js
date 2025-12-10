"use strict";
var CardinalDirections;
(function (CardinalDirections) {
    CardinalDirections[CardinalDirections["North"] = 3] = "North";
    CardinalDirections[CardinalDirections["East"] = 4] = "East";
    CardinalDirections[CardinalDirections["South"] = 5] = "South";
    CardinalDirections[CardinalDirections["West"] = 6] = "West";
})(CardinalDirections || (CardinalDirections = {}));
// console.log(CardinalDirections.North);
var currentDirection = CardinalDirections.North;
console.log(currentDirection);
currentDirection = 6;
console.log(currentDirection);
