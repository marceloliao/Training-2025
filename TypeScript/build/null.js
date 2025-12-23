"use strict";
function printYardSize(house) {
    var _a;
    var yardSize = (_a = house.yard) === null || _a === void 0 ? void 0 : _a.size;
    if (yardSize === undefined) {
        console.log("No yard");
    }
    else {
        console.log("Yard is about ".concat(yardSize, " square feet."));
    }
}
var myHouse = {
    neighborhood: "Brossard",
};
printYardSize(myHouse);
// Nullish Coalescing
function printMileage(mileage) {
    console.log("Mileage: ".concat(mileage !== null && mileage !== void 0 ? mileage : "Not available"));
}
printMileage(null);
printMileage(undefined);
printMileage(200);
// Null assertion
function getValue() {
    return "Hello";
}
var value = getValue();
console.log("Value lngth: " + value.length);
// Array bounds handling
var array = [1, 2, 3];
var val = array[0];
console.log(val);
