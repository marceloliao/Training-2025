"use strict";
// Required
Object.defineProperty(exports, "__esModule", { value: true });
var myCar = {
    make: "Ford",
    model: "Focus",
    year: 2024,
};
console.log(myCar.year);
// Record, Record<string, number> is equivalent to { [key: string]: number }
var nameAgeMap = {
    Alice: 21,
    Bob: 25,
};
console.log(nameAgeMap["Bob"]);
var nameAgeMap2 = {};
nameAgeMap2["Maria"] = 24;
nameAgeMap2["Marcus"] = 45;
console.log(nameAgeMap2);
