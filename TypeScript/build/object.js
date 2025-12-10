"use strict";
var car = {
    type: "Toyota",
    model: "Rav 4",
    year: 2001
};
console.log(car);
console.log(car.type);
console.log(car.model);
console.log(car.year);
var car2 = {
    type: "Toyota"
};
car2.type = "Honda";
console.log(car2.type);
// Optional properties
// This one would yield an error
// const car3:{type:string, mileage:number} = {
//     type: "Toyota"
// }; 
var car4 = {
    type: "Toyota"
};
// car4.mileage = 200;
console.log(car4);
var nameAgeMap = {};
nameAgeMap.Jack = 25;
nameAgeMap.Tom = 45;
console.log(nameAgeMap);
